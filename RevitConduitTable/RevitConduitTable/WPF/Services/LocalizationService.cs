using NLog;

using RevitConduitTable.Constants;
using RevitConduitTable.Resources;

using System.Globalization;
using System.Reflection;
using System.Resources;

namespace RevitConduitTable.WPF.Services
{
    public class LocalizationService : ILocalizationService
    {
        private ResourceManager _resourceManager;
        private CultureInfo _currentCulture;
        private readonly string _resourcePath;

        public LocalizationService()
        {
            _resourcePath = FileConstants.WPF_LOCALIZATION_RESOURCE;
            _resourceManager = new ResourceManager(_resourcePath, typeof(UI_Text).Assembly);
            _currentCulture = CultureInfo.CurrentCulture;
        }

        public string GetString(string key)
        {
            string resourceValue = null;

            try
            {
                resourceValue = _resourceManager.GetString(key, _currentCulture);
            }
            catch (MissingManifestResourceException ex)
            {
                logger.Error(ex, Logs_Text.LOCALIZATION_LOAD_ERROR, _currentCulture.Name);
                _resourceManager = new ResourceManager(_resourcePath, Assembly.GetExecutingAssembly());
                resourceValue = _resourceManager.GetString(key, CultureInfo.InvariantCulture);
            }

            if (resourceValue == null)
            {
                logger.Warn(Logs_Text.LOCALIZATION_KEY_WARN, key, _currentCulture.Name);
                resourceValue = key;
            }

            return resourceValue;
        }

        public void SetCulture(string cultureName)
        {
            _currentCulture = new CultureInfo(cultureName);
            CultureInfo.CurrentCulture = _currentCulture;
            CultureInfo.CurrentUICulture = _currentCulture;
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
