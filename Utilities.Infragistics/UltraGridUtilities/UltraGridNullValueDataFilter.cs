using System;

namespace Utilities.InfragisticsUtilities.UltraGridUtilities
{
    /// <summary>
    /// Converts NULL values of cells into text "(null) by default"
    /// </summary>
    public class UltraGridNullValueDataFilter : Infragistics.Win.IEditorDataFilter
    {
        private const string NULL_TEXT = "(null)";
        public string NullText { get; set; }

        public UltraGridNullValueDataFilter()
        {
            NullText = NULL_TEXT;
        }

        public UltraGridNullValueDataFilter(string nullText)
        {
            if (nullText == null) throw new ArgumentNullException("nullText");
            NullText = nullText;

        }

        /// <summary>
        /// Converts data between Display (user), Editor and Owner (data source).
        /// </summary>
        /// <param name="conversionArgs">Input arguments.</param>
        /// <returns>Converted data.</returns>
        public virtual object Convert(Infragistics.Win.EditorDataFilterConvertArgs conversionArgs)
        {
            switch (conversionArgs.Direction)
            {
                case Infragistics.Win.ConversionDirection.OwnerToEditor:
                    {
                        // only convert null values
                        if (conversionArgs.Value == null ||
                            conversionArgs.Value is DBNull)
                        {
                            // let the editor know we handled the conversion
                            conversionArgs.Handled = true;;
                            return NullText;
                        }
                        break;
                    }
            }

            // by default return null. as long as we don't indicate
            // that we handled the conversion (by setting conversionArgs.Handled
            // to true), the result will be ignored.
            return null;
        }
    }
}
