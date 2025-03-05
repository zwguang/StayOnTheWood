using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlOperiationPanel : ViewCtrlBase
{
    [SerializeField] private Button m_upBtn;
    [SerializeField] private Button m_downBtn;
    [SerializeField] private Button m_leftBtn;
    [SerializeField] private Button m_rightBtn;
    [SerializeField] private Button m_jumpBtn;

    public override void onAwake()
    {
        OnButtonClick(m_upBtn, () => { this.OnBtnClicked(PlayerDirType.Up); });
        OnButtonClick(m_downBtn, () => { this.OnBtnClicked(PlayerDirType.Down); });
        OnButtonClick(m_leftBtn, () => { this.OnBtnClicked(PlayerDirType.Left); });
        OnButtonClick(m_rightBtn, () => { this.OnBtnClicked(PlayerDirType.Right); });
        OnButtonClick(m_jumpBtn, () => { this.OnJumpBtnClicked(); });
    }

    void OnBtnClicked(PlayerDirType type)
    {
        EventManager.Instance.Trigger((int)EventID.PlayerMove, type);
    }

    void OnJumpBtnClicked()
    {
        EventManager.Instance.Trigger((int)(EventID.JumpBtnClicked));
    }
}