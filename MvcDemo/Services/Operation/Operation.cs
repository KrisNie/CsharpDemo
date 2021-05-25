using System;

namespace MvcDemo.Services.Operation
{
    public class Operation : IOperation, IOperationTransient, IOperationScoped, IOperationSingleton,
        IOperationSingletonInstance
    {
        public Guid OperationId { get; }

        public Operation(Guid operationId)
        {
            OperationId = operationId;
        }

        public Operation() : this(Guid.NewGuid())
        {
        }
    }
}