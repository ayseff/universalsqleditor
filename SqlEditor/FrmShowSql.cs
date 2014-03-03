using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ICSharpCode.TextEditor.Document;
using log4net;

namespace SqlEditor
{
    public partial class FrmShowSql : Form
    {
        private readonly string _highlightDefinitionsFiles;
        private static ILog _log;
        protected static ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(typeof(FrmShowSql));
                }
                return _log;
            }
        }

        public FrmShowSql(string sql, string highlightDefinitionsFiles)
        {
            _highlightDefinitionsFiles = highlightDefinitionsFiles;
            InitializeComponent();
            LoadHighlightingDefinitions();
            _editor.Document.TextContent = sql;
        }

        private void LoadHighlightingDefinitions()
        {
            try
            {
                const string dir = "Resources";
                if (Directory.Exists(dir))
                {
                    var fsmProvider = new FileSyntaxModeProvider(dir); // Provider
                    HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
                    _editor.SetHighlighting(_highlightDefinitionsFiles);
                }
                else
                {
                    throw new Exception("Missing highlihting definitions.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error openning highlihting definitions.");
                Log.Error(ex.Message);
            }
        }
    }
}
