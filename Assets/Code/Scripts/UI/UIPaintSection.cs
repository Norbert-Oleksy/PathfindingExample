using TMPro;
using UnityEngine;

public class UIPaintSection : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private TextMeshProUGUI _infoText;

    #endregion

    #region Variables
    public TileType CurrentType;

    #endregion

    #region Methods
    public void ChagneType(int newType)
    {
        CurrentType = (TileType)newType;
        _infoText.text = CurrentType.ToString();
    }

    public void PaintTile(Tile tile)
    {        
        if (tile.IsOccupied && CurrentType != TileType.Traversable)
            tile.CurrentEntitie.DestroyEntitie();

        tile.SetType(CurrentType);
    }
    #endregion

    #region Unity-API
    private void OnEnable()
    {
        CurrentType = TileType.Traversable;
        _infoText.text = CurrentType.ToString();
    }
    #endregion
}