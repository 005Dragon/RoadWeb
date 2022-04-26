using System;
using System.Collections.Generic;
using Code.Data;
using JetBrains.Annotations;
using RoadContracts.Descriptions;
using UnityEditor;
using UnityEngine;

namespace Code.Import
{
    public abstract class Importer<TDescription, TAsset, TStorage> : MonoBehaviour
        where TAsset : ScriptableObject
        where TStorage : StorageModelBase<TAsset>
    {
        [SerializeField] private TStorage _storage;
        [SerializeField] private TextAsset _descriptions;
        [SerializeField] private string _targetDirectoryName;
        
        [CanBeNull] private string _saveDirectory;

        protected void Import()
        {
            InitializeSaveDirectory();

            var descriptionPack = JsonUtility.FromJson<DescriptionPack<TDescription>>(_descriptions.text);
            
            Debug.Log($"Start importing. Count values: {descriptionPack.Descriptions.Length}");

            int successCount = 0;
            int errorsCount = 0;

            foreach (TDescription description in descriptionPack.Descriptions)
            {
                try
                {
                    foreach (TAsset assetData in CreateAssets(description))
                    {
                        SaveAsset(assetData);
                        successCount++;
                    }
                }
                catch (Exception exception)
                {
                    Debug.Log(exception);
                    errorsCount++;
                }
            }

            Debug.Log(
                $"Import completed. Errors - {errorsCount}, success - {successCount}."
            );
        }
        
        protected abstract IEnumerable<TAsset> CreateAssets(TDescription description);
        protected abstract string GetAssetName(TAsset asset);

        protected Vector2 FromDescription(Vector2Description description)
        {
            return new Vector2(description.X, description.Y);
        }

        protected RoadWayDirection FromDescription(RoadWayDirectionDescription description)
        {
            return new RoadWayDirection(description.From, description.To);
        }
        
        private void InitializeSaveDirectory()
        {
            _saveDirectory = null;
            
            foreach (string path in AssetDatabase.GetSubFolders(AssetPath.Data))
            {
                var folder = new AssetPath.Folder(path);

                if (string.Equals(folder.Name, _targetDirectoryName, StringComparison.OrdinalIgnoreCase))
                {
                    _saveDirectory = folder.Path;
                }
            }

            if (_saveDirectory == null)
            {
                AssetDatabase.CreateFolder(AssetPath.Data, _targetDirectoryName);
                _saveDirectory = AssetPath.Combine(AssetPath.Data, _targetDirectoryName);
            }
        }

        private void SaveAsset(TAsset asset)
        {
            AssetDatabase.CreateAsset(asset, AssetPath.Combine(_saveDirectory, $"{GetAssetName(asset)}.asset"));
            _storage.Elements.Add(asset);
        }
    }
}