﻿namespace DDS.ViewModels;

public sealed partial class MainViewModel : ViewModelBase
{
    private readonly IAvaloniaEssentials _avaloniaEssentials;

    public string Greeting => "Greetings from MainView";

    [ObservableProperty] 
    private string _gotPath = "fullPath is empty";

    public NavigationViewModel Navigation { get; }

    // Necessary for Designer: 
#pragma warning disable CS8618
    public MainViewModel() { }
#pragma warning restore CS8618

    [ActivatorUtilitiesConstructor, UsedImplicitly]
    public MainViewModel(NavigationViewModel navigation, IAvaloniaEssentials? avaloniaEssentials, 
        Lazy<TestViewModel> testViewModel, Lazy<SecondTestViewModel> secondTestViewModel)
    {
        _avaloniaEssentials ??= avaloniaEssentials ?? Globals.ServiceProvider.GetService<IAvaloniaEssentials>()!;

        HostScreen = Navigation = navigation;

        GoTest = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(testViewModel.Value),
            canExecute: this.WhenAnyObservable(x => x.Router.CurrentViewModel).Select(x => x is not TestViewModel)
        );
        GoSecondTest = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(secondTestViewModel.Value),
            canExecute: this.WhenAnyObservable(x => x.Router.CurrentViewModel).Select(x => x is not SecondTestViewModel)
        );
    }
    
    public ReactiveCommand<Unit, IRoutableViewModel> GoTest { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> GoSecondTest { get; }

    [RelayCommand]
    async Task OpenFilePicker()
    {
        if (Globals.IsDesignMode) return;
        var fileResult = await _avaloniaEssentials.FilePickerAsync();
        var fullPath = fileResult.FullPath;
        GotPath = fileResult.Exists ? $"fullPath={fullPath}" : "fullPath is empty";
    }
}