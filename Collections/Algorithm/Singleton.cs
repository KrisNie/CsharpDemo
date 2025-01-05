using System;

namespace Collections.Algorithm;

public sealed class Singleton
{
    private static readonly Lazy<Singleton> Lazy = new(() => new Singleton());
    public DateTime DateTime { get; }
    public static Singleton Instance => Lazy.Value;

    private Singleton()
    {
        DateTime = DateTime.Now;
    }
}