using GDK;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Canvas canvasRoot;

    public void Init()
    {
        canvasRoot = GameObject.Find("GameRoot/UIRoot").GetComponent<Canvas>();

    }
}