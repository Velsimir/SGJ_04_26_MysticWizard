using G.Scripts.Services.SoundService;

namespace G.Scripts.ShootersLogic
{
    public class SheepShooter : BaseShooter
    {
        private Sound _sound;
        
        public SheepShooter(float timeBetweenShots) 
            : base(timeBetweenShots, G.Instance.Bullets.ClassicBulletPrefab)
        {
            _sound = G.Instance.Services.GetService<Sound>();
        }

        public override void Shoot()
        {
            _sound.PlaySFX(_sound.авечка);
        }
    }
}