using G.Scripts.BulletLogic;
using G.Scripts.PlayerLogic;
using G.Scripts.SceneLoader;
using G.Scripts.Services;
using G.Scripts.Services.Input;
using G.Scripts.Services.Spawner;
using G.Scripts.Services.Update;
using UnityEngine;

namespace G.Scripts
{
    public class G : MonoBehaviour
    {
        public BulletsHandler Bullets;
        [SerializeField] private LoaderVisual _loaderVisual;
        
        public static G Instance;
        
        public ServiceHandler Services { get; private set; }
        public Player Player;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
            DontDestroyOnLoad(this);
            
            RegisterServices();
            
            Services.GetService<ISceneLoaderService>().LoadScene("PlayRoom");
        }

        private void RegisterServices()
        {
            Services = new ServiceHandler();
            
            IInputService inputService = new InputService();
            Services.AddService(inputService);
            
            ICoroutineRunnerService coroutineRunnerService = gameObject.AddComponent<CoroutineRunnerService>();
            Services.AddService(coroutineRunnerService);
            
            IUpdateService updatableService = gameObject.AddComponent<UpdateService>();
            Services.AddService(updatableService);

            LoaderVisual loaderVisual = Instantiate(_loaderVisual);
            DontDestroyOnLoad(loaderVisual);
            
            ISceneLoaderService sceneLoaderService = new SceneLoaderService(loaderVisual);
            Services.AddService(sceneLoaderService);
            
            ISpawnerService spawnerService = new SpawnerService();
            Services.AddService(spawnerService);
        }
    }
}