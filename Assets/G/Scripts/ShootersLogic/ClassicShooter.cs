using System;
using G.Scripts.BulletLogic;
using G.Scripts.Services.Spawner;
using G.Scripts.Services.Update;
using G.Scripts.ShootersLogic;
using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    public class ClassicShooter : IShooter
    {
        private ISpawnerService _spawnerService;
        private Bullet _bulletPrefab;
        private float _lastShootTime;
        private Transform _shootPosition;
        
        private readonly float _timeBetweenShoot = 1f;
        
        public ClassicShooter()
        {
            _shootPosition = G.Instance.Player.ClassicShootPoint;
            
            _spawnerService = G.Instance.Services.GetService<ISpawnerService>();
            _bulletPrefab = G.Instance.Bullets.ClassicBulletPrefab;
            
            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
        }

        public void Shoot()
        {
            _spawnerService.Spawn(_bulletPrefab, _shootPosition, null);
        }

        public void FixedUpdate(float deltaTime)
        { }

        public void Update(float deltaTime)
        {
            if (_lastShootTime >= _timeBetweenShoot)
            {
                Shoot();
                _lastShootTime = 0;
            }
            
            _lastShootTime += deltaTime;
        }

        public void LateUpdate(float deltaTime)
        { }

        public void Dispose()
        {
            G.Instance.Services.GetService<IUpdateService>().Remove(this);
        }
    }
}