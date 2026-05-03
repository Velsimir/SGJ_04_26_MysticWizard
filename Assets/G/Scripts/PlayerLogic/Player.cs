using G.Scripts.Ui;
using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private Transform _classicShootPoint;
        [SerializeField] private Rigidbody2D _rigibody;
        [SerializeField] private ComboSequenceView _comboView;
        [SerializeField] private SpriteRenderer _playerVisual;

        public float ClassicTimeBetweenShoots = 0.25f;
        public float ExtraTimeBetweenShoots = 0.4f;
        public float TimeToReturnToClassic  = 8f;
        public int ComboLengthStart  = 3;
        public int ComboLengthIncrease  = 1;
        public int MaxComboLength = 9;

        private PlayerController _playerController;
        private PlayerMetamorphSystem _metamorphSystem;

        public Transform ClassicShootPoint => _classicShootPoint;
        public PlayerMetamorphSystem MetamorphSystem => _metamorphSystem;

        private void Awake()
        {
            _playerController = new PlayerController(_rigibody, _animator, _playerSettings);
    
            G.Instance.Player = this;

            _metamorphSystem = new PlayerMetamorphSystem(this, _comboView, _animator, _playerVisual, _playerSettings);
            _metamorphSystem.ResetToClassic();
        }

        private void OnDestroy()
        {
            _metamorphSystem.Dispose();
            _playerController.Dispose();
        }

        public void OnMorphAnimationEnd()
        {
            _metamorphSystem.OnMorphAnimationEnd();
        }
    }

    public enum PlayerType
    {
        Classic,
        Rabbit,
        Hydra,
        Salamander,
        Sheep
    }
}