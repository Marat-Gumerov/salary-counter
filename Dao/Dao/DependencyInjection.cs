using Service.Dao;
using Service.Util;

namespace Dao.Dao
{
    public static class DependencyInjection
    {
        public static void Initialize(IDependencyInjectionContainer container)
        {
            //All data stored inside Dao instance, so use singletons
            container.AddSingleton<IWorkerTypeDao, WorkerTypeDao>();
            container.AddSingleton<IWorkerDao, WorkerDao>();
        }
    }
}