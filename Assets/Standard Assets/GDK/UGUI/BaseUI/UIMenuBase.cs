using GDK;

public class UIMenuBase : UIBase
{
    public override void onAwake()
    {
        this.layer = UILayer.Menu;
    }
}