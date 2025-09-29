using TMPro;
using UnityEngine;

public class UIPlacementSection : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [Space(10)]
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private TMP_InputField _moveInput;
    [SerializeField] private TMP_InputField _attackInput;
    #endregion

    #region Variables
    private Entitie _currentEntitie;
    #endregion

    #region Methods
    public void SpawnEntitie(Tile targetTile)
    {
        if(_currentEntitie == null)
            return;

        if(targetTile.Type != TileType.Traversable)
        {
            Debug.LogWarning("Entitie can't be place on tile other than a Traversable type");
            return;
        }

        int.TryParse(_moveInput.text, out int moveRange);
        int.TryParse(_attackInput.text, out int attackRange);
        if (moveRange <= 0 || attackRange <= 0)
        {
            Debug.LogWarning("moveRange and attackRange must be greater then 0");
            return;
        }

        if (targetTile.IsOccupied)
            targetTile.CurrentEntitie.DestroyEntitie();


        Entitie newEntitie = Instantiate(_currentEntitie);

        newEntitie.SetMoveRange(moveRange);
        newEntitie.SetAttackRange(attackRange);

        newEntitie.PlaceOnTile(targetTile);

        if (newEntitie is Player player)
            SystemManager.Instance.SetPlayer(player);
        else if (newEntitie is Enemy enemy)
            SystemManager.Instance.SetEnemy(enemy);

        _currentEntitie = null;
        _infoText.text = "";
    }

    public void PreparePlayerToSpawn()
    {
        _currentEntitie = _playerPrefab.GetComponent<Entitie>();
        _infoText.text = "Player";
    }

    public void PrepareEnemyToSpawn()
    {
        _currentEntitie = _enemyPrefab.GetComponent<Entitie>();
        _infoText.text = "Enemy";
    }

    #endregion

    #region Unity-API
    private void OnEnable()
    {
        _currentEntitie = null;
        _infoText.text = "";
    }
    #endregion
}