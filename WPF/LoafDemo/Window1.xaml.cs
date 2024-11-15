using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace LoafDemo
{
    public partial class Window1 : Window
    {
        private readonly DispatcherTimer progressTimer = new DispatcherTimer();
        private int currentProgress = 0;
        private readonly Random random = new Random();
        private readonly string[] updateMessages = new[]
        {
            "正在安装更新",
            "正在准备更新",
            "正在配置更新",
            "请稍候",
            "即将完成"
        };

        public Window1()
        {
            InitializeComponent();
            InitializeWindow();
            InitializeAnimations();
            InitializeTimer();
        }

        private void InitializeWindow()
        {
            // 禁用所有输入
            this.PreviewKeyDown += (s, e) => e.Handled = true;
            this.PreviewMouseDown += (s, e) => e.Handled = true;
            this.PreviewMouseUp += (s, e) => e.Handled = true;
            this.PreviewMouseWheel += (s, e) => e.Handled = true;
            this.Closing += (s, e) => e.Cancel = true;

            // 添加隐藏的退出机制
            InitializeExitMechanism();
        }

        private void InitializeAnimations()
        {
            // 进度环旋转动画
            var rotateAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(2),
                RepeatBehavior = RepeatBehavior.Forever
            };

            var rotateTransform = new RotateTransform();
            ProgressRing.RenderTransform = rotateTransform;
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

            // Logo淡入动画
            var fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1.5)
            };
            WindowsLogo.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void InitializeTimer()
        {
            progressTimer.Interval = TimeSpan.FromSeconds(1.5);
            progressTimer.Tick += UpdateProgress;
            progressTimer.Start();
        }

        private void UpdateProgress(object sender, EventArgs e)
        {
            // 随机增加进度
            int increment = random.Next(1, 3);
            currentProgress = Math.Min(currentProgress + increment, 100);

            // 更新进度文本
            ProgressText.Text = $"{currentProgress}%";

            // 随机更新消息
            if (random.Next(100) < 30) // 30%概率更新消息
            {
                UpdateText.Text = updateMessages[random.Next(updateMessages.Length)];
            }

            // 根据进度更新提示文本
            UpdateHintText();

            // 当进度达到100%时
            if (currentProgress >= 100)
            {
                progressTimer.Stop();
                SimulateRestart();
            }
        }

        private void UpdateHintText()
        {
            if (currentProgress < 30)
            {
                HintText.Text = "请保持计算机开机和连网";
            }
            else if (currentProgress < 60)
            {
                HintText.Text = "这可能需要一段时间";
            }
            else if (currentProgress < 90)
            {
                HintText.Text = "请不要关闭计算机";
            }
            else
            {
                HintText.Text = "即将完成";
            }
        }

        private void SimulateRestart()
        {
            // 创建完成动画
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(2)
            };

            fadeOutAnimation.Completed += (s, e) =>
            {
                var restartTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };

                restartTimer.Tick += (s2, e2) =>
                {
                    restartTimer.Stop();
                    Application.Current.Shutdown();
                };

                restartTimer.Start();
            };

            // 应用淡出动画到所有元素
            ProgressRing.BeginAnimation(OpacityProperty, fadeOutAnimation);
            UpdateText.BeginAnimation(OpacityProperty, fadeOutAnimation);
            ProgressText.BeginAnimation(OpacityProperty, fadeOutAnimation);
            HintText.BeginAnimation(OpacityProperty, fadeOutAnimation);
            WindowsLogo.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void InitializeExitMechanism()
        {
            string exitCode = "";
            this.KeyDown += (s, e) =>
            {
                exitCode += e.Key.ToString();
                if (exitCode.Length > 10)
                    exitCode = exitCode.Substring(1);

                // 输入"EXITWIN11"退出
                if (exitCode.EndsWith("EXITWIN11"))
                {
                    Application.Current.Shutdown();
                }
            };
        }
    }
}