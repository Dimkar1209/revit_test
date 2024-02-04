using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using RevitConduitTable.Constants;
using RevitConduitTable.Resources;
using RevitConduitTable.WPF.View;

using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class EditColumnsDialogViewModel : BindableBase, IDialogAware
    {
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public event Action<IDialogResult> RequestClose;

        public string Title => UI_Text.EDIT_COLUMN_DIALOG_TITLE;

        public string EditedText
        {
            get { return _editedText; }
            set { SetProperty(ref _editedText, value); }
        }

        public EditColumnsDialogViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(ParametersConstants.EXISTING_COLUMNS_DIALOG))
            {
                _editableString = parameters.GetValue<IEnumerable<string>>(ParametersConstants.EXISTING_COLUMNS_DIALOG);
            }
        }

        protected virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            dialogResult.Parameters.Add(ParametersConstants.COLUMN_NAME_DIALOG, _editedText);
            RequestClose?.Invoke(dialogResult);
        }

        private void Save()
        {
            if (_editableString != null && _editableString.Contains(_editedText))
            {
                var parameters = new DialogParameters() { { ParametersConstants.MESSAGE_DIALOG, UI_Text.EDIT_COLUMN_ERROR } };
                _dialogService.ShowDialog(typeof(MessageBoxDialog).Name, parameters, result => { });
            }
            else
            {
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            }
        }

        private void Cancel()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }


        private readonly IDialogService _dialogService;
        private string _editedText;
        private IEnumerable<string> _editableString;
    }
}
