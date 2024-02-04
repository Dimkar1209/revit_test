using Autodesk.Revit.UI;

using RevitConduitTable.WPF.Services;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace RevitConduitTable.RevitUIBuilder
{
    /// <summary>
    /// Add Addin Revit UI
    /// </summary>
    internal static class UIBuilder
    {
        /// <summary>
        /// Add tabs to Revit
        /// </summary>
        /// <param name="application">Revit application</param>
        /// <param name="tabs">Tabs to build <see cref="ICustomTab"/></param>
        public static void BuildTabs(UIControlledApplication application, IReadOnlyCollection<ICustomTab> tabs)
        {
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            ILocalizationService localizationService = new LocalizationService();

            foreach (var tab in tabs)
            {
                BuildTab(application, thisAssemblyPath, tab, localizationService);
            }
        }

        private static void BuildTab(UIControlledApplication application, string thisAssemblyPath, ICustomTab tab, ILocalizationService localizationService)
        {
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

        private static IEnumerable<ICustomTab> GetCustomTabs()
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
    }
}
