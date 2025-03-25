using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace WinFormWPFCohost;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // 创建 WPF 控件
        var wpfButton = new System.Windows.Controls.Button { Content = "Hello from WPF" };
        wpfButton.Width = 200;
        wpfButton.Height = 38;
        wpfButton.Click += (sender, e) => MessageBox.Show("Hello from WPF Button");

        //// 创建 WindowsFormsHost 控件来托管 WPF 控件
        //var host = new WindowsFormsHost();
        ////var host1 = new 
        //host.Child = wpfButton;

        //// 添加 WindowsFormsHost 控件到 WinForms 表单
        //this.Controls.Add(host);
        //// 设置 host 的位置和大小
        //host.Margin = new Padding(10, 10, 0, 0);
        //host.Size = new System.Drawing.Size(200, 50);

        ElementHost elementHost = new ElementHost();
        elementHost.Dock = DockStyle.Fill;
        elementHost.Child = wpfButton;
        this.Controls.Add(elementHost);
    }
}
