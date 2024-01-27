using Prism.Mvvm;

using RevitConduitTable.WPF.Model;
using RevitConduitTable.WPF.Services;

using System.Collections.ObjectModel;
using System.Windows;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class ConduitTableViewModel : BindableBase
    {
        public ObservableCollection<ConduitModel> Conduits { get; set; }
    }
}
