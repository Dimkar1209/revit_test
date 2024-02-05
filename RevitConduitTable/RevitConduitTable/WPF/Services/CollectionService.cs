using RevitConduitTable.WPF.Model;

using System.Collections.ObjectModel;

namespace RevitConduitTable.WPF.Services
{
    internal class CollectionService : ICollectionService
    {
        public ObservableCollection<ConduitItem> ConduitsShare { get; set; } = new ObservableCollection<ConduitItem>();
    }

}
