namespace Core.Services.ObjectPool
{
    public interface IPoolable
    {
        void OnSpawned();
        void OnDespawned();
    }
}