using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ViewCtrlBaseUI : MonoBehaviour
{
    public void BindEvent(Button btn, UnityAction onClicked, AudioType type = AudioType.common_click)
        {
            if (btn == null)
            {
                Debug.LogError("btn 为 null");
                return;
            }
            btn.onClick.AddListener(() =>
            {
                //播放音效
                if (type != AudioType.none)
                {
                    // audioAdapter.PlayAudio(type);
                }
                
                onClicked();
            });
            
        }
}
