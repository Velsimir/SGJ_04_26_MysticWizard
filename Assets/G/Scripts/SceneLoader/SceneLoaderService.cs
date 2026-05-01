using UnityEngine;
using UnityEngine.SceneManagement;

namespace G.Scripts.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private LoaderVisual _loaderVisual;

        public SceneLoaderService(LoaderVisual loaderVisual)
        {
            _loaderVisual = loaderVisual;
        }

        public void ShowLoadScreen()
        {
            _loaderVisual.Show();
        }

        public void LoadScene(string sceneName)
        {
            _loaderVisual.Show();

            AsyncOperation loadHandle = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            loadHandle.completed += OnSceneLoaded;
        }

        private void OnSceneLoaded(AsyncOperation obj)
        {
            _loaderVisual.Hide();
        }
    }
}