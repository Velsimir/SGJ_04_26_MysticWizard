namespace G.Scripts.Services.Update
{
    public interface IUpdateService : IService
    {
        void AddNew(IUpdatable updatable);
        void Remove(IUpdatable updatable);
        public void SetTimeScale(float targetScale);
        public void LerpTimeScale(float targetScale, float duration);
    }
}