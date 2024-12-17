using EventAggregator.Interfaces;
using MyFirstPlugin.Event;
using MyWpfApp.Infrastructure;
using System.Windows;
using System.Windows.Input;

namespace MyFirstPlugin.ViewModels
{
  public class MyPluginViewModel : IHandle<SpecialEvent>
  {
    private readonly IEventAggregator _eventAggregator;

    public ICommand MyCommand { get; }
   

    public MyPluginViewModel(IEventAggregator eventAggregator)
    {
      _eventAggregator = eventAggregator;
       MyCommand = new RelayCommand(_ => OnButtonClick());


      _eventAggregator.Subscribe(this);
    }

    private void OnButtonClick()
    {
      _eventAggregator.Publish("Hello from My First Plugin! to MainViewModel");
    }

    public void Handle(SpecialEvent message)
    {
      MessageBox.Show(message.Content);
    }
  }
}