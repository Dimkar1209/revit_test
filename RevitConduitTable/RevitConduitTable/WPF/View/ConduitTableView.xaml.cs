using Prism.Ioc;

using RevitConduitTable.WPF.ViewModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RevitConduitTable.WPF.View
{
    /// <summary>
    /// Interaction logic for ConduitTableView.xaml
    /// </summary>
    public partial class ConduitTableView : UserControl
    {
        public ConduitTableView(IContainerProvider containerProvider)
        {
            InitializeComponent();
            DataContext = containerProvider.Resolve<ConduitTableViewModel>();
            this.Loaded += ConduitTableView_Loaded;
        }

        private void UpdateDataGridColumns(DataGrid dataGrid)
        {
            if (DataContext is ConduitTableViewModel viewModel)
            {
                dataGrid.Columns.Clear();

                foreach (var header in viewModel.ColumnHeaders)
                {
                    var binding = new Binding($"[{header}]");
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = header, Binding = binding });
                }

                viewModel.Conduits.CollectionChanged += Conduits_CollectionChanged;
            }
        }

        private void Conduits_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateDataGridColumns(this.ConduitTable);
        }

        private void ConduitTableView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGridColumns(this.ConduitTable);
        }
    }
}
