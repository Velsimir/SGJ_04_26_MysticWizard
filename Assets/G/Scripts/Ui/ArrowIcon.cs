using System.Collections;
using G.Scripts.Services.ArrowSequence;
using UnityEngine;
using UnityEngine.UI;

namespace G.Scripts.Ui
{
    public class ArrowIcon : MonoBehaviour
    {
        [SerializeField] private Image _image;
    
        [Header("Спрайты")]
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _successSprite;
        [SerializeField] private Sprite _failSprite;

        private ArrowDirection _direction;
    
        public ArrowDirection Direction => _direction;

        public void Init(ArrowDirection direction)
        {
            _direction = direction;
            SetNormal();
        }

        public void SetNormal()
        {
            _image.sprite = _normalSprite;
        }

        public void SetSuccess()
        {
            _image.sprite = _successSprite;
        }

        public void SetFail()
        {
            StartCoroutine(ShowFail());
        }

        // Опционально: анимация появления/исчезновения
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        private IEnumerator ShowFail()
        {
            _image.sprite = _failSprite;
            yield return new WaitForSeconds(0.2f);
            _image.sprite = _normalSprite;
            yield return new WaitForSeconds(0.2f);
            _image.sprite = _failSprite;
            yield return new WaitForSeconds(0.2f);
            _image.sprite = _normalSprite;
            yield return new WaitForSeconds(0.2f);
            _image.sprite = _failSprite;
        }
    }
}