using Game;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupAnimBase : UIBase
{
    public override void SetMask()
    {
        base.SetMask();
        var image = GetComponent<Image>();
        if (!image)
        {
            image = gameObject.AddComponent<Image>();
        }

        image.color = new Color(0.0f, 0.0f, 0.0f, L.MaskAlpha);
    }
}