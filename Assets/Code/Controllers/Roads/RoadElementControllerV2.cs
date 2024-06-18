using Code.Data;
using UnityEngine;

namespace Code.Controllers.Roads
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RoadElementControllerV2 : MonoBehaviour
    {
        public Vector2 Position
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Color Color
        {
            get => _spriteRenderer.color;
            set => _spriteRenderer.color = value;
        }

        public int SortingOrder
        {
            get => _spriteRenderer.sortingOrder;
            set => _spriteRenderer.sortingOrder = value;
        }

        private RoadElementModel _model;
        private SpriteRenderer _spriteRenderer;

        public void Initialize(RoadElementModel model)
        {
            _model = model;
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (!Validate())
            {
                return;
            }

            _spriteRenderer.sprite = _model.Sprite;
            
            gameObject.SetActive(true);
        }
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private bool Validate()
        {
            return _model != null && _spriteRenderer != null;
        }
    }
}