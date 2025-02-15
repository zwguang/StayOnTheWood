using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private int m_woodIndex = 0;
        private List<List<WoodType>> m_woodList = new List<List<WoodType>>
        {
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.EmptyMid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.EmptyMid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.Up, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid,WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
        };

        //生成的误导木桩的数量
        private int m_misdirectCount = 0;
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
        
        public Wood CreateWood(WoodType type, int batch)
        {
            var wood = this.m_woodPool.Pop();
            if (!wood)
            {
                wood = GameObject.Instantiate(m_woodPre).GetComponent<Wood>();
                wood.transform.SetParent(this.m_woodParent, true);
            }
            
            wood.Init(type,batch);
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
        public List<WoodType> CreateWoodBronType()
        {
            if (m_woodIndex == this.m_woodList.Count)
            {
                this.m_woodIndex = 0;
            }

            return m_woodList[m_woodIndex++];
        }
        
        
        /// <summary>
        /// 木桩的出生位置,乱序的
        /// </summary>
        /// <returns></returns>
        public List<WoodType> CreateWoodBronType(List<WoodType> existingWoodTypes)
        {
            //初始情况
            if (existingWoodTypes.Count <= 0)
            {
                return new List<WoodType> { WoodType.Mid };
            }

            //生成必须存在的可达木桩
            WoodType requiredY = existingWoodTypes[m_random.Next(existingWoodTypes.Count)];
            // while (requiredY == WoodType.Invalid)
            // {
            //     requiredY = existingWoodTypes[m_random.Next(existingWoodTypes.Count)];
            // }
            
            List<WoodType> generatedList = new List<WoodType>{ requiredY };
            //随机生产0~1个木头
            int extraWoodCount = m_random.Next(0,2);
            List<WoodType> availableYs = new List<WoodType> { WoodType.Up, WoodType.Mid, WoodType.Down };
            for (int i = 0; i < extraWoodCount; i++)
            {
                //排除已生成的y
                var remainingY = availableYs.Except(generatedList).ToList();
                if (remainingY.Count == 0)
                {
                    break;
                }

                WoodType type = remainingY[m_random.Next(remainingY.Count)];
                generatedList.Add(type);
            }

            //连续多次生成了上下两个，则必须加一个中间位置 todo
            //----- 第5次随机生成在这儿
            //-  
            //----
            if (generatedList.Count == 2 && generatedList[0] != WoodType.Mid && generatedList[1] != WoodType.Mid)
            {
                if (++m_misdirectCount == 3)
                {
                    m_misdirectCount = 0;
                    generatedList.Add(WoodType.Mid);
                }
            }
            else
            {
                m_misdirectCount = 0;
            }
            return generatedList;
        }
    }

}