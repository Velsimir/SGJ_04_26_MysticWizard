using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    [CreateAssetMenu(fileName =  "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _topBorder;
        [SerializeField] private float _botBorder;

        public float speed => _speed;
        public float topBorder => _topBorder;
        public float botBorder => _botBorder;
    }
}