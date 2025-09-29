using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entitie
{
    #region SerializeFields
    [SerializeField] private float _moveSpeed = 5f;
    #endregion

    #region Variables
    private List<Tile> _currentPath;
    private bool _isMoving = false;
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
        if(targetTile.Type == TileType.Obstacle || targetTile.Type == TileType.Cover) return;


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

        _currentPath = Pathfinder.FindPath(CurrentTile, targetTile);

        if (_currentPath == null)
        {
            Debug.LogWarning("No path found");
            return;
        }

        for (int i = 1; i < _currentPath.Count; i++)
        {
            if (i <= MoveRange)
                _currentPath[i].HighlightTile("move");
            else
                _currentPath[i].HighlightTile("moveOutOfRange");
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

    private void ClearHighlights()
    {
        if (_currentPath == null) return;
        foreach (Tile tile in _currentPath)
            tile.ClearHighlight();
        _currentPath = null;
    }

    #endregion
}