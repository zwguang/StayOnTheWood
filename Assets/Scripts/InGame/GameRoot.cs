using System;
using System.Collections;
using System.Collections.Generic;
using Game.Monos;
using GDK;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private GameObject m_woodPre;
        [SerializeField] private GameObject m_playerPre;
        [SerializeField] private Transform m_woodParentTrans;

        private Player m_player;

        private float m_createWoodTimeCount = 0;
        private readonly float m_createWoodInterval = 1f;

        private int m_batchCount = 0;
        List<WoodType> m_woodList = new List<WoodType>();

        private PlayerDirType m_playerDirType = PlayerDirType.Right;

        private void Awake()
        {
            EventManager.Instance.On((int)EventID.PlayerScore, this.OnPlayerScore);
            EventManager.Instance.On((int)EventID.PlayerDeath, this.OnPlayerDie);
            EventManager.Instance.On<PlayerDirType>((int)EventID.PlayerMove, this.OnPlayerMove);
            EventManager.Instance.On((int)EventID.GameStart, GameStart);
            EventManager.Instance.On<Vector2>((int)EventID.JoyStickDroging, OnJoyStickDroging);
            EventManager.Instance.On((int)EventID.JumpBtnClicked, OnJumpBtnClicked);
        }

        // Start is called before the first frame update
        void Start()
        {
            UIManager.Instance.ShowPanel(ResPath.prefabPath_InGameMainPanel);
            UIManager.Instance.ShowPanel(ResPath.prefabPath_OperiationPanel);

            GameManager.Instance.OnStart();
            WoodManager.Instance.OnStart(this.m_woodParentTrans, this.m_woodPre);

            GameStart();
        }

        private void OnDestroy()
        {
            Clear();

            EventManager.Instance.Off((int)EventID.PlayerScore, OnPlayerScore);
            EventManager.Instance.Off((int)EventID.PlayerDeath, OnPlayerDie);
            EventManager.Instance.Off<PlayerDirType>((int)EventID.PlayerMove, this.OnPlayerMove);

            EventManager.Instance.Off((int)EventID.GameStart, GameStart);
            EventManager.Instance.Off<Vector2>((int)EventID.JoyStickDroging, OnJoyStickDroging);
            EventManager.Instance.Off((int)EventID.JumpBtnClicked, OnJumpBtnClicked);
        }

        void GameStart()
        {
            Clear();
            Init();
        }

        void Clear()
        {
            WoodManager.Instance.Clear();
            GameManager.Instance.Clear();

            if (m_player)
            {
                m_player.gameObject.SetActive(false);
            }
        }

        private void Init()
        {
            GameManager.Instance.OnInit();
            WoodManager.Instance.OnInit();

            m_createWoodTimeCount = m_createWoodInterval;
            m_batchCount = 0;
            m_playerDirType = PlayerDirType.Right;
        }

        // Update is called once per frame
        void Update()
        {
            OnKeyDown();

            CreatWood();

            GameManager.Instance.Update();
        }

        void OnKeyDown()
        {
            KeyCode type;
            if (Input.GetKeyDown(KeyCode.A))
            {
                type = KeyCode.A;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                type = KeyCode.D;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                type = KeyCode.W;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                type = KeyCode.S;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                this.OnJumpBtnClicked();
                return;
            }
            else
            {
                return;
            }

            m_player.Move(type);
        }


        void CreatWood()
        {
            m_createWoodTimeCount += Time.deltaTime;
            var timeInterval = GameManager.Instance.StartSpeed / GameManager.Instance.speed * m_createWoodInterval;
            if (m_createWoodTimeCount >= timeInterval)
            {
                m_createWoodTimeCount -= timeInterval;
                // m_woodList = WoodManager.Instance.CreateWoodBronType(m_woodList);
                m_woodList = WoodManager.Instance.CreateWoodBronType();
                m_batchCount++;
                for (int i = 0; i < this.m_woodList.Count; i++)
                {
                    WoodManager.Instance.CreateWood(m_woodList[i], m_batchCount);
                    if (this.m_woodList[i] == WoodType.BornPlayerMid)
                    {
                        CreatPlayer();
                    }
                }
            }
        }

        void CreatPlayer()
        {
            if (!m_player)
            {
                m_player = GameObject.Instantiate(m_playerPre).GetComponent<Player>();
                m_player.transform.SetParent(m_woodParentTrans, true);
            }

            m_player.gameObject.SetActive(true);
            m_player.transform.localPosition = new Vector3(L.WoodStartPosX, 0, 0);
        }

        void OnPlayerScore()
        {
        }

        void OnPlayerDie()
        {
        }

        void OnPlayerMove(PlayerDirType dir)
        {
            m_player.Move(dir);
        }

        void OnJoyStickDroging(Vector2 inPut)
        {
            if (inPut == Vector2.up)
            {
                m_playerDirType = PlayerDirType.Up;
            }
            else if (inPut == Vector2.down)
            {
                m_playerDirType = PlayerDirType.Down;
            }
            else if (inPut == Vector2.left)
            {
                m_playerDirType = PlayerDirType.Left;
            }
            else if (inPut == Vector2.right)
            {
                m_playerDirType = PlayerDirType.Right;
            }

            m_player.SetDir(m_playerDirType);
        }

        void OnJumpBtnClicked()
        {
            m_player.Move(m_playerDirType);
        }
    }
}