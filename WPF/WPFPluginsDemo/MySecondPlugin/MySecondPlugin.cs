using MyPluginInterface;
using MySecondPlugin.ViewModels;
using MySecondPlugin.Views;

namespace MyFirstPlugin
{
  public class MySecondPlugin : IPlugin
  {
    public string Name => "My Second Plugin";

    public IEnumerable<Type> ViewTypes => new List<Type>
    {
            typeof(MySecondPluginView)
    };

    public IEnumerable<Type> ViewModelTypes => new List<Type>
    {
            typeof(MySecondPluginViewModel)
    };
  }
}