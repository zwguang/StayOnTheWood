using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private static UIRoot m_instance = null;

    public static UIRoot Instance
    {
        get { return m_instance; }
    }

    public Transform bottomLayer = null;
    public Transform menuLayer = null;
    public Transform popLayer = null;
    public Transform tipsLayer = null;
    public Transform dialogLayer = null;
    public Transform guideLayer = null;
    public Canvas canvas = null;

    private UIRoot()
    {
    }

    private void Awake()
    {
        m_instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}