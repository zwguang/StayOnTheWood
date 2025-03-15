using GDK;

public class UITipsBase : UIBase
{
    public override void onAwake()
    {
        base.onAwake();
        this.layer = UILayer.Tips;
    }
}