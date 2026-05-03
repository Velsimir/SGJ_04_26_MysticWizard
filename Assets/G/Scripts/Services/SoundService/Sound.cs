using G.Scripts.Services.Input;
using UnityEngine;
using R3;

namespace G.Scripts.Services.SoundService
{
    public class Sound : MonoBehaviour, IService
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _loopMusic;

        [Header("Громкость")] [SerializeField] private float _sfxVolume = 1f;
        [SerializeField] private float _musicVolume = 1f;

        public AudioClip авечка;
        public AudioClip выстрелВодой;
        public AudioClip выстрелОгнем;
        public AudioClip выстрел1;
        public AudioClip метаморфоза;
        public AudioClip музыка;
        public AudioClip мяурлин;
        public AudioClip нажатие;
        public AudioClip пуф;
        public AudioClip смертьМыши1;
        public AudioClip смертьмыши2;
        public AudioClip смертьМышовура;
        public AudioClip смертьМяурлина;
        public AudioClip урон;
        public AudioClip феечка;
        public AudioClip уронМыши1;
        public AudioClip уронМыши2;

        private IInputService _inputService;

        // Свойства для управления громкостью
        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                _sfxVolume = Mathf.Clamp01(value);
                _audioSource.volume = _sfxVolume;
            }
        }

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = Mathf.Clamp01(value);
                _loopMusic.volume = _musicVolume;
            }
        }

        // Общая громкость (для слайдера)
        public float MasterVolume
        {
            get => AudioListener.volume;
            set
            {
                AudioListener.volume = Mathf.Clamp01(value);
                SfxVolume = value;
                MusicVolume = value;
            }
        }

        private void Awake()
        {
            G.Instance.Services.AddService(this);
        }

        private void Start()
        {
            _inputService = G.Instance.Services.GetService<IInputService>();

            // Применяем начальную громкость
            _audioSource.volume = _sfxVolume;
            _loopMusic.volume = _musicVolume;

            _inputService.Attack.Subscribe(isAttacking =>
            {
                if (isAttacking)
                {
                    PlaySFX(нажатие);
                }
            });

            StartMusic();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip != null)
                _audioSource.PlayOneShot(clip, _sfxVolume);
        }

        public void StartMusic()
        {
            _loopMusic.clip = музыка;
            _loopMusic.volume = _musicVolume;
            _loopMusic.Play();
        }

        public void StopMusic()
        {
            _loopMusic.Stop();
        }

        private void OnDestroy()
        {
            G.Instance.Services.RemoveService<Sound>();
        }
    }
}