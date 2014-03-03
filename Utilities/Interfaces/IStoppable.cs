#region



#endregion

namespace Utilities.Interfaces
{
    public interface IStoppable
    {
        /// <summary>
        /// Gets whether a stop is pending.
        /// </summary>
        bool StopPending { get; }

        /// <summary>
        /// Stops the operation.
        /// </summary>
        void Stop();
    }
}