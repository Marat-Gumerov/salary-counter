using System;
using System.Threading;

namespace Dao
{
    public static class ReaderWriterLockSlimExtensions
    {
        private enum LockType
        {
            Read,
            Write
        };
        private sealed class LockToken : IDisposable
        {
            private readonly ReaderWriterLockSlim sync;
            private readonly LockType lockType;

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
                        throw new ArgumentException("Unexpected lock type");
                }
                
            }
            public void Dispose()
            {
                if (sync != null)
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
                            throw new ArgumentException("Unexpected lock type");
                    }
                }
            }
        }

        public static IDisposable Read(this ReaderWriterLockSlim lockObject)
        {
            return new LockToken(lockObject, LockType.Read);
        }
        public static IDisposable Write(this ReaderWriterLockSlim lockObject)
        {
            return new LockToken(lockObject, LockType.Write);
        }
    }
}
