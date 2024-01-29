using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using RevitConduitTable.Constants;
using RevitConduitTable.Resources;
using RevitConduitTable.WPF.Events;
using RevitConduitTable.WPF.Model;
using RevitConduitTable.WPF.View;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class ConduitTableViewModel : BindableBase
    {
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand AddColumnCommand { get; private set; }
        public DelegateCommand RemoveCommand { get; private set; }

        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }

        public DelegateCommand ToggleHideCommand { get; private set; }

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

        public string HideButtonText
        {
            get { return _hideButtonText; }
            set { SetProperty(ref _hideButtonText, value); }
        }

        public ConduitTableViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _conduits = new ObservableCollection<ConduitItem> { new ConduitItem() { Properties = defautProperties } };

            AddCommand = new DelegateCommand(ExecuteAddCommand);
            RemoveCommand = new DelegateCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand)
                .ObservesProperty(() => SelectedConduit);

            CopyCommand = new DelegateCommand(ExecuteCopyCommand, CanExecuteCopyCommand)
                .ObservesProperty(() => SelectedConduit);

            PasteCommand = new DelegateCommand(ExecutePasteCommand, CanExecutePasteCommand);
            AddColumnCommand = new DelegateCommand(ExecuteAddColumnCommand);
            ToggleHideCommand = new DelegateCommand(ExecuteToggleHideCommand);

            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _isFiledsHidden = false;
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
                        ParameterValue = keyValuePair.Key == ParametersConstants.DATAGRID_ID ?
                        (int)lastConduit.Properties[ParametersConstants.DATAGRID_ID].ParameterValue + 1
                        : keyValuePair.Value.ParameterValue
                    };
                }

                _conduits.Add(newConduit);
            }
            else
            {
                _conduits.Add(new ConduitItem()
                {
                    Properties = defautProperties
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
                item.Properties.Add(columnAdd, new ConduitProperty()
                { ParameterName = columnAdd, ParameterValue = 1, IsReadonly = false, IsVisible = true });
            }

            RaisePropertyChanged(nameof(Conduits));

            _eventAggregator.GetEvent<UpdateTableEvent>().Publish(null);
            _copiedConduit = null;
        }

        #region Copy/Paste commands

        private void ExecuteCopyCommand()
        {
            _copiedConduit = SelectedConduit;
            PasteCommand.RaiseCanExecuteChanged();
        }

        private bool CanExecuteCopyCommand()
        {
            return SelectedConduit != null;
        }

        private void ExecutePasteCommand()
        {
            if (_copiedConduit != null && SelectedConduit != null)
            {
                var newProperties = new Dictionary<string, ConduitProperty>();

                foreach (var kvp in _copiedConduit.Properties)
                {
                    // ID parameter is unique.
                    if (kvp.Key == ParametersConstants.DATAGRID_ID)
                    {
                        newProperties[kvp.Key] = new ConduitProperty()
                        {
                            ParameterName = ParametersConstants.DATAGRID_ID,
                            ParameterValue = SelectedConduit.Properties[ParametersConstants.DATAGRID_ID].ParameterValue,
                            IsReadonly = true
                        };
                    }
                    else
                    {
                        newProperties[kvp.Key] = new ConduitProperty()
                        {
                            ParameterName = kvp.Value.ParameterName,
                            ParameterValue = kvp.Value.ParameterValue,
                            IsReadonly = kvp.Value.IsReadonly
                        };
                    }
                }

                SelectedConduit.Properties = newProperties;
                RaisePropertyChanged(nameof(SelectedConduit));
            }
        }

        private bool CanExecutePasteCommand()
        {
            return _copiedConduit != null;
        }

        #endregion

        private void ExecuteToggleHideCommand()
        {
            CalculatedFieldsVisibility(_isFiledsHidden);
            _isFiledsHidden = !_isFiledsHidden;
            HideButtonText = _isFiledsHidden ? UI_Text.UNHIDE_FIELDS_BUTTON : UI_Text.HIDE_FIELDS_BUTTON;
        }

        private void CalculatedFieldsVisibility(bool isVisible)
        {
            Conduits.ToList().ForEach(item => 
                _calcululatedProperties.ForEach(calculatedProperty =>
                    item.SetVisibilityByKey(calculatedProperty, isVisible)));

            _eventAggregator.GetEvent<UpdateTableEvent>().Publish(null);
        }

        private ObservableCollection<ConduitItem> _conduits;
        private ConduitItem _selectedConduit;
        private ConduitItem _copiedConduit;

        private IDialogService _dialogService;
        private IEnumerable<string> GetColumns => _conduits.FirstOrDefault()?.Properties.Keys ?? Enumerable.Empty<string>();
        private string _hideButtonText = UI_Text.HIDE_FIELDS_BUTTON;
        private bool _isFiledsHidden;

        private readonly static List<string> _calcululatedProperties = new List<string>() { "C0", "C1", "C2", "C3", "C4" };
        private readonly static List<string> _iniProperties = new List<string>() { "I_1", "I_2", "I_3", "I_4", "I_5" };

        private readonly static Dictionary<string, ConduitProperty> defautProperties = new Dictionary<string, ConduitProperty>()
        {   
            { ParametersConstants.DATAGRID_ID, new ConduitProperty() { ParameterName = ParametersConstants.DATAGRID_ID, ParameterValue = 1, IsReadonly = true, IsVisible = true }},
            { _calcululatedProperties[0], new ConduitProperty() { ParameterName = _calcululatedProperties[0], ParameterValue = 1, IsReadonly = true, IsVisible = true }},
            { _calcululatedProperties[1], new ConduitProperty() { ParameterName = _calcululatedProperties[1], ParameterValue = 2, IsReadonly = true, IsVisible = true }},
            { _calcululatedProperties[2], new ConduitProperty() { ParameterName = _calcululatedProperties[2], ParameterValue = 2, IsReadonly = true, IsVisible = true }},
            { _calcululatedProperties[3], new ConduitProperty() { ParameterName = _calcululatedProperties[3], ParameterValue = 2, IsReadonly = true, IsVisible = true }},
            { _calcululatedProperties[4], new ConduitProperty() { ParameterName = _calcululatedProperties[4], ParameterValue = 2, IsReadonly = true, IsVisible = true }},
            { _iniProperties[0], new ConduitProperty() { ParameterName = _iniProperties[0], ParameterValue = "text", IsReadonly = false, IsVisible = true }},
            { _iniProperties[1], new ConduitProperty() { ParameterName = _iniProperties[1], ParameterValue = 2, IsReadonly = false, IsVisible = true }},
            { _iniProperties[2], new ConduitProperty() { ParameterName = _iniProperties[2], ParameterValue = 2, IsReadonly = false, IsVisible = true }},
            { _iniProperties[3], new ConduitProperty() { ParameterName = _iniProperties[3], ParameterValue = 2, IsReadonly = false, IsVisible = true }},
            { _iniProperties[4], new ConduitProperty() { ParameterName = _iniProperties[4], ParameterValue = 2, IsReadonly = false, IsVisible = true }},
        };
    }
}
