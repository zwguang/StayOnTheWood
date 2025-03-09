using UnityEngine;

namespace GDK
{
    public class PoolManager : MonoSingleton<PoolManager>, ISerializationCallbackReceiver
    {
        //*********** 属性 ********
        [HideInInspector] [SerializeField] [Tooltip("每个对象池最多存多少个对象")]
        private int _maxCount = 50;

        [HideInInspector] [SerializeField] [Tooltip("每过多少秒自动清除这段时间不活动的对象池")]
        private int _autoClearTime = 5 * 60;

        public void OnBeforeSerialize()
        {
            throw new System.NotImplementedException();
        }

        public void OnAfterDeserialize()
        {
            throw new System.NotImplementedException();
        }
    }
}