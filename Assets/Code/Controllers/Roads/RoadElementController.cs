using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data;
using UnityEngine;
using Utils.Directions;
using Vector2 = UnityEngine.Vector2;

namespace Code.Controllers.Roads
{
    [ExecuteInEditMode]
    public class RoadElementController : MonoBehaviour
    {
        public Vector2Int Position => _position;
        public RoadElementModel Model => _model;

        public Color Color;

        [SerializeField] private RoadElementModel _model;
        [SerializeField] private Vector2Int _position;

        private SpriteRenderer _spriteRenderer;

        private readonly Dictionary<Direction8, SpriteRenderer> _directionToSpriteRendererIndex =
            new Dictionary<Direction8, SpriteRenderer>();

        [ContextMenu(nameof(Build))]
        public void Build()
        {
            SetModel(_model, null);
        }

        public void SetModel(RoadElementModel model, RoadElementModel additionalModel)
        {
            _model = model;
            if (_spriteRenderer == null)
            {
                _spriteRenderer = CreateSpriteRenderer(Vector3.zero, 0.0f);
            }
            _spriteRenderer.sprite = _model.Sprite;
            _spriteRenderer.color = Color;

            foreach (Direction8 direction in Direction.Diagonals.Select(x => x.ToDirection8()))
            {
                bool exists = _directionToSpriteRendererIndex.TryGetValue(direction, out SpriteRenderer spriteRenderer);
                bool mustExist = _model.Directions.Any(x => x.From == direction || x.To == direction);

                if (mustExist)
                {
                    if (!exists)
                    {
                        Vector2 position = GetSpriteRendererPosition(direction);
                        float angle = direction == Direction8.DownLeft || direction == Direction8.UpRight ? 90.0f : 0.0f;
                        spriteRenderer = CreateSpriteRenderer(new Vector3(position.x, position.y, 0.1f), angle);
                        spriteRenderer.sprite = additionalModel.Sprite;
                        spriteRenderer.color = Color;
                        _directionToSpriteRendererIndex[direction] = spriteRenderer;
                    }
                }
                else
                {
                    if (exists)
                    {
                        _directionToSpriteRendererIndex.Remove(direction);
                        Destroy(spriteRenderer.gameObject);
                    }
                }
            }
        }

        public void SetPosition(Vector2Int position)
        {
            _position = position;
            transform.localPosition = (Vector2)_position;
        }

        public IEnumerable<SpriteRenderer> GetRenderers()
        {
            if (_spriteRenderer != null)
            {
                yield return _spriteRenderer;
            }

            foreach (SpriteRenderer spriteRenderer in _directionToSpriteRendererIndex.Select(x => x.Value))
            {
                yield return spriteRenderer;
            }
        }

        private SpriteRenderer CreateSpriteRenderer(Vector3 position, float angle)
        {
            var childGameObject = new GameObject("RoadRenderer");
            var result = childGameObject.AddComponent<SpriteRenderer>();
            childGameObject.transform.SetParent(transform, false);
            childGameObject.transform.localPosition = position;
            childGameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            return result;
        }

        private Vector2 GetSpriteRendererPosition(Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.DownRight: return new Vector2(0.5f, -0.5f);
                case Direction8.DownLeft: return new Vector2(0.5f, -0.5f);
                case Direction8.UpLeft: return new Vector2(-0.5f, 0.5f);
                case Direction8.UpRight: return new Vector2(1.5f, 0.5f);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}