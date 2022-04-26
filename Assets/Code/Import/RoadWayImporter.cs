using System.Collections.Generic;
using System.Linq;
using Code.Data;
using RoadContracts.Descriptions;
using UnityEngine;

namespace Code.Import
{
    public class RoadWayImporter : Importer<RoadWayDescription, RoadWayModel, RoadWayStorageModel>
    {
        [ContextMenu(nameof(Import))]
        public new void Import() => base.Import();
        
        protected override IEnumerable<RoadWayModel> CreateAssets(RoadWayDescription description)
        {
            var asset = ScriptableObject.CreateInstance<RoadWayModel>();
            asset.Direction = FromDescription(description.Direction);
            asset.Points = description.Points.Select(FromDescription).ToArray();
            yield return asset;
        }

        protected override string GetAssetName(RoadWayModel asset) => asset.Direction.ToString();
    }
}