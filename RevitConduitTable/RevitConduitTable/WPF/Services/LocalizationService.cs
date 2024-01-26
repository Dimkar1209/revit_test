using NLog;

using RevitConduitTable.Resources;

using System;
using System.Globalization;
using System.Windows;

namespace RevitConduitTable.WPF.Services
{
    /// <summary>
    /// Localization service which dynamicly loads localization xaml schems.
    /// </summary>
    internal class LocalizationService : ILocalizationService
    {
        /// <summary>
        /// Set localization by CurrentCulture prefrenses.
        /// </summary>
        public bool ChangeLanguage(ResourceDictionary resources)
        {
           return ChangeLanguage(GetLanguage(), resources);
        }

        /// <summary>
        /// Pass two letter ISO Language Name code to change localization.
        /// </summary>
        public bool ChangeLanguage(string cultureName, ResourceDictionary resources)
        {
            try
            {
                logger.Error(Logs_Text.LOCALIZATION_CHANGED_INFO, cultureName);

                var dictionaryPath = $"pack://application:,,,/RevitConduitTable;component/WPF/Resources/LocalizedStrings_ua.xaml";
                var uri = new Uri(dictionaryPath, UriKind.Absolute);
                var resourceDict = new ResourceDictionary { Source = uri };

                resources.MergedDictionaries.Clear();
                resources.MergedDictionaries.Add(resourceDict);
            }
            catch (Exception)
            {
                logger.Error(Logs_Text.LOCALIZATION_CHANGED_ERROR, cultureName);
                ChangeLanguage("en", resources);
                return false;
            }

            return true;
        }

        private string GetLanguage()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return currentCulture.TwoLetterISOLanguageName;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
