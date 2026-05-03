using G.Scripts.Services.SoundService;
using G.Scripts.ShootersLogic;

namespace G.Scripts.PlayerLogic
{
    public class ClassicShooter : BaseShooter
    {
        private Sound _sound;
        
        public ClassicShooter(float timeBetweenShots) 
            : base(timeBetweenShots, G.Instance.Bullets.ClassicBulletPrefab)
        {
            _sound = G.Instance.Services.GetService<Sound>();
        }

        public override void Shoot()
        {
            base.Shoot();
            _sound.PlaySFX(_sound.выстрел1);
        }
    }
}