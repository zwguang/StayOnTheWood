using GDK;

public class UIDialogBase : UIPopupAnimBase
{
    public override void onAwake()
    {
        base.onAwake();
        this.layer = UILayer.Dialog;
    }
}