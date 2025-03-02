using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


enum JoyStickType
{
    Fixed,
    Floating
}

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform _dotRt;
    [SerializeField] RectTransform _dotParentRt;

    RectTransform _joystickRt;
    JoyStickType _type;
    Vector2 _fixedPos;
    Camera _uiCamera;
    private Canvas _canvas;

    public Vector2 input = Vector2.zero;

    //限制摇杆方向，只有上下左右
    bool _bJoystickLimitDir = true;

    void Awake()
    {
        _joystickRt = GetComponent<RectTransform>();
        // _type = _bJoystickLimitDir? JoyStickType.Fixed: JoyStickType.Floating;
        _type = JoyStickType.Fixed;
        _fixedPos = _dotParentRt.anchoredPosition;

        _canvas = UIRoot.Instance.canvas;
        _uiCamera = _canvas.worldCamera;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_type != JoyStickType.Fixed)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickRt, eventData.position, _uiCamera,
                out pos);

            _dotParentRt.anchoredPosition =
                pos; //ConvertUtils.ScreenPoint2UILocalPoint(_joystickRt, eventData.position);
        }

        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // var canvasGo = GameObject.Find("GameRoot/UIRoot");
        // var canvas = GameObject.Find("GameBoot/UIRoot").GetComponent<Canvas>();//UIManager.Instance.canvasRoot;

        Vector2 position = _uiCamera.WorldToScreenPoint(_dotParentRt.position); //将ui坐标中的background映射到屏幕中的实际坐标
        Vector2 radius = _dotParentRt.sizeDelta / 2;
        input = (eventData.position - position) / (radius * _canvas.scaleFactor); //将屏幕中的触点和background的距离映射到ui空间下实际的距离
        HandleInput(input.magnitude, input.normalized, radius); //对输入进行限制

        var temp = input;
        EventManager.Instance.Trigger((int)EventID.JoyStickDroging, FixInput(temp));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        _dotRt.anchoredPosition = Vector2.zero;
        _dotParentRt.anchoredPosition = _fixedPos;
        EventManager.Instance.Trigger((int)EventID.JoyStickDrogUp);
    }

    public void HandleInput(float magnitude, Vector2 normalised, Vector2 radius)
    {
        if (magnitude > 0)
        {
            if (magnitude > 1)
                input = normalised;
        }
        else
            input = Vector2.zero;

        //_dotRt的位置
        _dotRt.anchoredPosition = input * radius;

        if (_bJoystickLimitDir)
        {
            input = FixInput(input);
        }
    }

    Vector2 FixInput(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            if (input.x >= 0)
            {
                input = Vector2.right;
            }
            else
            {
                input = Vector2.left;
            }
        }
        else
        {
            if (input.y >= 0)
            {
                input = Vector2.up;
            }
            else
            {
                input = Vector2.down;
            }
        }

        return input;
    }
}