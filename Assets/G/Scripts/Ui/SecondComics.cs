using G.Scripts.SceneLoader;
using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.Ui
{
    public class SecondComics : MonoBehaviour
    {
        [SerializeField] private Button _buttonNext;

        private void OnEnable()
        {
            _buttonNext.onClick.AddListener(LoadGamePlay);
        }

        private void LoadGamePlay()
        {
            G.Instance.Services.GetService<ISceneLoaderService>().LoadScene("PlayRoom");
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}