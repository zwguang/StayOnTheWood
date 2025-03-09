using GDK;

public class UIPopBase : UIPopupAnimBase
{
    public override void onAwake()
    {
        base.onAwake();
        this.layer = UILayer.Pop;
    }
}