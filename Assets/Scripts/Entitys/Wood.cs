using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Game.Monos;
using GDK;
using UnityEngine;

namespace Game
{
    public class Wood : MonoBehaviour
    {
        private Vector3 m_localPos;
        public WoodType woodType = WoodType.Invalid;
        public Vector3 LocalPos
        {
            get => m_localPos;
            set
            {
                m_localPos = value;
            }
        }

        public float GetSpeed()
        {
            return GameManager.Instance.speed;
        }
        
        public void Init(WoodType type)
        {
            this.gameObject.SetActive(true);

            float startPosY = 0;
            switch (type)
            {
                case WoodType.Up:
                {
                    startPosY = 2.5f;
                    break;
                }
                case WoodType.Mid:
                {
                    startPosY = 0;
                    break;
                }
                case WoodType.Down:
                {
                    startPosY = -2.5f;
                    break;
                }
            }
            this.SetStartPos(new Vector3(L.WoodStartPosX, 0, 0));
        }
        
        public void Update()
        {
            m_localPos.x -= this.GetSpeed() * Time.deltaTime;
            this.transform.localPosition = m_localPos;
            if (m_localPos.x <= -10)
            {
                this.Recycle();
            }
        }

        private void Recycle()
        {
            WoodManager.Instance.Recycle(this);
        }

        public void SetStartPos(Vector3 pos)
        {
            this.m_localPos = pos;
            this.transform.localPosition = m_localPos;
        }
        
        // Update is called once per frame
        // void Update()
        // {
        //     if (transform.localPosition.x <= -10)
        //     {
        //         Destroy(this.gameObject);
        //         return;
        //     }
        //
        //     transform.localPosition -= new Vector3(m_speed * Time.deltaTime, 0, 0);
        // }

        // private void OnCollisionEnter2D(Collision2D other)
        // {
        //     if (other.gameObject.CompareTag("Player"))
        //     {
        //         EventManager.Instance.Trigger((int)E.PlayerScore);
        //     }
        // }
        //
        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (other.gameObject.CompareTag("Player"))
        //     {
        //         EventManager.Instance.Trigger((int)E.PlayerScore);
        //     }
        // }
    }


}
