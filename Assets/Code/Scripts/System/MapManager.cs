using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private Transform _gridParent;
    #endregion

    #region Variables
    private Tile[,] _tiles;
    private int width;
    private int height;

    public bool MapGenerated => _gridParent.childCount > 0;
    #endregion

    #region Methods
    public void GenerateMap(int w , int h)
    {
        width = Mathf.Max(1, w);
        height = Mathf.Max(1, h);

        ClearMap();

        _gridParent.localPosition = Vector3.zero;

        _tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var go = Instantiate(_tilePrefab, _gridParent);
                go.name = $"Tile_{x}_{y}";
                go.transform.localPosition = new Vector3(x , 0f, y);

                Tile tile = go.GetComponent<Tile>();
                tile.Initialize((x, y));

                _tiles[x, y] = tile;
            }
        }

        float offsetX = -(width - 1) / 2f;
        float offsetZ = -(height - 1) / 2f;
        _gridParent.localPosition = new Vector3(offsetX, 0f, offsetZ);
    }

    private void ClearMap()
    {
        if (!MapGenerated || _tiles == null || _tiles.Length == 0) return;

        _tiles = null;

        foreach (Transform child in _gridParent)
        {
            Destroy(child.gameObject);
        }

        SystemManager.Instance.CurrentPlayer?.DestroyEntitie();
        SystemManager.Instance.CurrentEnemy?.DestroyEntitie();
    }

    public Tile GetTile(int x, int y)
    {
        if (_tiles == null) return null;
        if (x < 0 || y < 0 || x >= width || y >= height) return null;

        return _tiles[x, y];
    }

    public Tile GetTileAtWorldPosition(Vector3 worldPosition)
    {
        if (_gridParent == null || _tiles == null) return null;

        Vector3 local = _gridParent.InverseTransformPoint(worldPosition);
        int x = Mathf.RoundToInt(local.x);
        int y = Mathf.RoundToInt(local.z);
        return GetTile(x, y);
    }

    public List<Tile> GetNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();
        (int x, int y) = tile.Position;

        if (y + 1 < _tiles.GetLength(1))
            neighbours.Add(_tiles[x, y + 1]);

        if (y - 1 >= 0)
            neighbours.Add(_tiles[x, y - 1]);

        if (x + 1 < _tiles.GetLength(0))
            neighbours.Add(_tiles[x + 1, y]);

        if (x - 1 >= 0)
            neighbours.Add(_tiles[x - 1, y]);

        return neighbours;
    }

    #endregion
}