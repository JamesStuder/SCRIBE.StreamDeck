using System.Windows.Forms;

namespace SCRIBE.StreamDeck.Services
{
    public class NotificationService
    {
        public void ToastMessage(NotifyIcon notifyIcon, string title, string message)
        {
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = message;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(3);
        }
    }
}