using System;
using Microsoft.Extensions.DependencyInjection;
using Services.Accounting;

namespace Services;

public class UnbelievableClass
{
    public void UnbelievableMethod()
    {
        Console.WriteLine(nameof(UnbelievableMethod));
    }

    #region Dependency Injection Demo

    private void DependencyInjectionDemo()
    {
        var ba = new CompositionRoot().GetService<IBankAccount>();
        if (ba == null) return;
        ba.Create("Mr. Bryan Walton", 11.99);
        ba.Credit(5.77);
        ba.Debit(11.22);
        Console.WriteLine("Current balance is ${0}", ba.Balance);
    }

    #endregion
}