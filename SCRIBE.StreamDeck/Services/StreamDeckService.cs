using SCRIBE.StreamDeck.Data_Access_Objects;
using SCRIBE.StreamDeck.Models;
using SCRIBE.StreamDeck.Properties;
using StreamDeckSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCRIBE.StreamDeck.Services
{
    class StreamDeckService
    {
        private List<FunctionModel> oFunctions;
        private List<FunctionModel> oSubFunctions;
        NotifyIcon notify;
        IStreamDeck deck;
        string elementName;
        int subPageNumber;
        bool allowPaste;

        public StreamDeckService(IStreamDeck deck)
        {
            notify = new NotifyIcon();
            this.deck = deck;
            //Initialze Keypress events
            deck.KeyPressed += Deck_KeyPressed;
        }
        public void InitialDisplay()
        {
            subPageNumber = 0;
            deck.SetBrightness(100);
            //Clear images from 
            for(int loop = 0; loop < deck.NumberOfKeys; loop++)
            {
                deck.ClearKey(loop);
            }
            //Set Initial Images
            try
            {
                //In order from left to right
                //Row 1
                deck.SetKeyBitmap(4, StreamDeckKeyBitmap.FromFile(Resources.ImagesDefault + "\\Back.png"));
                deck.SetKeyBitmap(3, StreamDeckKeyBitmap.FromFile(Resources.ImagesWeb + "\\Web-ScribeOnline.png"));
                deck.SetKeyBitmap(2, StreamDeckKeyBitmap.FromFile(Resources.ImagesWeb + "\\Web-ScribeForums.png"));
                deck.SetKeyBitmap(1, StreamDeckKeyBitmap.FromFile(Resources.ImagesWeb + "\\Web-ScribeHelp.png"));
                deck.SetKeyBitmap(0, StreamDeckKeyBitmap.FromFile(Resources.ImagesFiles + "\\OpenXML.png"));
                //Row 2
                deck.SetKeyBitmap(9, StreamDeckKeyBitmap.FromFile(Resources.ImagesFolders + "\\UserDefined.png"));
                deck.SetKeyBitmap(8, StreamDeckKeyBitmap.FromFile(Resources.ImagesDefault + "\\Copy.png"));
                deck.SetKeyBitmap(7, StreamDeckKeyBitmap.FromFile(Resources.ImagesDefault + "\\Paste.png"));
                deck.SetKeyBitmap(6, StreamDeckKeyBitmap.FromFile(Resources.ImagesDefault + "\\Cut.png"));
                deck.SetKeyBitmap(5, StreamDeckKeyBitmap.FromFile(Resources.ImagesFolders + "\\Conversion.png"));
                //Row 3                                                               
                deck.SetKeyBitmap(14, StreamDeckKeyBitmap.FromFile(Resources.ImagesFolders + "\\Date.png"));
                deck.SetKeyBitmap(13, StreamDeckKeyBitmap.FromFile(Resources.ImagesFolders + "\\Logical.png"));
                deck.SetKeyBitmap(12, StreamDeckKeyBitmap.FromFile(Resources.ImagesFolders + "\\Math.png"));
                deck.SetKeyBitmap(11, StreamDeckKeyBitmap.FromFile(Resources.ImagesFolders + "\\Text.png"));
                deck.SetKeyBitmap(10, StreamDeckKeyBitmap.FromFile(Resources.ImagesFolders + "\\Miscellaneous.png"));
            }
            catch (Exception ex)
            {
                NotificationService notification = new NotificationService();
                notification.ToastMessage(notify, "Error Setting Home Screen", ex.Message);
            }
        }

        private void SubInitialDisplay(IStreamDeck deck, string elementName)
        {
            this.deck = deck;
            this.elementName = elementName;
            subPageNumber = 1;
            oSubFunctions = new List<FunctionModel>();
            XmlDAO xml = new XmlDAO();
            oFunctions = xml.GetFunctions(elementName, Resources.XMLFile, notify);
            ChangePage();
        }

        private void ChangePage()
        {
            deck.SetBrightness(100);
            //Clear images from 
            for (int loop = 0; loop < deck.NumberOfKeys; loop++)
            {
                deck.ClearKey(loop);
            }

            oSubFunctions = (oFunctions.Count > 13) ? CreateSubList() : oFunctions;

            try
            {
                //In order from left to right
                //Row 1
                deck.SetKeyBitmap(4, StreamDeckKeyBitmap.FromFile(Resources.ImagesDefault + "\\Back.png"));
                if (oSubFunctions.ElementAtOrDefault(0) != null) { deck.SetKeyBitmap(3, StreamDeckKeyBitmap.FromFile(oSubFunctions[0].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(1) != null) { deck.SetKeyBitmap(2, StreamDeckKeyBitmap.FromFile(oSubFunctions[1].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(2) != null) { deck.SetKeyBitmap(1, StreamDeckKeyBitmap.FromFile(oSubFunctions[2].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(3) != null) { deck.SetKeyBitmap(0, StreamDeckKeyBitmap.FromFile(oSubFunctions[3].ImageLocation)); }
                //Row 2                                           
                if (oSubFunctions.ElementAtOrDefault(4) != null) { deck.SetKeyBitmap(9, StreamDeckKeyBitmap.FromFile(oSubFunctions[4].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(5) != null) { deck.SetKeyBitmap(8, StreamDeckKeyBitmap.FromFile(oSubFunctions[5].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(6) != null) { deck.SetKeyBitmap(7, StreamDeckKeyBitmap.FromFile(oSubFunctions[6].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(7) != null) { deck.SetKeyBitmap(6, StreamDeckKeyBitmap.FromFile(oSubFunctions[7].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(8) != null) { deck.SetKeyBitmap(5, StreamDeckKeyBitmap.FromFile(oSubFunctions[8].ImageLocation)); }
                //Row 3                                           
                if (oSubFunctions.ElementAtOrDefault(9) != null) { deck.SetKeyBitmap(14, StreamDeckKeyBitmap.FromFile(oSubFunctions[9].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(10) != null) { deck.SetKeyBitmap(13, StreamDeckKeyBitmap.FromFile(oSubFunctions[10].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(11) != null) { deck.SetKeyBitmap(12, StreamDeckKeyBitmap.FromFile(oSubFunctions[11].ImageLocation)); }
                if (oSubFunctions.ElementAtOrDefault(12) != null) { deck.SetKeyBitmap(11, StreamDeckKeyBitmap.FromFile(oSubFunctions[12].ImageLocation)); }
                if (oSubFunctions.Count == 13 && oFunctions.Count > 13) { deck.SetKeyBitmap(10, StreamDeckKeyBitmap.FromFile(Resources.ImagesDefault + "\\More.png")); }
            }
            catch (Exception ex)
            {
                NotificationService notification = new NotificationService();
                notification.ToastMessage(notify, "Error Setting " + elementName + " Screen", ex.Message);
            }
        }
        private void Deck_KeyPressed(object sender, StreamDeckKeyEventArgs e)
        {
            if(e.IsDown)
            {
                new Task(() => ButtonDepressed(e.Key)).Start();
            }
            else
            {
                new Task(() => ButtonReleased()).Start();
            }
        }
        
        private void ButtonReleased()
        {
            RunAsSTAThread(
                () =>
                {
                    if (allowPaste)
                    {
                        SendKeys.SendWait("^{v}");
                    }
                });
        }
        private void ButtonDepressed(int key)
        {
            RunAsSTAThread(
                () =>
                {
                    switch (key)
                    {
                        case 0:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                XmlDAO xml = new XmlDAO();
                                xml.OpenXmlFile(Resources.XMLFile, notify);
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(3) != null) { Clipboard.SetText(oSubFunctions[3].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 1:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                Process.Start("https://help.scribesoft.com/scribe/en/index.htm");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(2) != null) { Clipboard.SetText(oSubFunctions[2].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 2:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                Process.Start("https://success.scribesoft.com/s/");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(1) != null) { Clipboard.SetText(oSubFunctions[1].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 3:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                Process.Start("https://app.scribesoft.com");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(0) != null) { Clipboard.SetText(oSubFunctions[0].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 4:
                            allowPaste = false;
                            if (subPageNumber == 0)
                            {
                                EndProgramService end = new EndProgramService();
                                end.EndProgram();
                            }
                            else
                            {
                                if (oFunctions.Count < 13)
                                {
                                    InitialDisplay();
                                }
                                else
                                {
                                    subPageNumber--;
                                    if(subPageNumber == 0)
                                    {
                                        InitialDisplay();
                                    }
                                    else
                                    {
                                        ChangePage();
                                    }
                                    
                                }
                            }
                            break;
                        case 5:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SubInitialDisplay(deck, "Conversion");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(8) != null) { Clipboard.SetText(oSubFunctions[8].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 6:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SendKeys.SendWait("^{x}");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(7) != null) { Clipboard.SetText(oSubFunctions[7].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 7:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SendKeys.SendWait("^{v}");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(6) != null) { Clipboard.SetText(oSubFunctions[6].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 8:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SendKeys.SendWait("^{c}");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(5) != null) { Clipboard.SetText(oSubFunctions[5].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 9:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SubInitialDisplay(deck, "UserDefined");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(4) != null) { Clipboard.SetText(oSubFunctions[4].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 10:
                            allowPaste = false;
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SubInitialDisplay(deck, "Miscellaneous");
                            }
                            else
                            {
                                if (oFunctions.Count > 13 && oSubFunctions.Count == 13)
                                {
                                    subPageNumber++;
                                    ChangePage();
                                }
                            }
                            break;
                        case 11:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SubInitialDisplay(deck, "Text");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(12) != null) { Clipboard.SetText(oSubFunctions[12].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 12:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SubInitialDisplay(deck, "Math");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(11) != null) { Clipboard.SetText(oSubFunctions[11].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 13:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SubInitialDisplay(deck, "Logical");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(10) != null) { Clipboard.SetText(oSubFunctions[10].Value, TextDataFormat.Text); }
                            }
                            break;
                        case 14:
                            if (subPageNumber == 0)
                            {
                                allowPaste = false;
                                SubInitialDisplay(deck, "Date");
                            }
                            else
                            {
                                allowPaste = true;
                                if (oSubFunctions.ElementAtOrDefault(9) != null) { Clipboard.SetText(oSubFunctions[9].Value, TextDataFormat.Text); }
                            }
                            break;
                    }
                });
        }
        static void RunAsSTAThread(Action keyPressed)
        {
            AutoResetEvent rEvent = new AutoResetEvent(false);
            Thread thread = new Thread(
                () =>
                {
                    keyPressed();
                    rEvent.Set();
                });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            rEvent.WaitOne();
        }
        private List<FunctionModel> CreateSubList()
        {
            oSubFunctions = new List<FunctionModel>();
            int maxValue = (13 * subPageNumber);
            int minValue = (maxValue == 13) ? 0 : maxValue - 12;

            //Check to make sure that we don't exceed the total number of functions.  We do it here because we need the initial max value above to set the min value.
            maxValue = (maxValue > oFunctions.Count) ? oFunctions.Count : maxValue;

            //Loop to populate the sub list.
            do
            {
                oSubFunctions.Add(oFunctions[minValue]);
                minValue++;
            }
            while (minValue != maxValue);

            return oSubFunctions;
        }
    }
}