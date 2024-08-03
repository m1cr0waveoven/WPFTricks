using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace ShowingCustomWindowContextMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const Int32 WM_NCRBUTTONDOWN = 0xa4; // WM => Window Message
        private const uint HTCAPTION = 0x02; // HT => Hit Test
        public MainWindow()
        {
            InitializeComponent();
            Loaded += WindowLoaded;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            IntPtr windowHandler = new WindowInteropHelper(this).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(windowHandler);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }

        private nint WndProc(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
        {
            // When user resses down the right mouse button on non client area of the window and the click happened in the title bar
            if (msg == WM_NCRBUTTONDOWN && wParam.ToInt32() == HTCAPTION)
            {
                ShowContextMenu();
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void ShowContextMenu()
        {
            if (Resources["contextMenu"] is ContextMenu contextMenu)
            {
                contextMenu.IsOpen = true;
            }
        }

        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as MenuItem ?? new MenuItem() { Header = "Empty" };
            MessageBox.Show($"You clicked on {item.Header}.");
        }
    }
}