using Autodesk.Revit.UI;

using NLog;

using RevitConduitTable.Resources;
using RevitConduitTable.RevitUIBuilder;
using RevitConduitTable.RevitUIBuilder.ConduitTableTab;

using System;
using System.Collections.Generic;

namespace RevitConduitTable
{
    public class RevitConduitTableApp : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            NlogSchemaInitialize.InitializeLogger();
            logger.Info(Logs_Text.APP_STARTUP_INFO);

            try
            {
                BuildUI(application);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                logger.Error(ex, Logs_Text.APP_STARTUP_ERROR);
                return Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private void BuildUI(UIControlledApplication application)
        {
            UIBuilder.BuildTabs(application, listOfTabs);
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static List<ICustomTab> listOfTabs = new List<ICustomTab>() { new ConduitTab() };
    }
}
