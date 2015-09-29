using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Documents.Excel;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinMaskedEdit;
using Microsoft.WindowsAPICodePack.Dialogs;
using log4net;

namespace Utilities.InfragisticsUtilities.UltraGridUtilities
{
    /// <summary>
    /// Provides methods for manipulating UltraGrid
    /// </summary>
    public static class UltraGridExtensions
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets first selected object in the <code>UltraGrid</code>.
        /// </summary>
        /// <typeparam name="T">Type of object in the list.</typeparam>
        /// <param name="grid">Target grid.</param>
        /// <param name="includeActiveRow">Indicates whether to include an active row when looking for selected objects.</param>
        /// <returns>First selected object in the <code>UltraGrid</code>.</returns>
        public static T GetSelectedObject<T>(this UltraGrid grid, bool includeActiveRow = true)
        {
            if (grid == null)
            {
                _log.Error("Grid parameter is null.");
                throw new ArgumentNullException("grid");
            }

            if (grid.InvokeRequired)
            {
                var result = default(T);
                grid.Invoke(new MethodInvoker(() =>
                {
                    result = GetSelectedObject<T>(grid, includeActiveRow);
                }));
                return result;
            }

            if (grid.Selected.Rows.Count > 0 && grid.Selected.Rows[0].ListIndex >= 0)
            {
                return (T)grid.Selected.Rows[0].ListObject;
            }
            else if (includeActiveRow && grid.ActiveRow != null && grid.ActiveRow.Index >= 0)
            {
                return (T)grid.ActiveRow.ListObject;
            }
            return default(T);
        }

        /// <summary>
        /// Gets first selected object in the <code>UltraGrid</code>.
        /// </summary>
        /// <param name="grid">Target grid.</param>
        /// <param name="includeActiveRow">Indicates whether to include an active row when looking for selected objects.</param>
        /// <returns>First selected object in the <code>UltraGrid</code>.</returns>
        public static object GetSelectedObject(this UltraGrid grid, bool includeActiveRow = true)
        {
            if (grid == null)
            {
                _log.Error("Grid parameter is null.");
                throw new ArgumentNullException("grid");
            }

            if (grid.InvokeRequired)
            {
                object result = null;
                grid.Invoke(new MethodInvoker(() =>
                {
                    result = GetSelectedObject(grid, includeActiveRow);
                }));
                return result;
            }

            if (grid.Selected.Rows.Count > 0 && grid.Selected.Rows[0].ListIndex >= 0)
            {
                return grid.Selected.Rows[0].ListObject;
            }
            else if (includeActiveRow && grid.ActiveRow != null && grid.ActiveRow.Index >= 0)
            {
                return grid.ActiveRow.ListObject;
            }
            return null;
        }

        /// <summary>
        /// Gets the list of selected objects in the <code>UltraGrid</code> optionally including the active row.
        /// </summary>
        /// <typeparam name="T">Type of object in the list.</typeparam>
        /// <param name="grid">Target grid.</param>
        /// /// <param name="includeActiveRow">Indicates whether to include an active row when looking for selected objects.</param>
        /// <returns>First selected object in the <code>UltraGrid</code>.</returns>
        public static IList<T> GetSelectedObjectList<T>(this UltraGrid grid, bool includeActiveRow = true)
        {
            if (grid == null)
            {
                _log.Error("Grid parameter is null.");
                throw new ArgumentNullException("grid");
            }

            if (grid.InvokeRequired)
            {
                IList<T> result = null;
                grid.Invoke(new MethodInvoker(() =>
                {
                    result = GetSelectedObjectList<T>(grid, includeActiveRow);
                }));
                return result;
            }

            var list = (grid.Selected.Rows.Cast<UltraGridRow>().Where(row => row.Index >= 0).Select(row => (T)row.ListObject)).ToList();

            if (includeActiveRow && grid.ActiveRow != null && grid.ActiveRow.Index >= 0 && !list.Contains((T)grid.ActiveRow.ListObject))
            {
                list.Add((T)grid.ActiveRow.ListObject);
            }
            return list;
        }

        /// <summary>
        /// Gets the list of selected objects in the <code>UltraGrid</code> optionally including the active row.
        /// </summary>
        /// <param name="grid">Target grid.</param>
        /// /// <param name="includeActiveRow">Indicates whether to include an active row when looking for selected objects.</param>
        /// <returns>First selected object in the <code>UltraGrid</code>.</returns>
        public static IList<object> GetSelectedObjectList(this UltraGrid grid, bool includeActiveRow = true)
        {
            if (grid == null)
            {
                _log.Error("Grid parameter is null.");
                throw new ArgumentNullException("grid");
            }

            if (grid.InvokeRequired)
            {
                IList<object> result = null;
                grid.Invoke(new MethodInvoker(() =>
                {
                    result = GetSelectedObjectList(grid, includeActiveRow);
                }));
                return result;
            }

            var list = (grid.Selected.Rows.Cast<UltraGridRow>().Where(row => row.Index >= 0).Select(row => row.ListObject)).ToList();

            if (includeActiveRow && grid.ActiveRow != null && grid.ActiveRow.Index >= 0 && !list.Contains(grid.ActiveRow.ListObject))
            {
                list.Add(grid.ActiveRow.ListObject);
            }
            return list;
        }

        /// <summary>
        /// resized all columns in the grid to fit the contents.
        /// </summary>
        /// <param name="grid">Grid on which the resize is performed.</param>
        /// <param name="resizeType">Type of resize.</param>
        public static void ResizeColumnsToFit(this UltraGrid grid, PerformAutoSizeType resizeType = PerformAutoSizeType.AllRowsInBand)
        {
            if (grid == null)
            {
                _log.Error("Grid parameter is null.");
                throw new ArgumentNullException("grid");
            }

            if (grid.InvokeRequired)
            {
                grid.Invoke(new MethodInvoker(() => ResizeColumnsToFit(grid, resizeType)));
            }
            else
            {
                _log.Debug("Resizing grid to fit ...");
                foreach (var column in grid.DisplayLayout.Bands[0].Columns)
                {
                    column.PerformAutoResize(resizeType, true);
                }
                _log.Debug("Resizing grid complete.");
            }
        }

        /// <summary>
        /// Set format on all columns in grid witch certain data type.
        /// </summary>
        /// <param name="grid">Grid whose columns will be formatted.</param>
        /// <param name="columnDataType">Data type of columns which will be formatted.</param>
        /// <param name="format">Column format to apply.</param>
        public static void SetColumnFormat(this UltraGrid grid, Type columnDataType, string format)
        {
            if (grid.InvokeRequired)
            {
                grid.Invoke(new MethodInvoker(() => SetColumnFormat(grid, columnDataType, format)));
                return;
            }

            _log.DebugFormat("Setting column format to {0} for all column of type {1} ...", format, columnDataType);
            if (grid == null)
            {
                throw new ArgumentNullException("grid", "Grid is null.");
            }
            else if (string.IsNullOrWhiteSpace(format) || string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format is empty.");
            }
            else if (columnDataType == null)
            {
                throw new ArgumentNullException("columnDataType", "Column data type is null.");
            }
            else if (grid.DisplayLayout.Bands.Count == 0)
            {
                _log.WarnFormat("Unable to set format on columns because grid has no bands.");
                return;
            }

            foreach (var band in grid.DisplayLayout.Bands)
            {
                foreach (var column in band.Columns)
                {
                    if (column.DataType == columnDataType)
                    {
                        column.Format = format;
                    }
                }
            }
            _log.DebugFormat("Setting column format complete.");
        }

        /// <summary>
        /// Load <see cref="Infragistics.Win.UltraWinGrid.UltraGrid"/> setting from an XML chunk.
        /// </summary>
        /// <param name="grid"><see cref="Infragistics.Win.UltraWinGrid.UltraGrid"/> for which to load the settings.</param>
        /// <param name="gridXmlSettings">Settings XML chunk.</param>
        public static void LoadSettingsFromXmlString(this UltraGrid grid, string gridXmlSettings)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            if (gridXmlSettings == null) throw new ArgumentNullException("gridXmlSettings");
            try
            {
                _log.Debug("Loading grid settings ...");
                var bytes = Encoding.UTF8.GetBytes(gridXmlSettings);
                using (var stream = new MemoryStream(bytes))
                {
                    grid.DisplayLayout.LoadFromXml(stream);
                }
                _log.Debug("Loading grid settings finished.");
            }
            catch (Exception ex)
            {
                _log.Error("Error loading grid settings from xml chunk: " + gridXmlSettings);
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets <see cref="Infragistics.Win.UltraWinGrid.UltraGrid"/> settings in XML format.
        /// </summary>
        /// <param name="grid"><see cref="Infragistics.Win.UltraWinGrid.UltraGrid"/> from which to get the settings.</param>
        /// <returns>XML string containing the settings.</returns>
        public static string GetSettingsAsXmlString(this UltraGrid grid)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            try
            {
                _log.Debug("Getting grid settings as XML string ...");
                using (var stream = new MemoryStream())
                {
                    grid.DisplayLayout.SaveAsXml(stream);
                    stream.Position = 0;
                    string text;
                    using (var tr = new StreamReader(stream))
                    {
                        text = tr.ReadToEnd();
                    }
                    _log.Debug("Getting grid settings as XML string finished.");
                    return text;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error getting grid settings in XML format.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Exports contents of <see cref="Infragistics.Win.UltraWinGrid"/> to Excel file.
        /// </summary>
        /// <param name="grid"><see cref="Infragistics.Win.UltraWinGrid"/> whose contents are to be exported.</param>
        /// <exception cref="ArgumentNullException">When <see cref="Infragistics.Win.UltraWinGrid"/> is null.</exception>
        public static Task ExportToExcelAsync(this UltraGrid grid)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            var selectedFile = Path.GetTempFileName();
            var fileName = Path.GetFileNameWithoutExtension(selectedFile);
            var path = Path.GetDirectoryName(selectedFile);
            var excelFile = Path.Combine(path ?? string.Empty, fileName) + ".xlsx";
            File.Move(selectedFile, excelFile);
            //var dialogResult = Forms.Dialogs.Dialog.ShowSaveFileDialog("Excel Export", out selectedFile,
            //                                                                 new[] {
            //                                                                         new CommonFileDialogFilter(
            //                                                                     "Excel 2007 files", ".xlsx"),
            //                                                                     new CommonFileDialogFilter(
            //                                                                     "Excel 2003 files", ".xls"),
            //                                                                     new CommonFileDialogFilter(
            //                                                                     "All files", "*.*")
            //                                                                     }, ".xlsx");
            //if (dialogResult != CommonFileDialogResult.Ok)
            //{
            //    return Task.FromResult(0);
            //}

            return Task.Run(() =>
                            {
                                ExportToExcel(grid, excelFile);
                                System.Diagnostics.Process.Start(excelFile);
                            });
        }

        /// <summary>
        /// Exports contents of <see cref="Infragistics.Win.UltraWinGrid"/> to Excel file.
        /// </summary>
        /// <param name="grid"><see cref="Infragistics.Win.UltraWinGrid"/> whose contents are to be exported.</param>
        /// <exception cref="ArgumentNullException">When <see cref="Infragistics.Win.UltraWinGrid"/> is null.</exception>
        public static void ExportToExcel(this UltraGrid grid)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            string selectedFile;
            var dialogResult = Forms.Dialogs.Dialog.ShowSaveFileDialog("Excel Export", out selectedFile,
                                                                             new[] {
                                                                                     new CommonFileDialogFilter(
                                                                                 "Excel 2007 files", ".xlsx"),
                                                                                 new CommonFileDialogFilter(
                                                                                 "Excel 2003 files", ".xls"),
                                                                                 new CommonFileDialogFilter(
                                                                                 "All files", "*.*")
                                                                                 }, ".xlsx");
            if (dialogResult != CommonFileDialogResult.Ok)
            {
                return;
            }

            ExportToExcel(grid, selectedFile);
        }

        /// <summary>
        /// Exports contents of <see cref="Infragistics.Win.UltraWinGrid"/> to Excel file.
        /// </summary>
        /// <param name="grid"><see cref="Infragistics.Win.UltraWinGrid"/> whose contents are to be exported.</param>
        /// <param name="fileName">File name to write the data.</param>
        /// <param name="worksheetName">Name of the worksheet.</param>
        /// <exception cref="ArgumentNullException">When <see cref="Infragistics.Win.UltraWinGrid"/> is null.</exception>
        public static void ExportToExcel(this UltraGrid grid, string fileName, string worksheetName = "Results")
        {
            if (grid == null) throw new ArgumentNullException("grid");
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (worksheetName == null) throw new ArgumentNullException("worksheetName");

            _log.Info("Exporting to Excel ...");
            // Validate file name
            try
            {
                _log.DebugFormat("Validating path for file {0} ...", fileName);
                Path.GetFullPath(fileName);
            }
            catch (Exception ex)
            {
                var message = string.Format("Error saving grid contents to Excel. File {0} is not valid.", fileName);
                _log.Error(message);
                _log.Error(ex.Message, ex);
                throw new Exception(message, ex);
            }

            // Export data
            try
            {
                var workbookFormat = WorkbookFormat.Excel2007;
                var extension = Path.GetExtension(fileName);
                if (extension.Trim().ToLower() == ".xls")
                {
                    workbookFormat = WorkbookFormat.Excel97To2003;
                }
                var wkbk = new Workbook(workbookFormat);
                var resultsSheet = wkbk.Worksheets.Add(worksheetName);
                var excelExporrter = new UltraGridExcelExporter();
                excelExporrter.Export(grid, resultsSheet, 0, 0);

                _log.DebugFormat("Saving Excel worksheet to file {0} ...", fileName);
                wkbk.Save(fileName);
                _log.DebugFormat("Excel worksheet saved.");

                _log.Info("Export complete.");
            }
            catch (Exception ex)
            {
                var message = string.Format("Error exporting grid to Excel file {0}.", fileName);
                _log.Error(message);
                _log.Error(ex.Message, ex);
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Exports contents of <see cref="Infragistics.Win.UltraWinGrid"/> to CSV separated file.
        /// </summary>
        /// <param name="grid"><see cref="Infragistics.Win.UltraWinGrid"/> whose contents are to be exported.</param>
        /// <exception cref="ArgumentNullException">When <see cref="Infragistics.Win.UltraWinGrid"/> is null.</exception>
        public static void ExportToCsvWithPrompt(this UltraGrid grid)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            string selectedFile;
            var dialogResult = Forms.Dialogs.Dialog.ShowSaveFileDialog("CSV Export", out selectedFile,
                                                                             new[] {
                                                                                     new CommonFileDialogFilter(
                                                                                 "CSV files", ".csv"),
                                                                                 new CommonFileDialogFilter(
                                                                                 "All files", "*.*")
                                                                                 }, ".csv");
            if (dialogResult != CommonFileDialogResult.Ok)
            {
                return;
            }

            ExportToDelimitedFile(grid, selectedFile, ",");
        }

        /// <summary>
        /// Exports contents of <see cref="Infragistics.Win.UltraWinGrid"/> to a delimited text file.
        /// </summary>
        /// <param name="grid"><see cref="Infragistics.Win.UltraWinGrid"/> whose contents are to be exported.</param>
        /// <param name="delimiter">File delimiter.</param>
        /// <exception cref="ArgumentNullException">When <see cref="Infragistics.Win.UltraWinGrid"/> is null.</exception>
        public static void ExportToDelimitedFileWithPrompt(this UltraGrid grid, string delimiter)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            if (delimiter == null) throw new ArgumentNullException("delimiter");

            string selectedFile;
            var dialogResult = Forms.Dialogs.Dialog.ShowSaveFileDialog("Export", out selectedFile,
                                                                             new[] {new CommonFileDialogFilter(
                                                                                 "All files", "*.*")
                                                                                 });
            if (dialogResult != CommonFileDialogResult.Ok)
            {
                return;
            }

            ExportToDelimitedFile(grid, selectedFile, delimiter);
        }

        /// <summary>
        /// Exports contents of <see cref="Infragistics.Win.UltraWinGrid"/> to CSV separated file.
        /// </summary>
        /// <param name="grid"><see cref="Infragistics.Win.UltraWinGrid"/> whose contents are to be exported.</param>
        /// <param name="file">File name to write the data.</param>
        /// <param name="delimiter">File delimiter.</param>
        /// <exception cref="ArgumentNullException">When <see cref="Infragistics.Win.UltraWinGrid"/> is null.</exception>
        public static void ExportToDelimitedFile(this UltraGrid grid, string file, string delimiter)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            if (file == null) throw new ArgumentNullException("file");
            if (delimiter == null) throw new ArgumentNullException("delimiter");

            _log.InfoFormat("Exporting to delimited file with delimiter {0} ...", delimiter);

            // Validate file name
            try
            {
                _log.DebugFormat("Validating path for file {0} ...", file);
                Path.GetFullPath(file);
            }
            catch (Exception ex)
            {
                var message = string.Format("Error saving grid contents to delimited. File {0} is not valid.", file);
                _log.Error(message);
                _log.Error(ex.Message, ex);
                throw new Exception(message, ex);
            }

            // Export
            using (var sw = new StreamWriter(file, false))
            {
                var header = string.Join(delimiter,
                                         grid.DisplayLayout.Bands[0].Columns.Cast<UltraGridColumn>()
                                         .Where(x => !x.Hidden)
                                         .Select(x => x.Header.Caption));
                sw.WriteLine(header);
                foreach (var row in grid.Rows)
                {
                    var line = string.Join(delimiter,
                                               row.Cells.Cast<UltraGridCell>()
                                               .Where(x => !x.Column.Hidden)
                                               .Select(x => x.Value == null || Convert.IsDBNull(x.Value) ? string.Empty : x.GetText(MaskMode.IncludeBoth)));
                    sw.WriteLine(line);
                }
                sw.Close();
            }
        }

        /// <summary>
        /// Exports contents of <see cref="Infragistics.Win.UltraWinGrid"/> to a delimited file.
        /// </summary>
        /// <param name="grid"><see cref="Infragistics.Win.UltraWinGrid"/> whose contents are to be exported.</param>
        /// <param name="delimiter">Column delimiter</param>
        /// <param name="fileExtension">File extension (default is .txt)</param>
        /// <exception cref="ArgumentNullException">When <see cref="Infragistics.Win.UltraWinGrid"/> is null.</exception>
        public static Task ExportToDelimitedFileAsync(this UltraGrid grid, string delimiter, string fileExtension = ".txt")
        {
            if (grid == null) throw new ArgumentNullException("grid");
            if (delimiter == null) throw new ArgumentNullException("delimiter");
            if (fileExtension == null) throw new ArgumentNullException("fileExtension");
            var tempFileFullPath = Path.GetTempFileName();
            var tempFileName = Path.GetFileNameWithoutExtension(tempFileFullPath);
            var tempFilePath = Path.GetDirectoryName(tempFileFullPath);
            var exportFile = Path.Combine(tempFilePath ?? string.Empty, tempFileName) + fileExtension;
            File.Move(tempFileFullPath, exportFile);

            return Task.Run(() =>
            {
                ExportToDelimitedFile(grid, exportFile, delimiter);
                System.Diagnostics.Process.Start(exportFile);
            });
        }
    }
}
