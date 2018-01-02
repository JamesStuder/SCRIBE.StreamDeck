using SCRIBE.StreamDeck.Services;
using StreamDeckSharp;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SCRIBE.StreamDeck
{
    static class Program
    {
        //Get the Guid from Assembly Info.  We need this for teh mutex to make sure only one instance is allowed to run.
        private static string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Mutex used to make sure only one instance of the application is running.
            using (Mutex mutex = new Mutex(false, @"Global\" + appGuid))
            {
                //If statement only enters if this applicaiton isn't already running.
                if(mutex.WaitOne(0,false))
                {
                    //Used to check and see if the Stock Stream Deck Software is running and if it is, shuts it down.
                    //This way it doesn't interfer with running ouf this applcation.
                    //We will restart it on application exit in the Context Menu Service Class.
                    foreach(Process p in Process.GetProcessesByName("StreamDeck"))
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                    //This is where the program starts.  Using statements used for clean disposals.
                    using (SystemTrayService sysTray = new SystemTrayService())
                    {
                        try
                        {
                            using (IStreamDeck deck = StreamDeckSharp.StreamDeck.FromHID())
                            {
                                StreamDeckService streamDeckService = new StreamDeckService(deck);
                                streamDeckService.InitialDisplay();
                                sysTray.Display();
                                GC.Collect();
                                Application.Run();
                            }
                        }
                        catch
                        {
                            Application.Exit();
                        }
                    }
                }
            }         
        }
    }
}