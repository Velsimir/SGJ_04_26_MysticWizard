namespace G.Scripts.ShootersLogic
{
    public class RabbitShooter : BaseShooter
    {
        public RabbitShooter(float timeBetweenShots) 
            : base(timeBetweenShots, G.Instance.Bullets.RabbitBulletPrefab)
        {
        }
    }
}