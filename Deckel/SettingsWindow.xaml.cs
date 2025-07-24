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
using Deckel.Helpers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace Deckel
{
    public partial class SettingsWindow : FluentWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();

            AutoStartToggle.IsChecked = AutoStartHelper.IsAutoStartTaskEnabled();
            AutoStartToggle.Checked += (_, _) => AutoStartHelper.ToggleAutoStartTask();
            AutoStartToggle.Unchecked += (_, _) => AutoStartHelper.ToggleAutoStartTask();
        }
    }
}
