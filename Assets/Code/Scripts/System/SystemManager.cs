using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance { get; private set; }

    #region References
    public MapManager MapManager { get; private set; }
    public UIEditorMenu Menu { get; private set; }
    #endregion

    #region Unity-API
    private void Awake()
    {
        Instance = this;

        MapManager = FindFirstObjectByType<MapManager>();
        Menu = FindFirstObjectByType<UIEditorMenu>();
    }
    #endregion
}