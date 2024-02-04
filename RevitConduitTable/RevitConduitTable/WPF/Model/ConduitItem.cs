using Autodesk.Revit.DB.Electrical;

using Prism.Mvvm;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace RevitConduitTable.WPF.Model
{
    internal class ConduitItem : BindableBase
    {
        public Dictionary<string, ConduitProperty> Properties
        {
            get { return _properties; }
            set { SetProperty(ref _properties, value); }
        }

        public void SetVisibilityByKey(string key, bool isVisible)
        {
            if (_properties.TryGetValue(key, out var conduitProperty))
            {
                conduitProperty.IsVisible = isVisible;
            }
        }

        public bool IsAllHidden()
        {
            return _properties.Values.Any(x => x.IsVisible == false);
        }

        public void SetReadOnlyByKey(string key, bool isReadonly)
        {
            if (_properties.TryGetValue(key, out var conduitProperty))
            {
                conduitProperty.IsReadonly = isReadonly;
            }

            RaisePropertyChanged(nameof(Properties));
        }

        private Dictionary<string, ConduitProperty> _properties;
    }
}
