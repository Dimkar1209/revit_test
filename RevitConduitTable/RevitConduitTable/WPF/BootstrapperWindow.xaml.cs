using System.Windows;

namespace RevitConduitTable.WPF
{
    /// <summary>
    /// Interaction logic for BootstrapperWindow.xaml
    /// </summary>
    public partial class BootstrapperWindow : Window
    {
        public BootstrapperWindow()
        {
            InitializeComponent();
        }

        public void ShowMainWindow()
        {
            base.Show();
        }

        public static string MainRegionName => Constants.RegionConstants.MAIN_REGION_NAME;
    }
}
