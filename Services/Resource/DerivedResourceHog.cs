namespace Services.Resource
{
    public class DerivedResourceHog : ResourceHog
    {
        // Have its own disposed flag.
        private bool _disposed = false;

        protected override void Dispose(bool isDisposing)
        {
            // Don't dispose more than once.
            if (_disposed) return;
            
            if (isDisposing)
            {
                // TODO: free managed resources here.
            }

            // TODO: free unmanaged resources here.

            // Let the base class free its resources.
            // Base class is responsible for calling
            // GC.SuppressFinalize( )
            base.Dispose(isDisposing);
            // Set derived class disposed flag:
            _disposed = true;
        }
    }
}