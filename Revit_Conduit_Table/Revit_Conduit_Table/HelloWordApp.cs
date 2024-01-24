using Autodesk.Revit.UI;
using Revit_Conduit_Table;
using Revit_Conduit_Table.Resources;
using System.Reflection;

namespace RevitHelloWorld
{
    public class HelloWordApp : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab(UI_Text.ADDIN_TAB_NAME);

            RibbonPanel ribbonPanel = application.CreateRibbonPanel(UI_Text.ADDIN_TAB_NAME, UI_Text.ADDIN_PANEL_NAME);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            var buttonData = new PushButtonData(
                "HelloWordCmd",
                "Hello World",
                thisAssemblyPath,
                HelloWordCmd.CommandPath);

            var pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {

            return Result.Succeeded;
        }
    }
}
