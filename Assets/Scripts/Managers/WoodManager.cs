using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using GDK;
using UnityEngine;

namespace Game.Monos
{
    public class WoodManager:Singleton<WoodManager>
    {
        private ResAdapter m_resAdapter;
        private Transform m_woodParent;
        private GameObject m_woodPre;

        private List<Wood> m_woodPool = new List<Wood>();
        private List<Wood> m_woodActiveList = new List<Wood>();
        
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
        
        public Wood CreateWood(CreateWoodType type)
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
    }
}