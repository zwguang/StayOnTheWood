using System;
using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public Wood wood = null;
        private void Awake()
        {
            // EventManager.Instance.On();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            transform.localPosition -= new Vector3(GameManager.Instance.Speed * Time.deltaTime, 0, 0);
        }

        public void Move(KeyCode type)
        {
            Vector3 moveDir = Vector2.zero;
            switch (type)
            {
                case KeyCode.A:
                {
                    moveDir = new Vector2(-L.WoodStartSpeed, 0);
                    break;
                }
                case KeyCode.D:
                {
                    moveDir = new Vector2(L.WoodStartSpeed, 0);
                    break;
                }
                case KeyCode.W:
                {
                    moveDir = new Vector2(0, L.WoodStartSpeed);
                    break;
                }
                case KeyCode.S:
                {
                    moveDir = new Vector2(0, -L.WoodStartSpeed);
                    break;
                }
            }

            // transform.localPosition += moveDir;
            transform.Translate(moveDir);
        }
    }

}
