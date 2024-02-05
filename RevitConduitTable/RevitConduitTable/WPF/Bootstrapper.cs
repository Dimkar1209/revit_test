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
        private Bootstrapper() { }

        public static Bootstrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Bootstrapper();
                }
                return instance;
            }
        }

        protected override DependencyObject CreateShell()
        {
            return new BootstrapperWindow();
        }

        public static void RunSingleWindow()
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            Instance.Run();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<BootstrapperWindow>();
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();

            containerRegistry.Register<ConduitTableViewModel>();
            containerRegistry.Register<SharedParametersViewModel>();
            containerRegistry.Register<UpdateTableEvent>();

            containerRegistry.RegisterForNavigation<ConduitTableView>();
            containerRegistry.RegisterForNavigation<SharedParametersView>();

            containerRegistry.RegisterDialog<EditColumsDialog, EditColumnsDialogViewModel>();
            containerRegistry.RegisterDialog<MessageBoxDialog, MessageBoxDialogViewModel>();
        }

        private static Bootstrapper instance;
        private static bool isRunning = false;
    }
}
