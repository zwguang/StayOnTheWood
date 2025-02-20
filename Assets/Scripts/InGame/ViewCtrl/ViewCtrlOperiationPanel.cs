using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlOperiationPanel : ViewCtrlBaseUI
{
    [SerializeField] private Button m_upBtn;
    [SerializeField] private Button m_downBtn;
    [SerializeField] private Button m_leftBtn;
    [SerializeField] private Button m_rightBtn;
    [SerializeField] private Button m_jumpBtn;

    // Start is called before the first frame update
    void Start()
    {
        BindEvent(m_upBtn, () => {this.OnBtnClicked(PlayerDirType.Up);});
        BindEvent(m_downBtn, () => {this.OnBtnClicked(PlayerDirType.Down);});
        BindEvent(m_leftBtn, () => {this.OnBtnClicked(PlayerDirType.Left);});
        BindEvent(m_rightBtn, () => {this.OnBtnClicked(PlayerDirType.Right);});
        BindEvent(m_jumpBtn, () => {this.OnJumpBtnClicked();});
    }

    // Update is called once per frame
    void Update()
    {
        
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
