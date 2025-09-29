
public class Player : Entitie
{
    public override void DestroyEntitie()
    {
        SystemManager.Instance.ClearPlayer();
        base.DestroyEntitie();
    }
}