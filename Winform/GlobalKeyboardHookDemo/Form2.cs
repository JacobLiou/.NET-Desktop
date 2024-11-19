using Gma.System.MouseKeyHook.WinApi;
using System.Runtime.InteropServices;

namespace GlobalKeyboardHookDemo
{
    public partial class Form2 : Form
    {
        private HookProcedure _hook;

        private GlobalHotkey globalHotkey;
        private const int WM_HOTKEY = 0x0312;

        public Form2()
        {
            globalHotkey = new GlobalHotkey(this);

            // 注册Ctrl+Shift+A热键
            globalHotkey.RegisterHotKey(Keys.A, ctrl: true, shift: true);
        }

        protected override void WndProc(ref Message m)
        {
            // 处理热键消息
            if (m.Msg == WM_HOTKEY)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Focus();
            }
            base.WndProc(ref m);
        }




    }

    public class GlobalHotkey
    {
        // Win32 API 导入
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // 修饰键定义
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;

        private int hotKeyId;
        private IntPtr windowHandle;
        private Form form;

        public GlobalHotkey(Form form)
        {
            this.form = form;
            this.windowHandle = form.Handle;
            form.FormClosing += Form_FormClosing;
        }

        public bool RegisterHotKey(Keys key, bool ctrl = false, bool alt = false, bool shift = false, bool win = false)
        {
            // 生成唯一ID
            hotKeyId = form.GetHashCode();

            // 组合修饰键
            uint modifiers = 0;
            if (ctrl) modifiers |= MOD_CONTROL;
            if (alt) modifiers |= MOD_ALT;
            if (shift) modifiers |= MOD_SHIFT;
            if (win) modifiers |= MOD_WIN;

            // 注册热键
            return RegisterHotKey(windowHandle, hotKeyId, modifiers, (uint)key);
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(windowHandle, hotKeyId);
        }
    }



    // 更完整的实现版本
    public class EnhancedGlobalHotkey : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;
        private const uint MOD_NOREPEAT = 0x4000;

        private readonly Dictionary<int, HotkeyInfo> registeredHotkeys = new Dictionary<int, HotkeyInfo>();
        private readonly IntPtr windowHandle;
        private readonly Form form;
        private bool disposed;

        public class HotkeyInfo
        {
            public Keys Key { get; set; }
            public bool Ctrl { get; set; }
            public bool Alt { get; set; }
            public bool Shift { get; set; }
            public bool Win { get; set; }
            public Action Callback { get; set; }
        }

        public EnhancedGlobalHotkey(Form form)
        {
            this.form = form ?? throw new ArgumentNullException(nameof(form));
            this.windowHandle = form.Handle;
            form.FormClosing += Form_FormClosing;
        }

        public bool RegisterHotKey(Keys key, Action callback, bool ctrl = false, bool alt = false,
            bool shift = false, bool win = false)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(EnhancedGlobalHotkey));

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            // 生成唯一ID
            int id = GenerateHotkeyId();

            // 组合修饰键
            uint modifiers = 0;
            if (ctrl) modifiers |= MOD_CONTROL;
            if (alt) modifiers |= MOD_ALT;
            if (shift) modifiers |= MOD_SHIFT;
            if (win) modifiers |= MOD_WIN;
            modifiers |= MOD_NOREPEAT; // 防止重复触发

            if (RegisterHotKey(windowHandle, id, modifiers, (uint)key))
            {
                registeredHotkeys[id] = new HotkeyInfo
                {
                    Key = key,
                    Ctrl = ctrl,
                    Alt = alt,
                    Shift = shift,
                    Win = win,
                    Callback = callback
                };
                return true;
            }

            return false;
        }

        public bool UnregisterHotKey(Keys key, bool ctrl = false, bool alt = false,
            bool shift = false, bool win = false)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(EnhancedGlobalHotkey));

            var hotkey = registeredHotkeys.FirstOrDefault(x =>
                x.Value.Key == key &&
                x.Value.Ctrl == ctrl &&
                x.Value.Alt == alt &&
                x.Value.Shift == shift &&
                x.Value.Win == win);

            if (hotkey.Value != null)
            {
                if (UnregisterHotKey(windowHandle, hotkey.Key))
                {
                    registeredHotkeys.Remove(hotkey.Key);
                    return true;
                }
            }

            return false;
        }

        public void ProcessHotkey(int id)
        {
            if (registeredHotkeys.TryGetValue(id, out var hotkeyInfo))
            {
                try
                {
                    hotkeyInfo.Callback?.Invoke();
                }
                catch (Exception ex)
                {
                    // 处理回调异常
                    MessageBox.Show($"热键回调执行出错: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int GenerateHotkeyId()
        {
            Random rand = new Random();
            int id;
            do
            {
                id = rand.Next(1, 65535);
            } while (registeredHotkeys.ContainsKey(id));
            return id;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                foreach (var hotkey in registeredHotkeys)
                {
                    UnregisterHotKey(windowHandle, hotkey.Key);
                }
                registeredHotkeys.Clear();
                form.FormClosing -= Form_FormClosing;
                disposed = true;
            }
        }
    }

    //// 增强版使用示例
    //public class EnhancedMainForm : Form
    //{
    //    private EnhancedGlobalHotkey globalHotkey;
    //    private const int WM_HOTKEY = 0x0312;

    //    public EnhancedMainForm()
    //    {
    //        InitializeComponent();
    //        globalHotkey = new EnhancedGlobalHotkey(this);

    //        // 注册多个热键
    //        globalHotkey.RegisterHotKey(Keys.A, ShowDialog1, ctrl: true, shift: true);
    //        globalHotkey.RegisterHotKey(Keys.B, ShowDialog2, ctrl: true, alt: true);
    //    }

    //    protected override void WndProc(ref Message m)
    //    {
    //        if (m.Msg == WM_HOTKEY)
    //        {
    //            globalHotkey.ProcessHotkey(m.WParam.ToInt32());
    //        }
    //        base.WndProc(ref m);
    //    }

    //    private void ShowDialog1()
    //    {
    //        using (var dialog = new Form())
    //        {
    //            dialog.Text = "对话框 1";
    //            dialog.ShowDialog();
    //        }
    //    }

    //    private void ShowDialog2()
    //    {
    //        using (var dialog = new Form())
    //        {
    //            dialog.Text = "对话框 2";
    //            dialog.ShowDialog();
    //        }
    //    }

    //    protected override void OnFormClosing(FormClosingEventArgs e)
    //    {
    //        globalHotkey.Dispose();
    //        base.OnFormClosing(e);
    //    }
    //}

    // 系统托盘实现示例

    //public class TrayApplication : ApplicationContext
    //{
    //    private NotifyIcon trayIcon;
    //    private EnhancedGlobalHotkey globalHotkey;
    //    private Form invisibleForm;

    //    public TrayApplication()
    //    {
    //        // 创建不可见窗体用于注册热键
    //        invisibleForm = new Form
    //        {
    //            WindowState = FormWindowState.Minimized,
    //            ShowInTaskbar = false
    //        };
    //        invisibleForm.Load += (s, e) => invisibleForm.Hide();

    //        // 初始化托盘图标
    //        trayIcon = new NotifyIcon()
    //        {
    //            Icon = SystemIcons.Application,
    //            ContextMenu = new ContextMenu(new MenuItem[] {
    //            new MenuItem("退出", Exit)
    //        }),
    //            Visible = true
    //        };

    //        // 初始化全局热键
    //        globalHotkey = new EnhancedGlobalHotkey(invisibleForm);
    //        globalHotkey.RegisterHotKey(Keys.Q, ShowPopup, ctrl: true, shift: true);

    //        // 显示不可见窗体
    //        Application.Run(invisibleForm);
    //    }

    //    private void ShowPopup()
    //    {
    //        using (var popup = new Form())
    //        {
    //            popup.Text = "快捷键触发的弹窗";
    //            popup.StartPosition = FormStartPosition.CenterScreen;
    //            popup.TopMost = true;
    //            popup.ShowDialog();
    //        }
    //    }

    //    private void Exit(object sender, EventArgs e)
    //    {
    //        trayIcon.Visible = false;
    //        globalHotkey.Dispose();
    //        Application.Exit();
    //    }
    //}

    //// 程序入口
    //static class Program
    //{
    //    [STAThread]
    //    static void Main()
    //    {
    //        Application.EnableVisualStyles();
    //        Application.SetCompatibleTextRenderingDefault(false);

    //        // 运行托盘应用
    //        Application.Run(new TrayApplication());

    //        // 或运行普通窗体应用
    //        // Application.Run(new EnhancedMainForm());
    //    }
    //}
}

