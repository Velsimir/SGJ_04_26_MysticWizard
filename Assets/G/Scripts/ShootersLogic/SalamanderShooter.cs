using UnityEngine;

namespace G.Scripts.ShootersLogic
{
    public class SalamanderShooter : BaseShooter
    {
        public SalamanderShooter(float timeBetweenShots) 
            : base(timeBetweenShots, G.Instance.Bullets.SalamanderBullet)
        {
        }

        public override void Shoot()
        {
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