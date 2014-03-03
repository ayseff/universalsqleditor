using System;
using System.Windows.Forms;
using Infragistics.Win;

namespace Utilities.InfragisticsUtilities.UltraGridUtilities
{
    /// <summary>
    /// Converts boolean values to YES/NO words
    /// </summary>
    public class UltraGridBooleanYesNoDataFilter : IEditorDataFilter
    {
        object IEditorDataFilter.Convert(EditorDataFilterConvertArgs args)
        {
            switch (args.Direction)
            {
                case ConversionDirection.EditorToOwner:
                case ConversionDirection.EditorToDisplay:
                    args.Handled = true;
                    var state = (CheckState)args.Value;
                    switch (state)
                    {
                        case CheckState.Checked:
                            return "Yes";
                        case CheckState.Unchecked:
                            return "No";
                        case CheckState.Indeterminate:
                            return String.Empty;
                    }
                    break;
                case ConversionDirection.OwnerToEditor:
                case ConversionDirection.DisplayToEditor:
                    args.Handled = true;
                    if (args.Value.ToString().ToLower() == "true")
                        return CheckState.Checked;
                    else if (args.Value.ToString().ToLower() == "false")
                        return CheckState.Unchecked;
                    else
                        return CheckState.Indeterminate;
            }
            throw new Exception("Invalid value passed into CheckEditorDataFilter.Convert()");
        }
    }
}
