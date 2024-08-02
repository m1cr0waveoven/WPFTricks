using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace AppentToWPFWindowContextMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, IntPtr wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        public const Int32 MF_BYPOSITION = 0x400;

        public const Int32 MF_SEPARATOR = 0x800;

        public const Int32 ITEM0ID = 0;
        public const Int32 ITEM1ID = 1001;
        public const Int32 ITEM2ID = 1002;

        public const Int32 WM_SYSCOMMAND = 0x112; // Window message sys command
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(windowHandle);

            IntPtr systemMenuHandle = GetSystemMenu(windowHandle, false);

            InsertMenu(systemMenuHandle, 5, MF_BYPOSITION | MF_SEPARATOR, ITEM0ID, string.Empty);
            InsertMenu(systemMenuHandle, 6, MF_BYPOSITION, ITEM1ID, "Alma");
            InsertMenu(systemMenuHandle, 7, MF_BYPOSITION, ITEM2ID, "Körte");

            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwn, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SYSCOMMAND)
            {
                switch (wParam.ToInt32())
                {
                    case ITEM1ID:
                        MessageBox.Show("Alma was clicked");
                        handled = true;
                        break;

                    case ITEM2ID:
                        MessageBox.Show("Körte was clicked");
                        handled = true;
                        break;
                }
            }

            return IntPtr.Zero;
        }
    }
}