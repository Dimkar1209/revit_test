﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using NLog;

using Prism.Ioc;
using Prism.Regions;

using RevitConduitTable.Constants;
using RevitConduitTable.Helpers;
using RevitConduitTable.Resources;
using RevitConduitTable.WPF;
using RevitConduitTable.WPF.View;

using System;
using System.Linq;

namespace RevitConduitTable.RevitUIBuilder
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    internal class ConduitButton : ICustomButton
    {
        public string Name => this.GetType().Name;

        public string ClassName => this.GetType().FullName;

        public string ButtonText => "BUTTON_CONDUIT_NAME";

        public string ImagePath => string.Empty;

        public string Tooltip => string.Empty;

        public bool IsEnabled => true;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                RevitDocumentHelper.UiApplication = commandData.Application;
                logger.Info(Logs_Text.BUTTON_CONDUIT_INFO);

                Bootstrapper.Instance.RunSingleWindow();

                var _regionManager = Bootstrapper.Instance.Container.Resolve<IRegionManager>();
                _regionManager.RequestNavigate(RegionConstants.MAIN_REGION_NAME, typeof(ConduitTableView).Name);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                logger.Error(ex, Logs_Text.BUTTON_CONDUIT_ERROR);
                return Result.Failed;
            }
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
