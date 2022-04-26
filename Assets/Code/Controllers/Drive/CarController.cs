using UnityEngine;

namespace Code.Controllers.Drive
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CarController : MonoBehaviour
    {
        public bool Accelerate { get; set; }
        public bool Brake { get; set; }
        public bool TurnLeft
        {
            get => _turnLeft;
            set
            {
                _turnLeft = value;

                if (_turnLeft && _turnRight)
                {
                    _turnRight = false;
                }
            }
        }
        public bool TurnRight
        {
            get => _turnRight;
            set
            {
                _turnRight = value;

                if (_turnRight && _turnLeft)
                {
                    _turnLeft = false;
                }
            }
        }
        
        [SerializeField] private float _maxAngleForce;
        [SerializeField] private float _force;
        [SerializeField] private float _minDrag;
        [SerializeField] private float _maxDrag;
        [SerializeField] private float _minAngularDrag;
        [SerializeField] private float _maxAngularDrag;
        [SerializeField] private float _clutchForce;

        private Rigidbody2D _rigidbody;
        private Transform _transform;

        private bool _turnLeft;
        private bool _turnRight;

        public Vector2 GetDirection()
        {
            float angle = _transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        public float GetSpeed() => _rigidbody.velocity.magnitude;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Vector2 velocity = _rigidbody.velocity;
            float velocityMagnitude = velocity.magnitude;

            float angle = _transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            float velocityAngle = Mathf.Atan2(velocity.y, velocity.x);
            float angleDifference = NormalizeAngle(angle - velocityAngle);

            float brakeAngleCoefficient = Mathf.Sin(angleDifference);

            float angleDifferenceForce = velocityMagnitude * velocityMagnitude * _rigidbody.mass;
            float clutchCoefficient = angleDifferenceForce < 0.0001f
                ? float.MaxValue
                : _clutchForce * Time.deltaTime / angleDifferenceForce;
            
            float resultVelocityAngle =
                velocityAngle + Mathf.Lerp(0.0f, angleDifference, clutchCoefficient);

            float linearClutchCoefficient = 10.0f * clutchCoefficient * velocityMagnitude;

            _rigidbody.drag = Mathf.Lerp(
                _minDrag,
                _maxDrag,
                brakeAngleCoefficient * brakeAngleCoefficient * linearClutchCoefficient
            );
            
            _rigidbody.angularDrag = 
                Mathf.Lerp(_minAngularDrag, _maxAngularDrag, linearClutchCoefficient);
           
            _rigidbody.velocity = new Vector2(
                Mathf.Cos(resultVelocityAngle) * velocityMagnitude,
                Mathf.Sin(resultVelocityAngle) * velocityMagnitude
            );

            float angleForce = _maxAngleForce * (1 / clutchCoefficient);
            
            if (angleForce > _maxAngleForce)
            {
                angleForce = _maxAngleForce;
            }
            
            UpdateInputs(angle, angleForce, linearClutchCoefficient);
        }

        private void UpdateInputs(float angle, float angleForce, float linearClutchCoefficient)
        {
            if (Accelerate)
            {
                Vector2 force = new Vector2(
                    Mathf.Cos(angle) * _force * Time.deltaTime,
                    Mathf.Sin(angle) * _force * Time.deltaTime
                );

                _rigidbody.AddForce(force, ForceMode2D.Force);
            }
            
            if (TurnLeft)
            {
                _rigidbody.AddTorque(angleForce * Time.deltaTime, ForceMode2D.Force);
            }

            if (TurnRight)
            {
                _rigidbody.AddTorque(-angleForce * Time.deltaTime, ForceMode2D.Force);
            }

            if (Brake)
            {
                _rigidbody.drag = _maxDrag * linearClutchCoefficient;
            }
        }

        private float NormalizeAngle(float angle)
        {
            if (angle > Mathf.PI)
            {
                return angle - Mathf.PI * 2;
            }

            return angle;
        }
    }
}