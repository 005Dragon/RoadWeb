using System;
using Code.Data;
using UnityEngine;
using Utils.Directions;

namespace Code.Controllers.Roads
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class RoadSignController : MonoBehaviour
    {
        public Vector2Int Position => _bindRoad.Position;
        
        [SerializeField] private RoadSignModel _model;
        [SerializeField] private RoadElementController _bindRoad;
        [SerializeField] private Direction8 _bindDirection;
        [SerializeField] private float _heightLevel = -1;
        
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        
        private readonly float _selfShift = 0.0625f;
        
        [ContextMenu(nameof(Build))]
        public void Build()
        {
            SetModel(_model, _bindRoad, _bindDirection);
        }

        public void SetModel(RoadSignModel model, RoadElementController bindRoad, Direction8 bindDirection)
        {
            _model = model;
            _bindRoad = bindRoad;
            _bindDirection = bindDirection;
            
            _transform.SetParent(bindRoad.transform);
            
            Vector2 position = GetLocalPosition();
            _transform.localPosition = new Vector3(position.x, position.y, _heightLevel);
            _spriteRenderer.sprite = _model.Sprite;
        }

        private void Awake()
        {
            _transform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private Vector2 GetLocalPosition()
        {
            float shift = _selfShift + _bindRoad.Model.Width / 2.0f;

            switch (_bindDirection)
            {
                case Direction8.Right: return new Vector2(1.0f, 0.5f + shift);
                case Direction8.DownRight: return new Vector2(1.0f, 0.0f + shift * Mathf.Sqrt(2));
                case Direction8.Down: return new Vector2(0.5f + shift, 0.0f);
                case Direction8.DownLeft: return new Vector2(0.0f + shift * Mathf.Sqrt(2), 0.0f);
                case Direction8.Left: return new Vector2(0.0f, 0.5f - shift);
                case Direction8.UpLeft: return new Vector2(0.0f, 1.0f - shift * Mathf.Sqrt(2));
                case Direction8.Up: return new Vector2(0.5f - shift, 1.0f);
                case Direction8.UpRight: return new Vector2(1.0f - shift* Mathf.Sqrt(2), 1.0f );
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}