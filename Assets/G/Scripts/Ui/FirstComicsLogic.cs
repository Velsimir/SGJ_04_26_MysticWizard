using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.Ui
{
    public class FirstComicsLogic : MonoBehaviour
    {
        [SerializeField] private Button _buttonNext;
        [SerializeField] private GameObject _nextComics;
        [SerializeField] private GameObject _firstComics;
        
        private void OnEnable()
        {
            _buttonNext.onClick.AddListener(ShowNextComics);
        }

        private void ShowNextComics()
        {
            _nextComics.SetActive(true);
            Hide();
        }

        public void Show()
        {
            _firstComics.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}