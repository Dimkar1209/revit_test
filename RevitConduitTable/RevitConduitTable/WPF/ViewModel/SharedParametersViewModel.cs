using Autodesk.Revit.DB.Electrical;

using Prism.Events;

using RevitConduitTable.Helpers;
using RevitConduitTable.WPF.Events;
using RevitConduitTable.WPF.Model;
using RevitConduitTable.WPF.Services;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class SharedParametersViewModel
    {
        public ObservableCollection<SharedParametersModel> DisplayItems { get; private set; } = new ObservableCollection<SharedParametersModel>();

        public SharedParametersViewModel(IEventAggregator eventAggregator, ICollectionService collectionService)
        {

            _collectionService = collectionService;
            _conduits = _collectionService.ConduitsShare;

            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UpdateTableEvent>().Subscribe(UpdateDataGridColumns);
            _parameterNames = RevitDocumentHelper.GetSharedParameterNames();
            PrepareDisplayItems(_conduits, _parameterNames);
        }

        private void UpdateDataGridColumns(object obj)
        {
            _conduits = (ObservableCollection<ConduitItem>)obj;
            _parameterNames = RevitDocumentHelper.GetSharedParameterNames();

            PrepareDisplayItems(_conduits, _parameterNames);
        }

        private void PrepareDisplayItems(ObservableCollection<ConduitItem> _conduits, IDictionary<string, System.Guid> parameterNames)
        {
            DisplayItems.Clear();

            List<string> paramNames = _conduits.FirstOrDefault()?.Properties.Keys.ToList() ?? Enumerable.Empty<string>().ToList();
            paramNames.Reverse();
            int paramNamesCount = paramNames.Count;
            // Add mapping (paramNames map to DictionaryKey via JSON load, and default values.


            foreach (string key in parameterNames.Keys)
            {
                DisplayItems.Add(new SharedParametersModel
                {
                    ParameterName = paramNamesCount < 1 ? string.Empty : paramNames[--paramNamesCount],
                    DictionaryKey = key,
                    GuidString = parameterNames[key].ToString()
                });
            }
        }

        private readonly IEventAggregator _eventAggregator;
        private ICollectionService _collectionService;
        private IDictionary<string, System.Guid> _parameterNames = new Dictionary<string, System.Guid>();

        // Set Defaut conduit data if needed.
        private ObservableCollection<ConduitItem> _conduits = new ObservableCollection<ConduitItem>();
    }
}
