using System;
using G.Scripts.Services;
using G.Scripts.Services.ArrowSequence;
using G.Scripts.Services.SoundService;
using G.Scripts.Services.Update;
using G.Scripts.ShootersLogic;
using G.Scripts.Ui;
using UnityEngine;

namespace G.Scripts.PlayerLogic
{
    public class PlayerMetamorphSystem : IDisposable, IUpdatable, IService
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
        private Sound _sound;
        
        private float _returnTimer;
        private bool _isReturningToClassic;
        private bool _isWorking;

        public PlayerType CurrentType => _currentType;
        public int SuccessStreak => _successStreak;
        public int MaxStreakLevel { get; private set; } = 0;

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
            _sound = G.Instance.Services.GetService<Sound>();
            G.Instance.Services.AddService(this);
        }

        public void Update(float deltaTime)
        {
            if (_isWorking == false)
                return;
            
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

        public void FixedUpdate(float deltaTime)
        { }

        public void LateUpdate(float deltaTime)
        { }
        
        public void IncreaseMetamorphLevel()
        {
            MaxStreakLevel = Mathf.Min(MaxStreakLevel + 1, 3); // максимум 3 уровня
            _successStreak = MaxStreakLevel;
            Debug.Log($"Метаморфозы улучшены! Уровень: {MaxStreakLevel}");
        }

        public void ResetToClassic()
        {
            _currentType = PlayerType.Classic;
            _successStreak = 0;
            _isReturningToClassic = false;

            ApplyCurrentForm();

            if (MaxStreakLevel > 0)
                StartNewCombo();
        }

        public void ForceClassic()
        {
            _currentType = PlayerType.Classic;
            _successStreak = 0;
            _isReturningToClassic = false;

            ApplyCurrentForm();
        }

        public void StartSystem()
        {
            _isWorking = true;
            _comboSystem.StartSystem();
            StartNewCombo();
        }

        public void StopSystem()
        {
            _isWorking = false;
            _comboSystem.StopSystem();
        }

        private void ApplyCurrentForm()
        {
            _sound.PlaySFX(_sound.метаморфоза);
            
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
                    ChangeShooter(new SheepShooter(_player.ClassicTimeBetweenShoots));
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
            int length;
    
            switch (MaxStreakLevel)
            {
                case 1:
                    length = 3;
                    break;
                case 2:
                    length = 5;
                    break;
                default: // 3 и выше
                    length = 7;
                    break;
            }
    
            length = Mathf.Clamp(length, _player.ComboLengthStart, _player.MaxComboLength);
    
            _comboSystem.GiveNewCombo(length);
        }

        private void HandleComboSuccess()
        {
            _successStreak = Mathf.Min(_successStreak + 1, MaxStreakLevel);

            PlayerType nextType = _successStreak switch
            {
                1 => PlayerType.Rabbit,
                2 => PlayerType.Hydra,
                3 => PlayerType.Salamander,
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

        public void Dispose()
        {
            _comboSystem.OnSuccess -= HandleComboSuccess;
            _comboSystem.OnFail -= HandleComboFail;
            _comboSystem.Dispose();
            _currentShooter?.Dispose();
            G.Instance.Services.GetService<IUpdateService>().Remove(this);
            G.Instance.Services.RemoveService<PlayerMetamorphSystem>();;
        }

        public void OnMorphAnimationEnd()
        {
            ApplyCurrentForm();

            if (_currentType == PlayerType.Classic)
                StartNewCombo();
        }
    }
}