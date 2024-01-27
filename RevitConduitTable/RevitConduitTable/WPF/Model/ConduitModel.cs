using Prism.Mvvm;

using System.Collections.Generic;
using System.ComponentModel;

namespace RevitConduitTable.WPF.Model
{
    internal class ConduitModel : BindableBase
    {
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        public event PropertyChangedEventHandler DictionaryChanged;

        public object this[string propertyName]
        {
            get => _properties.ContainsKey(propertyName) ? _properties[propertyName] : null;
            set
            {
                bool changed = false;
                if (_properties.ContainsKey(propertyName))
                {
                    if (_properties[propertyName] != value)
                    {
                        _properties[propertyName] = value;
                        changed = true;
                    }
                }
                else
                {
                    _properties.Add(propertyName, value);
                    changed = true;
                }
                if (changed)
                {
                    RaisePropertyChanged(propertyName);
                    OnDictionaryChanged(propertyName);
                }
            }
        }

        public void AddProperty(string propertyName, object value)
        {
            if (!_properties.ContainsKey(propertyName))
            {
                _properties.Add(propertyName, value);
                RaisePropertyChanged(propertyName);
            }
        }

        public bool RemoveProperty(string propertyName)
        {
            if (_properties.ContainsKey(propertyName))
            {
                return _properties.Remove(propertyName);
            }
            return false;
        }

        private void OnDictionaryChanged(string propertyName)
        {
            DictionaryChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
