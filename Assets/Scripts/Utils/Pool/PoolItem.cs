using System;
using UnityEngine.AddressableAssets;

namespace Core.Utils
{
    [Serializable]
    public class PoolItem
    {
        public PoolItemType           PoolItemType;
        public AssetReference Asset;
        public int            Size;
    }
}