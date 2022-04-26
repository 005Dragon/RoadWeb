using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data;
using UnityEngine;

namespace Code.Storage
{
    public abstract class StorageControllerBase<TAsset, TStorage, TId> : MonoBehaviour
        where TAsset : ScriptableObject
        where TStorage : StorageModelBase<TAsset>
        where TId : IEquatable<TId>
    {
        [SerializeField] private TStorage _model;

        private readonly Dictionary<TId, TAsset> _idToAssetIndex = new Dictionary<TId, TAsset>();

        public bool TryGetElement(TId id, out TAsset asset)
        {
            bool result = _idToAssetIndex.TryGetValue(id, out asset);

            string resultString = result ? "Success" : "Failed";
            
            Debug.Log($"Get id: {id} from {GetType()}. {resultString}.");

            return result;
        }

        protected virtual void Awake()
        {
            foreach (TAsset asset in _model.Elements)
            {
                _idToAssetIndex[GetId(asset)] = asset;
            }

            Debug.Log(
                // ReSharper disable once UseStringInterpolation
                string.Format(
                    "Build indexes for {0} count - {1}.{2}{3}",
                    GetType(),
                    _idToAssetIndex.Count,
                    Environment.NewLine,
                    String.Join(",", _idToAssetIndex.Select(x => x.Key))
                )
            );
            
            Validate();
        }

        protected abstract TId GetId(TAsset asset);

        private void Validate()
        {
            if (_model.Elements.Count != _idToAssetIndex.Count)
            {
                throw new Exception(
                    // ReSharper disable once UseStringInterpolation
                    string.Format(
                        "{0}. Failed build indexes. Expected count elements - {1}, build elements - {2}",
                        GetType(),
                        _model.Elements.Count,
                        _idToAssetIndex.Count
                    )
                );
            }
        }
    }
}