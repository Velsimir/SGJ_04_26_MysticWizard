using G.Scripts.Services.ArrowSequence;
using G.Scripts.Services.Input;
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
        private PlayerComboSystem _arrowSequenceHandler;

        public Transform ClassicShootPoint => _classicShootPoint;
        
        private void Awake()
        {
            _playerController = new PlayerController(transform, _playerSettings);
            G.Instance.Player = this;

            m_shooter = new ClassicShooter();

            _arrowSequenceHandler = new PlayerComboSystem();
            _arrowSequenceHandler.Start();
            _arrowSequenceHandler.GiveNewCombo(3);
        }
    }
}