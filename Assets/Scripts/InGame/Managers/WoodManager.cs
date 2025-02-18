using System;
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
        
        private List<List<WoodType>> m_woodList1 = new List<List<WoodType>>
        {
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.EmptyMid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid,WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
        };
        
        //中下回到中下
        private List<List<WoodType>> m_woodList2 = new List<List<WoodType>>
        {
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
            new List<WoodType>{WoodType.Up, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
        };
        
        //中上回到中间
        private List<List<WoodType>> m_woodList3 = new List<List<WoodType>>
        {
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.Down},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
        };
        
        //上中回到中
        private List<List<WoodType>> m_woodList4 = new List<List<WoodType>>
        {
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.EmptyMid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.Up, WoodType.EmptyMid, WoodType.Down},
            new List<WoodType>{WoodType.Up, WoodType.Mid, WoodType.EmptyDown},
            new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
        };
        
        //中回到中下 困难
        private List<List<WoodType>> m_woodList5 = new List<List<WoodType>>
        {
            new List<WoodType> { WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown },
            new List<WoodType> { WoodType.Up, WoodType.Mid, WoodType.Down },
            new List<WoodType> { WoodType.Up, WoodType.EmptyMid, WoodType.Down },
            new List<WoodType> { WoodType.EmptyUp, WoodType.Mid, WoodType.Down },
            new List<WoodType> { WoodType.Up, WoodType.EmptyMid, WoodType.Down },
            new List<WoodType> { WoodType.EmptyUp, WoodType.Mid, WoodType.Down },
            new List<WoodType> { WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown },
        };
        
        //中回到上中 困难
        private List<List<WoodType>> m_woodList6 = new List<List<WoodType>>
        {
            new List<WoodType> { WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown },
            new List<WoodType> { WoodType.Up, WoodType.Mid, WoodType.Down },
            new List<WoodType> { WoodType.Up, WoodType.EmptyMid, WoodType.Down },
            new List<WoodType> { WoodType.Up, WoodType.Mid, WoodType.EmptyDown },
            new List<WoodType> { WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown },
            new List<WoodType> { WoodType.EmptyUp, WoodType.Mid, WoodType.Down },
            new List<WoodType> { WoodType.Up, WoodType.Mid, WoodType.EmptyDown },
        };

        private List<List<WoodType>> m_woodList;
        private List<List<List<WoodType>>> m_woodListArr;
        
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

        public void OnStart(Transform woodParent, GameObject woodPre)
        {
            m_resAdapter = new ResAdapter();
            m_woodParent = woodParent;
            m_woodPre = woodPre;
            // m_woodPre = m_resAdapter.Load<GameObject>("");
            m_woodListArr = new List<List<List<WoodType>>>
            {
                this.m_woodList1, this.m_woodList2, this.m_woodList3, this.m_woodList4
                , this.m_woodList5, this.m_woodList6
            };
        }

        public void Clear()
        {
            for (int i = m_woodActiveList.Count-1; i >= 0; i--)
            {
                Recycle(this.m_woodActiveList[i]);
            }
            this.m_woodActiveList.Clear();
        }
        
        public void OnInit()
        {
            m_woodIndex = 0;
            //每次随机一个木桩组合
            this.m_woodList = new List<List<WoodType>>
            {
                new List<WoodType>{WoodType.EmptyUp, WoodType.EmptyMid, WoodType.EmptyDown},
                new List<WoodType>{WoodType.EmptyUp, WoodType.BornPlayerMid, WoodType.EmptyDown},
                new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
                new List<WoodType>{WoodType.EmptyUp, WoodType.Mid, WoodType.EmptyDown},
            };
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


        public List<WoodType> CreateWoodBronType()
        {
            if (m_woodIndex == this.m_woodList.Count)
            {
                this.m_woodIndex = 0;
                
                var index = m_random.Next(this.m_woodListArr.Count);
                Debug.Log($"StyOnTheWood 新的木桩组诞生 index = {index}");
                this.m_woodList = m_woodListArr[index];
                
                // ShuffleWoodListArr();
            }

            return m_woodList[m_woodIndex++];
        }
        
        //乱序，保证每个木桩组都能出现
        void ShuffleWoodListArr()
        {
            Random random = new Random();
            int n = this.m_woodListArr.Count;
            for (int i = n - 1; i > 0; i--)
            {
                // 生成一个 0 到 i 之间（包含 0 和 i）的随机索引
                int j = random.Next(i + 1);

                // 交换位置 i 和位置 j 上的元素
                var temp = this.m_woodListArr[i];
                this.m_woodListArr[i] = this.m_woodListArr[j];
                this.m_woodListArr[j] = temp;
            }

            this.m_woodList.Clear();
            for (int i = 0; i < n; i++)
            {
                this.m_woodList.AddRange(this.m_woodListArr[i]);
            }
        }
    
    }

}