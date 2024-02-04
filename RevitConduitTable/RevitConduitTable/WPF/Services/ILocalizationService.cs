using System.Globalization;
using System.Resources;
using System.Windows;

namespace RevitConduitTable.WPF.Services
{
    public interface ILocalizationService
    {
        string GetString(string key);

        void SetCulture(string cultureName);
    }
}
