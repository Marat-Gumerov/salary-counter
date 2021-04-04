using System;
using System.Threading;
using SalaryCounter.Service.Exception;

namespace SalaryCounter.Dao.Extensions
{
    internal static class ReaderWriterLockSlimExtensions
    {
        public static IDisposable Read(this ReaderWriterLockSlim lockObject) =>
            new LockToken(lockObject, LockType.Read);

        public static IDisposable Write(this ReaderWriterLockSlim lockObject) =>
            new LockToken(lockObject, LockType.Write);

        private enum LockType
        {
            Read,
            Write
        }

        private sealed class LockToken : IDisposable
        {
            private readonly LockType lockType;
            private readonly ReaderWriterLockSlim sync;

            public LockToken(ReaderWriterLockSlim sync, LockType lockType)
            {
                this.sync = sync;
                this.lockType = lockType;
                switch (lockType)
                {
                    case LockType.Read:
                        sync.EnterReadLock();
                        break;
                    case LockType.Write:
                        sync.EnterWriteLock();
                        break;
                    default:
                        throw new SalaryCounterGeneralException("Unexpected lock type");
                }
            }

            public void Dispose()
            {
                switch (lockType)
                {
                    case LockType.Read:
                        sync.ExitReadLock();
                        break;
                    case LockType.Write:
                        sync.ExitWriteLock();
                        break;
                    default:
                        throw new SalaryCounterGeneralException("Unexpected lock type");
                }
            }
        }
    }
}
