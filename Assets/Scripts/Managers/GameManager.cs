using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public float Speed = L.WoodStartSpeed *2;
        // Start is called before the first frame update
        void Start()
        {
            // WoodManager.Instance.Init();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public void Init(){}
    }
}

