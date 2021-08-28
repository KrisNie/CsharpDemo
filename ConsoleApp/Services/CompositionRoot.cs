﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Finance;

namespace Services
{
    public sealed class CompositionRoot : IServiceProvider
    {
        private IServiceProvider _serviceProvider;

        private ServiceCollection GetServiceCollection()
        {
            var sc = new ServiceCollection();
            sc.AddScoped<ICalculator, Calculator>();
            sc.AddScoped<IBankAccount, BankAccount>();
            sc.AddScoped(typeof(IBankAccount), typeof(BankAccount));
            sc.Add(new ServiceDescriptor(typeof(IBankAccount), typeof(BankAccount), ServiceLifetime.Scoped));
            return sc;
        }

        private IServiceProvider GetServiceProvider()
        {
            return GetServiceCollection().BuildServiceProvider();
        }

        public object GetService(Type serviceType)
        {
            _serviceProvider ??= new CompositionRoot().GetServiceProvider();
            return _serviceProvider.GetService(serviceType);
        }
    }
}