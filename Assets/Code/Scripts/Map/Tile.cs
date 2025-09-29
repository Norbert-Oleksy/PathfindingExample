using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public class Tile : MonoBehaviour
{
    #region SerializeFields
    [Header("Materials")]
    [SerializeField] private Material _traversableMat;
    [SerializeField] private Material _obstacleMat;
    [SerializeField] private Material _coverMat;
    #endregion

    #region Variables
    public TileType Type {  get; private set; }
    public (int X, int Y) Position { get; private set; }
    private Renderer _render;
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

    #endregion
}

public enum TileType
{
    Traversable = 0,
    Obstacle = 1,
    Cover = 2,
}