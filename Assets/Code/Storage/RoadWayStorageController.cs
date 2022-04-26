using Code.Data;
using Utils.Directions;

namespace Code.Storage
{
    public class RoadWayStorageController : StorageControllerBase<RoadWayModel, RoadWayStorageModel, int>
    {
        public RoadWayModel GetElement(Direction8 from, Direction8 to)
        {
            TryGetElement(GetId(from, to), out RoadWayModel roadWayModel);

            return roadWayModel;
        }
        
        public RoadWayModel GetElement(RoadWayDirection direction)
        {
            TryGetElement(GetId(direction), out RoadWayModel roadWayModel);

            return roadWayModel;
        }
        
        protected override int GetId(RoadWayModel asset) => GetId(asset.Direction);
        
        private int GetId(RoadWayDirection direction) => GetId(direction.From, direction.To);
        
        private int GetId(Direction8 from, Direction8 to) => (int)from * 8 + (int)to;
    }
}