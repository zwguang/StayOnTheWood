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
        private readonly float m_addSpeedInterval = 0;

        private int m_score = 0;
        List<WoodType> m_woodList = new List<WoodType>();


        private void Awake()
        {
            EventManager.Instance.On((int)E.PlayerScore, this.OnPlayerScore);
            EventManager.Instance.On((int)E.PlayerDie, this.OnPlayerDie);

            WoodManager.Instance.Init(this.m_woodParentTrans, this.m_woodPre);

        }

        // Start is called before the first frame update
        void Start()
        {
            var wood = WoodManager.Instance.CreateWood(WoodType.Mid);
            var pos = new Vector3(L.WoodStartPosX, 0, 0);
            wood.SetStartPos(pos);

            m_player = GameObject.Instantiate(m_playerPre).GetComponent<Player>();
            m_player.transform.localPosition = pos;
            m_player.transform.SetParent(m_woodParentTrans, true);
        }

        private void Init()
        {
            this.m_score = 0;
        }

        // Update is called once per frame
        void Update()
        {
            OnKeyDown();

            CreatWood();   
            
            
            // WoodManager.Instance.Update();
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
            var timeInterval = L.WoodStartSpeed / GameManager.Instance.speed * m_createWoodInterval;
            if (m_createWoodTimeCount >= timeInterval)
            {
                m_createWoodTimeCount -= timeInterval;
                m_woodList = WoodManager.Instance.CreateWoodBronType(m_woodList);
                for (int i = 0; i < this.m_woodList.Count; i++)
                {
                    WoodManager.Instance.CreateWood(m_woodList[i]);
                }
            }
        }

        void OnPlayerScore()
        {
            m_score++;
            // this.m_scoreText.text = m_score.ToString();
        }
        
        void OnPlayerDie()
        {
            
        }
    }

}
