using System;

namespace Services.Resource
{
    public class ResourceHog : IDisposable
    {
        // Flag for already disposed
        private bool _alreadyDisposed = false;

        // Implementation of IDisposable.
        // Call the virtual Dispose method.
        // Suppress Finalization.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Virtual Dispose method
        protected virtual void Dispose(bool isDisposing)
        {
            // Don't dispose more than once.
            if (_alreadyDisposed)
                return;

            if (isDisposing)
            {
                // elided: free managed resources here.
            }

            // elided: free unmanaged resources here.
            // Set disposed flag:
            _alreadyDisposed = true;
        }

        public void ExampleMethod()
        {
            if (_alreadyDisposed)
                throw new
                    ObjectDisposedException(nameof(ResourceHog),
                        "Called Example Method on Disposed object");
            // remainder elided.
        }
    }
}