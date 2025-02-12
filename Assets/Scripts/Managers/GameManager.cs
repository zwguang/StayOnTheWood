using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public float speed = L.WoodStartSpeed *2;

        /// <summary>
        /// 玩家当前是在哪个wood上
        /// </summary>
        public WoodType selectedWoodType = WoodType.Invalid;
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

