using G.Scripts.Services.SoundService;
using UnityEngine;

namespace G.Scripts.ShootersLogic
{
    public class SalamanderShooter : BaseShooter
    {
        private Sound _sound;
        
        public SalamanderShooter(float timeBetweenShots) 
            : base(timeBetweenShots, G.Instance.Bullets.SalamanderBullet)
        {
            _sound = G.Instance.Services.GetService<Sound>();
        }

        public override void Shoot()
        {
            _sound.PlaySFX(_sound.выстрелОгнем);
            
            Vector3 pos = _shootPoint.position;
            Quaternion rot = _shootPoint.rotation;

            _spawnerService.Spawn(_bulletPrefab, pos + new Vector3(0, 3f, 0), rot);
            _spawnerService.Spawn(_bulletPrefab, pos + new Vector3(0, 2f, 0), rot);
            _spawnerService.Spawn(_bulletPrefab, pos + new Vector3(0, 1f, 0), rot);
            _spawnerService.Spawn(_bulletPrefab, pos + new Vector3(0, 0f, 0), rot);
            _spawnerService.Spawn(_bulletPrefab, pos + new Vector3(0, -1f, 0), rot);
            _spawnerService.Spawn(_bulletPrefab, pos + new Vector3(0, -2f, 0), rot);
            _spawnerService.Spawn(_bulletPrefab, pos + new Vector3(0, -3f, 0), rot);
        }
    }
}