using G.Scripts.Services.ArrowSequence;
using G.Scripts.ShootersLogic;
using G.Scripts.Ui;
using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private Transform _classicShootPoint;
        [SerializeField] private Rigidbody2D _rigibody;
        [SerializeField] private ComboSequenceView _comboView;
        
        private PlayerController _playerController;
        private IShooter _shooter;
        private PlayerComboSystem _arrowSequenceHandler;

        public Transform ClassicShootPoint => _classicShootPoint;
        
        private void Awake()
        {
            _playerController = new PlayerController(_rigibody, _playerSettings);
            G.Instance.Player = this;

            _shooter = new ClassicShooter();

            _arrowSequenceHandler = new PlayerComboSystem(_comboView);
            _arrowSequenceHandler.GiveNewCombo(3);
        }

        private void OnDestroy()
        {
            _shooter.Dispose();
            _arrowSequenceHandler.Dispose();
            _playerController.Dispose();
        }

        public void MakeSuccessMetamorph()
        {
            Debug.Log("Player make morph!");
        }

        public void MakeBadMetamorph()
        {
            Debug.Log("Player became sheep!");
        }
    }
}