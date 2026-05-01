using G.Scripts.Services.Spawner;
using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class BulletWallDespawner : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet poolable))
                poolable.Despawn();
        }
    }
}