using G.Scripts.BulletLogic;
using G.Scripts.PlayerLogic;
using G.Scripts.Services.SoundService;
using G.Scripts.Services.Spawner;
using G.Scripts.Services.Update;
using UnityEngine;

namespace G.Scripts.ShootersLogic
{
    public class HydraShooter : BaseShooter
    {
        private Sound _sound;
        
        public HydraShooter(float timeBetweenShots) 
            : base(timeBetweenShots, G.Instance.Bullets.HydraPrefab)
        {
            _sound =  G.Instance.Services.GetService<Sound>();
        }

        public override void Shoot()
        {
            _sound.PlaySFX(_sound.выстрелВодой);
            
            Vector3 pos = _shootPoint.position;
            Quaternion rot = _shootPoint.rotation;

            _spawnerService.Spawn(_bulletPrefab, pos + new Vector3(0, 0.5f, 0), rot);
            _spawnerService.Spawn(_bulletPrefab, pos + new Vector3(0, -0.5f, 0), rot);
        }
    }
}