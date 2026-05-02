using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class BulletRotation : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 45f;
        [SerializeField] private bool _clockwise = true;
    
        private void Update()
        {
            float direction = _clockwise ? -1f : 1f;
            transform.Rotate(0f, 0f, _rotationSpeed * direction * Time.deltaTime);
        }
    }
}