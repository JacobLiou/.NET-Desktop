using System.Windows;

namespace LoafDemo
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            webView2.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            webView2.GoForward();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //https://www.cnblogs.com

            // SharpBrowser 采用CefSharp搭建开源浏览器环境
            //bailijun 当初采用Chromium+C++MFC开发点击自动化   如今的Python非常容易搞定
            //比如采用端到端测试工具 PlayWright 或者 Selenium
            if (Uri.IsWellFormedUriString(urlTextBox.Text, UriKind.RelativeOrAbsolute))
                webView2.Source = new Uri(urlTextBox.Text);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            // BlazorWebBrowser搭建跨端环境
            webView2.Reload();
        }
    }
}
