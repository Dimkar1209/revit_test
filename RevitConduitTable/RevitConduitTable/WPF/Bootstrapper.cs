using Prism.Ioc;
using Prism.Unity;

using RevitConduitTable.WPF.Events;
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

            containerRegistry.Register<ConduitTableViewModel>();
            containerRegistry.Register<UpdateTableEvent>();

            containerRegistry.RegisterForNavigation<ConduitTableView>();

            containerRegistry.RegisterDialog<EditColumsDialog, EditColumnsDialogViewModel>();
            containerRegistry.RegisterDialog<MessageBoxDialog, MessageBoxDialogViewModel>();
        }
    }
}
