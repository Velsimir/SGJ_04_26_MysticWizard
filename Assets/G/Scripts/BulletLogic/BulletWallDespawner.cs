using G.Scripts.EnemyLogic;
using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class BulletWallDespawner : MonoBehaviour
    {
        [SerializeField] private DespawnerType _despawnerType;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (_despawnerType)
            {
                case DespawnerType.Enemies:
                    if (other.TryGetComponent(out Enemy enemy))
                        enemy.Despawn();
                    break;
                
                case DespawnerType.Bullets:
                    if (other.TryGetComponent(out Bullet bullet))
                        bullet.Despawn();
                    break;
            }
            
        }
    }

    public enum DespawnerType
    {
        Enemies,
        Bullets
    }
}