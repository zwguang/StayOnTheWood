using System;
using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
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
            transform.localPosition -= new Vector3(GameManager.Instance.speed * Time.deltaTime, 0, 0);
            if (transform.localPosition.x < -11 || transform.localPosition.x > 11 ||
            transform.localPosition.y < -4 || transform.localPosition.y > 4)
            {
                GameOver();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Wood"))
            {
                Wood wood = other.gameObject.GetComponent<Wood>();
                if (wood.batch > GameManager.Instance.soreMaxBatch)
                {
                    GameManager.Instance.soreMaxBatch = wood.batch;
                    GameManager.Instance.soreNum++;
                    EventManager.Instance.Trigger((int)EventID.PlayerScore);
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
            GameManager.Instance.PlayerDeath();
            EventManager.Instance.Trigger((int)EventID.PlayerDeath);
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

            // transform.localPosition += moveDir;
            transform.Translate(moveDir);
        }
    }
    
    

}
