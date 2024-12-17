using Autofac;
using EventAggregator.Interfaces;
using MyPluginInterface;
using MyWpfApp.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MyWpfApp.ViewModels
{
  public class MainViewModel : ViewModelBase, IHandle<string>
  {
    private readonly IEventAggregator _eventAggregator;
    private readonly ILifetimeScope _lifetimeScope;

    public ObservableCollection<object> LoadedViews { get; } = new ObservableCollection<object>();
    public ObservableCollection<IPlugin> Plugins { get; }
    public ICommand LoadPluginCommand { get; }

    public MainViewModel(IEventAggregator eventAggregator, ILifetimeScope lifetimeScope)
    {
      _eventAggregator = eventAggregator;
      _lifetimeScope = lifetimeScope;

      _eventAggregator.Subscribe(this);

      Plugins = new ObservableCollection<IPlugin>();
      LoadPluginCommand = new RelayCommand(LoadPlugin);

      foreach (var plugin in _lifetimeScope.Resolve<IEnumerable<IPlugin>>())
      {
        Plugins.Add(plugin);
      }
    }

    private void LoadPlugin(object parameter)
    {
      if (parameter is Type viewType)
      {
        var view = _lifetimeScope.Resolve(viewType);
        LoadedViews.Add(view);  
      }
    }

    public void Handle(string message)
    {
      MessageBox.Show(message);
    }
  }
}
