using Autofac;
using EventAggregator;
using EventAggregator.Interfaces;
using MyPluginInterface;
using MyWpfApp.Infrastructure;
using MyWpfApp.ViewModels;
using MyWpfApp.Views;
using System.IO;
using System.Reflection;
using System.Windows;
using IContainer = Autofac.IContainer;

namespace MyWpfApp
{
  public partial class App : Application
  {
    private IContainer _container;

    protected override void OnStartup(StartupEventArgs e)
    {
      var builder = new ContainerBuilder();

      // Register EventAggregator as a singleton
      builder.RegisterType<EventAggregator.EventAggregator>().As<IEventAggregator>().SingleInstance();

      // Register MainView and MainViewModel
      builder.RegisterType<MainView>().AsSelf().SingleInstance();
      builder.RegisterType<MainViewModel>().AsSelf().InstancePerDependency();

      LoadPlugins(builder);

      _container = builder.Build();

      var mainWindow = _container.Resolve<MainView>();
      mainWindow.Show();
    }

    private void LoadPlugins(ContainerBuilder builder)
    {
      var pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
      
      if (!Directory.Exists(pluginPath))
      {
        Directory.CreateDirectory(pluginPath);
      }

      var pluginFiles = Directory.GetFiles(pluginPath, "*.dll");

      foreach (var pluginFile in pluginFiles)
      {
        var assembly = Assembly.LoadFrom(pluginFile);
        var pluginTypes = assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var pluginType in pluginTypes)
        {
          var plugin = (IPlugin)Activator.CreateInstance(pluginType);

          foreach (var viewType in plugin.ViewTypes)
          {
            builder.RegisterType(viewType).AsSelf().InstancePerDependency();
          }

          foreach (var viewModelType in plugin.ViewModelTypes)
          {
            builder.RegisterType(viewModelType).AsSelf().InstancePerDependency();
          }

          builder.RegisterInstance(plugin).As<IPlugin>();


          builder.RegisterViewsAndViewModels(assembly);
        }
      }
    }
  }
}
