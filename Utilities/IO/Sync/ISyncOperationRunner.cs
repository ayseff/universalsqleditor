using System.Collections.Generic;
using Utilities.Interfaces;

namespace Utilities.IO.Sync
{
    public interface ISyncOperationRunner : IProgressReporter, IActionReporter, IStoppable, IErrorReporter
    {
        long BytesToCopy { get; }
        long BytesCopied { get; }
        void RunOperations(IList<SyncOperation> operations);
    }
}