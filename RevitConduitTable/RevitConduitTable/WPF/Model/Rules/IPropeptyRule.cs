using System;
using System.Collections.Generic;

namespace RevitConduitTable.WPF.Model
{
    internal interface IPropeptyRule
    {
        Type ParameterType { get; }

        double UpperLimit { get; }

        double LowerLimit { get; }

        string RuleMessage { get; }

        IEnumerable<string> AcceptedText { get; }
    }
}
