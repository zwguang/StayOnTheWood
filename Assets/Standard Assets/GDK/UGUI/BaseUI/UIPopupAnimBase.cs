using UnityEngine;
using UnityEngine.UI;

public class UIPopupAnimBase : UIBase
{
    public bool bNeekMask = true;

    public override void SetMask()
    {
        base.SetMask();
        if (bNeekMask)
        {
            var image = GetComponent<Image>();
            if (!image)
            {
                image = gameObject.AddComponent<Image>();
            }

            image.color = new Color(0.0f, 0.0f, 0.0f, 0.75f);
        }
    }
}