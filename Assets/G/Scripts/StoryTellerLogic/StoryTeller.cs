using System.Collections;
using G.Scripts.EnemyLogic;
using G.Scripts.PlayerLogic;
using G.Scripts.Services.Update;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.StoryTellerLogic
{
    public class StoryTeller : MonoBehaviour
    {
        [Header("Персонажи")] [SerializeField] private Animator _wizardAnimator;
        [SerializeField] private Animator _fairyAnimator;
        [SerializeField] private Animator _textBarAnimator;

        [Header("UI")] [SerializeField] private TMP_Text _storyText;
        [SerializeField] private Button _nextButton;

        [Header("Волны для историй")] [SerializeField]
        private int _waveToFirstStory = 1;

        [SerializeField] private int _waveToSecondStory = 5;
        [SerializeField] private int _waveToThirdStory = 10;
        [SerializeField] private int _waveToForthStory = 15;

        private PlayerMetamorphSystem _metamorphSystem;
        private EnemyWaveSpawner _enemyWaveSpawner;
        private IUpdateService _updateService;

        private bool _isShowingStory = false;
        private bool _nextClicked = false;

        private readonly DialogueLine[] _story1 =
        {
            new DialogueLine(Speaker.Fairy,
                "Я придумала! Мы сами используем заклинания метаморфоз из книги, чтобы победить мышей! Скорее прочитай их!"),
            new DialogueLine(Speaker.Wizard, "Я не могу их прочитать."),
            new DialogueLine(Speaker.Fairy, "Это ещё почему? Ты же волшебник!"),
            new DialogueLine(Speaker.Wizard, "У меня лапки!"),
            new DialogueLine(Speaker.Fairy, "Ладно, дай мне немного времени, я прочитаю их для тебя!")
        };

        private readonly DialogueLine[] _story2 =
        {
            new DialogueLine(Speaker.Fairy,
                "Так, кажется разобралась! Попробуй это заклинание: *Набор стрелочек для активации*"),
            new DialogueLine(Speaker.Wizard,
                "Блиииин, мне что ещё и самому их творить нужно? О таком заранее предупреждают…"),
            new DialogueLine(Speaker.Fairy, "Ты самый ленивый волшебник, которого я встречала!")
        };

        private readonly DialogueLine[] _story3 =
        {
            new DialogueLine(Speaker.Fairy,
                "Их всё больше! Нужно что-то помощнее. Попробуй другое заклинание! *Комбинация стрелочек*"),
            new DialogueLine(Speaker.Wizard,
                "У меня по расписанию сейчас третий завтрак. Я надеюсь мне кто-то компенсирует такие невыносимые условия?"),
            new DialogueLine(Speaker.Fairy, "Составишь жалобу в волшебный профсоюз после битвы. А теперь за работу!")
        };

        private readonly DialogueLine[] _story4 =
        {
            new DialogueLine(Speaker.Fairy,
                "Ооооо. Нашла! Это заклинание, возможно, даже вне закона. Скорее, Используй его!"),
            new DialogueLine(Speaker.Wizard, "Что?! А батон с паштетом ты мне будешь в темницу носить?!"),
            new DialogueLine(Speaker.Fairy, "Если не используешь его - нас самих в паштет превратят! Не капризничай!")
        };

        private void Start()
        {
            _metamorphSystem = G.Instance.Services.GetService<PlayerMetamorphSystem>();
            _enemyWaveSpawner = G.Instance.Services.GetService<EnemyWaveSpawner>();
            _updateService = G.Instance.Services.GetService<IUpdateService>();
            
            if (_enemyWaveSpawner != null)
                _enemyWaveSpawner.OnWaveSpawned += TryShowNextStoryChapter;

            _nextButton.onClick.AddListener(OnNextButtonClicked);

            HideCharacters();
        }

        private void TryShowNextStoryChapter(int currentWave)
        {
            if (_isShowingStory) return;
            
            DialogueLine[] dialogue = currentWave switch
            {
                var w when w == _waveToFirstStory => _story1,
                var w when w == _waveToSecondStory => _story2,
                var w when w == _waveToThirdStory => _story3,
                var w when w == _waveToForthStory => _story4,
                _ => null
            };

            if (dialogue != null)
                StartCoroutine(PlayStoryDialogue(dialogue));
        }

        private IEnumerator PlayStoryDialogue(DialogueLine[] dialogue)
        {
            _updateService.LerpTimeScale(0.05f, 1f);
            _textBarAnimator.SetBool("IsVisible", true);
            
            _isShowingStory = true;
            _metamorphSystem?.StopSystem();
            _nextButton.gameObject.SetActive(true);
            _storyText.text = "";

            foreach (var line in dialogue)
            {
                // Показываем персонажа
                if (line.Speaker == Speaker.Fairy)
                    _fairyAnimator.SetBool("IsVisible", true);
                else
                    _wizardAnimator.SetBool("IsVisible", true);

                // Ждём окончания анимации появления
                yield return new WaitForSeconds(0.6f);

                // Показываем текст
                _storyText.text = line.Text;

                // Ждём нажатия кнопки
                _nextClicked = false;
                yield return new WaitUntil(() => _nextClicked);

                // Скрываем текст и персонажа
                _storyText.text = "";
                if (line.Speaker == Speaker.Fairy)
                    _fairyAnimator.SetBool("IsVisible", false);
                else
                    _wizardAnimator.SetBool("IsVisible", false);

                yield return new WaitForSeconds(0.4f);
            }

            // Завершение истории
            HideCharacters();
            _nextButton.gameObject.SetActive(false);
            _storyText.text = "";

            _metamorphSystem?.StartSystem();
            _isShowingStory = false;
        }

        private void OnNextButtonClicked()
        {
            _nextClicked = true;
        }

        private void HideCharacters()
        {
            _wizardAnimator.SetBool("IsVisible", false);
            _fairyAnimator.SetBool("IsVisible", false);
            _textBarAnimator.SetBool("IsVisible", false);
            _updateService.LerpTimeScale(1f, 1f);
        }

        private void OnDestroy()
        {
            if (_enemyWaveSpawner != null)
                _enemyWaveSpawner.OnWaveSpawned -= TryShowNextStoryChapter;
        }
    }
}