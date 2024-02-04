using RevitConduitTable.Constants;
using RevitConduitTable.Resources;
using RevitConduitTable.WPF.Model;

using System;
using System.Collections.Generic;
using System.Linq;

internal class DefaultValuePercentageRule : IPropeptyRule
{
    public Type ParameterType => typeof(double);
    public double UpperLimit => ParametersConstants.PERCENTAGE_RULE_UPPER_LIMIT;
    public double LowerLimit => ParametersConstants.PERCENTAGE_RULE_LOWER_LIMIT;
    public string RuleMessage => UI_Text.DEFAULT_RULE_MESSAGE;
    public IEnumerable<string> AcceptedText => Enumerable.Empty<string>();
}