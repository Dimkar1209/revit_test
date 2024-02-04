using System.Collections.Generic;

namespace RevitConduitTable.RevitUIBuilder
{
    internal interface ICustomTab
    {
        string TabName { get; }

        IReadOnlyCollection<ICustomRibbonPanel> Panels { get; }
    }
}
