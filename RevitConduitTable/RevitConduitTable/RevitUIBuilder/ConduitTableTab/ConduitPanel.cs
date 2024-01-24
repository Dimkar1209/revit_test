using System;
using System.Collections.Generic;

namespace RevitConduitTable.RevitUIBuilder.ConduitTableTab
{
    internal class ConduitPanel : ICustomRibbonPanel
    {
        public string PanelName => Resources.UI_Text.PANEL_NAME;

        public IReadOnlyCollection<ICustomButton> Buttons => _buttons;

        private List<ICustomButton> _buttons = new List<ICustomButton>() { new ConduitButton() };
    }
}
