using System.Collections;
using G.Scripts.Services;
using UnityEngine;

namespace G.Scripts.SceneLoader
{
    public class LoaderVisual : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private readonly WaitForSeconds _wait = new WaitForSeconds(0.05f);
        
        public void Show()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
            
            Debug.Log("Show Loader screen");
        }

        public void Hide()
        {
            G.Instance.Services.GetService<ICoroutineRunnerService>().StartRoutine(StartHide());
            Debug.Log("Hide Loader screen");
        }
        
        private IEnumerator StartHide()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= 0.05f;
                yield return _wait;
            }
            
            _canvasGroup.interactable = false;
        }
    }
}