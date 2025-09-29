using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entitie
{
    #region SerializeFields
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _defaultMoveRange = 3;
    [SerializeField] private int _defaultAttackRange = 1;

    #endregion

    #region Variables
    private Entitie _currentTarget;
    private List<Tile> _currentPath;
    private bool _isMoving = false;
    private Dictionary<Tile, string> _highlightedTiles = new Dictionary<Tile, string>();

    #endregion

    #region Methods

    public override void DestroyEntitie()
    {
        StopAllCoroutines();
        SystemManager.Instance.ClearPlayer();
        base.DestroyEntitie();
    }

    public void HandleTileClick(Tile targetTile)
    {
        if (_isMoving) return;

        if (targetTile.IsOccupied && targetTile.CurrentEntitie != null && targetTile.CurrentEntitie != this)
        {
            if (_currentTarget == targetTile.CurrentEntitie && _currentPath != null)
            {
                int pathCost = _currentPath.Count - 1;
                if (pathCost <= MoveRange)
                    StartCoroutine(MoveAndAttack(_currentPath, _currentTarget));
                else
                    Debug.Log("Target out of range");
                return;
            }

            _currentTarget = targetTile.CurrentEntitie;
            HandleAttack(_currentTarget);
            return;
        }

        if (targetTile.Type == TileType.Traversable && !targetTile.IsOccupied)
        {
            _currentTarget = null;
            HandleMove(targetTile);
        }
    }

    private void HandleMove(Tile targetTile)
    {
        if (_currentPath != null && _currentPath.Count > 0 && targetTile == _currentPath[^1])
        {
            int pathCost = _currentPath.Count - 1;
            if (pathCost <= MoveRange)
                StartCoroutine(MoveAlongPath(_currentPath));
            else
                Debug.Log("Target out of move range");
            return;
        }

        ClearHighlights();
        _currentPath = Pathfinder.FindPathToMove(CurrentTile, targetTile);
        if (_currentPath == null) return;

        for (int i = 1; i < _currentPath.Count; i++)
        {
            if (i <= MoveRange)
                MarkHighlight(_currentPath[i], "move");
            else
                MarkHighlight(_currentPath[i], "moveOutOfRange");
        }
    }

    private IEnumerator MoveAlongPath(List<Tile> path)
    {
        _isMoving = true;
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 start = transform.position;
            Vector3 end = path[i].transform.position;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * _moveSpeed;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
            PlaceOnTile(path[i]);
        }
        ClearHighlights();
        _isMoving = false;
    }

    private void HandleAttack(Entitie target)
    {
        ClearHighlights();
        _currentPath = Pathfinder.FindPathToAttack(CurrentTile, target.CurrentTile, MoveRange, AttackRange);
        if (_currentPath == null) return;

        for (int i = 1; i < _currentPath.Count; i++)
        {
            if (i <= MoveRange)
                MarkHighlight(_currentPath[i], "move");
            else
                MarkHighlight(_currentPath[i], "moveOutOfRange");
        }

        Tile finalTile = _currentPath[^1];
        int pathCost = _currentPath.Count - 1;
        bool canReachFinal = pathCost <= MoveRange;
        HighlightAttackLineFrom(finalTile, target.CurrentTile, canReachFinal);
    }

    private IEnumerator MoveAndAttack(List<Tile> path, Entitie target)
    {
        _isMoving = true;
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 start = transform.position;
            Vector3 end = path[i].transform.position;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * _moveSpeed;
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }
            PlaceOnTile(path[i]);
        }
        ClearHighlights();
        _isMoving = false;

        if (target != null && Pathfinder.InAttackLine(CurrentTile, target.CurrentTile, AttackRange))
        {
            target.RemoveFromTile();
            _currentTarget = null;
        }
    }

    private void MarkHighlight(Tile tile, string highlightType)
    {
        if (tile == null) return;
        int newPr = GetHighlightPriority(highlightType);
        if (_highlightedTiles.TryGetValue(tile, out string existingType))
        {
            int oldPr = GetHighlightPriority(existingType);
            if (newPr < oldPr) return;
            tile.ClearHighlight();
            tile.HighlightTile(highlightType);
            _highlightedTiles[tile] = highlightType;
            return;
        }
        tile.ClearHighlight();
        tile.HighlightTile(highlightType);
        _highlightedTiles.Add(tile, highlightType);
    }

    private int GetHighlightPriority(string type)
    {
        switch (type)
        {
            case "attack": return 4;
            case "attackOutOfRange": return 3;
            case "move": return 2;
            case "moveOutOfRange": return 1;
            default: return 0;
        }
    }

    private void ClearHighlights()
    {
        if (_highlightedTiles != null && _highlightedTiles.Count > 0)
        {
            var keys = new List<Tile>(_highlightedTiles.Keys);
            foreach (Tile t in keys)
                t.ClearHighlight();
            _highlightedTiles.Clear();
        }
        _currentPath = null;
    }

    private void HighlightAttackLineFrom(Tile fromTile, Tile enemyTile, bool canReachFinal)
    {
        if (fromTile == null || enemyTile == null) return;
        int dx = enemyTile.Position.X - fromTile.Position.X;
        int dy = enemyTile.Position.Y - fromTile.Position.Y;
        if (dx != 0 && dy != 0) return;

        int dirX = dx == 0 ? 0 : (dx > 0 ? 1 : -1);
        int dirY = dy == 0 ? 0 : (dy > 0 ? 1 : -1);
        int distanceToEnemy = Mathf.Abs(dx) + Mathf.Abs(dy);
        int stepsInRange = Mathf.Min(distanceToEnemy, AttackRange);

        string type = canReachFinal ? "attack" : "attackOutOfRange";
        Tile current = fromTile;
        for (int step = 1; step <= stepsInRange; step++)
        {
            Tile next = GetNeighbourInDirection(current, dirX, dirY);
            if (next == null) break;
            if (next == enemyTile)
            {
                MarkHighlight(next, type);
                return;
            }
            MarkHighlight(next, type);
            current = next;
        }

        if (distanceToEnemy > AttackRange)
            MarkHighlight(enemyTile, "attackOutOfRange");
    }

    private Tile GetNeighbourInDirection(Tile tile, int dirX, int dirY)
    {
        if (tile == null) return null;
        var neighbours = SystemManager.Instance.MapManager.GetNeighbours(tile);
        if (neighbours == null) return null;
        foreach (Tile n in neighbours)
        {
            int ddx = n.Position.X - tile.Position.X;
            int ddy = n.Position.Y - tile.Position.Y;
            if (ddx == dirX && ddy == dirY)
                return n;
        }
        return null;
    }

    #endregion

    #region Unity-API
    private void Awake()
    {
        SetMoveRange(_defaultMoveRange);
        SetAttackRange(_defaultAttackRange);
    }

    #endregion
}