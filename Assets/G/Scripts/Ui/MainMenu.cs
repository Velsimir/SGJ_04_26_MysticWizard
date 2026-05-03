using System;
using System.Collections;
using DG.Tweening;
using G.Scripts.SceneLoader;
using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.Ui
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Главные панели")] [SerializeField]
        private GameObject _characterSelectPanel;

        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _creditsPanel;

        [Header("Кнопки меню")] [SerializeField]
        private Button _chooseCharacterButton;

        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _authorsButton;

        [Header("Выбор персонажа")] [SerializeField]
        private Button _charButton1;

        [SerializeField] private Button _charButton2;
        [SerializeField] private Button _charButton3;

        [Header("Фоны и изображения для кнопок")] [SerializeField]
        private GameObject _char1Background;

        [SerializeField] private Image _char1Image;

        [SerializeField] private GameObject _char2Background;
        [SerializeField] private Image _char2Image;

        [SerializeField] private GameObject _char3Background;
        [SerializeField] private Image _char3Image;

        [Header("Настройки")] [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Button _exitFromSettingsButton;
        [SerializeField] private Button _memeButton;
        [SerializeField] private GameObject _memeRtx;
        [SerializeField] private Image _memeImage; // Если есть Image компонент для анимации

        [Header("Авторы")] [SerializeField] private Button _exitFromCreditsButton;

        private Coroutine _memeCoroutine;

        private void OnEnable()
        {
            _chooseCharacterButton.onClick.AddListener(() => OpenCharacterSelect());
            _settingsButton.onClick.AddListener(() => OpenSettings());
            _authorsButton.onClick.AddListener(() => OpenCredits());
            _exitFromSettingsButton.onClick.AddListener(() => BackToMainMenu());
            _exitFromCreditsButton.onClick.AddListener(() => BackToMainMenu());

            // Подписка кнопок
            _charButton1.onClick.AddListener(() => OnCharacterButtonClicked(1));
            _charButton2.onClick.AddListener(() => OnCharacterButtonClicked(2));
            _charButton3.onClick.AddListener(() => OnCharacterButtonClicked(3));

            _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

            _volumeSlider.value = AudioListener.volume;

            // Мем-кнопка
            _memeButton.onClick.AddListener(OnMemeButtonClicked);

            // Скрываем всё в начале
            HideAllCharacterEffects();
            _memeRtx.SetActive(false);
        }

        private void OnDisable()
        {
            _chooseCharacterButton.onClick.RemoveListener(() => OpenCharacterSelect());
            _settingsButton.onClick.RemoveListener(() => OpenSettings());
            _authorsButton.onClick.RemoveListener(() => OpenCredits());
            _exitFromSettingsButton.onClick.RemoveListener(() => BackToMainMenu());
            _exitFromCreditsButton.onClick.RemoveListener(() => BackToMainMenu());

            // Подписка кнопок
            _charButton1.onClick.RemoveListener(() => OnCharacterButtonClicked(1));
            _charButton2.onClick.RemoveListener(() => OnCharacterButtonClicked(2));
            _charButton3.onClick.RemoveListener(() => OnCharacterButtonClicked(3));

            _volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);

            _memeButton.onClick.RemoveListener(OnMemeButtonClicked);
        }

        #region Навигация

        public void OpenCharacterSelect() => ShowPanel(_characterSelectPanel);
        public void OpenSettings() => ShowPanel(_settingsPanel);
        public void OpenCredits() => ShowPanel(_creditsPanel);

        public void BackToMainMenu()
        {
            ShowPanel(null);
        }

        private void ShowPanel(GameObject activePanel)
        {
            _characterSelectPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _creditsPanel.SetActive(false);

            if (activePanel != null)
                activePanel.SetActive(true);
        }

        #endregion

        #region Выбор персонажа

        private void OnCharacterButtonClicked(int index)
        {
            switch (index)
            {
                case 1:
                    PlayCharacterAnimation(_char1Background, _char1Image, 1, () =>
                    {
                        /* Ничего не делаем */
                    });
                    break;

                case 2:
                    PlayCharacterAnimation(_char2Background, _char2Image, 2, () =>
                    {
                        /* Ничего не делаем */
                    });
                    break;

                case 3:
                    PlayCharacterAnimation(_char3Background, _char3Image, 3, StartGame);
                    break;
            }
        }

        private void PlayCharacterAnimation(GameObject background, Image image, int index, System.Action onComplete)
        {
            Button button;
            // Включаем фон
            background.SetActive(true);

            // Сбрасываем прозрачность изображения
            Color color = image.color;
            color.a = 0f;
            image.color = color;
            image.gameObject.SetActive(true);

            switch (index)
            {
                case 1:
                    button = _charButton1;
                    break;

                case 2:
                    button = _charButton2;
                    break;

                case 3:
                    button = _charButton3;
                    break;

                default:
                    button = _charButton1;
                    break;
            }

            button.interactable = false;
            // Появление

            image.DOFade(1f, 0.3f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                // Исчезновение
                image.DOFade(0f, 0.9f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    background.SetActive(false);
                    image.gameObject.SetActive(false);
                    button.interactable = true;
                    onComplete?.Invoke();
                });
            });
        }

        private void StartGame()
        {
            G.Instance.Services.GetService<ISceneLoaderService>().LoadScene("PlayRoom");
        }

        private void HideAllCharacterEffects()
        {
            _char1Background.SetActive(false);
            _char2Background.SetActive(false);
            _char3Background.SetActive(false);

            _char1Image.gameObject.SetActive(false);
            _char2Image.gameObject.SetActive(false);
            _char3Image.gameObject.SetActive(false);
        }

        #endregion

        #region Настройки

        private void OnVolumeChanged(float value)
        {
            AudioListener.volume = value;
        }

        #endregion

        #region Мем-функционал

        private void OnMemeButtonClicked()
        {
            // Если уже показываем мем - перезапускаем
            if (_memeCoroutine != null)
                StopCoroutine(_memeCoroutine);

            _memeCoroutine = StartCoroutine(ShowMemeRoutine());
        }

        private IEnumerator ShowMemeRoutine()
        {
            _memeRtx.SetActive(true);

            // Если есть Image компонент, делаем красивую анимацию появления
            if (_memeImage != null)
            {
                Color color = _memeImage.color;
                color.a = 0f;
                _memeImage.color = color;
                _memeImage.DOFade(1f, 0.3f).SetEase(Ease.InOutSine);

                // Ждём 1.4 секунды (чтобы с анимацией было 2 секунды)
                yield return new WaitForSeconds(2.5f);

                // Исчезновение
                _memeImage.DOFade(0f, 0.3f).SetEase(Ease.InOutSine).OnComplete(() => { _memeRtx.SetActive(false); });
            }
            else
            {
                // Если нет Image, просто показываем на 2 секунды
                yield return new WaitForSeconds(2f);
                _memeRtx.SetActive(false);
            }

            _memeCoroutine = null;
        }

        #endregion
    }
}