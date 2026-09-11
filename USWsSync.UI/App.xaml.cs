using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using USWsSync.Core.Engine;

namespace USWsSync_UI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private Window? _window;
    public static IServiceProvider Services { get; private set; } = null!;

    public App()
    {
        InitializeComponent();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddHttpClient("SyncEngineClient", client =>
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        });
        serviceCollection.AddSingleton<ISyncEngine, SyncEngine>();
        Services = serviceCollection.BuildServiceProvider();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }
}
