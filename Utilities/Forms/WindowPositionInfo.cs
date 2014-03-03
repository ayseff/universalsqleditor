using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Utilities.Forms
{
    /// <summary>
    /// Represents a position of the form window on the screen.
    /// </summary>
    public class WindowPositionInfo
    {
        /// <summary>
        /// Gets of sets window location on the screen.
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Gets of sets window size.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Gets of sets window state.
        /// </summary>
        public FormWindowState WindowState { get; set; }

        /// <summary>
        /// Gets of sets window maximized point.
        /// </summary>
        public Point MaximisedPoint { get; set; }
        
        /// <summary>
        /// Gets of sets window working area.
        /// </summary>
        public List<Rectangle> WorkingArea { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public WindowPositionInfo()
        {
            WorkingArea = new List<Rectangle>();
        }
    }
}
