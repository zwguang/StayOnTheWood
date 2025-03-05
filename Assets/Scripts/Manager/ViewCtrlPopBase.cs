public class ViewCtrlPopBase : ViewCtrlPopupAnimBase
{
    public override void onAwake()
    {
        base.onAwake();
        this.layer = UILayer.Pop;
    }
}