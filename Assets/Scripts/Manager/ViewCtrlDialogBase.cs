public class ViewCtrlDialogBase : ViewCtrlPopupAnimBase
{
    public override void onAwake()
    {
        base.onAwake();
        this.layer = UILayer.Dialog;
    }
}