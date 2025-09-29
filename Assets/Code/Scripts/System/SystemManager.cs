using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance { get; private set; }

    #region References
    public MapManager MapManager { get; private set; }
    public UIEditorMenu Menu { get; private set; }
    #endregion

    #region Variables
    public SystemStage CurrentStage { get; private set; }
    public Player CurrentPlayer { get; private set; }
    public Enemy CurrentEnemy { get; private set; }
    #endregion

    #region Methods
    public void ChangeState(SystemStage newStage)
    {
        CurrentStage = newStage;
    }

    public void SetPlayer(Player newPlayer)
    {
        if (CurrentPlayer != null) CurrentPlayer.DestroyEntitie();
        CurrentPlayer = newPlayer;
    }

    public void ClearPlayer() => CurrentPlayer = null;

    public void SetEnemy(Enemy newEnemy)
    {
        if (CurrentEnemy != null) CurrentEnemy.DestroyEntitie();
        CurrentEnemy = newEnemy;
    }

    public void ClearEnemy() => CurrentEnemy = null;
    #endregion

    #region Unity-API
    private void Awake()
    {
        Instance = this;

        CurrentStage = SystemStage.Generate;

        MapManager = FindFirstObjectByType<MapManager>();
        Menu = FindFirstObjectByType<UIEditorMenu>();
    }
    #endregion
}

public enum SystemStage
{
    Generate,
    Paint,
    Placement,
    Play,
}