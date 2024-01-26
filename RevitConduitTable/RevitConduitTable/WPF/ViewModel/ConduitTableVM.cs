using Prism.Mvvm;

using RevitConduitTable.WPF.Model;

using System.Collections.ObjectModel;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class ConduitTableVM : BindableBase
    {
        public ObservableCollection<ConduitModel> Conduits { get; set; }
    }
}
