using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using RevitConduitTable.Constants;
using RevitConduitTable.Resources;

using System;
using System.Collections.Generic;
using System.Linq;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class EditColumnsDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => UI_Text.EDIT_COLUMN_DIALOG_TITLE;

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }


        private string _editedText;
        private IEnumerable<string> _editableString;

        public string EditedText
        {
            get { return _editedText; }
            set { SetProperty(ref _editedText, value); }
        }

        public string SelectedType
        {
            get { return _isStringSelected ? "String" : "Int"; }
            set
            {
                if (value == "String")
                {
                    SetProperty(ref _isStringSelected, true);
                }
                else if (value == "Int")
                {
                    SetProperty(ref _isStringSelected, false);
                }
            }
        }

        public EditColumnsDialogViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Save()
        {
            if (_editableString != null && _editableString.Contains(_editedText))
            {
                var parameters = new DialogParameters() { { ParametersConstants.MESSAGE_DIALOG, UI_Text.EDIT_COLUMN_ERROR } };
                _dialogService.ShowDialog(UI_Text.MESSAGE_DIALOG_TITLE, parameters, result => { });
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

        public event Action<IDialogResult> RequestClose;

        protected virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            dialogResult.Parameters.Add(ParametersConstants.COLUMN_NAME_DIALOG, _editedText);
            RequestClose?.Invoke(dialogResult);
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

        private readonly IDialogService _dialogService;
        private bool _isStringSelected;
    }
}
