using MyFirstPlugin.ViewModels;
using MyFirstPlugin.Views;
using MyPluginInterface;

namespace MyFirstPlugin
{
  public class MyFirstPlugin : IPlugin
  {
    public string Name => "My First Plugin with Multiple Views";

    public IEnumerable<Type> ViewTypes => new List<Type>
    {
            typeof(MyPluginView),
            typeof(MyPlugin1View)
    };

    public IEnumerable<Type> ViewModelTypes => new List<Type>
    {
            typeof(MyPluginViewModel),
            typeof(MyPlugin1ViewModel)
    };
  }
}
