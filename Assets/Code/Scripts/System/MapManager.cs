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

    }

    public Tile GetTile(int x, int y)
    {
        if (_tiles == null) return null;
        if (x < 0 || y < 0 || x >= width || y >= height) return null;

        return _tiles[x, y];
    }
    #endregion
}