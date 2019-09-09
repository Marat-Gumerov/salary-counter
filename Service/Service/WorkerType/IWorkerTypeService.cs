namespace Service
{
    public interface IWorkerTypeService
    {
        System.Collections.Generic.IList<WorkerType> Get();
        bool IsValid(WorkerType workerType);
    }
}