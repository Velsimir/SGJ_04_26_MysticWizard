using G.Scripts.Services.SoundService;

namespace G.Scripts.ShootersLogic
{
    public class RabbitShooter : BaseShooter
    {
        private Sound _sound;
        
        public RabbitShooter(float timeBetweenShots) 
            : base(timeBetweenShots, G.Instance.Bullets.RabbitBulletPrefab)
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