using UnityEngine;

namespace G.Scripts.BulletLogic
{
    [CreateAssetMenu(fileName = "BulletSettings", menuName = "BulletLogic/BulletSettings")]
    public class BulletSettings : ScriptableObject
    {
        [SerializeField]
        private Bullet bulletPrefab;
        [SerializeField]
        private float _speed;

        public float Speed => this._speed;

        public Bullet BulletPrefab => this.bulletPrefab;
    }
}