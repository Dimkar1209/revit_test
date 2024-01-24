using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitConduitTable.RevitUIBuilder
{
    internal interface ICustomTab
    {
        string TabName { get; }

        IReadOnlyCollection<ICustomRibbonPanel> Panels { get; }
    }
}
