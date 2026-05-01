namespace G.Scripts.Services.Update
{
    public interface IUpdateService : IService
    {
        void AddNew(IUpdatable updatable);
        void Remove(IUpdatable updatable);
    }
}