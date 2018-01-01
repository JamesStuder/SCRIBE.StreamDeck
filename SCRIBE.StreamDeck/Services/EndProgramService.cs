using SCRIBE.StreamDeck.Properties;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SCRIBE.StreamDeck.Services
{
    class EndProgramService
    {
        public void EndProgram()
        {
            //Used to see if stream deck stock software is installed and it if it we start it then exit out application.
            //Otherwise we just exit out application
            string streamDeckInstallLocation = Resources.StreamDeckInstallLocation + "\\StreamDeck.exe";
            if (File.Exists(streamDeckInstallLocation))
            {
                Process proc = new Process();
                proc.StartInfo.FileName = streamDeckInstallLocation;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
            }
            Application.Exit();
        }
    }
}