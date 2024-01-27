using Autodesk.Revit.DB.Electrical;

using Prism.Commands;
using Prism.Mvvm;

using RevitConduitTable.WPF.Model;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class ConduitTableViewModel : BindableBase
    {
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand RemoveCommand { get; private set; }


        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }

        public ObservableCollection<string> ColumnHeaders
        {
            get { return _columnHeaders; }
            set { SetProperty(ref _columnHeaders, value); }
        }

        public ObservableCollection<ConduitModel> Conduits
        {
            get { return _conduits; }
            set { SetProperty(ref _conduits, value); }
        }

        public ConduitModel SelectedConduit
        {
            get { return _selectedConduit; }
            set { SetProperty(ref _selectedConduit, value); }
        }

        public ConduitTableViewModel()
        {
            Conduits = new ObservableCollection<ConduitModel>();
            _columnHeaders = new ObservableCollection<string>() { "ID", "AAA" };
            // Example: Add a ConduitModel and subscribe to its DictionaryChanged event
            var conduit = new ConduitModel();
            
            conduit.AddProperty("ID", 1);
            conduit.AddProperty("AAA", 1);
            Conduits.Add(conduit);


            AddCommand = new DelegateCommand(EditSelectedConduit);
            RemoveCommand = new DelegateCommand(RemoveSelectedConduit);
            EditCommand = new DelegateCommand(EditSelectedConduit);

            CopyCommand = new DelegateCommand(CopySelectedConduit);
            PasteCommand = new DelegateCommand(PasteSelectedConduit);
            _copiedConduit = new ConduitModel();
        }

        public void AddConduit()
        {
            Conduits.Add(new ConduitModel());
        }

        public void RemoveSelectedConduit()
        {
            if (SelectedConduit != null)
            {
                Conduits.Remove(SelectedConduit);
            }
        }

        private void EditSelectedConduit()
        {

        }

        public void CopySelectedConduit()
        {

        }

        public void AddColumn(string keyProperty)
        {


        }

        public void RemoveColumn(string keyProperty)
        {

        }

        private void PasteSelectedConduit()
        {
            
        }

        private ObservableCollection<ConduitModel> _conduits;
        private ObservableCollection<string> _columnHeaders;

        private ConduitModel _selectedConduit;
        private ConduitModel _copiedConduit;
    }
}
