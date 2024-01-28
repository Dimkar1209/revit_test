using Prism.Commands;
using Prism.Mvvm;

using RevitConduitTable.WPF.Model;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class ConduitTableViewModel : BindableBase
    {
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand RemoveCommand { get; private set; }

        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }

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

        public ConduitTableViewModel()
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
        }

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


        ObservableCollection<ConduitItem> _conduits;
        private ConduitItem _selectedConduit;
        private Dictionary<string, ConduitProperty> defautproperties = new Dictionary<string, ConduitProperty>()
                    {
                        { "ID", new ConduitProperty() { ParameterName = "ID", ParameterValue = 1 }},
                        { "C1", new ConduitProperty() { ParameterName = "C1", ParameterValue = 2 }}
                    };
    }
}
