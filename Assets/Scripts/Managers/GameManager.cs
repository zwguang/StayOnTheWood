using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public float speed = L.WoodStartSpeed * 2;
        //得分批次
        public int soreMaxBatch = 0;
        public int soreNum = 0;

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

        public void PlayerDeath()
        {
            Time.timeScale = 0;
        }

        public void Clear()
        {
            this.soreMaxBatch = 0;
            this.soreNum = 0;
        }

        public void Restart()
        {
            this.Clear();
            Time.timeScale = 1;
        }
    }
}

