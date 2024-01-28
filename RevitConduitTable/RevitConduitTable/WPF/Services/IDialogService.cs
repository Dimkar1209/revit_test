using System;

namespace RevitConduitTable.WPF.Services
{
    public interface IDialogService
    {
        void ShowDialog<T>(Action<T> actionCallback) where T : class;
    }
}
