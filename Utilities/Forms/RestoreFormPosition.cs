using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Utilities.Forms
{
    /// <summary>
    /// Class that restores form's position and size on the screen.
    /// </summary>
    public static class RestoreFormPosition
    {
        /// <summary>
        /// Sets Form's geometry from input string.
        /// </summary>
        /// <param name="thisWindowGeometry">Geometry string.</param>
        /// <param name="formIn">Form</param>
        public static void GeometryFromString(string thisWindowGeometry, Form formIn)
        {
            if (thisWindowGeometry == null) throw new ArgumentNullException("thisWindowGeometry");
            if (formIn == null) throw new ArgumentNullException("formIn");
            try
            {
                WindowPositionInfo windowPosInfo;
                using (var memStream = new MemoryStream(Encoding.Default.GetBytes(thisWindowGeometry)))
                {
                    var ser = new XmlSerializer(typeof(WindowPositionInfo));
                    windowPosInfo = ser.Deserialize(memStream) as WindowPositionInfo;
                }
                
                if (windowPosInfo == null)
                {
                    throw new Exception("Could not determine geometry from string.");
                }

                switch (windowPosInfo.WindowState)
                {
                    case FormWindowState.Maximized:
                        formIn.Location = windowPosInfo.MaximisedPoint;
                        formIn.StartPosition = FormStartPosition.Manual;
                        break;
                    case FormWindowState.Normal:
                        if (IdenticalScreenConfiguration(windowPosInfo.WorkingArea))
                        {
                            formIn.Location = windowPosInfo.Location;
                            formIn.Size = windowPosInfo.Size;
                            formIn.StartPosition = FormStartPosition.Manual;
                        }
                        break;
                }
                formIn.WindowState = windowPosInfo.WindowState;
            }
            catch { }
        }

        /// <summary>
        /// Serializes Form's geometry to string.
        /// </summary>
        /// <param name="mainForm">Form.</param>
        /// <returns>String representing form's geometry.</returns>
        public static string GeometryToString(Form mainForm)
        {
            if (mainForm == null) throw new ArgumentNullException("mainForm");
            try
            {
                var windowPosInfo = new WindowPositionInfo
                                        {
                                            Location = mainForm.Location,
                                            Size = mainForm.Size,
                                            WindowState = mainForm.WindowState
                                        };
                foreach (var screen in Screen.AllScreens)
                {
                    windowPosInfo.WorkingArea.Add(screen.WorkingArea);
                }
                using (var memStream = new MemoryStream())
                {
                    var ser = new XmlSerializer(typeof(WindowPositionInfo));
                    ser.Serialize(memStream, windowPosInfo);
                    return Encoding.Default.GetString(memStream.ToArray());
                }
            }
            catch
            { return string.Empty; }
        }

        static bool IdenticalScreenConfiguration(IList<Rectangle> savedScreenInfo)
        {
            if (savedScreenInfo.Count != Screen.AllScreens.Length)
            {
                return false;
            }
            for (var i = 0; i < savedScreenInfo.Count; i++)
            {
                if (savedScreenInfo[i] != Screen.AllScreens[i].WorkingArea)
                {
                    return false;
                }
            }
            return true;
        }
    }
}