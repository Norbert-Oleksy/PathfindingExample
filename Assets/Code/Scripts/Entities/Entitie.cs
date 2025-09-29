using UnityEngine;

public class Entitie : MonoBehaviour
{
    public Tile CurrentTile { get; private set; }
    public int MoveRange { get; private set; }
    public int AttackRange { get; private set; }

    public virtual void PlaceOnTile(Tile tile)
    {
        if (tile == null) return;

        if (tile.IsOccupied && tile.CurrentEntitie != this)
        {
            Debug.LogWarning("Tile is already occupied");
            return;
        }

        if (CurrentTile != null && CurrentTile != tile)
            CurrentTile.ClearPosition();

        CurrentTile = tile;
        tile.PlaceEntitie(this);

        Vector3 pos = tile.transform.position;
        pos.y = transform.position.y;
        transform.position = pos;
    }

    public virtual void RemoveFromTile()
    {
        if (CurrentTile != null)
        {
            CurrentTile.ClearPosition();
            CurrentTile = null;
        }
        DestroyEntitie();
    }

    public void SetMoveRange(int r) => MoveRange = Mathf.Max(0, r);
    public void SetAttackRange(int r) => AttackRange = Mathf.Max(0, r);

    public virtual void DestroyEntitie()
    {
        Destroy(gameObject);
    }
}