using System.Windows;

namespace RevitConduitTable.WPF.Services
{
    internal interface ILocalizationService
    {
        bool ChangeLanguage(ResourceDictionary resources);

        bool ChangeLanguage(string cultureName, ResourceDictionary resources);
    }
}
