using System;

namespace Utilities.InfragisticsUtilities.UltraGridUtilities
{
    /// <summary>
    /// DataFilter used to allow the end user to see disk sizes in B, KB, MB or GB
    /// </summary>
    public class UltraGridDiskSizeFilter : Infragistics.Win.IEditorDataFilter
    {
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
                        // only convert non null values
                        if (conversionArgs.Value != null &&
                            !(conversionArgs.Value is DBNull) &&
                            conversionArgs.Value is IConvertible)
                        {
                            // get the decimal value that the owner has
                            var bytes = System.Convert.ToInt64(conversionArgs.Value);

                            // let the editor know we handled the conversion
                            conversionArgs.Handled = true;

                            if (bytes == -1)
                            {
                                return "N/A";
                            }

                            var gb = (double)bytes / 1024 / 1024 / 1024;
                            if (gb >= 1)
                            {
                                return gb.ToString("#,0.00") + " GB";
                            }

                            var mb = (double)bytes / 1024 / 1024;
                            if (mb >= 1)
                            {
                                return mb.ToString("#,0.00") + " MB";
                            }

                            var kb = (double)bytes / 1024;
                            if (kb >= 1)
                            {
                                return kb.ToString("#,0.00") + " KB";
                            }

                            return bytes.ToString("#,0") + " B";
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
