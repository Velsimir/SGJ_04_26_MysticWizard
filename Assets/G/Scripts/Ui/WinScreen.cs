using System;
using G.Scripts.EnemyLogic;
using G.Scripts.SceneLoader;
using G.Scripts.Services.Update;
using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.Ui
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private Button _toMenuButton;
        [SerializeField] private GameObject _comics;
        [SerializeField] private GameObject _finalGameScreen;
        
        private IUpdateService _updateService;

        private void OnEnable()
        {
            _toMenuButton.onClick.AddListener(ReturnToMenu);
            Enemy.Killed += Show;
        }

        private void Start()
        {
            _updateService = G.Instance.Services.GetService<IUpdateService>();
        }

        private void OnDisable()
        {
            _toMenuButton.onClick.AddListener(ReturnToMenu);
            Enemy.Killed -= Show;
        }

        private void Show()
        {
            _comics.SetActive(true);
            _updateService.SetTimeScale(0.01f);
        }

        private void ReturnToMenu()
        {
            G.Instance.Services.GetService<ISceneLoaderService>().LoadScene("Menu");
        }
    }
}