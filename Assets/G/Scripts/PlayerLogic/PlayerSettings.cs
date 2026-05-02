using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    [CreateAssetMenu(fileName =  "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField ]private Sprite _classicSprite;
        [SerializeField ]private Sprite _rabbitSprite;
        [SerializeField ]private Sprite _hydraSprite;
        [SerializeField ]private Sprite _salamanderSprite;
        [SerializeField ]private Sprite _sheepSprite;
        [SerializeField] private float _speed;
        [SerializeField] private float _topBorder;
        [SerializeField] private float _botBorder;

        public float Speed => _speed;
        public float TopBorder => _topBorder;
        public float BotBorder => _botBorder;
        public Sprite ClassicSprite => _classicSprite;
        public Sprite RabbitSprite => _rabbitSprite;
        public Sprite HydraSprite => _hydraSprite;
        public Sprite SalamanderSprite => _salamanderSprite;
        public Sprite SheepSprite => _sheepSprite;
    }
}