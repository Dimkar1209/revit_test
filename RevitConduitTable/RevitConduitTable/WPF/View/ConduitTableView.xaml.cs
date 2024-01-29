using Prism.Events;
using Prism.Ioc;

using RevitConduitTable.WPF.Events;
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
        public ConduitTableView(IContainerProvider containerProvider, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            DataContext = containerProvider.Resolve<ConduitTableViewModel>();
            this.Loaded += ConduitTableView_Loaded;
            eventAggregator.GetEvent<UpdateTableEvent>().Subscribe(UpdateDataGridColumns);
        }

        /// <summary>
        /// Update DataGrid
        /// </summary>
        /// <param name="obj">Event object</param>
        private void UpdateDataGridColumns(object obj)
        {
            UpdateDynamicTable();
        }

        private void ConduitTableView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDynamicTable();
        }

        private void UpdateDynamicTable()
        {
            if (DataContext is ConduitTableViewModel viewModel)
            {
                DataGrid dataGrid = this.ConduitTable;
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
                        bool isVisible = viewModel.Conduits.FirstOrDefault()?.Properties[paramName].IsVisible ?? false;

                        var binding = new System.Windows.Data.Binding($"Properties[{paramName}].ParameterValue")
                        {
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            ValidatesOnNotifyDataErrors = true
                        };

                        dataGrid.Columns.Add(new DataGridTextColumn
                        {
                            Header = paramName,
                            Binding = binding,
                            Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                            MinWidth = 20,
                            IsReadOnly = isReadOnly,
                            Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed,
                        });
                    }
                }
            }
        }

    }
}
