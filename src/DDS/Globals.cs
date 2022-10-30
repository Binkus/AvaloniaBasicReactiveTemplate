using System.Diagnostics;
using JetBrains.Annotations;

namespace DDS;

public static class Globals
{
    private static bool _startupDone;
    private static T D<T>(this T t) => 
        _startupDone ? throw new InvalidOperationException("Startup is already done, Globals are immutable") : t;

    internal interface ISetGlobalsOnlyOnceOnStartup
    {
        [UsedImplicitly] static App? InstanceNullable { internal get => _instanceNullable; set => _instanceNullable = value.D(); }
        [UsedImplicitly] static bool IsDesignMode { private get => Globals.IsDesignMode; set => Globals.IsDesignMode = value.D(); }
        [UsedImplicitly] static IServiceProvider ServiceProvider { private get => Globals.ServiceProvider; set => Globals.ServiceProvider = value.D(); }
        [UsedImplicitly] static object ApplicationLifetime { private get => Globals.ApplicationLifetime; set => Globals.ApplicationLifetime = value.D(); }

        [UsedImplicitly]
        static bool IsClassicDesktopStyleApplicationLifetime
        {
            private get => Globals.IsClassicDesktopStyleApplicationLifetime; set => Globals.IsClassicDesktopStyleApplicationLifetime = value.D();
        }

        [UsedImplicitly] static void FinishGlobalsSetupByMakingGlobalsImmutable() => _startupDone = true.D();
    }
    
    #region Static Globals

    public static readonly Stopwatch Stopwatch = new();
    public static Task InitStartup = null!;
    public static Task DbMigrationTask = null!;
    
    private static App? _instanceNullable;
    [UsedImplicitly] public static App Instance 
        => _instanceNullable ?? throw new NullReferenceException($"{nameof(_instanceNullable)} is null");
    
    [UsedImplicitly] public static bool IsDesignMode { get; private set; }
    [UsedImplicitly] public static IServiceProvider ServiceProvider { get; private set; } = null!;
    [UsedImplicitly] public static object ApplicationLifetime { get; private set; } = null!;
    [UsedImplicitly] public static bool IsClassicDesktopStyleApplicationLifetime { get; private set; }

    #endregion
}