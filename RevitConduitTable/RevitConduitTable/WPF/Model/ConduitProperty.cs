﻿using Prism.Mvvm;

public class ConduitProperty : BindableBase
{
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

    public object ParameterValue
    {
        get { return _parameterValue; }
        set { SetProperty(ref _parameterValue, value); }
    }

    private string _parameterName;
    private bool _isReadonly;
    private object _parameterValue;

}