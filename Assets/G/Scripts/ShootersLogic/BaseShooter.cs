using G.Scripts.BulletLogic;
using G.Scripts.Services.Spawner;
using G.Scripts.Services.Update;
using UnityEngine;

namespace G.Scripts.ShootersLogic
{
    public abstract class BaseShooter : IShooter
    {
        protected readonly ISpawnerService _spawnerService;
        protected readonly Bullet _bulletPrefab;
        protected readonly Transform _shootPoint;
        protected readonly float _timeBetweenShots;

        private float _lastShootTime;

        protected BaseShooter(float timeBetweenShots, Bullet bulletPrefab)
        {
            _timeBetweenShots = timeBetweenShots;
            _bulletPrefab = bulletPrefab;
            _shootPoint = G.Instance.Player.ClassicShootPoint;
        
            _spawnerService = G.Instance.Services.GetService<ISpawnerService>();
        
            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
        }

        public virtual void Shoot()
        {
            // По умолчанию — одиночный выстрел
            _spawnerService.Spawn(_bulletPrefab, _shootPoint.position, _shootPoint.rotation);
        }

        public void FixedUpdate(float deltaTime) { }

        public void Update(float deltaTime)
        {
            _lastShootTime += deltaTime;

            if (_lastShootTime >= _timeBetweenShots)
            {
                Shoot();
                _lastShootTime = 0f;
            }
        }

        public void LateUpdate(float deltaTime) { }

        public virtual void Dispose()
        {
            G.Instance.Services.GetService<IUpdateService>().Remove(this);
        }
    }
}