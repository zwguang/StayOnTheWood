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
    [SerializeField] private Image _tucengImage = null;
    [SerializeField] private Button _quitBtn = null;
    [SerializeField] private Canvas _canvas;

    private UnityEngine.WebCamTexture _webCameraTex = null;
    
    private int showImageStartWidth = 0;
    private int showImageStartHeight = 0;

    private float _lastScreenWidth = 0;
    private float _lastScreenHeight = 0;

    private float _lastTucengWidth = 0;
    private float _lastTucengHeight = 0;

    private float _count = 0;
    
    public override void onAwake()
    {
        base.onAwake();
#if UNITY_ANDROID
                Debug.Log($"UNITY_ANDROID");

                Permission.RequestUserPermission(Permission.Camera);
#elif UNITY_IOS || UNITY_WEBGL
    
#endif
        
        _quitBtn.onClick.AddListener(OnQuitBtnClicked);

        _canvas = UIRoot.Instance.canvas;
    }

    // Start is called before the first frame update
    public override void onStart()
    {
        var showImageRect = this._tucengImage.rectTransform.rect;
        showImageStartWidth = (int)showImageRect.width;
        showImageStartHeight = (int)showImageRect.height;
        if (showImageStartWidth < showImageStartHeight)
        {
            var temp = showImageStartHeight;
            showImageStartHeight = showImageStartWidth;
            showImageStartWidth = temp;
        }
        
#if UNITY_ANDROID && !UNITY_EDITOR
        var devices = WebCamTexture.devices;
        var deviceName = devices[0].name;
        //Unity 会尝试请求该分辨率，但最终会自动降级到设备支持的最接近的分辨率。
        _webCameraTex = new WebCamTexture(deviceName, showImageStartWidth, showImageStartHeight, 30);
        _showImage.texture = _webCameraTex;
        _webCameraTex.Play();
        Debug.Log($"onStart webCameraTex width = {_webCameraTex.width}  height = {_webCameraTex.height}");

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

        var tucengRect = _tucengImage.rectTransform.rect;
        _lastTucengWidth = tucengRect.width;
        _lastTucengHeight = tucengRect.height;
        Debug.Log($"_cutengImage width = {tucengRect.width}, height = {tucengRect.height}");

        this.onStart();

        //根本做不到完全适配，_webCameraTex的分辨率跟设置的不是等比的
        if (_webCameraTex)
        {
            // 按高适配, 大屏左右适配不够
            // float aspect = (float)_webCameraTex.width / (float)_webCameraTex.height;
            // float height = tucengRect.height;
            // var width = height * aspect;
            // _showImage.rectTransform.sizeDelta = new Vector2(width, tucengRect.height);
            
            //大屏时，上下超出遮罩
            // _showImage.rectTransform.sizeDelta = new Vector2(_webCameraTex.width, _webCameraTex.height);

            //稍微有点拉伸
            _showImage.rectTransform.sizeDelta = new Vector2(tucengRect.width, tucengRect.height);
            var rect = _showImage.rectTransform.rect;
            rect.size = new Vector2(1, 1);

            //根本无法完全适配，比例不一样，上下或者左右总会不对
            // float cameraRatio = (float)_webCameraTex.width / (float)_webCameraTex.height;
            // float tucengRatio = tucengRect.width/ tucengRect.height;
            // if (cameraRatio > tucengRatio)
            // {
            //     _showImage.rectTransform.sizeDelta = new Vector2(tucengRect.height*cameraRatio, tucengRect.height);
            // }
            // else
            // {
            //     _showImage.rectTransform.sizeDelta = new Vector2(tucengRect.width, tucengRect.width/cameraRatio);
            // }
            //
            // Debug.Log($"_webCameraTex width = {_webCameraTex.width}, height = {_webCameraTex.height}");
            // Debug.Log($"_showImage width = {_showImage.rectTransform.rect.width}, height = {_showImage.rectTransform.rect.height}");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight) {
            Debug.Log("------------------------发生折叠---------------------------");
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            
            Canvas.ForceUpdateCanvases();
            
            SetShowImageSize();
            
            _count = 0;
        }

        if (_webCameraTex)
        {
            var scaleY = _webCameraTex.videoVerticallyMirrored ? -1 : 1;
            _showImage.transform.localScale = new UnityEngine.Vector3(1,scaleY,1);
            var orient = -_webCameraTex.videoRotationAngle;
            _showImage.transform.localEulerAngles = new Vector3(0, 0, orient);
        }
        
    }

    private void LateUpdate()
    {
        // //update中，image平铺还没生效
        // if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight) {
        //     Debug.Log("------------------------发生折叠---------------------------");
        //     _lastScreenWidth = Screen.width;
        //     _lastScreenHeight = Screen.height;
        //     _count = 0;
        // }
        //
        // var tucengRect = _tucengImage.rectTransform.rect;
        // if (tucengRect.width != _lastTucengWidth || tucengRect.height != _lastTucengHeight)
        // {
        //     Debug.Log($"---------------------LateUpdate------------------");
        //     Debug.Log($"_cutengImage width = {tucengRect.width}, height = {tucengRect.height}, count = {_count}");
        //     _lastTucengWidth = tucengRect.width;
        //     _lastTucengHeight = tucengRect.height;
        //     
        //     SetShowImageSize();
        // }
        // else
        // {
        //     _count++;
        // }
    }

    void OnQuitBtnClicked()
    {
        SystemUIManager.Instance.HidePanel(ResPath.prefabPath_QRCodeScan);
    }
}
