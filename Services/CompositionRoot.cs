using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Accounting;

namespace Services;

public sealed class CompositionRoot : IServiceProvider
{
    private IServiceProvider _serviceProvider;

    public object GetService(Type serviceType)
    {
        _serviceProvider ??= new CompositionRoot().GetServiceProvider();
        return _serviceProvider.GetService(serviceType);
    }

    private ServiceCollection GetServiceCollection()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICalculator, Calculator>();
        serviceCollection.AddScoped<IBankAccount, BankAccount>();
        serviceCollection.AddScoped(typeof(IBankAccount), typeof(BankAccount));
        serviceCollection.Add(
            new ServiceDescriptor(
                typeof(IBankAccount),
                typeof(BankAccount),
                ServiceLifetime.Scoped));
        return serviceCollection;
    }

    private IServiceProvider GetServiceProvider()
    {
        return GetServiceCollection().BuildServiceProvider();
    }
}