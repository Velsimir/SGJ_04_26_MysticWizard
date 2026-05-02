using G.Scripts.ShootersLogic;

namespace G.Scripts.PlayerLogic
{
    public class ClassicShooter : BaseShooter
    {
        public ClassicShooter(float timeBetweenShots) 
            : base(timeBetweenShots, G.Instance.Bullets.ClassicBulletPrefab)
        {
        }
    }
}