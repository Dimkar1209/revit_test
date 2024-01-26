using System.Collections.Generic;

namespace RevitConduitTable.RevitUIBuilder
{
    internal interface ICustomRibbonPanel
    {
        string PanelName { get; }
        IReadOnlyCollection<ICustomButton> Buttons { get; }
    }
}
