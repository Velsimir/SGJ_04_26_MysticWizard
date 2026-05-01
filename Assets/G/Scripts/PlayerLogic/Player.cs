using G.Scripts.ShootersLogic;
using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private Transform _classicShootPoint;
        
        PlayerController _playerController;
        IShooter m_shooter;

        public Transform ClassicShootPoint => _classicShootPoint;
        
        private void Awake()
        {
            _playerController = new PlayerController(transform, _playerSettings);
            G.Instance.Player = this;

            m_shooter = new ClassicShooter();
        }
    }
}