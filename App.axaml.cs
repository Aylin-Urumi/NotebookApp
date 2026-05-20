using Avalonia;
using Avalonia.Markup.Xaml;
using NotebookApp.Views;

namespace NotebookApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();
        
        var lifetime = ApplicationLifetime;
        if (lifetime != null)
        {
            var lifetimeType = lifetime.GetType();
            var mainWindowProp = lifetimeType.GetProperty("MainWindow");
            if (mainWindowProp != null && mainWindowProp.CanWrite)
            {
                mainWindowProp.SetValue(lifetime, new MainWindow());
            }
        }
    }
}