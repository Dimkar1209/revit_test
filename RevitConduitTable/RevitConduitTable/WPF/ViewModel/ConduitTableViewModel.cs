﻿using ClosedXML.Excel;

using Microsoft.Win32;

using NLog;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using RevitConduitTable.Constants;
using RevitConduitTable.Resources;
using RevitConduitTable.WPF.Events;
using RevitConduitTable.WPF.ExportData;
using RevitConduitTable.WPF.Model;
using RevitConduitTable.WPF.Services;
using RevitConduitTable.WPF.View;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RevitConduitTable.WPF.ViewModel
{
    internal class ConduitTableViewModel : BindableBase
    {
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand AddColumnCommand { get; private set; }
        public DelegateCommand RemoveCommand { get; private set; }

        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }

        public DelegateCommand ToggleHideCommand { get; private set; }

        public DelegateCommand ExportCommand { get; private set; }
        public DelegateCommand ImportCommand { get; private set; }

        public ObservableCollection<ConduitItem> Conduits
        {
            get { return _conduits; }
            set { SetProperty(ref _conduits, value); }
        }

        public ConduitItem SelectedConduit
        {
            get { return _selectedConduit; }
            set { SetProperty(ref _selectedConduit, value); }
        }

        public string HideButtonText
        {
            get { return _hideButtonText; }
            set { SetProperty(ref _hideButtonText, value); }
        }

        public ConduitTableViewModel(IDialogService dialogService, IEventAggregator eventAggregator, ICollectionService collectionService)
        {
            _conduits = new ObservableCollection<ConduitItem> { new ConduitItem() { Properties = defautProperties } };
            _collectionService = collectionService;
            _collectionService.ConduitsShare = _conduits;

            AddCommand = new DelegateCommand(ExecuteAddCommand);
            RemoveCommand = new DelegateCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand)
                .ObservesProperty(() => SelectedConduit);

            CopyCommand = new DelegateCommand(ExecuteCopyCommand, CanExecuteCopyCommand)
                .ObservesProperty(() => SelectedConduit);

            PasteCommand = new DelegateCommand(ExecutePasteCommand, CanExecutePasteCommand);
            AddColumnCommand = new DelegateCommand(ExecuteAddColumnCommand);
            ToggleHideCommand = new DelegateCommand(ExecuteToggleHideCommand);

            ExportCommand = new DelegateCommand(ExecuteExportCommand);
            ImportCommand = new DelegateCommand(ExecuteImportCommand);
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _isFiledsHidden = false;

            InitializeCalculatedProperties();
            InitializeIniProperties();
        }

        #region Add/Remove commands

        private void ExecuteAddCommand()
        {
            if (_conduits.Count > 0)
            {
                var lastConduit = _conduits[_conduits.Count - 1];
                var newConduit = new ConduitItem()
                {
                    Properties = new Dictionary<string, ConduitProperty>()
                };

                foreach (var keyValuePair in lastConduit.Properties)
                {
                    newConduit.Properties[keyValuePair.Key] = new ConduitProperty()
                    {
                        ParameterName = keyValuePair.Value.ParameterName,
                        ParameterValue = keyValuePair.Key == ParametersConstants.DATAGRID_ID ?
                        (int)lastConduit.Properties[ParametersConstants.DATAGRID_ID].ParameterValue + 1
                        : keyValuePair.Value.ParameterValue
                    };
                }

                _conduits.Add(newConduit);
            }
            else
            {
                _conduits.Add(new ConduitItem()
                {
                    Properties = defautProperties
                });
            }
        }

        private void ExecuteRemoveCommand()
        {
            if (SelectedConduit != null && _conduits.Contains(SelectedConduit))
            {
                _conduits.Remove(SelectedConduit);
            }
        }

        private bool CanExecuteRemoveCommand()
        {
            return SelectedConduit != null;
        }

        #endregion

        private void ExecuteAddColumnCommand()
        {
            var parameters = new DialogParameters() { { ParametersConstants.EXISTING_COLUMNS_DIALOG, GetColumns } };
            string columnAdd = string.Empty;
            bool okResult = false;

            _dialogService.ShowDialog(typeof(EditColumsDialog).Name, parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    columnAdd = result.Parameters.GetValue<string>(ParametersConstants.COLUMN_NAME_DIALOG);
                    okResult = true;
                }

            });

            if (!okResult || string.IsNullOrEmpty(columnAdd))
            {
                return;
            }

            foreach (ConduitItem item in _conduits)
            {
                item.Properties.Add(columnAdd, new ConduitProperty()
                { ParameterName = columnAdd, ParameterValue = 1, IsReadonly = false, IsVisible = true });
            }

            RaisePropertyChanged(nameof(Conduits));

            _eventAggregator.GetEvent<UpdateTableEvent>().Publish(Conduits);
            _copiedConduit = null;
        }

        #region Copy/Paste commands

        private void ExecuteCopyCommand()
        {
            _copiedConduit = SelectedConduit;
            PasteCommand.RaiseCanExecuteChanged();
        }

        private bool CanExecuteCopyCommand()
        {
            return SelectedConduit != null;
        }

        private void ExecutePasteCommand()
        {
            if (_copiedConduit != null && SelectedConduit != null)
            {
                var newProperties = new Dictionary<string, ConduitProperty>();

                foreach (var kvp in _copiedConduit.Properties)
                {
                    // ID parameter is unique.
                    if (kvp.Key == ParametersConstants.DATAGRID_ID)
                    {
                        newProperties[kvp.Key] = new ConduitProperty()
                        {
                            ParameterName = ParametersConstants.DATAGRID_ID,
                            ParameterValue = SelectedConduit.Properties[ParametersConstants.DATAGRID_ID].ParameterValue,
                            IsReadonly = true
                        };
                    }
                    else
                    {
                        newProperties[kvp.Key] = new ConduitProperty()
                        {
                            ParameterName = kvp.Value.ParameterName,
                            ParameterValue = kvp.Value.ParameterValue,
                            IsReadonly = kvp.Value.IsReadonly
                        };
                    }
                }

                SelectedConduit.Properties = newProperties;
                RaisePropertyChanged(nameof(SelectedConduit));
            }
        }

        private bool CanExecutePasteCommand()
        {
            return _copiedConduit != null;
        }

        #endregion

        #region Import/Export cmd

        private void ExecuteExportCommand()
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = FileConstants.SAVE_DIALOG_EXCEL_FILTER,
                DefaultExt = FileConstants.EXCEL_FILE_EXTENSION,
                FileName = FileConstants.EXCEL_FILE_NAME
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                if (!ExportToExcel.Export(_conduits.ToList(), saveFileDialog.FileName))
                {
                    _dialogService.ShowDialog(typeof(MessageBoxDialog).Name,
                        new DialogParameters() { { ParametersConstants.MESSAGE_DIALOG, UI_Text.EXPORT_EXCEL_ERORR } },
                        r => { });

                    return;
                }

                var parameters = new DialogParameters()
                { { ParametersConstants.MESSAGE_DIALOG, string.Format(UI_Text.EXPORT_EXCEL_INFO, ExportToExcel.GetSavedPath()) } };

                _dialogService.ShowDialog(typeof(MessageBoxDialog).Name, parameters, r => { });
            }
        }

        private void ExecuteImportCommand()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = FileConstants.LOAD_DIALOG_EXCEL_FILTER,
                Title = UI_Text.EXCEL_FILE_IMPORT_DIALOG_TITLE
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string filePath = openFileDialog.FileName;
                PopulateFromExcel(filePath);
            }
        }

        private void PopulateFromExcel(string filePath)
        {
            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    IEnumerable<IXLRangeRow> rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                    _conduits.Clear();

                    foreach (IXLRangeRow row in rows)
                    {
                        var conduitItem = new ConduitItem
                        {
                            Properties = new Dictionary<string, ConduitProperty>()
                        };

                        foreach (IXLCell header in worksheet.FirstRowUsed().CellsUsed())
                        {
                            string headerText = header.GetString();
                            IXLCell cell = row.Cell(header.WorksheetColumn().ColumnNumber());
                            XLCellValue cellValue = cell.Value;

                            if (headerText.Equals(ParametersConstants.DATAGRID_ID))
                            {
                                ImportItemID(conduitItem, headerText, cellValue);

                                continue;
                            }

                            conduitItem.Properties[headerText] = new ConduitProperty
                            {
                                ParameterName = headerText,
                                ParameterValue = GetCellValue(cell),
                                IsReadonly = _calcululatedProperties.Contains(headerText),
                                IsVisible = true
                            };
                        }

                        _conduits.Add(conduitItem);
                    }

                    RaisePropertyChanged(nameof(Conduits));
                    _eventAggregator.GetEvent<UpdateTableEvent>().Publish(Conduits);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowDialog(typeof(MessageBoxDialog).Name,
                    new DialogParameters { { ParametersConstants.MESSAGE_DIALOG, UI_Text.IMPORT_EXCEL_ERROR } },
                    r => { });

                logger.Error(UI_Text.IMPORT_EXCEL_ERROR, ex);
            }
        }

        private static void ImportItemID(ConduitItem conduitItem, string headerText, XLCellValue cellValue)
        {
            if (int.TryParse(cellValue.ToString(), out int cellID))
            {
                conduitItem.Properties[headerText] = new ConduitProperty
                {
                    ParameterName = headerText,
                    ParameterValue = cellID,
                    IsReadonly = _calcululatedProperties.Contains(headerText),
                    IsVisible = true
                };
            }
        }

        private object GetCellValue(IXLCell cell)
        {
            if (cell.TryGetValue(out double number))
            {
                return number;
            }
            else
            {
                return cell.GetValue<string>();
            }
        }

        #endregion

        private static void InitializeCalculatedProperties()
        {
            foreach (var propName in _calcululatedProperties)
            {
                if (defautProperties.ContainsKey(propName))
                {
                    continue;
                }

                defautProperties.Add(propName, ConduitPropertyFactory.CreateDefault(propName, 2, true, true));
            }
        }

        private static void InitializeIniProperties()
        {
            foreach (var propName in _iniProperties)
            {
                if (defautProperties.ContainsKey(propName))
                {
                    continue;
                }

                defautProperties.Add(propName, ConduitPropertyFactory.CreateDefault(propName, 2, false, true));
            }
        }

        private void ExecuteToggleHideCommand()
        {
            CalculatedFieldsVisibility(_isFiledsHidden);
            _isFiledsHidden = !_isFiledsHidden;
            HideButtonText = _isFiledsHidden ? UI_Text.UNHIDE_FIELDS_BUTTON : UI_Text.HIDE_FIELDS_BUTTON;
        }

        private void CalculatedFieldsVisibility(bool isVisible)
        {
            Conduits.ToList().ForEach(item =>
                _calcululatedProperties.ForEach(calculatedProperty =>
                    item.SetVisibilityByKey(calculatedProperty, isVisible)));

            _eventAggregator.GetEvent<UpdateTableEvent>().Publish(Conduits);
        }

        private ObservableCollection<ConduitItem> _conduits;
        private ConduitItem _selectedConduit;
        private ConduitItem _copiedConduit;

        private IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private ICollectionService _collectionService;

        private IEnumerable<string> GetColumns => _conduits.FirstOrDefault()?.Properties.Keys ?? Enumerable.Empty<string>();
        private string _hideButtonText = UI_Text.HIDE_FIELDS_BUTTON;
        private bool _isFiledsHidden;

        private readonly static List<string> _calcululatedProperties = new List<string>() { "C0", "C1", "C2", "C3", "C4" };
        private readonly static List<string> _iniProperties = new List<string>() { "I_1", "I_2", "I_3", "I_4", "I_5" };

        private readonly static Dictionary<string, ConduitProperty> defautProperties = new Dictionary<string, ConduitProperty>()
        {
            { ParametersConstants.DATAGRID_ID, new ConduitProperty() { ParameterName = ParametersConstants.DATAGRID_ID, ParameterValue = 1, IsReadonly = true, IsVisible = true }}
        };

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
