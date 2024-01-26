using Prism.Ioc;
using Prism.Unity;

using RevitConduitTable.WPF.Services;
using RevitConduitTable.WPF.View;
using RevitConduitTable.WPF.ViewModel;

using System.Globalization;

using System.Threading;
using System.Windows;

namespace RevitConduitTable.WPF
{
    internal class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var parentWindow = new Window();
            var view = Container.Resolve<ConduitTableView>();
            view.DataContext = Container.Resolve<ConduitTableViewModel>();
            parentWindow.Content = Container.Resolve<ConduitTableView>();

            return parentWindow;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();
            containerRegistry.RegisterForNavigation<ConduitTableView>();
            containerRegistry.Register<ConduitTableViewModel>();
        }
    }
}
