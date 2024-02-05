using RevitConduitTable.WPF.Model;

using System.Collections.ObjectModel;

namespace RevitConduitTable.WPF.Services
{
    internal interface ICollectionService
    {
        ObservableCollection<ConduitItem> ConduitsShare { get; set; }
    }
}
