using Code.Data;

namespace Code.Storage
{
    public class RoadSignStorageController : StorageControllerBase<RoadSignModel, RoadSignStorageModel, byte>
    {
        public RoadSignModel GetElement(RoadSign sign)
        {
            TryGetElement(GetId(sign), out RoadSignModel roadSignModel);

            return roadSignModel;
        }
        
        protected override byte GetId(RoadSignModel asset) => GetId(asset.Sign);

        private byte GetId(RoadSign sign) => (byte)sign;
    }
}