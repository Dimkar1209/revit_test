﻿using Autodesk.Revit.UI;

using NLog;

using RevitConduitTable.Helpers;
using RevitConduitTable.Resources;
using RevitConduitTable.RevitUIBuilder;
using RevitConduitTable.WPF.Services;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

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
            var listOfTabs = GetCustomTabs();
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            foreach (var tab in listOfTabs)
            {
                BuildTab(application, thisAssemblyPath, tab);
            }
        }

        private void BuildTab(UIControlledApplication application, string thisAssemblyPath, ICustomTab tab)
        {
            ILocalizationService localizationService = new LocalizationService();
            application.CreateRibbonTab(localizationService.GetString(tab.TabName));

            foreach (var panel in tab.Panels)
            {
                RibbonPanel ribbonPanel = application.CreateRibbonPanel(localizationService.GetString(tab.TabName),
                    localizationService.GetString(panel.PanelName));

                foreach (var button in panel.Buttons)
                {
                    var buttonData = new PushButtonData(
                        button.Name,
                        localizationService.GetString(button.ButtonText),
                        thisAssemblyPath,
                        button.ClassName);

                    var pushButton = ribbonPanel.AddItem(buttonData) as PushButton;

                    pushButton.Enabled = button.IsEnabled;
                    var imageUri = new Uri(button.ImagePath, UriKind.RelativeOrAbsolute);
                    pushButton.LargeImage = new BitmapImage(imageUri);
                    pushButton.ToolTip = button.Tooltip;
                }
            }
        }

        private IEnumerable<ICustomTab> GetCustomTabs()
        {
            var customTabInstances = new List<ICustomTab>();
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                if (typeof(ICustomTab).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    if (Activator.CreateInstance(type) is ICustomTab tabInstance)
                    {
                        customTabInstances.Add(tabInstance);
                    }
                }
            }

            return customTabInstances;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
