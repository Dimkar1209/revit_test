using RevitConduitTable.WPF.Services;

using System.Windows;
using System.Windows.Controls;

namespace RevitConduitTable.WPF.View
{
    /// <summary>
    /// Interaction logic for ConduitTableView.xaml
    /// </summary>
    public partial class ConduitTableView : UserControl
    {
        private readonly ILocalizationService _localizationService;

        public ConduitTableView()
        {
            InitializeComponent();
            ILocalizationService _localizationService = new LocalizationService();
            var resources = new ResourceDictionary();
            _localizationService.ChangeLanguage(resources);
            this.Resources = resources;
        }
    }
}
