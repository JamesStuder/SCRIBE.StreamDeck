using SCRIBE.StreamDeck.Properties;
using System;
using System.Windows.Forms;

namespace SCRIBE.StreamDeck.Services
{ 
    class SystemTrayService :IDisposable
    {
        NotifyIcon notify;
        public SystemTrayService()
        {
            notify = new NotifyIcon();
        }
        public void Display()
        {
            notify.Icon = Resources.SCRIBE;
            notify.Text = "SCRIBE Stream Deck Application";
            notify.Visible = true;
            notify.ContextMenuStrip = new ContextMenuService().Create();
        }

        public void Dispose()
        {
            notify.Dispose();
        }
    }
}