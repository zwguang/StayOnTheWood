using System;
using UnityEngine;

namespace GDK
{
    public class ResourcesResLoader : BaseResLoader
    {
        
        public override bool IsExits()
        {
            // if (ReferenceEquals(asset, null))
            // {
            //     asset = Resources.Load(path, type);
            // }

            return !ReferenceEquals(asset, null);
        }
        
        public override object Load()
        {
            if (ReferenceEquals(asset, null))
            {
                asset = Resources.Load(path, type);
            }
            Done();

            return asset;
        }

        public override void LoadAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}