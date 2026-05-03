using G.Scripts.Services.SoundService;
using G.Scripts.StoryTellerLogic;
using UnityEngine;

namespace G.Scripts.Ui
{
    public class PlaySfxFairy : MonoBehaviour
    {
        private Sound _sound;
        
        private void Start()
        {
            _sound = G.Instance.Services.GetService<Sound>();
        }

        public void PlaSoundFairy()
        {
            _sound.PlaySFX(_sound.феечка);

        }
    }
}