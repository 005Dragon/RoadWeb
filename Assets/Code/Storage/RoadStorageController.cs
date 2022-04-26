using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Utils.Directions;

namespace Code.Storage
{
    public class RoadStorageController : StorageControllerBase<RoadElementModel, RoadStorageModel, ulong>
    {
        private readonly Dictionary<RoadWayDirection, int> _roadWayDirectionIndex = 
            new Dictionary<RoadWayDirection, int>();

        protected override void Awake()
        {
            int index = 0;

            foreach (Direction8 from in Direction.Directions8)
            {
                foreach (Direction8 to in Direction.Directions8)
                {
                    _roadWayDirectionIndex[new RoadWayDirection(from, to)] = index++;
                }
            }

            base.Awake();
        }
        
        public RoadElementModel GetElement(Direction8[] directions)
        {
            return GetElement(GetRoadWayDirections(directions).ToArray());
        }

        public RoadElementModel GetElement(params RoadWayDirection[] directions)
        {
            TryGetElement(GetId(directions), out RoadElementModel roadElementModel);

            return roadElementModel;
        }

        protected override ulong GetId(RoadElementModel asset) => GetId(asset.Directions);

        private ulong GetId(RoadWayDirection[] directions)
        {
            var bits = new BitArray(64);
            bits.SetAll(false);

            foreach (RoadWayDirection direction in directions)
            {
                bits[_roadWayDirectionIndex[direction]] = true;
            }

            var array = new int[2];
            bits.CopyTo(array, 0);
            ulong result = (uint)array[0] + ((ulong)array[1] << 32);
            return result;
        }

        private IEnumerable<RoadWayDirection> GetRoadWayDirections(Direction8[] directions)
        {
            foreach (Direction8 from in directions)
            {
                foreach (Direction8 to in directions)
                {
                    yield return new RoadWayDirection(from, to);
                }
            }
        }
    }
}