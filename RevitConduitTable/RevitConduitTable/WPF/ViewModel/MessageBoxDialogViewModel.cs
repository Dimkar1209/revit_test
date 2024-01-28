using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using RevitConduitTable.Constants;
using RevitConduitTable.Resources;

using System;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class MessageBoxDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => UI_Text.MESSAGE_DIALOG_TITLE;

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public DelegateCommand OKCommand { get; }

        public MessageBoxDialogViewModel()
        {
            OKCommand = new DelegateCommand(OK);
        }

        private void OK()
        {
            RequestClose(new DialogResult(ButtonResult.OK));
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(ParametersConstants.MESSAGE_DIALOG))
            {
                Message = parameters.GetValue<string>(ParametersConstants.MESSAGE_DIALOG);
            }
        }
    }
}
