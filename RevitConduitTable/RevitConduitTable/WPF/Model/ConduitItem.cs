using Prism.Mvvm;

using System.Collections.Generic;

namespace RevitConduitTable.WPF.Model
{
    internal class ConduitItem : BindableBase
    {
        public Dictionary<string, ConduitProperty> Properties
        {
            get { return _properties; }
            set { SetProperty(ref _properties, value); }
        }

        private Dictionary<string, ConduitProperty> _properties;
    }
}
