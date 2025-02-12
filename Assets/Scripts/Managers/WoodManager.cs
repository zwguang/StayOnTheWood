using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using GDK;
using UnityEngine;
using Random = System.Random;

namespace Game.Monos
{
    public class WoodManager:Singleton<WoodManager>
    {
        private ResAdapter m_resAdapter;
        private Transform m_woodParent;
        private GameObject m_woodPre;

        private List<Wood> m_woodPool = new List<Wood>();
        private List<Wood> m_woodActiveList = new List<Wood>();

        private Random m_random = new Random();
        protected override void OnDestroy()
        {
            m_resAdapter.Dispose();
        }
        
        public void Recycle(Wood wood)
        {
            wood.gameObject.SetActive(false);
            this.m_woodActiveList.Remove(wood);
            
            this.m_woodPool.Add(wood);

        }

        public void Init(Transform woodParent, GameObject woodPre)
        {
            m_resAdapter = new ResAdapter();
            m_woodParent = woodParent;
            m_woodPre = woodPre;
            // m_woodPre = m_resAdapter.Load<GameObject>("");
        }
        
        public Wood CreateWood(WoodType type)
        {
            var wood = this.m_woodPool.Pop();
            if (!wood)
            {
                wood = GameObject.Instantiate(m_woodPre).GetComponent<Wood>();
                wood.transform.SetParent(this.m_woodParent, true);
            }
            
            wood.Init(type);
            m_woodActiveList.Add(wood);
            return wood;
        }


        // public void Update()
        // {
        //     foreach (var wood in m_woodActiveList)
        //     {
        //         wood.Update();
        //     }
        // }

        /// <summary>
        /// 木桩的出生位置
        /// </summary>
        /// <returns></returns>
        public List<WoodType> CreateWoodBronType(List<WoodType> existingWoodTypes)
        {
            //初始情况
            if (existingWoodTypes.Count <= 0)
            {
                return new List<WoodType> { WoodType.Mid };
            }

            HashSet<WoodType> generatedHashSet = new HashSet<WoodType> { GameManager.Instance.selectedWoodType };
            //随机生产0~2个木头
            int extraWoodCount = m_random.Next(0,3);
            for (int i = 0; i < extraWoodCount; i++)
            {
            }

            return existingWoodTypes;
        }
    }

}