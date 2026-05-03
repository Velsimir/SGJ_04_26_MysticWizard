using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.Ui
{
    public class Prehistory : MonoBehaviour
    {
        [Header("Комикс")] 
        [SerializeField] private FirstComicsLogic _firstComics;
        [SerializeField] private Button _next;
        
        private void OnEnable()
        {
            _next.onClick.AddListener(ShowComics);
        }

        private void OnDisable()
        {
            _next.onClick.RemoveListener(ShowComics);
        }

        private void ShowComics()
        {
            gameObject.SetActive(false);
            _firstComics.Show();
        }
    }
}