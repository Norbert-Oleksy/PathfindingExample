using UnityEngine;
using UnityEngine.InputSystem;

public class MousePointer : MonoBehaviour
{
    #region Veriables
    private Camera _mainCamera;
    #endregion

    #region Methods
    private void HandleClick()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Tile clickedTile = hit.collider.GetComponent<Tile>();
            if (clickedTile != null)
            {
                HandleTile(clickedTile);
            }
        }
    }

    private void HandleTile(Tile tile)
    {
        if(SystemManager.Instance.CurrentStage == SystemStage.Paint)
            SystemManager.Instance.Menu.PaintSection.PaintTile(tile);

        if (SystemManager.Instance.CurrentStage == SystemStage.Placement)
            SystemManager.Instance.Menu.PlacementSection.SpawnEntitie(tile);
    }
    #endregion

    #region Unity-API
    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
            HandleClick();
    }
    #endregion
}