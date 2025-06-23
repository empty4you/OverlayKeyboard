using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace OverlayKey
{
    public partial class MainWindow : Window
    {
        private const int HOTKEY_ID = 9000;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public ObservableCollection<KeyViewModel> TopRowKeys { get; } = new();
        public ObservableCollection<KeyViewModel> NumRowKeys { get; } = new();
        public ObservableCollection<KeyViewModel> QwertyRowKeys { get; } = new();
        public ObservableCollection<KeyViewModel> AsdfRowKeys { get; } = new();
        public ObservableCollection<KeyViewModel> ZxcvRowKeys { get; } = new();
        public ObservableCollection<KeyViewModel> BottomRowKeys { get; } = new();
        public Color KeyboardBgColor { get; set; } = Color.FromArgb(204, 34, 40, 49);
        public Color KeyPressColor { get; set; } = Color.FromArgb(210, 255, 140, 0);


        private IntPtr _hookId = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;

        public MainWindow()
        {
            InitializeComponent();
            SettingsManager.LoadSettings();
            this.Loaded += MainWindow_Loaded;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = 0;
            this.Top = SystemParameters.PrimaryScreenHeight - this.Height - 40;


            TopRow.ItemsSource = TopRowKeys;
            NumRow.ItemsSource = NumRowKeys;
            QwertyRow.ItemsSource = QwertyRowKeys;
            AsdfRow.ItemsSource = AsdfRowKeys;
            ZxcvRow.ItemsSource = ZxcvRowKeys;
            BottomRow.ItemsSource = BottomRowKeys;

            // Верхний ряд (ESC, F1-F12, Del)
            TopRowKeys.Add(new KeyViewModel { Label = "E", Key = Key.Escape, GridColumn = 0 });
            TopRowKeys.Add(new KeyViewModel { Label = "F1", Key = Key.F1, GridColumn = 1 });
            TopRowKeys.Add(new KeyViewModel { Label = "F2", Key = Key.F2, GridColumn = 2 });
            TopRowKeys.Add(new KeyViewModel { Label = "F3", Key = Key.F3, GridColumn = 3 });
            TopRowKeys.Add(new KeyViewModel { Label = "F4", Key = Key.F4, GridColumn = 4 });
            TopRowKeys.Add(new KeyViewModel { Label = "F5", Key = Key.F5, GridColumn = 5 });
            TopRowKeys.Add(new KeyViewModel { Label = "F6", Key = Key.F6, GridColumn = 6 });
            TopRowKeys.Add(new KeyViewModel { Label = "F7", Key = Key.F7, GridColumn = 7 });
            TopRowKeys.Add(new KeyViewModel { Label = "F8", Key = Key.F8, GridColumn = 8 });
            TopRowKeys.Add(new KeyViewModel { Label = "F9", Key = Key.F9, GridColumn = 9 });
            TopRowKeys.Add(new KeyViewModel { Label = "F10", Key = Key.F10, GridColumn = 10 });
            TopRowKeys.Add(new KeyViewModel { Label = "Del", Key = Key.Delete, GridColumn = 13 });

            // Цифровой ряд 
            NumRowKeys.Add(new KeyViewModel { Label = "1", Key = Key.D1, GridColumn = 0 });
            NumRowKeys.Add(new KeyViewModel { Label = "2", Key = Key.D2, GridColumn = 1 });
            NumRowKeys.Add(new KeyViewModel { Label = "3", Key = Key.D3, GridColumn = 2 });
            NumRowKeys.Add(new KeyViewModel { Label = "4", Key = Key.D4, GridColumn = 3 });
            NumRowKeys.Add(new KeyViewModel { Label = "5", Key = Key.D5, GridColumn = 4 });
            NumRowKeys.Add(new KeyViewModel { Label = "6", Key = Key.D6, GridColumn = 5 });
            NumRowKeys.Add(new KeyViewModel { Label = "7", Key = Key.D7, GridColumn = 6 });
            NumRowKeys.Add(new KeyViewModel { Label = "8", Key = Key.D8, GridColumn = 7 });
            NumRowKeys.Add(new KeyViewModel { Label = "9", Key = Key.D9, GridColumn = 8 });
            NumRowKeys.Add(new KeyViewModel { Label = "0", Key = Key.D0, GridColumn = 9 });
            NumRowKeys.Add(new KeyViewModel { Label = "-", Key = Key.OemMinus, GridColumn = 10 });
            NumRowKeys.Add(new KeyViewModel { Label = "=", Key = Key.OemPlus, GridColumn = 11 });
            NumRowKeys.Add(new KeyViewModel { Label = "BS", Key = Key.Back, GridColumn = 12, ColSpan = 2 });

            // QWERTY ряд
            QwertyRowKeys.Add(new KeyViewModel { Label = "Tab", Key = Key.Tab, GridColumn = 0, ColSpan = 1 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "Q", Key = Key.Q, GridColumn = 1 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "W", Key = Key.W, GridColumn = 2 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "E", Key = Key.E, GridColumn = 3 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "R", Key = Key.R, GridColumn = 4 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "T", Key = Key.T, GridColumn = 5 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "Y", Key = Key.Y, GridColumn = 6 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "U", Key = Key.U, GridColumn = 7 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "I", Key = Key.I, GridColumn = 8 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "O", Key = Key.O, GridColumn = 9 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "P", Key = Key.P, GridColumn = 10 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "[", Key = Key.OemOpenBrackets, GridColumn = 11 });
            QwertyRowKeys.Add(new KeyViewModel { Label = "]", Key = Key.Oem6, GridColumn = 12 });


            // ASDFGH ряд
            AsdfRowKeys.Add(new KeyViewModel { Label = "Caps", Key = Key.CapsLock, GridColumn = 0, ColSpan = 1 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "A", Key = Key.A, GridColumn = 1 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "S", Key = Key.S, GridColumn = 2 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "D", Key = Key.D, GridColumn = 3 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "F", Key = Key.F, GridColumn = 4 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "G", Key = Key.G, GridColumn = 5 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "H", Key = Key.H, GridColumn = 6 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "J", Key = Key.J, GridColumn = 7 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "K", Key = Key.K, GridColumn = 8 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "L", Key = Key.L, GridColumn = 9 });
            AsdfRowKeys.Add(new KeyViewModel { Label = ";", Key = Key.Oem1, GridColumn = 10 });
            AsdfRowKeys.Add(new KeyViewModel { Label = "Enter", Key = Key.Enter, GridColumn = 12, ColSpan = 1 });

            //  ZXC ряд
            ZxcvRowKeys.Add(new KeyViewModel { Label = "Shift", Key = Key.LeftShift, GridColumn = 0, ColSpan = 2 });
            ZxcvRowKeys.Add(new KeyViewModel { Label = "Z", Key = Key.Z, GridColumn = 2 });
            ZxcvRowKeys.Add(new KeyViewModel { Label = "X", Key = Key.X, GridColumn = 3 });
            ZxcvRowKeys.Add(new KeyViewModel { Label = "C", Key = Key.C, GridColumn = 4 });
            ZxcvRowKeys.Add(new KeyViewModel { Label = "V", Key = Key.V, GridColumn = 5 });
            ZxcvRowKeys.Add(new KeyViewModel { Label = "B", Key = Key.B, GridColumn = 6 });
            ZxcvRowKeys.Add(new KeyViewModel { Label = "N", Key = Key.N, GridColumn = 7 });
            ZxcvRowKeys.Add(new KeyViewModel { Label = "M", Key = Key.M, GridColumn = 8 });
            ZxcvRowKeys.Add(new KeyViewModel { Label = ",", Key = Key.OemComma, GridColumn = 9 });
            ZxcvRowKeys.Add(new KeyViewModel { Label = "Shift", Key = Key.RightShift, GridColumn = 12, ColSpan = 2 });

            // Нижний ряд (Ctrl, Win, Alt, Space и т.д.)
            BottomRowKeys.Add(new KeyViewModel { Label = "Ctrl", Key = Key.LeftCtrl, GridColumn = 0 });
            BottomRowKeys.Add(new KeyViewModel { Label = "Win", Key = Key.LWin, GridColumn = 1 });
            BottomRowKeys.Add(new KeyViewModel { Label = "Alt", Key = Key.LeftAlt, GridColumn = 2 });
            BottomRowKeys.Add(new KeyViewModel { Label = "Space", Key = Key.Space, GridColumn = 3, ColSpan = 2 });
            BottomRowKeys.Add(new KeyViewModel { Label = "", Key = Key.None, GridColumn = 5 });
            BottomRowKeys.Add(new KeyViewModel { Label = "Alt", Key = Key.RightAlt, GridColumn = 6 });
            BottomRowKeys.Add(new KeyViewModel { Label = "Ctrl", Key = Key.RightCtrl, GridColumn = 8 });

            _proc = HookCallback;
            _hookId = SetHook(_proc);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using var curProcess = System.Diagnostics.Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            return SetWindowsHookEx(13, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;
            const int WM_SYSKEYDOWN = 0x0104;
            const int WM_SYSKEYUP = 0x0105;

            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                bool isPressed = false;
                bool isAltKey = (vkCode == 0xA4 || vkCode == 0xA5);

                if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
                {
                    isPressed = true;
                }
                else if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
                {
                    isPressed = false;
                }
                else
                {
                    return CallNextHookEx(_hookId, nCode, wParam, lParam);
                }

                Key key = KeyInterop.KeyFromVirtualKey(vkCode);

                foreach (var row in new[] { TopRowKeys, NumRowKeys, QwertyRowKeys, AsdfRowKeys, ZxcvRowKeys, BottomRowKeys })
                {
                    foreach (var keyVM in row)
                    {
                        if ((vkCode == 0xA4 && keyVM.Key == Key.LeftAlt) ||
                            (vkCode == 0xA5 && keyVM.Key == Key.RightAlt) ||
                            (!isAltKey && keyVM.Key == key))
                        {
                            keyVM.IsPressed = isPressed;
                        }
                    }
                }
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            RegisterHotKey(hwnd, HOTKEY_ID, 0, (uint)KeyInterop.VirtualKeyFromKey(Key.F10));
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }
        private void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg.message == WM_HOTKEY && (int)msg.wParam == HOTKEY_ID)
            {
                ToggleClickThrough();
                handled = true;
            }
        }
        private bool _clickThrough = true;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (!_clickThrough && e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void ToggleClickThrough()
        {
            _clickThrough = !_clickThrough;
            var hwnd = new WindowInteropHelper(this).Handle;
            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            if (_clickThrough)
                SetWindowLong(hwnd, GWL_EXSTYLE, exStyle | WS_EX_TRANSPARENT | WS_EX_LAYERED);
            else
                SetWindowLong(hwnd, GWL_EXSTYLE, exStyle & ~WS_EX_TRANSPARENT);

            this.IsHitTestVisible = !_clickThrough;
            this.Topmost = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            SettingsManager.SaveSettings();
            UnhookWindowsHookEx(_hookId);
            base.OnClosed(e);
        }

        private void OpenSettings()
        {
            var wnd = new SettingsWindow(KeyColorConverter.NormalColor, KeyColorConverter.PressedColor);
            wnd.Owner = this;
            wnd.OnApplyColors = (bg, press) =>
            {
                KeyColorConverter.NormalColor = bg;
                KeyColorConverter.PressedColor = press;

                
                SettingsManager.Settings.KeyColor = $"#{bg.A:X2}{bg.R:X2}{bg.G:X2}{bg.B:X2}";
                SettingsManager.Settings.PressedColor = $"#{press.A:X2}{press.R:X2}{press.G:X2}{press.B:X2}";
                SettingsManager.SaveSettings();

                
                foreach (var row in new[] { TopRowKeys, NumRowKeys, QwertyRowKeys, AsdfRowKeys, ZxcvRowKeys, BottomRowKeys })
                    foreach (var key in row)
                        key.IsPressed = key.IsPressed;
            };
            wnd.Show();
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                OpenSettings();
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }











        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int WS_EX_LAYERED = 0x00080000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


    }
}
