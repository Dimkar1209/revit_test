using ClosedXML.Excel;

using NLog;

using RevitConduitTable.Constants;
using RevitConduitTable.Resources;
using RevitConduitTable.WPF.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RevitConduitTable.WPF.ExportData
{
    internal class ExportToExcel
    {
        public static bool Export(IReadOnlyCollection<ConduitItem> conduits)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(FileConstants.EXCEL_WORKSHEET_NAME);

                    var headers = conduits.FirstOrDefault()?.Properties
                        .Where(p => p.Value.IsVisible)
                        .Select(p => p.Key)
                        .ToList();

                    if (headers != null)
                    {
                        WriteTable(conduits, worksheet, headers);
                    }

                    workbook.SaveAs(GetSavedPath());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, Logs_Text.EXPORT_EXCEL_ERORR);
                return false;
            }

            return true;
        }

        private static void WriteTable(IReadOnlyCollection<ConduitItem> conduits, IXLWorksheet worksheet, List<string> headers)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
            }

            int row = 2;
            foreach (var conduit in conduits)
            {
                for (int col = 0; col < headers.Count; col++)
                {
                    if (conduit.Properties.TryGetValue(headers[col], out ConduitProperty prop))
                    {
                        var valueText = prop.ParameterValue.ToString();
                        if (double.TryParse(valueText, out double number))
                        {
                            worksheet.Cell(row, col + 1).Value = number;
                        }
                        else
                        {
                            worksheet.Cell(row, col + 1).Value = valueText;
                        }

                    }
                }

                row++;
            }
        }

        public static string GetSavedPath()
        {
            string directory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return System.IO.Path.Combine(directory, FileConstants.EXCEL_FILE_NAME);
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
    }
}
