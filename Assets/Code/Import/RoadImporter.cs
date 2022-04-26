using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data;
using RoadContracts.Descriptions;
using UnityEditor;
using UnityEngine;
using Utils.Directions;

namespace Code.Import
{
    [ExecuteInEditMode]
    public class RoadImporter : Importer<RoadAtlasDescription, RoadElementModel, RoadStorageModel>
    {
        [ContextMenu(nameof(Import))]
        public new void Import() => base.Import();

        protected override IEnumerable<RoadElementModel> CreateAssets(RoadAtlasDescription description)
        {
            string path = AssetPath.Combine(AssetPath.Textures, "Roads", $"{description.Name}.png");
            
            Debug.Log(path);
        
            Sprite[] sprites =
                AssetDatabase.LoadAllAssetRepresentationsAtPath(path)
                    .Select(x => (Sprite)x)
                    .ToArray();
        
            Debug.Log(
                // ReSharper disable once UseStringInterpolation
                string.Format(
                    "Atlas: {0}, Descriptions: {1}, Sprites: {2}",
                    description.Name,
                    description.Roads.Length,
                    sprites.Length
                )
            );
        
            int elementsCount = Math.Min(description.Roads.Length, sprites.Length);
            
            for (int i = 0; i < elementsCount; i++)
            {
                yield return CreateRoadElement(description.Roads[i], sprites[i]);
            }
        }

        protected override string GetAssetName(RoadElementModel asset)
        {
            var directionGroup = new Direction8Group(
                asset.Directions.Select(x => x.From).Union(asset.Directions.Select(x => x.To)).ToArray()
            );

            return "R" + directionGroup;
        }
        
        private RoadElementModel CreateRoadElement(RoadDescription road, Sprite sprite)
        {
            var roadElementModel = ScriptableObject.CreateInstance<RoadElementModel>();
            roadElementModel.Directions = road.Directions.Select(FromDescription).ToArray();
            roadElementModel.Sprite = sprite;
            roadElementModel.Width = road.Width;
            roadElementModel.LineWidth = road.LineWidth;
            return roadElementModel;
        }
    }
}