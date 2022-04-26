using UnityEngine;

namespace Code.Storage
{
    [RequireComponent(
        typeof(RoadStorageController),
        typeof(RoadWayStorageController),
        typeof(RoadSignStorageController)
    )]
    public class StorageController : MonoBehaviour
    {
        public RoadStorageController Roads { get; private set; }
        public RoadWayStorageController RoadWays { get; private set; }
        
        public RoadSignStorageController RoadSigns { get; private set; }

        private void Awake()
        {
            Roads = GetComponent<RoadStorageController>();
            RoadWays = GetComponent<RoadWayStorageController>();
            RoadSigns = GetComponent<RoadSignStorageController>();
        }
    }
}