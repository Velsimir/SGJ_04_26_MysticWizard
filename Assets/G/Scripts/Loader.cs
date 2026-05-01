using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace G.Scripts
{
    public class Loader : MonoBehaviour
    {
        private void Awake()
        {
            ContainerScope.OnSceneContainerBuilding += InstallExtra;

            AsyncOperation loadHandle = SceneManager.LoadSceneAsync("PlayRoom", LoadSceneMode.Single);

            loadHandle.completed += OnSceneLoaded;
        }

        private void InstallExtra(UnityEngine.SceneManagement.Scene scene, ContainerBuilder builder)
        {
            builder.RegisterValue("of Developers");
        }

        private void OnSceneLoaded(AsyncOperation obj)
        {
            ContainerScope.OnSceneContainerBuilding -= InstallExtra;
        }
    }
}