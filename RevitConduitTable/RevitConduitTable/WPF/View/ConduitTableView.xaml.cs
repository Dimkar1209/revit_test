using Prism.Ioc;

using RevitConduitTable.WPF.ViewModel;

using System.Collections.Generic;
using System.Linq;
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

                if (viewModel.Conduits != null)
                {

                    HashSet<string> uniqueParameterNames = viewModel.Conduits
                        .SelectMany(conduit => conduit.Properties.Keys)
                        .Distinct()
                        .ToHashSet();

                    foreach (string paramName in uniqueParameterNames)
                    {
                        bool isReadOnly = viewModel.Conduits.FirstOrDefault()?.Properties[paramName].IsReadonly ?? false;

                        var binding = new System.Windows.Data.Binding($"Properties[{paramName}].ParameterValue")
                        {
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };

                        dataGrid.Columns.Add(new DataGridTextColumn
                        {
                            Header = paramName,
                            Binding = binding,
                            Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                            MinWidth = 20,
                            IsReadOnly= isReadOnly
                        });
                    }
                }
            }
        }

        private void ConduitTableView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ConduitTableViewModel viewModel)
            {
                viewModel.Conduits.CollectionChanged += Conduits_CollectionChanged;
                UpdateDataGridColumns(this.ConduitTable);
            }
        }

        private void Conduits_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           
        }
    }
}
