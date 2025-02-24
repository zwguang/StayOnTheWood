using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        private float m_timeCount = 0;
        private float m_addSpeed = 0.1f;
        
        public float StartSpeed = 3; //木头x间距/初始速度
        public float woodGapY = 2.5f;//木头Y间距

        public float speed;
        //得分批次
        public int soreMaxBatch = 0;
        public int soreNum = 0;

        public bool GameOver = false;
        
        public void OnStart()
        {
            
        }
        
        // Update is called once per frame
        public void Update()
        {
            m_timeCount += Time.deltaTime;
            if (m_timeCount > 1)
            {
                m_timeCount -= 1;
                AddSpeed();
            }
        }

        public void OnInit()
        {
            speed = StartSpeed;

            this.soreMaxBatch = 0;
            this.soreNum = 0;
            m_timeCount = 0;
            GameOver = false;
        }
        
        public void PlayerDeath()
        {
            Time.timeScale = 0;
        }

        public void Clear()
        {
            Time.timeScale = 1;
        }

        public void AddSpeed()
        {
            speed += m_addSpeed;
            Debug.Log($"StyOnTheWood 水流速度：{speed}");
        }
    }
}

