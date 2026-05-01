using G.Scripts.Services;

namespace G.Scripts.SceneLoader
{
    public interface ISceneLoaderService : IService
    {
        void ShowLoadScreen();
        void LoadScene(string sceneName);
    }
}