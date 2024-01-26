using System.Collections.Generic;

namespace RevitConduitTable.RevitUIBuilder.ConduitTableTab
{
    internal class ConduitTab : ICustomTab
    {
        public string TabName => Resources.UI_Text.TAB_NAME;

        public IReadOnlyCollection<ICustomRibbonPanel> Panels => _panels;

        private List<ICustomRibbonPanel> _panels = new List<ICustomRibbonPanel>() { new ConduitPanel() };

    }
}
