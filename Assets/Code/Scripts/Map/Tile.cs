using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public class Tile : MonoBehaviour
{
    #region SerializeFields
    [Header("Materials")]
    [SerializeField] private Material _traversableMat;
    [SerializeField] private Material _obstacleMat;
    [SerializeField] private Material _coverMat;
    [Space(10)]
    [Header("Highlight")]
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Material _hlMoveMat;
    [SerializeField] private Material _hlMoveOffRangeMat;
    [SerializeField] private Material _hlAttackMat;
    [SerializeField] private Material _hlAttackOffRangeMat;
    #endregion

    #region Variables
    public TileType Type {  get; private set; }
    public (int X, int Y) Position { get; private set; }
    private Renderer _render;

    public Entitie CurrentEntitie { get; private set; }
    public bool IsOccupied => CurrentEntitie != null;
    #endregion

    #region Methods

    public void Initialize((int x, int y) position, TileType type = TileType.Traversable)
    {
        _render = GetComponent<Renderer>();

        Position = position;
        SetType(type);
    }

    public void SetType(TileType type)
    {
        Type = type;
        _render.sharedMaterial = type switch
        {
            TileType.Traversable => _traversableMat,
            TileType.Obstacle => _obstacleMat,
            TileType.Cover => _coverMat,
            _ => _traversableMat
        };
    }

    public void PlaceEntitie(Entitie unit)
    {
        CurrentEntitie = unit;
        ClearHighlight();
    }

    public void ClearPosition()
    {
        CurrentEntitie = null;
    }

    public void HighlightTile(string hlType)
    {
        _highlight.SetActive(true);
        _highlight.GetComponent<Renderer>().sharedMaterial = hlType switch
        {
            "move" => _hlMoveMat,
            "moveOutOfRange" => _hlMoveOffRangeMat,
            "attack" => _hlAttackMat,
            "attackOutOfRange" => _hlAttackOffRangeMat,
            _ => _hlMoveMat
        };
    }

    public void ClearHighlight()
    {
        _highlight.SetActive(false);
    }

    #endregion
}

public enum TileType
{
    Traversable = 0,
    Obstacle = 1,
    Cover = 2,
}