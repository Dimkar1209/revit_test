using RevitConduitTable.WPF.ViewModel;

using System.Windows.Controls;

namespace RevitConduitTable.WPF.View
{
    /// <summary>
    /// Interaction logic for ConduitTableView.xaml
    /// </summary>
    public partial class ConduitTableView : UserControl
    {
        public ConduitTableView()
        {
            InitializeComponent();
            
            DataContext = new ConduitTableVM();
        }
    }
}
