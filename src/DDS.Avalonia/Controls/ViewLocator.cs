using System.Reflection;
using Avalonia.Controls.Templates;
using Binkus.ReactiveMvvm;
using DDS.Core;
using DDS.Core.Helper;

namespace DDS.Avalonia.Controls;

public sealed class ViewLocator : IDataTemplate
{
    private readonly IViewLocator _reactiveViewLocator;
    private readonly bool _doReflectiveSearch;

    [ActivatorUtilitiesConstructor] // DI is working here, ReactiveViewLocator is a Singleton by default
    public ViewLocator(IViewLocator viewLocator, bool doReflectiveSearch = false)
    {
        _reactiveViewLocator = viewLocator;
        _doReflectiveSearch = doReflectiveSearch;
    }

    public IControl? Build(object? data) => _reactiveViewLocator.ResolveView(data) as IControl ??
                                            (_doReflectiveSearch ? BackupBuild(data) : null);

    public bool Match(object? data) => data is IViewModel;
    
    private static IControl? BackupBuild(object? data)
    {
        if (data is null) return null;
        
        var vm = data as IViewModel;
        var services = vm?.Services ?? Globals.Services;
        
        var name = data.GetType().FullName?.Replace("ViewModel", "View") 
                   ?? throw new UnreachableException();
        var type = Type.GetType(name);

        type ??= ReflectiveViewLocation.GetViewType(data.GetType());

        return services.TryGetServiceOrCreateInstance<IControl>(type);
    }
    
    static ViewLocator()
    {
        ReflectiveViewLocation.AddViewSearchPath(typeof(MainView));
    }
}