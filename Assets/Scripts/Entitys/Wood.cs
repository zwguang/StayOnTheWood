using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Game.Monos;
using GDK;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class Wood : MonoBehaviour
    {
        private SpriteRenderer m_sprite;
        
        private Vector3 m_localPos;
        public WoodType woodType = WoodType.Invalid;
        [FormerlySerializedAs("batchIndex")] public int batch = 0;
        public Vector3 LocalPos
        {
            get => m_localPos;
            set
            {
                m_localPos = value;
            }
        }

        private void Awake()
        {
            m_sprite = this.GetComponent<SpriteRenderer>();
        }

        public float GetSpeed()
        {
            return GameManager.Instance.speed;
        }
        
        public void Init(WoodType type, int batch)
        {
            this.gameObject.SetActive(true);

            float startPosY = 0;
            switch (type)
            {
                case WoodType.Up:
                {
                    startPosY = 2.5f;
                    m_sprite.color = Color.white;
                    this.gameObject.tag = "Wood";
                    break;
                }
                case WoodType.Mid:
                {
                    startPosY = 0;
                    m_sprite.color = Color.white;
                    this.gameObject.tag = "Wood";

                    break;
                }
                case WoodType.Down:
                {
                    startPosY = -2.5f;
                    m_sprite.color = Color.white;
                    this.gameObject.tag = "Wood";

                    break;
                }
                case WoodType.EmptyUp:
                {
                    startPosY = 2.5f;
                    m_sprite.color = new Color(1, 1, 1, 0.2f);
                    this.gameObject.tag = "River";
                    break;
                }
                case WoodType.EmptyMid:
                {
                    startPosY = 0;
                    m_sprite.color = new Color(1, 1, 1, 0.2f);
                    this.gameObject.tag = "River";
                    break;
                }
                case WoodType.EmptyDown:
                {
                    startPosY = -2.5f;
                    m_sprite.color = new Color(1, 1, 1, 0.2f);
                    this.gameObject.tag = "River";
                    break;
                }
            }

            this.batch = batch;
            this.SetStartPos(new Vector3(L.WoodStartPosX, startPosY, 0));
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
