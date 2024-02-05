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

        public void RunSingleWindow()
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            Instance.Run();
        }

        protected override DependencyObject CreateShell()
        {
            var shell = new BootstrapperWindow();
            shell.Closed += Shell_Closed; ; 
            return shell;
        }

        private void Shell_Closed(object sender, System.EventArgs e)
        {
            isRunning = false;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.RegisterSingleton<BootstrapperWindow>();
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();
            containerRegistry.RegisterSingleton<ICollectionService, CollectionService>();

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
