using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class HallRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIStartPanel.OpenUIPanel();
    }

    private void OnDestroy()
    {
        // UIManager.Instance.ClearAllWithoutResident();
    }

    // Update is called once per frame
    void Update()
    {
    }
}