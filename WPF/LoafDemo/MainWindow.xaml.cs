using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LoafDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DispatcherTimer progressTimer = new DispatcherTimer();
    private int currentProgress = 0;
    private readonly Random random = new Random();

    public MainWindow()
    {
        InitializeComponent();
        InitializeAnimation();
        InitializeTimer();

        // 全屏显示并禁用Alt+F4
        this.KeyDown += (s, e) =>
        {
            if (e.Key == System.Windows.Input.Key.System && e.SystemKey == System.Windows.Input.Key.F4)
            {
                e.Handled = true;
            }
        };

        // 禁用所有输入
        this.PreviewKeyDown += (s, e) => e.Handled = true;
        this.PreviewMouseDown += (s, e) => e.Handled = true;
        this.PreviewMouseUp += (s, e) => e.Handled = true;
        this.PreviewMouseWheel += (s, e) => e.Handled = true;
        
        // 捕获Alt+F4
        this.Closing += (s, e) => e.Cancel = true;
    }

    private void InitializeAnimation()
    {
        // 创建旋转动画
        var rotateAnimation = new DoubleAnimation
        {
            From = 0,
            To = 360,
            Duration = TimeSpan.FromSeconds(3),
            RepeatBehavior = RepeatBehavior.Forever
        };

        // 为每个点创建淡入淡出动画
        var dots = new[] { Dot1, Dot2, Dot3, Dot4, Dot5, Dot6 };
        for (int i = 0; i < dots.Length; i++)
        {
            var fadeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.3,
                Duration = TimeSpan.FromMilliseconds(750),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                BeginTime = TimeSpan.FromMilliseconds(i * 100)
            };

            dots[i].BeginAnimation(OpacityProperty, fadeAnimation);
        }
    }

    private void InitializeTimer()
    {
        progressTimer.Interval = TimeSpan.FromSeconds(1);
        progressTimer.Tick += UpdateProgress;
        progressTimer.Start();
    }

    private void UpdateProgress(object sender, EventArgs e)
    {
        // 随机增加进度
        int increment = random.Next(1, 5);
        currentProgress = Math.Min(currentProgress + increment, 100);

        // 更新进度文本
        ProgressText.Text = $"{currentProgress}% 已完成";

        // 更新状态文本
        if (currentProgress < 33)
        {
            UpdateText.Text = "正在安装更新，请不要关闭计算机";
        }
        else if (currentProgress < 66)
        {
            UpdateText.Text = "这可能需要一段时间";
        }
        else
        {
            UpdateText.Text = "请稍候，正在完成更新";
        }

        // 当进度达到100%时
        if (currentProgress >= 100)
        {
            progressTimer.Stop();
            SimulateRestart();
        }
    }

    private void SimulateRestart()
    {
        // 延迟2秒后显示重启消息
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(2)
        };

        timer.Tick += (s, e) =>
        {
            timer.Stop();
            UpdateText.Text = "正在重新启动...";
            ProgressText.Text = "";

            // 再延迟2秒后关闭应用
            var closeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            closeTimer.Tick += (s2, e2) =>
            {
                closeTimer.Stop();
                Application.Current.Shutdown();
            };

            closeTimer.Start();
        };

        timer.Start();
    }
}

/// <summary>
/// 自定义进度显示样式
/// </summary>
public class CustomProgressBar : System.Windows.Controls.ProgressBar
{
    static CustomProgressBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(CustomProgressBar),
            new FrameworkPropertyMetadata(typeof(CustomProgressBar)));
    }
}