using Deckel.NativeMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace Deckel
{
    public partial class CoverWindow : Window
    {
        public CoverWindow()
        {
            InitializeComponent();

            Rect workingArea = SystemParameters.WorkArea;
            Left = workingArea.Left;
            Top = workingArea.Top;
            Width = workingArea.Width;
            Height = workingArea.Height;

            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nint hwnd = new WindowInteropHelper(this).Handle;
            User32.MakeWindowStayBehin(hwnd);
            User32.MakeWindowBlured(hwnd);
        }
    }
}
