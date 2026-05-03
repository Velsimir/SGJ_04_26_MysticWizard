using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Image _tutorialInfo;
    
        [Header("Настройки появления")]
        [SerializeField] private float _hideAfterSeconds;
        [SerializeField] private float _betweenHide = 0.05f;
        [SerializeField] private float _fadeStep = 0.02f;
    
        private void Start()
        {
            StartCoroutine(HideTutorial());
        }

        private IEnumerator HideTutorial()
        {
            yield return new WaitForSeconds(_hideAfterSeconds);
        
            while (_tutorialInfo.color.a > 0)
            {
                Color color = _tutorialInfo.color;
                color.a -= _fadeStep;
                _tutorialInfo.color = color;
            
                yield return new WaitForSeconds(_betweenHide);
            }
        
            Color finalColor = _tutorialInfo.color;
            finalColor.a = 0f;
            _tutorialInfo.color = finalColor;
        
            gameObject.SetActive(false);
        }
    }
}