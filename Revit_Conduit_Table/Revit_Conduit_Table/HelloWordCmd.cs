using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Revit_Conduit_Table
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class HelloWordCmd : IExternalCommand
    {
        public static string CommandPath => "Revit_Conduit_Table.HelloWordCmd";

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
