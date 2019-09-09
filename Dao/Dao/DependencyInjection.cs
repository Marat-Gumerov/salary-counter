using System;
using Service;

namespace Dao
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
