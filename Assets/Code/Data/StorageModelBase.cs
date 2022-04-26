using System.Collections.Generic;
using UnityEngine;

namespace Code.Data
{
    public abstract class StorageModelBase<TAsset> : ScriptableObject
        where TAsset : ScriptableObject
    {
        public List<TAsset> Elements = new List<TAsset>();
    }
}