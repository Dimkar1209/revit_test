using Prism.Commands;
using Prism.Mvvm;

using RevitConduitTable.WPF.Model;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Interactivity;
using Prism.Services.Dialogs;
using RevitConduitTable.WPF.View;
using RevitConduitTable.Constants;
using System.Linq;
using Prism.Events;
using RevitConduitTable.WPF.Events;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class ConduitTableViewModel : BindableBase
    {
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand AddColumnCommand { get; private set; }
        public DelegateCommand RemoveCommand { get; private set; }

        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }

        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<ConduitItem> Conduits
        {
            get { return _conduits; }
            set { SetProperty(ref _conduits, value); }
        }

        public ConduitItem SelectedConduit
        {
            get { return _selectedConduit; }
            set { SetProperty(ref _selectedConduit, value); }
        }

        private IEnumerable<string> GetColumns => _conduits.FirstOrDefault()?.Properties.Keys ?? Enumerable.Empty<string>();


        public ConduitTableViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _conduits = new ObservableCollection<ConduitItem>
            {
                new ConduitItem()
                {
                    Properties = new Dictionary<string, ConduitProperty>()
                    {
                        { "ID", new ConduitProperty() { ParameterName = "ID", ParameterValue = 1, IsReadonly=true }},
                        { "C1", new ConduitProperty() { ParameterName = "C1", ParameterValue = 2, IsReadonly=false }}
                    }
                }
            };

            AddCommand = new DelegateCommand(ExecuteAddCommand);
            RemoveCommand = new DelegateCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand)
            .ObservesProperty(() => SelectedConduit);

            AddColumnCommand = new DelegateCommand(ExecuteAddColumnCommand);

            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
        }

        #region Add/Remove commands

        private void ExecuteAddCommand()
        {
            if (_conduits.Count > 0)
            {
                var lastConduit = _conduits[_conduits.Count - 1];
                var newConduit = new ConduitItem()
                {
                    Properties = new Dictionary<string, ConduitProperty>()
                };

                foreach (var keyValuePair in lastConduit.Properties)
                {
                    newConduit.Properties[keyValuePair.Key] = new ConduitProperty()
                    {
                        ParameterName = keyValuePair.Value.ParameterName,
                        ParameterValue = keyValuePair.Key == "ID" ? (int)lastConduit.Properties["ID"].ParameterValue + 1 : keyValuePair.Value.ParameterValue
                    };
                }

                _conduits.Add(newConduit);
            }
            else
            {
                _conduits.Add(new ConduitItem()
                {
                    Properties = defautproperties
                });
            }
        }

        private void ExecuteRemoveCommand()
        {
            if (SelectedConduit != null && _conduits.Contains(SelectedConduit))
            {
                _conduits.Remove(SelectedConduit);
            }
        }

        private bool CanExecuteRemoveCommand()
        {
            return SelectedConduit != null;
        }

        #endregion

        private void ExecuteAddColumnCommand()
        {
            var parameters = new DialogParameters() { { ParametersConstants.EXISTING_COLUMNS_DIALOG, GetColumns } };
            string columnAdd = string.Empty;

            _dialogService.ShowDialog(typeof(EditColumsDialog).Name, parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    columnAdd = result.Parameters.GetValue<string>(ParametersConstants.COLUMN_NAME_DIALOG);
                }
            });

            foreach (var item in _conduits)
            {
                item.Properties.Add(columnAdd, new ConduitProperty() { ParameterName = columnAdd, ParameterValue = 1, IsReadonly = true });
            }

            RaisePropertyChanged(nameof(Conduits));

            _eventAggregator.GetEvent<UpdateTableEvent>().Publish(null);
        }

        ObservableCollection<ConduitItem> _conduits;
        private ConduitItem _selectedConduit;
        private IDialogService _dialogService;

        private Dictionary<string, ConduitProperty> defautproperties = new Dictionary<string, ConduitProperty>()
                    {
                        { "ID", new ConduitProperty() { ParameterName = "ID", ParameterValue = 1 }},
                        { "C1", new ConduitProperty() { ParameterName = "C1", ParameterValue = 2 }}
                    };
    }
}
