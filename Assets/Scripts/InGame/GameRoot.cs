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

        public Player player
        {
            get
            {
                if (m_player == null)
                {
                    m_player = GameObject.Instantiate(m_playerPre).GetComponent<Player>();
                    m_player.transform.SetParent(m_woodParentTrans, true);
                }

                return m_player;
            }
        }

        private float m_createWoodTimeCount = 0;

        private int m_batchCount = 0;
        List<WoodType> m_woodList = new List<WoodType>();

        private PlayerDirType m_playerDirType = PlayerDirType.Right;

        private void Awake()
        {
            SystemEventManager.Instance.On((int)EventID.PlayerScore, this.OnPlayerScore);
            SystemEventManager.Instance.On((int)EventID.PlayerDeath, this.OnPlayerDie);
            SystemEventManager.Instance.On<PlayerDirType>((int)EventID.PlayerMove, this.OnPlayerMove);
            SystemEventManager.Instance.On((int)EventID.GameStart, GameStart);
            SystemEventManager.Instance.On<Vector2>((int)EventID.JoyStickDroging, OnJoyStickDroging);
            SystemEventManager.Instance.On((int)EventID.JumpBtnClicked, OnJumpBtnClicked);
        }

        // Start is called before the first frame update
        void Start()
        {
            SystemUIManager.Instance.ShowPanel(ResPath.prefabPath_InGameMainPanel);
            SystemUIManager.Instance.ShowPanel(ResPath.prefabPath_OperiationPanel);

            GameManager.Instance.OnStart();
            WoodManager.Instance.OnStart(this.m_woodParentTrans, this.m_woodPre);

            GameStart();
        }

        private void OnDestroy()
        {
            Clear();

            SystemEventManager.Instance.OffAll(this);
            // UIManager.Instance.ClearAllWithoutResident();
            SystemUIManager.Instance.HidePanel(ResPath.prefabPath_InGameMainPanel);
            SystemUIManager.Instance.HidePanel(ResPath.prefabPath_OperiationPanel);
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

            player.gameObject.SetActive(false);
        }

        private void Init()
        {
            GameManager.Instance.OnInit();
            WoodManager.Instance.OnInit();

            m_createWoodTimeCount = GameManager.Instance.createWoodInterval;
            m_batchCount = 0;
            m_playerDirType = PlayerDirType.Right;
            player.gameObject.SetActive(false);
            player.Init();
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

            player.Move(type);
        }


        void CreatWood()
        {
            m_createWoodTimeCount += Time.deltaTime;
            var timeInterval = GameManager.Instance.StartSpeed / GameManager.Instance.speed *
                               GameManager.Instance.createWoodInterval;
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
                        player.Init();
                        player.gameObject.SetActive(true);
                        player.transform.localPosition = new Vector3(L.WoodStartPosX, 0, 0);
                    }
                }
            }
        }

        void OnPlayerScore()
        {
        }

        void OnPlayerDie()
        {
        }

        void OnPlayerMove(PlayerDirType dir)
        {
            player.Move(dir);
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

            player.SetDir(m_playerDirType);
        }

        void OnJumpBtnClicked()
        {
            player.Move(m_playerDirType);
        }
    }
}