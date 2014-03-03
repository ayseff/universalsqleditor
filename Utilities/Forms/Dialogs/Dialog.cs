using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;
using Utilities.Text;
using log4net;

namespace Utilities.Forms.Dialogs
{
    /// <summary>
    /// Allows functionality for displaying dialogs.
    /// </summary>
    public class Dialog
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Dialog));

        /// <summary>
        /// Show generic user dialog.
        /// </summary>
        /// <param name="windowTitleText">Title that appears in the title bar of the popup window.</param>
        /// <param name="title">Title that appears inside the popup window.</param>
        /// <param name="message">Detailed messages appearing in the popup window.</param>
        /// <param name="commandLinks">Command links to add</param>
        /// <param name="icon">Icon to display.</param>
        /// <returns><code>TaskDialogResult</code> object indicating which option user selected.</returns>
        public static TaskDialogResult ShowDialog(string windowTitleText, string title, string message, IEnumerable<TaskDialogCommandLink> commandLinks, TaskDialogStandardIcon icon = TaskDialogStandardIcon.Information)
        {
            if (TaskDialog.IsPlatformSupported)
            {
                var taskdlg = new TaskDialog
                                  {
                                      Icon = icon,
                                      Caption = windowTitleText,
                                      InstructionText = title,
                                      Text = message
                                  };
                if (commandLinks != null)
                {
                    foreach (var taskDialogCommandLink in commandLinks)
                    {
                        taskdlg.Controls.Add(taskDialogCommandLink);
                    }
                }
                return taskdlg.Show();
            }
            else
            {
                var result = MessageBox.Show(message, windowTitleText, GetMessageBoxButtons(TaskDialogStandardButtons.Ok),
                                             GetMessageBoxIcons(icon));
                return GetTaskDialogResult(result);
            }
        }

        /// <summary>
        /// Show generic user dialog.
        /// </summary>
        /// <param name="windowTitleText">Title that appears in the title bar of the popup window.</param>
        /// <param name="title">Title that appears inside the popup window.</param>
        /// <param name="message">Detailed messages appearing in the popup window.</param>
        /// <param name="footerText">Footer text</param>
        /// <param name="buttons">Buttons to display.</param>
        /// <param name="icon">Icon to display.</param>
        /// <returns><code>TaskDialogResult</code> object indicating which option user selected.</returns>
        public static TaskDialogResult ShowDialog(string windowTitleText, string title, string message, TaskDialogStandardButtons buttons = TaskDialogStandardButtons.Ok, TaskDialogStandardIcon icon = TaskDialogStandardIcon.Information, string footerText = null)
        {
            if (TaskDialog.IsPlatformSupported)
            {
                var taskdlg = new TaskDialog
                {
                    Icon = icon,
                    Caption = windowTitleText,
                    InstructionText = title,
                    Text = message,
                    StandardButtons = buttons
                };

                if (!string.IsNullOrEmpty(footerText))
                {
                    taskdlg.DetailsExpandedLabel = "Hide Details";
                    taskdlg.DetailsCollapsedLabel = "Show Details";
                    taskdlg.DetailsExpandedText = footerText;
                    taskdlg.ExpansionMode = TaskDialogExpandedDetailsLocation.ExpandFooter;
                }
                return taskdlg.Show();
            }
            else
            {
                var result = MessageBox.Show(message, windowTitleText, GetMessageBoxButtons(buttons),
                                             GetMessageBoxIcons(icon));
                return GetTaskDialogResult(result);
            }
        }

        /// <summary>
        /// Show error dialog.
        /// </summary>
        /// <param name="windowTitleText">Title that appears in the title bar of the popup window.</param>
        /// <param name="title">Title that appears inside the popup window.</param>
        /// <param name="message">Detailed messages appearing in the popup window.</param>
        /// <param name="stackTrace">Stack trace.</param>
        /// <param name="buttons">Buttons to display.</param>
        /// <param name="icon">Icon to display.</param>
        /// <returns><code>TaskDialogResult</code> object indicating which option user selected.</returns>
        [Obsolete("ShowErrorDialog is deprecated, please use ShowErrorDialog with different signature.")]
        public static TaskDialogResult ShowErrorDialog(string windowTitleText, string title, string message, TaskDialogStandardButtons buttons = TaskDialogStandardButtons.Ok, TaskDialogStandardIcon icon = TaskDialogStandardIcon.Error, string stackTrace = null)
        {
            return ShowDialog(windowTitleText, title, message, buttons, icon, stackTrace);
        }

        /// <summary>
        /// Show error dialog.
        /// </summary>
        /// <param name="windowTitleText">Title that appears in the title bar of the popup window.</param>
        /// <param name="title">Title that appears inside the popup window.</param>
        /// <param name="message">Detailed messages appearing in the popup window.</param>
        /// <param name="stackTrace">Stack trace.</param>
        /// <param name="buttons">Buttons to display.</param>
        /// <param name="icon">Icon to display.</param>
        /// <returns><code>TaskDialogResult</code> object indicating which option user selected.</returns>
        public static TaskDialogResult ShowErrorDialog(string windowTitleText, string title, string message, string stackTrace, TaskDialogStandardButtons buttons = TaskDialogStandardButtons.Ok, TaskDialogStandardIcon icon = TaskDialogStandardIcon.Error)
        {
            return ShowDialog(windowTitleText, title, message, buttons, icon, stackTrace);
        }

        /// <summary>
        /// Show generic user dialog with don't show this again option.
        /// </summary>
        /// <param name="windowTitleText">Title that appears in the title bar of the popup window.</param>
        /// <param name="title">Title that appears inside the popup window.</param>
        /// <param name="message">Detailed messages appearing in the popup window.</param>
        /// <param name="dontShowAgain">Boolean value indicating whether user selected don't sow this again.</param>
        /// <param name="buttons">Buttons to display.</param>
        /// <param name="icon">Icon to display.</param>
        /// <returns><code>TaskDialogResult</code> object indicating which option user selected.</returns>
        public static TaskDialogResult ShowDialogWithDontShowAgain(string windowTitleText, string title, string message, out bool dontShowAgain, TaskDialogStandardButtons buttons = TaskDialogStandardButtons.Ok, TaskDialogStandardIcon icon = TaskDialogStandardIcon.Information)
        {
            if (TaskDialog.IsPlatformSupported)
            {
                var taskdlg = new TaskDialog();
                var button = buttons;
                taskdlg.Icon = icon;
                taskdlg.Caption = windowTitleText;
                taskdlg.InstructionText = title;
                taskdlg.Text = message;
                taskdlg.StandardButtons = button;
                taskdlg.FooterCheckBoxChecked = false;
                taskdlg.FooterCheckBoxText = string.Format("Don't show this for remaining items");
                var result = taskdlg.Show();
                dontShowAgain = taskdlg.FooterCheckBoxChecked.GetValueOrDefault(false);
                return result;
            }
            else
            {
                dontShowAgain = false;
                var result = MessageBox.Show(message, windowTitleText, GetMessageBoxButtons(buttons),
                                             GetMessageBoxIcons(icon));
                return GetTaskDialogResult(result);
            }
        }

        /// <summary>
        /// Show generic user dialog with yes to all option.
        /// </summary>
        /// <param name="windowTitleText">Title that appears in the title bar of the popup window.</param>
        /// <param name="title">Title that appears inside the popup window.</param>
        /// <param name="message">Detailed messages appearing in the popup window.</param>
        /// <param name="itemsCount">Number of items for which the operation will be repeated.</param>
        /// <param name="repeatForAll">Indicates whether a user wants to apply the action to all items.</param>
        /// <param name="icon"></param>
        /// <returns><code>TaskDialogResult</code> object indicating which option user selected.</returns>
        public static TaskDialogResult ShowYesNoDialog(string windowTitleText, string title, string message, int itemsCount, out bool repeatForAll, TaskDialogStandardIcon icon = TaskDialogStandardIcon.Information)
        {
            if (TaskDialog.IsPlatformSupported)
            {
                var taskdlg = new TaskDialog();
                const TaskDialogStandardButtons button = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No |
                                                         TaskDialogStandardButtons.Cancel;
                taskdlg.Icon = icon;
                taskdlg.Caption = windowTitleText;
                taskdlg.InstructionText = title;
                taskdlg.Text = message;
                taskdlg.StandardButtons = button;
                if (itemsCount > 1)
                {
                    taskdlg.FooterCheckBoxChecked = false;
                    taskdlg.FooterCheckBoxText = string.Format("Do this for remaining {0} items.", itemsCount - 1);
                }
                var result = taskdlg.Show();
                repeatForAll = taskdlg.FooterCheckBoxChecked.GetValueOrDefault(false);
                return result;
            }
            else
            {
                repeatForAll = false;
                var result = MessageBox.Show(message, windowTitleText, MessageBoxButtons.YesNoCancel,
                                             MessageBoxIcon.Question);
                return GetTaskDialogResult(result);
            }
        }

        /// <summary>
        /// Show generic user dialog with yes to all option.
        /// </summary>
        /// <param name="windowTitleText">Title that appears in the title bar of the popup window.</param>
        /// <param name="title">Title that appears inside the popup window.</param>
        /// <param name="message">Detailed messages appearing in the popup window.</param>
        /// <param name="icon"></param>
        /// <returns><code>TaskDialogResult</code> object indicating which option user selected.</returns>
        public static TaskDialogResult ShowYesNoDialog(string windowTitleText, string title, string message, TaskDialogStandardIcon icon = TaskDialogStandardIcon.Information)
        {
            if (TaskDialog.IsPlatformSupported)
            {
                var taskdlg = new TaskDialog();
                const TaskDialogStandardButtons button = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No |
                                                         TaskDialogStandardButtons.Cancel;
                taskdlg.Icon = icon;
                taskdlg.Caption = windowTitleText;
                taskdlg.InstructionText = title;
                taskdlg.Text = message;
                taskdlg.StandardButtons = button;
                var result = taskdlg.Show();
                return result;
            }
            else
            {
                var result = MessageBox.Show(message, windowTitleText, MessageBoxButtons.YesNoCancel,
                                             MessageBoxIcon.Question);
                return GetTaskDialogResult(result);
            }

        }

        /// <summary>
        /// Shows select folder dialog.
        /// </summary>
        /// <param name="title">Title that appears in the title bar of the popup window.</param>
        /// <param name="selectedFolder">Folder user selected.</param>
        /// <param name="currentPath">Current path where select folder dialog will start.</param>
        /// <returns><code>CommonFileDialogResult</code> object indicating how user responded to the dialog.</returns>
        public static CommonFileDialogResult ShowFolderDialog(string title, out string selectedFolder, string currentPath = null)
        {
            if (CommonFileDialog.IsPlatformSupported)
            {
                // Display a CommonOpenFileDialog to select only folders 
                selectedFolder = string.Empty;
                var cfd = new CommonOpenFileDialog
                              {
                                  Title = title,
                                  EnsurePathExists = true,
                                  Multiselect = false,
                                  EnsureReadOnly = true,
                                  IsFolderPicker = true,
                                  AllowNonFileSystemItems = true
                              };

                if (currentPath != null && !currentPath.IsNullEmptyOrWhitespace() && Directory.Exists(currentPath))
                {
                    cfd.DefaultDirectory = currentPath;
                }

                var result = cfd.ShowDialog();
                if (result == CommonFileDialogResult.OK)
                {
                    var selectedShellObject = cfd.FileAsShellObject as ShellContainer;
                    if (selectedShellObject != null)
                    {
                        selectedFolder = selectedShellObject.ParsingName;
                    }
                    else
                    {
                        throw new Exception("Could not parse shell object!");
                    }
                }
                return result;
            }
            else
            {
                var folderDialog = new FolderBrowserDialog {ShowNewFolderButton = true, Description = title};
                if (currentPath != null && !currentPath.IsNullEmptyOrWhitespace() && Directory.Exists(currentPath))
                {
                    folderDialog.SelectedPath = currentPath;
                }
                var result = folderDialog.ShowDialog();
                selectedFolder = folderDialog.SelectedPath;
                return GetCommonFileDialogResult(result);
            }
        }

        /// <summary>
        /// Shows select file dialog.
        /// </summary>
        /// <param name="title">Title that appears in the title bar of the popup window.</param>
        /// <param name="selectedFile">File user selected.</param>
        /// <param name="filters">Filters to apply to file extensions.</param>
        /// <param name="defaultExtension">Default extension to apply.</param>
        /// <param name="path">Current path where select folder dialog will start.</param>
        /// <returns><code>CommonFileDialogResult</code> object indicating how user responded to the dialog.</returns>
        public static CommonFileDialogResult ShowOpenFileDialog(string title, out string selectedFile, IEnumerable<CommonFileDialogFilter> filters = null, string defaultExtension = null, string path = null)
        {
            selectedFile = null;
            string[] selectedFiles;
            var result = ShowOpenFilesDialog(title, out selectedFiles, filters, false, defaultExtension, path);
            if (result == CommonFileDialogResult.OK && selectedFiles != null && selectedFiles.Length > 0)
            {
                selectedFile = selectedFiles[0];
            }
            return result;
        }

        /// <summary>
        /// Shows select files dialog.
        /// </summary>
        /// <param name="title">Title that appears in the title bar of the popup window.</param>
        /// <param name="selectedFiles">Files user selected.</param>
        /// <param name="filters">Filters to apply to file extensions.</param>
        /// <param name="multiselect">Whether dialog supports selecting multiple files.</param>
        /// <param name="defaultExtension">Default extension to apply.</param>
        /// <param name="path">Current path where select folder dialog will start.</param>
        /// <returns><code>CommonFileDialogResult</code> object indicating how user responded to the dialog.</returns>
        public static CommonFileDialogResult ShowOpenFilesDialog(string title, out string[] selectedFiles, IEnumerable<CommonFileDialogFilter> filters = null, bool multiselect = true, string defaultExtension = null, string path = null)
        {
            if (CommonFileDialog.IsPlatformSupported)
            {
                // Display a CommonOpenFileDialog to select only files 
                selectedFiles = null;
                var cfd = new CommonOpenFileDialog
                {
                    Title = title,
                    EnsurePathExists = true,
                    Multiselect = multiselect,
                    EnsureReadOnly = true,
                    AllowNonFileSystemItems = true
                };
                if (!defaultExtension.IsNullEmptyOrWhitespace())
                {
                    cfd.DefaultExtension = defaultExtension;
                }

                if (!path.IsNullEmptyOrWhitespace())
                {
                    try
                    {
                        while (path != null && !path.IsNullEmptyOrWhitespace() && !Directory.Exists(path))
                        {
                            path = Path.GetDirectoryName(path);
                        }
                        if (!path.IsNullEmptyOrWhitespace())
                        {
                            cfd.InitialDirectory = path;
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Error(e.Message, e);
                    }
                }

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        cfd.Filters.Add(filter);
                    }
                }

                var result = cfd.ShowDialog();
                if (result == CommonFileDialogResult.OK)
                {
                    selectedFiles = cfd.FileNames.ToArray();
                }
                return result;
            }
            else
            {
                var fileDialog = new OpenFileDialog { RestoreDirectory = true, Title = title, Multiselect = multiselect };
                while (path != null && !path.IsNullEmptyOrWhitespace() && Directory.Exists(path))
                {
                    fileDialog.InitialDirectory = path;
                }
                if (filters != null)
                {
                    var sb = new StringBuilder();
                    foreach (var filter in filters)
                    {
                        foreach (var extension in filter.Extensions)
                        {
                            if (sb.Length != 0)
                            {
                                sb.Append("|");
                            }
                            sb.AppendFormat("{0}|{1}", filter.DisplayName, extension);
                        }
                    }
                    fileDialog.Filter = sb.ToString();
                }
                var result = fileDialog.ShowDialog();
                selectedFiles = fileDialog.FileNames;
                return GetCommonFileDialogResult(result);
            }
        }

        /// <summary>
        /// Shows select file dialog.
        /// </summary>
        /// <param name="title">Title that appears in the title bar of the popup window.</param>
        /// <param name="selectedFile">File user selected.</param>
        /// <param name="filters">Filters to apply to file extensions.</param>
        /// <param name="defaultExtension">Default extension to apply.</param>
        /// <param name="path">Current path where select folder dialog will start.</param>
        /// <returns><code>CommonFileDialogResult</code> object indicating how user responded to the dialog.</returns>
        public static CommonFileDialogResult ShowSaveFileDialog(string title, out string selectedFile, IEnumerable<CommonFileDialogFilter> filters = null, string defaultExtension = null, string path = null)
        {
            var filtersList = filters as IList<CommonFileDialogFilter> ?? (filters == null ? null : filters.ToList());
            if (CommonFileDialog.IsPlatformSupported)
            {
                // Display a CommonSaveFileDialog to select only files 
                selectedFile = string.Empty;
                var cfd = new CommonSaveFileDialog
                {
                    Title = title,
                    EnsurePathExists = true,
                    EnsureReadOnly = true
                };

                if (!string.IsNullOrWhiteSpace(defaultExtension))
                {
                    cfd.DefaultExtension = defaultExtension;
                }
                else if (filtersList != null && filtersList.Any() && filtersList.First().Extensions.Count > 0)
                {
                    cfd.DefaultExtension = filtersList.First().Extensions[0];
                }

                if (!string.IsNullOrWhiteSpace(path))
                {
                    try
                    {
                        while (!string.IsNullOrWhiteSpace(path) && !Directory.Exists(path))
                        {
                            path = Path.GetDirectoryName(path);
                        }
                        cfd.InitialDirectory = path;
                    }
                    catch (Exception e)
                    {
                        _log.Error(e.Message, e);
                    }
                }

                if (filtersList != null)
                {
                    foreach (var filter in filtersList)
                    {
                        cfd.Filters.Add(filter);
                    }
                }

                var result = cfd.ShowDialog();
                if (result == CommonFileDialogResult.OK)
                {
                    var selectedShellObject = cfd.FileAsShellObject;
                    selectedFile = selectedShellObject.ParsingName;
                }
                return result;
            }
            else
            {
                var fileDialog = new SaveFileDialog { RestoreDirectory = true, Title = title };
                while (!string.IsNullOrWhiteSpace(path) && !Directory.Exists(path))
                {
                    fileDialog.InitialDirectory = path;
                }

                if (!string.IsNullOrWhiteSpace(defaultExtension))
                {
                    fileDialog.DefaultExt = defaultExtension;
                }
                else if (filtersList != null && filtersList.Any() && filtersList.First().Extensions.Count > 0)
                {
                    fileDialog.DefaultExt = filtersList.First().Extensions[0];
                }

                if (filtersList != null)
                {
                    var sb = new StringBuilder();
                    foreach (var filter in filtersList)
                    {
                        foreach (var extension in filter.Extensions)
                        {
                            if (sb.Length != 0)
                            {
                                sb.Append("|");
                            }
                            sb.AppendFormat("{0}|{1}", filter.DisplayName, extension);
                        }
                    }
                    fileDialog.Filter = sb.ToString();
                }
                var result = fileDialog.ShowDialog();
                selectedFile = fileDialog.FileName;
                return GetCommonFileDialogResult(result);
            }
        }

        /// <summary>
        /// Delete file with prompt.
        /// </summary>
        /// <param name="fileName">File to delete.</param>
        /// <param name="windowTitle">Title that appears in the title bar of the popup window.</param>
        /// <returns><code>true</code> if the delete was successful or <code>false</code> otherwise.</returns>
        public static bool DeleteFileWithPrompt(string fileName, string windowTitle)
        {
            if (!fileName.IsNullEmptyOrWhitespace() && File.Exists(fileName))
            {
                var result = ShowDialog("Do you want to delete the file from disk?",
                                                               windowTitle,
                                                               string.Format(
                                                                   "Do you want to delete file {0} from disk?",
                                                                   fileName),
                                                               TaskDialogStandardButtons.Yes |
                                                               TaskDialogStandardButtons.No);
                if (result == TaskDialogResult.Yes)
                {
                    while (true)
                    {
                        try
                        {
                            File.Delete(fileName);
                            return true;
                        }
                        catch (Exception e)
                        {
                            _log.ErrorFormat("Error occurred while deleting file {0}", fileName);
                            _log.Error(e.Message, e);
                            if (
                                ShowDialog("Error deleting file.", windowTitle,
                                                     string.Format(
                                                         "Error occurred while deleting file {0}. Error message: {1}.",
                                                         fileName, e.Message),
                                                     TaskDialogStandardButtons.Ok | TaskDialogStandardButtons.Retry) !=
                                TaskDialogResult.Retry)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the progress on an icon in the taskbar.
        /// </summary>
        /// <param name="percentageProgress">Percentage progress to set.</param>
        /// <remarks>Set the progress to zero to turn off progress on the taskbar.</remarks>
        public static void SetTaskbarProgress(int percentageProgress)
        {
            if (TaskbarManager.IsPlatformSupported)
            {
                if (percentageProgress > 0)
                {
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                    TaskbarManager.Instance.SetProgressValue(Math.Min(100, percentageProgress), 100);
                }
                else
                {
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                }
            }
        }

        private static MessageBoxButtons GetMessageBoxButtons(TaskDialogStandardButtons buttons)
        {
            if ((buttons & TaskDialogStandardButtons.Ok) == TaskDialogStandardButtons.Ok
                && (buttons & TaskDialogStandardButtons.Cancel) == TaskDialogStandardButtons.Cancel)
            {
                return MessageBoxButtons.OKCancel;
            }
            else if ((buttons & TaskDialogStandardButtons.Retry) == TaskDialogStandardButtons.Retry
                && (buttons & TaskDialogStandardButtons.Cancel) == TaskDialogStandardButtons.Cancel)
            {
                return MessageBoxButtons.RetryCancel;
            }
            else if ((buttons & TaskDialogStandardButtons.Yes) == TaskDialogStandardButtons.Yes
                && (buttons & TaskDialogStandardButtons.No) == TaskDialogStandardButtons.No)
            {
                return MessageBoxButtons.YesNo;
            }
            else if ((buttons & TaskDialogStandardButtons.Yes) == TaskDialogStandardButtons.Yes
                && (buttons & TaskDialogStandardButtons.No) == TaskDialogStandardButtons.No
                && (buttons & TaskDialogStandardButtons.Cancel) == TaskDialogStandardButtons.Cancel)
            {
                return MessageBoxButtons.YesNoCancel;
            }
            return MessageBoxButtons.OK;
        }

        private static MessageBoxIcon GetMessageBoxIcons(TaskDialogStandardIcon icon)
        {
            if (icon == TaskDialogStandardIcon.Warning)
            {
                return MessageBoxIcon.Warning;
            }
            else if (icon == TaskDialogStandardIcon.Information)
            {
                return MessageBoxIcon.Information;
            }
            else if (icon == TaskDialogStandardIcon.None)
            {
                return MessageBoxIcon.None;
            }
            else if (icon == TaskDialogStandardIcon.Error)
            {
                return MessageBoxIcon.Error;
            }
            else if (icon == TaskDialogStandardIcon.Shield)
            {
                return MessageBoxIcon.Hand;
            }
            return MessageBoxIcon.None;
        }

        private static TaskDialogResult GetTaskDialogResult(DialogResult result)
        {
            if (result == DialogResult.Cancel)
            {
                return TaskDialogResult.Cancel;
            }
            else if (result == DialogResult.No)
            {
                return TaskDialogResult.No;
            }
            else if (result == DialogResult.OK)
            {
                return TaskDialogResult.Ok;
            }
            else if (result == DialogResult.Retry)
            {
                return TaskDialogResult.Retry;
            }
            else if (result == DialogResult.Yes)
            {
                return TaskDialogResult.Yes;
            }
            return TaskDialogResult.Ok;
        }

        private static CommonFileDialogResult GetCommonFileDialogResult(DialogResult result)
        {
            if (result == DialogResult.Cancel)
            {
                return CommonFileDialogResult.Cancel;
            }
            else if (result == DialogResult.OK)
            {
                return CommonFileDialogResult.OK;
            }
            return CommonFileDialogResult.OK;
        }
    }
}
