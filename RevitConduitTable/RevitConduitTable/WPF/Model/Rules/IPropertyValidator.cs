using System;
using System.Collections.Generic;

namespace RevitConduitTable.WPF.Model
{
    public interface IPropertyValidator
    {
        IEnumerable<string> Validate(object value);
    }
}
