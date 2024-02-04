using RevitConduitTable.Constants;
using RevitConduitTable.Resources;
using RevitConduitTable.WPF.Model;

using System;
using System.Collections.Generic;

internal class DefaultValuePercentageRule : IPropertyValidator
{
    public IEnumerable<string> Validate(object value)
    {
        var errors = new List<string>();

        if (double.TryParse(value.ToString(), out double paramValue))
        {
            if (paramValue > ParametersConstants.PERCENTAGE_RULE_UPPER_LIMIT || paramValue <ParametersConstants.PERCENTAGE_RULE_LOWER_LIMIT)
            {
                errors.Add(UI_Text.DEFAULT_RULE_MESSAGE);
            }
        }

        return errors;
    }
}