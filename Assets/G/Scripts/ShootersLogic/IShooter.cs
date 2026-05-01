using System;
using G.Scripts.Services.Update;

namespace G.Scripts.ShootersLogic
{
    public interface IShooter : IUpdatable, IDisposable
    {
        void Shoot();
    }
}