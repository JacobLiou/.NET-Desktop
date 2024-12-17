using EventAggregator.Interfaces;
using MyWpfApp.Infrastructure;
using System.Windows.Input;

namespace MySecondPlugin.ViewModels
{
    public class MySecondPluginViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public ICommand MyCommand { get; }

        public MySecondPluginViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            MyCommand = new RelayCommand(_ => OnButtonClick());
        }

        private void OnButtonClick()
        {
            _eventAggregator.Publish("Hello from My Second Plugin! to MainViewModel");
        }
    }
}