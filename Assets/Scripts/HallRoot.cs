using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class HallRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.ShowPanel(ResPath.prefabPath_StartPanel);
    }

    // Update is called once per frame
    void Update()
    {
    }
}