using System;
using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class BulletsHandler : MonoBehaviour
    {
        [SerializeField] private  Bullet _rabbitBulletPrefab;
        [SerializeField] private Bullet _classicBulletPrefab;
        [SerializeField] private Bullet _hydraBulletPrefab;
        [SerializeField] private Bullet _salamanderBullet;
    
        public Bullet ClassicBulletPrefab => _classicBulletPrefab;
        public Bullet HydraPrefab => _hydraBulletPrefab;
        public Bullet RabbitBulletPrefab => _rabbitBulletPrefab;
        public Bullet SalamanderBullet => _salamanderBullet;
    }
}