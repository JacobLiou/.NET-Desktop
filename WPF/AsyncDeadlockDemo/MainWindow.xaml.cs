using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncDeadlockDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        // 同步等待异步方法
        // 异步操作完成后，尝试切换回 UI 上下文，但 UI 线程被阻塞，任务无法继续完成，从而形成死锁。
        PerformAsyncOperation().Wait();

        // 正确的做法是使用 await 关键字，这样异步操作会在后台线程上执行，而 UI 线程不会被阻塞。
        // await PerformAsyncOperation();

        //同步等待异步避免捕获
        // PerformAsyncOperation().ConfigureAwait(false).GetAwaiter().GetResult(); // 禁用上下文捕获
    }

    private async Task PerformAsyncOperation()
    {
        await Task.Delay(1000); // 模拟异步操作
        Console.WriteLine("Task Completed");
    }
}