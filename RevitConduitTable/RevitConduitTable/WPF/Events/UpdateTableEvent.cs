using Prism.Events;

using RevitConduitTable.WPF.Model;
using System.Collections.ObjectModel;

namespace RevitConduitTable.WPF.Events
{
    internal class UpdateTableEvent : PubSubEvent<ObservableCollection<ConduitItem>>
    {
        // TODO: Future implentation updating table from Revit App
    }
}
