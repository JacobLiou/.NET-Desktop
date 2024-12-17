using Autofac;
using System.Reflection;
using System.Windows;

namespace MyWpfApp.Infrastructure
{
  public static class AutofacExtensions
  {
    public static void RegisterViewsAndViewModels(this ContainerBuilder builder, Assembly assembly)
    {

      builder.RegisterAssemblyTypes(assembly)
             .Where(t => t.Name.EndsWith("View") && typeof(FrameworkElement).IsAssignableFrom(t))
             .OnActivated(args =>
             {
               var viewType = args.Instance.GetType(); 
               var viewModelTypeName = viewType.Name.Replace("View", "ViewModel");
               var viewModelType = viewType.Assembly.GetType(viewType.Namespace + "." + viewModelTypeName); 

               if (viewModelType == null)
               {
                 viewModelType = assembly.GetTypes().FirstOrDefault(t => t.Name == viewModelTypeName);
               }

               if (viewModelType != null)
               {
                 var viewModel = args.Context.Resolve(viewModelType);
                 ((FrameworkElement)args.Instance).DataContext = viewModel;
               }
             })
             .AsSelf()
             .InstancePerDependency();


      builder.RegisterAssemblyTypes(assembly)
             .Where(t => t.Name.EndsWith("ViewModel"))
             .AsSelf()
             .InstancePerDependency();
    }
  }
}
