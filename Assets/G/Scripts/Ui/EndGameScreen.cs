using System.Collections;
using G.Scripts.SceneLoader;
using G.Scripts.Services.Update;
using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.Ui
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _endButtonImage;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(RestartGame);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(RestartGame);
        }

        public void Show()
        {
            G.Instance.Services.GetService<IUpdateService>().SetTimeScale(0.01f);
            gameObject.SetActive(true);
            StartCoroutine(ShowButtonWithDelay());
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator ShowButtonWithDelay()
        {
            yield return new WaitForSeconds(1.5f);
            _endButtonImage.SetActive(true);
            _button.gameObject.SetActive(true);
        }

        private void RestartGame()
        {
            G.Instance.Services.GetService<ISceneLoaderService>().LoadScene("PlayRoom");
        }
    }
}