using Prism.Mvvm;

using RevitConduitTable.WPF.Model;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

internal class ConduitProperty : BindableBase, INotifyDataErrorInfo
{
    public ConduitProperty()
    {
        PropeptyRule = new DefaultValuePercentageRule();
    }

    public string ParameterName
    {
        get { return _parameterName; }
        set { SetProperty(ref _parameterName, value); }
    }

    public bool IsReadonly
    {
        get { return _isReadonly; }
        set { SetProperty(ref _isReadonly, value); }
    }

    public bool IsVisible
    {
        get { return _isVisible; }
        set { SetProperty(ref _isVisible, value); }
    }

    public object ParameterValue
    {
        get => _parameterValue;
        set => SetProperty(ref _parameterValue, value, () => ValidatePropertyValue(value));
    }

    public IPropeptyRule PropeptyRule
    {
        get { return _propeptyRule; }
        set { SetProperty(ref _propeptyRule, value); }
    }

    public IEnumerable GetErrors(string propertyName)
    {
        if (_errors.ContainsKey(propertyName))
            return _errors[propertyName];
        return null;
    }

    public bool HasErrors => _errors.Count > 0;

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    protected virtual void RaiseErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    private void ValidatePropertyValue(object value)
    {
        _errors.Remove(nameof(ParameterValue));

        if (double.TryParse(value.ToString(), out double paramValue))
        {
            if (paramValue > PropeptyRule.UpperLimit
                || paramValue < PropeptyRule.LowerLimit)
            {
                _errors[nameof(ParameterValue)] = new List<string> { PropeptyRule.RuleMessage };
                RaiseErrorsChanged(nameof(ParameterValue));
            }
        }

    }

    private string _parameterName;
    private bool _isReadonly;
    private bool _isVisible;
    private object _parameterValue;
    private IPropeptyRule _propeptyRule;
    private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

}
