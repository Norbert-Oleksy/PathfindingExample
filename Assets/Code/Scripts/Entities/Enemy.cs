
public class Enemy : Entitie
{
    public override void DestroyEntitie()
    {
        SystemManager.Instance.ClearEnemy();
        base.DestroyEntitie();
    }
}