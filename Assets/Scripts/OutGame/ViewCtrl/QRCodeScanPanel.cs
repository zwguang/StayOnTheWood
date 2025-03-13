using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class QRCodeScanPanel : UIPopBase
{
    [SerializeField] private RawImage _showImage = null;
    [SerializeField] private Button _quitBtn = null;

    private UnityEngine.WebCamTexture _webCameraTex = null;
    
    private int showImageStartWidth = 0;
    private int showImageStartHeight = 0;
    private float lastWebCamTextureWidth = 0;
    private float lastWebCamTextureHeight = 0;

    private float _lastScreenWidth = 0;
    private float _lastScreenHeight = 0;

    public override void onAwake()
    {
        base.onAwake();
#if UNITY_ANDROID
                Debug.Log($"UNITY_ANDROID");

                Permission.RequestUserPermission(Permission.Camera);
#elif UNITY_IOS || UNITY_WEBGL
    
#endif
        
        _quitBtn.onClick.AddListener(OnQuitBtnClicked);
    }

    // Start is called before the first frame update
    public override void onStart()
    {
        var showImageRect = this._showImage.rectTransform.rect;
        showImageStartWidth = (int)showImageRect.width;
        showImageStartHeight = (int)showImageRect.height;
        Debug.Log($"onStart showImage真实 width = {showImageRect.width}  height = {showImageRect.height}");

        if (showImageStartWidth < showImageStartHeight)
        {
            var temp = showImageStartHeight;
            showImageStartHeight = showImageStartWidth;
            showImageStartWidth = temp;
        }
        
#if UNITY_ANDROID
        var devices = WebCamTexture.devices;
        var deviceName = devices[0].name;
        _webCameraTex = new WebCamTexture(deviceName, showImageStartWidth, showImageStartHeight, 30);
        _showImage.texture = _webCameraTex;
        _webCameraTex.Play();
        Debug.Log($"onStart showImage调整 width = {showImageStartWidth}  height = {showImageStartHeight}");
        Debug.Log($"onStart webCameraTex width = {_webCameraTex.width}  height = {_webCameraTex.height}");

        Debug.Log($"onStart screen width = {Screen.width}  height = {Screen.height}");

        // this.SetShowImageSize();

#endif
    }

    private void SetRawImageCenterAlign()
    {
        var rawImgRectTransform = _showImage.rectTransform;
        rawImgRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rawImgRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rawImgRectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
    
    void SetShowImageSize()
    {
        // float curRatio = this.showImageStartWidth / _webCameraTex.width;
        // float fittedHeight = curRatio * _webCameraTex.height;
        // Debug.Log($"SetShowImageSize 适配camera showImage width = {showImageStartWidth}  height = {fittedHeight}");
        //
        //
        // this.SetRawImageCenterAlign();
        // // this.m_showImage.GetComponent(CS.UnityEngine.RectTransform).sizeDelta = new CS.UnityEngine.Vector2(this.curScreenWidth, Number(fittedHeight));
        //
        // //上述的结果，屏幕上下会有留白，再根据分辨率，等比例缩放下
        // float screenRotio = (float)Screen.width / (float)Screen.height;
        //
        // Debug.Log($"SetShowImageSize this.showImageStartWidth = {this.showImageStartWidth}, screenRotio = {screenRotio}");
        //
        // float adapteHight = this.showImageStartWidth / screenRotio;//749
        // float scale = adapteHight / fittedHeight;
        // float adapterWidth = this.showImageStartWidth * scale;
        // this._showImage.rectTransform.sizeDelta = new Vector2(adapterWidth, adapteHight);
        //
        // Debug.Log($"SetShowImageSize 适配屏幕 adapter width = {adapterWidth}  height = {adapteHight}");
        // Debug.Log($"SetShowImageSize 适配屏幕 showImage width = {_showImage.rectTransform.rect.width}  height = {_showImage.rectTransform.rect.height}");
        // Debug.Log($"SetShowImageSize webCameraTex width = {_webCameraTex.width}  height = {_webCameraTex.height}");
        //
        // Debug.Log($"SetShowImageSize screen width = {(int)Screen.width}  height = {(int)Screen.height}");

        float aspect = (float)_webCameraTex.width / (float)_webCameraTex.height;
        var rect = _showImage.rectTransform.rect;
        var height = 750;// rect.height;
        var width = rect.height * aspect;
        _showImage.rectTransform.sizeDelta = new Vector2(width, height);
        
        Debug.Log($"_webCameraTex width = {_webCameraTex.width}, height = {_webCameraTex.height}");
        Debug.Log($"_showImage width = {width}, height = {height}");

    }
    
    // Update is called once per frame
    void Update()
    {
        if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight) {
            Debug.Log("------------------------发生折叠---------------------------");
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            SetShowImageSize();
        }
        var scaleY = _webCameraTex.videoVerticallyMirrored ? -1 : 1;
        _showImage.transform.localScale = new UnityEngine.Vector3(1,scaleY,1);
        var orient = -_webCameraTex.videoRotationAngle;
        _showImage.transform.localEulerAngles = new Vector3(0, 0, orient);
    }

    void OnQuitBtnClicked()
    {
        UIManager.Instance.HidePanel(ResPath.prefabPath_QRCodeScan);
    }
}
