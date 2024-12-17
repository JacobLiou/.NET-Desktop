using EventAggregator.Interfaces;
using MyFirstPlugin.Event;
using MyWpfApp.Infrastructure;
using System.Windows.Input;

namespace MyFirstPlugin.ViewModels
{
    public class MyPlugin1ViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public ICommand MyCommand { get; }

        public MyPlugin1ViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            MyCommand = new RelayCommand(_ => OnButtonClick());
        }

        private void OnButtonClick()
        {
            var msg = new SpecialEvent("Hello from the other view internal my first plugin");
            _eventAggregator.Publish(msg);
        }

    }
}

