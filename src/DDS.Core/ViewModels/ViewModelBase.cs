// ReSharper disable MemberCanBePrivate.Global

namespace DDS.Core.ViewModels;

file static class C
{
    public const string InstanceNull = " instance's type's FullName is null";
    public const string Vmb = nameof(ViewModelBase);
}

[DataContract]
public abstract class ViewModelBase : ViewModelBase<IViewModel>
{
    protected ViewModelBase(IServiceProvider services, IScreen hostScreen) : base(services, hostScreen) { }
    protected ViewModelBase(IServiceProvider services, Lazy<IScreen> lazyHostScreen) : base(services, lazyHostScreen) { }
    protected ViewModelBase(IServiceProvider services) : base(services) { }
}


[DataContract]
public abstract class ViewModelBase<TIViewModel> : ReactiveObservableObject,
    IViewModelBase,  IViewModelBase<TIViewModel>
    where TIViewModel : class, IViewModel
{
    [DataMember] public string? UrlPathSegment { get; }

    [IgnoreDataMember] private Lazy<IScreen>? _lazyHostScreen;
    
    [IgnoreDataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual IScreen HostScreen
    {
        get => _lazyHostScreen?.Value ?? this.RaiseAndSetIfChanged(
            ref _lazyHostScreen, new Lazy<IScreen>(GetService<IScreen>()))!.Value;
        protected init => this.RaiseAndSetIfChanged(ref _lazyHostScreen, new Lazy<IScreen>(value));
    }
    
    [IgnoreDataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public INavigationViewModel Navigation => HostScreen as INavigationViewModel ?? GetService<INavigationViewModel>();

    [IgnoreDataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual RoutingState Router => HostScreen.Router;

    [IgnoreDataMember] public ViewModelActivator Activator { get; } = new();

    [DataMember] public Guid InstanceId { get; } = Guid.NewGuid();
    [DataMember] public string AssemblyQualifiedName { get; }
    [DataMember] public string FullNameOfType { get; }
    [DataMember] public string ViewModelName { get; }
    [DataMember] public string RawViewName { get; }
    
    [DataMember]
    public string CustomViewName
    {
        // removes "ViewModel" or e.g. "ViewModel'1" at the end if possible, "MainViewModel" => "Main"
        get => _customViewName ??= ViewModelName.EndsWith("ViewModel") ? ViewModelName[..^9]
            : ViewModelName[^11..^2] == "ViewModel" ? ViewModelName[..^11] : ViewModelName;
        set => this.RaiseAndSetIfChanged(ref _customViewName, value);
    }
    [IgnoreDataMember] private string? _customViewName;

    protected ViewModelBase(IServiceProvider services, IScreen hostScreen) : this(services) => _lazyHostScreen = new Lazy<IScreen>(hostScreen);
    protected ViewModelBase(IServiceProvider services, Lazy<IScreen> lazyHostScreen) : this(services) => _lazyHostScreen = lazyHostScreen;
    protected ViewModelBase(IServiceProvider services)
    {
        Services = services;
        var type = GetType().UnderlyingSystemType;
        AssemblyQualifiedName = type.AssemblyQualifiedName
                                ?? throw new UnreachableException($"{C.Vmb} {type.Name}{C.InstanceNull}");
        FullNameOfType = type.FullName ?? throw new UnreachableException($"{C.Vmb} {type.Name}{C.InstanceNull}");
        ViewModelName = type.Name;
        RawViewName = CustomViewName;
        UrlPathSegment = $"/{RawViewName.ToLowerInvariant()}?id={InstanceId}"; // InstanceId.ToString()[..8]

        this.WhenActivated(disposables => 
        {
            Debug.WriteLine(UrlPathSegment + ":");

            HandleActivation();
            Disposable
                .Create(HandleDeactivation)
                .DisposeWith(disposables);
        });
        
        Debug.WriteLine("c:"+ViewModelName);
    }
    
    protected virtual void HandleActivation() { }
    protected virtual void HandleDeactivation() { }
    
    [IgnoreDataMember] public IServiceProvider Services { get; protected init; } //= Globals.Services;

    public TService GetService<TService>() where TService : notnull => Services.GetRequiredService<TService>();
    
    public object GetService(Type serviceType) => Services.GetRequiredService(serviceType);

    
    //

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateReactiveCommand<TViewModel>(IObservable<bool>? canExecute = default)
        where TViewModel : class, IRoutableViewModel 
        => CreateNavigationReactiveCommandFromObservable<TViewModel>(
            new Lazy<ReactiveCommandBase<IRoutableViewModel, IRoutableViewModel>>(() => Navigation.Router.Navigate), canExecute);
    
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateAndResetReactiveCommand<TViewModel>(IObservable<bool>? canExecute = default)
        where TViewModel : class, IRoutableViewModel 
        => CreateNavigationReactiveCommandFromObservable<TViewModel>(
            new Lazy<ReactiveCommandBase<IRoutableViewModel, IRoutableViewModel>>(() => Navigation.Router.NavigateAndReset), canExecute);

    public ReactiveCommand<Unit, IRoutableViewModel> CreateNavigationReactiveCommandFromObservable<TViewModel>(
        Lazy<ReactiveCommandBase<IRoutableViewModel, IRoutableViewModel>> navi, IObservable<bool>? canExecute = default) where TViewModel : class, IRoutableViewModel 
        => ReactiveCommand.CreateFromObservable(
            () => navi.Value.Execute(GetService<TViewModel>()),
            // todo make more lazy, can execute will load values
            canExecute: canExecute ?? this.WhenAnyObservable(x => x.Navigation.Router.CurrentViewModel).Select(x => x is not TViewModel)
        );
    
    //

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateReactiveCommand(Type viewModelType, IObservable<bool>? canExecute = default)
        => CreateNavigationReactiveCommandFromObservable(viewModelType, 
            new Lazy<ReactiveCommandBase<IRoutableViewModel, IRoutableViewModel>>(() => Navigation.Router.Navigate), canExecute);
    
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateAndResetReactiveCommand(Type viewModelType, IObservable<bool>? canExecute = default)
        => CreateNavigationReactiveCommandFromObservable(viewModelType, 
            new Lazy<ReactiveCommandBase<IRoutableViewModel, IRoutableViewModel>>(() => Navigation.Router.NavigateAndReset),canExecute);
    
    public ReactiveCommand<Unit, IRoutableViewModel> CreateNavigationReactiveCommandFromObservable(Type viewModelType,
        Lazy<ReactiveCommandBase<IRoutableViewModel, IRoutableViewModel>> navi, IObservable<bool>? canExecute = default)
    {
        if (viewModelType.IsAssignableTo(typeof(IRoutableViewModel)) is false) throw new InvalidOperationException();
        return ReactiveCommand.CreateFromObservable(
            () => navi.Value.Execute((IRoutableViewModel)GetService(viewModelType)),
            // todo make more lazy, can execute will load values
            canExecute: canExecute ?? this.WhenAnyObservable(x => x.Navigation.Router.CurrentViewModel)
                .Select(x => !x?.GetType().IsAssignableTo(viewModelType) ?? true)
        );
    }
}
