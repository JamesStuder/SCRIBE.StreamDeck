using SCRIBE.StreamDeck.Properties;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SCRIBE.StreamDeck.Services
{
    class ContextMenuService
    {
        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            item = new ToolStripMenuItem
            {
                Text = "Open XMLFile"
            };
            item.Click += new EventHandler(OpenXML_Click);
            menu.Items.Add(item);

            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            item = new ToolStripMenuItem
            {
                Text = "End Program"
            };
            item.Click += new EventHandler(Exit_Click);
            menu.Items.Add(item);

            return menu;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            EndProgramService end = new EndProgramService();
            end.EndProgram();
        }

        private void OpenXML_Click(object sender, EventArgs e)
        {
            Process.Start(Resources.XMLFile);
        }
    }
}