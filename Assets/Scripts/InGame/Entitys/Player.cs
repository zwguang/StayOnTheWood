using System;
using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Game
{
    public class Player : MonoBehaviour
    {
        private bool m_bDeath = false;

        private void Awake()
        {
            // EventManager.Instance.On();
            Init();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Init()
        {
            m_bDeath = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_bDeath)
            {
                return;
            }

            transform.localPosition -= new Vector3(GameManager.Instance.speed * Time.deltaTime, 0, 0);
            if (transform.localPosition.x < -11 || transform.localPosition.x > 11 ||
                transform.localPosition.y < -4 || transform.localPosition.y > 4)
            {
                GameOver();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (m_bDeath)
            {
                return;
            }

            if (other.gameObject.CompareTag("Wood"))
            {
                Wood wood = other.gameObject.GetComponent<Wood>();
                if (wood.batch > GameManager.Instance.soreMaxBatch)
                {
                    GameManager.Instance.soreMaxBatch = wood.batch;
                    GameManager.Instance.soreNum++;
                    SystemEventManager.Instance.Trigger((int)EventID.PlayerScore);
                    this.transform.localPosition = wood.transform.localPosition;
                }
            }
            else if (other.gameObject.CompareTag("River"))
            {
                this.GameOver();
            }
        }

        private void GameOver()
        {
            m_bDeath = true;
            GameManager.Instance.PlayerDeath();
            SystemEventManager.Instance.Trigger((int)EventID.PlayerDeath);
        }

        public void SetDir(PlayerDirType type)
        {
            var rotationZ = 0;
            switch (type)
            {
                case PlayerDirType.Left:
                {
                    rotationZ = 180;
                    break;
                }
                case PlayerDirType.Right:
                {
                    rotationZ = 0;
                    break;
                }
                case PlayerDirType.Up:
                {
                    rotationZ = 90;
                    break;
                }
                case PlayerDirType.Down:
                {
                    rotationZ = 270;
                    break;
                }
            }

            this.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
        }

        public void Move(KeyCode type)
        {
            switch (type)
            {
                case KeyCode.A:
                {
                    this.Move(PlayerDirType.Left);
                    break;
                }
                case KeyCode.D:
                {
                    this.Move(PlayerDirType.Right);
                    break;
                }
                case KeyCode.W:
                {
                    this.Move(PlayerDirType.Up);
                    break;
                }
                case KeyCode.S:
                {
                    this.Move(PlayerDirType.Down);
                    break;
                }
            }
        }

        public void Move(PlayerDirType type)
        {
            var dirX = GameManager.Instance.StartSpeed;
            var dirY = GameManager.Instance.woodGapY;
            Vector3 moveDir = Vector2.zero;
            switch (type)
            {
                case PlayerDirType.Left:
                {
                    moveDir = new Vector2(-dirX, 0);
                    break;
                }
                case PlayerDirType.Right:
                {
                    moveDir = new Vector2(dirX, 0);
                    break;
                }
                case PlayerDirType.Up:
                {
                    moveDir = new Vector2(0, dirY);
                    break;
                }
                case PlayerDirType.Down:
                {
                    moveDir = new Vector2(0, -dirY);
                    break;
                }
            }

            transform.localPosition += moveDir;
            // transform.Translate(moveDir); //todo 跟Rotatio有关系？
        }
    }
}