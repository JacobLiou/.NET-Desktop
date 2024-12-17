namespace MyPluginInterface
{
  public interface IPlugin
  {
    string Name { get; }
    IEnumerable<Type> ViewTypes { get; } 
    IEnumerable<Type> ViewModelTypes { get; }
  }
}
