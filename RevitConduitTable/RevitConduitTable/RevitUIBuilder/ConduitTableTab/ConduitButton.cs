using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace RevitConduitTable.RevitUIBuilder
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ConduitButton : ICustomButton
    {
        public string Name => this.GetType().Name;

        public string ClassName => this.GetType().FullName;

        public string ButtonText => Resources.UI_Text.BUTTON_CONDUIT_NAME;

        public string ImagePath => string.Empty;

        public string Tooltip => string.Empty;

        public bool IsEnabled => true;


        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                TaskDialog.Show("Hello World", "Hello, Revit!");
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

    }
}
