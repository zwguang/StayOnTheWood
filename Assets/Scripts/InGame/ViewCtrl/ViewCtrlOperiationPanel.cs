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

    // Start is called before the first frame update
    void Start()
    {
        BindEvent(m_upBtn, () => {this.OnBtnClicked(PlayerDirType.Up);});
        BindEvent(m_downBtn, () => {this.OnBtnClicked(PlayerDirType.Down);});
        BindEvent(m_leftBtn, () => {this.OnBtnClicked(PlayerDirType.Left);});
        BindEvent(m_rightBtn, () => {this.OnBtnClicked(PlayerDirType.Right);});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBtnClicked(PlayerDirType type)
    {
        EventManager.Instance.Trigger((int)E.PlayerMove, type);
    }
}
