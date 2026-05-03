using System;
using System.Collections;
using G.Scripts.PlayerLogic;
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
        [SerializeField] private GameObject _background;
        
        private void OnEnable()
        {
            PlayerDamageZone.isBossWin += Show;
            _button.onClick.AddListener(RestartGame);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(RestartGame);
            PlayerDamageZone.isBossWin -= Show;
        }

        public void Show()
        {
            G.Instance.Services.GetService<IUpdateService>().SetTimeScale(0.01f);
            _background.SetActive(true);
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