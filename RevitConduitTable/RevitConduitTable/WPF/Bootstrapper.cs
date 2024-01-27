using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;

using RevitConduitTable.Constants;
using RevitConduitTable.Helpers;
using RevitConduitTable.RevitUIBuilder;
using RevitConduitTable.WPF.Services;
using RevitConduitTable.WPF.View;
using RevitConduitTable.WPF.ViewModel;

using System.Windows;

namespace RevitConduitTable.WPF
{
    internal class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return new BootstrapperWindow();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<BootstrapperWindow>();
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();

            containerRegistry.RegisterForNavigation<ConduitTableView>();

            containerRegistry.Register<ConduitTableViewModel>();
        }
    }
}
