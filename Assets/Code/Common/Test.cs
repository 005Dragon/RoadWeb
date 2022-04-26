using UnityEngine;

namespace Code.Common
{
    public class Test : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            Vector2 position = _camera.ScreenToWorldPoint(Input.mousePosition);
            
            Debug.Log(position);
        }
    }
}