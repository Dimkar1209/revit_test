using Prism.Ioc;
using Prism.Unity;
using RevitConduitTable.WPF.View;
using System.Windows;

namespace RevitConduitTable.WPF
{
    internal class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var parentWindow = new Window();
            parentWindow.Content = Container.Resolve<ConduitTableView>();

            return parentWindow;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ConduitTableView>();
        }
    }
}
