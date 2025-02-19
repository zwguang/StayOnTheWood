using System;
using System.Collections;
using System.Collections.Generic;
using Game.Monos;
using GDK;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField] private GameObject m_woodPre;
        [SerializeField] private GameObject m_playerPre;
        [SerializeField] private Transform m_woodParentTrans;
        [SerializeField] private TextMeshProUGUI m_scoreText;

        private Player m_player;

        private float m_createWoodTimeCount = 0;
        private readonly float m_createWoodInterval = 1f;

        private int m_batchCount = 0;
        List<WoodType> m_woodList = new List<WoodType>();


        private void Awake()
        {
            EventManager.Instance.On((int)E.PlayerScore, this.OnPlayerScore);
            EventManager.Instance.On((int)E.PlayerDeath, this.OnPlayerDie);
            EventManager.Instance.On<PlayerDirType>((int)E.PlayerMove, this.OnPlayerMove);
            EventManager.Instance.On((int)E.GameStart, GameStart);
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnStart();
            WoodManager.Instance.OnStart(this.m_woodParentTrans, this.m_woodPre);

            GameStart();
        }

        private void OnDestroy()
        {
            Clear();
            
            EventManager.Instance.Off((int)E.PlayerScore, OnPlayerScore);
            EventManager.Instance.Off((int)E.PlayerDeath, OnPlayerDie);
            EventManager.Instance.Off<PlayerDirType>((int)E.PlayerMove, this.OnPlayerMove);

            EventManager.Instance.Off((int)E.GameStart, GameStart);
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
    }

}
