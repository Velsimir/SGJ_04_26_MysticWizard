using G.Scripts.Services.SoundService;
using UnityEngine;

namespace G.Scripts.Ui
{
    public class PlaySfxWizard : MonoBehaviour
    {
        private Sound _sound;
        
        private void Start()
        {
            _sound = G.Instance.Services.GetService<Sound>();
        }

        public void PlaSoundWizard()
        {
            _sound.PlaySFX(_sound.мяурлин);
        }
    }
}