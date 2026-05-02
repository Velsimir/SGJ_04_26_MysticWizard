using System;
using G.Scripts.Services.ArrowSequence;
using G.Scripts.Services.Update;
using G.Scripts.ShootersLogic;
using G.Scripts.Ui;
using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    public class PlayerMetamorphSystem : IDisposable, IUpdatable
    {
        private readonly Player _player;
        private readonly ComboSequenceView _comboView;
        private readonly Animator _animator;
        private readonly SpriteRenderer _visual;
        private readonly PlayerSettings _settings;

        private PlayerType _currentType = PlayerType.Classic;
        private int _successStreak = 0;

        private IShooter _currentShooter;
        private PlayerComboSystem _comboSystem;

        private float _returnTimer;
        private bool _isReturningToClassic;

        public PlayerType CurrentType => _currentType;
        public int SuccessStreak => _successStreak;

        public PlayerMetamorphSystem(Player player, ComboSequenceView comboView, Animator animator,
            SpriteRenderer visual, PlayerSettings settings)
        {
            _player = player;
            _comboView = comboView;
            _animator = animator;
            _visual = visual;
            _settings = settings;

            _comboSystem = new PlayerComboSystem(comboView);
            _comboSystem.OnSuccess += HandleComboSuccess;
            _comboSystem.OnFail += HandleComboFail;

            G.Instance.Services.GetService<IUpdateService>().AddNew(this);
        }

        public void ResetToClassic()
        {
            _currentType = PlayerType.Classic;
            _successStreak = 0;
            _isReturningToClassic = false;

            ApplyCurrentForm();
            StartNewCombo();
        }

        public void ApplyCurrentForm()
        {
            switch (_currentType)
            {
                case PlayerType.Classic:
                    _visual.sprite = _settings.ClassicSprite;
                    ChangeShooter(new ClassicShooter(_player.ClassicTimeBetweenShoots));
                    break;

                case PlayerType.Rabbit:
                    _visual.sprite = _settings.RabbitSprite;
                    ChangeShooter(new RabbitShooter(_player.ExtraTimeBetweenShoots));
                    break;

                case PlayerType.Hydra:
                    _visual.sprite = _settings.HydraSprite;
                    ChangeShooter(new HydraShooter(_player.ClassicTimeBetweenShoots));
                    break;

                case PlayerType.Salamander:
                    _visual.sprite = _settings.SalamanderSprite;
                    ChangeShooter(new SalamanderShooter(_player.ClassicTimeBetweenShoots));
                    break;

                case PlayerType.Sheep:
                    _visual.sprite = _settings.SheepSprite;
                    ChangeShooter(null);
                    break;
            }
        }

        private void ChangeShooter(IShooter newShooter)
        {
            _currentShooter?.Dispose();
            _currentShooter = newShooter;
        }

        private void StartNewCombo()
        {
            int length = Mathf.Clamp(_player.ComboLengthStart + _successStreak * _player.ComboLengthIncrease,
                _player.ComboLengthStart, _player.MaxComboLength);

            _comboSystem.GiveNewCombo(length);
        }

        private void HandleComboSuccess()
        {
            _successStreak++;

            PlayerType nextType = _successStreak switch
            {
                1 => PlayerType.Rabbit,
                2 => PlayerType.Hydra,
                3 or > 3 => PlayerType.Salamander,
                _ => PlayerType.Classic
            };

            _currentType = nextType;
            _animator.SetTrigger("Morph");

            _isReturningToClassic = true;
            _returnTimer = _player.TimeToReturnToClassic;
        }

        private void HandleComboFail()
        {
            _successStreak = 0;
            _currentType = PlayerType.Sheep;
            _animator.SetTrigger("Morph");

            _isReturningToClassic = true;
            _returnTimer = _player.TimeToReturnToClassic;
        }

        public void FixedUpdate(float deltaTime)
        { }

        public void Update(float deltaTime)
        {
            _comboSystem.Update(deltaTime);

            if (_isReturningToClassic)
            {
                _returnTimer -= deltaTime;
                if (_returnTimer <= 0f)
                {
                    _currentType = PlayerType.Classic;
                    _animator.SetTrigger("Morph");
                    _isReturningToClassic = false;
                }
            }
        }

        public void LateUpdate(float deltaTime)
        { }

        public void Dispose()
        {
            _comboSystem.OnSuccess -= HandleComboSuccess;
            _comboSystem.OnFail -= HandleComboFail;
            _comboSystem.Dispose();
            _currentShooter?.Dispose();
            G.Instance.Services.GetService<IUpdateService>().Remove(this);
        }

        public void OnMorphAnimationEnd()
        {
            ApplyCurrentForm();

            if (_currentType == PlayerType.Classic)
                StartNewCombo();
        }
    }
}