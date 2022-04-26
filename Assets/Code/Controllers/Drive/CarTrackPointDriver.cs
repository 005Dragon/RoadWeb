using UnityEngine;

namespace Code.Controllers.Drive
{
    [RequireComponent(typeof(CarController))]
    public class CarTrackPointDriver : MonoBehaviour
    {
        public Vector2 TargetPoint;

        [SerializeField] private float _MaxSpeed;
        [SerializeField] private float _angleAccuracy;
        [SerializeField] private float _distanceAccuracy;

        private Camera _camera;
        private Transform _transform;
        private CarController _carController;
        

        private void Awake()
        {
            _camera = Camera.main;
            _transform = transform;
            _carController = GetComponent<CarController>();
        }

        private void Update()
        {
            //Debug.Log(_carController.GetSpeed() * 36);
            if (Input.GetMouseButton(0))
            {
                TargetPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
            }
            
            Vector2 positionToTargetVector = TargetPoint - (Vector2)_transform.position;
            
            UpdateTurn(positionToTargetVector);
            UpdateMove(positionToTargetVector);
        }

        private void UpdateTurn(Vector2 positionToTargetVector)
        {
            float differenceAngle = Vector2.SignedAngle(positionToTargetVector, _carController.GetDirection());
            if (differenceAngle < _angleAccuracy && differenceAngle > -_angleAccuracy)
            {
                _carController.TurnLeft = false;
                _carController.TurnRight = false;
            }
            else
            {
                if (differenceAngle > 0)
                {
                    _carController.TurnRight = true;
                }
                else
                {
                    _carController.TurnLeft = true;
                }
            }
        }

        private void UpdateMove(Vector2 positionToTargetVector)
        {
            float distanceToTarget = positionToTargetVector.magnitude;

            _carController.Brake = distanceToTarget < _distanceAccuracy;
            _carController.Accelerate = NeedAccelerate(distanceToTarget);
        }

        private bool NeedAccelerate(float distanceToTarget)
        {
            if (_carController.GetSpeed()  > _MaxSpeed / 36.0f)
            {
                return false;
            }

            if (distanceToTarget < _distanceAccuracy)
            {
                return false;
            }

            return true;
        }
    }
}