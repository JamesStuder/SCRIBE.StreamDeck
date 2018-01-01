using SCRIBE.StreamDeck.Data_Access_Objects;
using SCRIBE.StreamDeck.Models;
using SCRIBE.StreamDeck.Properties;
using StreamDeckSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SCRIBE.StreamDeck.Services
{
    class SubMenuService
    {
        private List<FunctionModel> oFunctions;
        private List<FunctionModel> oSubFunctions;
        NotifyIcon notify;
        IStreamDeck deck;
        string elementName;
        int subPageNumber;

        public SubMenuService()
        {
            notify = new NotifyIcon();
        }

        public void SubInitialDisplay(IStreamDeck deck, string elementName)
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
                deck.SetKeyBitmap(3, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(0) != null )? oSubFunctions[0].ImageLocation : null));
                deck.SetKeyBitmap(2, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(1) != null) ? oSubFunctions[1].ImageLocation : null));
                deck.SetKeyBitmap(1, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(2) != null) ? oSubFunctions[2].ImageLocation : null));
                deck.SetKeyBitmap(0, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(3) != null) ? oSubFunctions[3].ImageLocation : null));
                //Row 2                                           
                deck.SetKeyBitmap(9, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(4) != null) ? oSubFunctions[4].ImageLocation : null));
                deck.SetKeyBitmap(8, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(5) != null) ? oSubFunctions[5].ImageLocation : null));
                deck.SetKeyBitmap(7, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(6) != null) ? oSubFunctions[6].ImageLocation : null));
                deck.SetKeyBitmap(6, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(7) != null) ? oSubFunctions[7].ImageLocation : null));
                deck.SetKeyBitmap(5, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(8) != null) ? oSubFunctions[8].ImageLocation : null));
                //Row 3                                           
                deck.SetKeyBitmap(14, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(9) != null) ? oSubFunctions[9].ImageLocation : null));
                deck.SetKeyBitmap(13, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(10) != null) ? oSubFunctions[10].ImageLocation : null));
                deck.SetKeyBitmap(12, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(11) != null) ? oSubFunctions[11].ImageLocation : null));
                deck.SetKeyBitmap(11, StreamDeckKeyBitmap.FromFile((oSubFunctions.ElementAtOrDefault(12) != null) ? oSubFunctions[12].ImageLocation : null));
                deck.SetKeyBitmap(10, StreamDeckKeyBitmap.FromFile((oSubFunctions.Count > 13) ? Resources.ImagesDefault + "\\More.png" : null));
            }
            catch (Exception ex)
            {
                NotificationService notification = new NotificationService();
                notification.ToastMessage(notify, "Error Setting " + elementName + " Screen", ex.Message);
            }
            deck.KeyPressed += Deck_KeyPressed;
        }

        private void Deck_KeyPressed(object sender, StreamDeckKeyEventArgs e)
        {
            if (e.IsDown)
            {
                switch (e.Key)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        if(oFunctions.Count < 13)
                        {
                            StreamDeckService startScreen = new StreamDeckService((IStreamDeck)sender);
                            startScreen.InitialDisplay();
                        }
                        else
                        {
                            subPageNumber--;
                            ChangePage();
                        }
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8:
                        break;
                    case 9:
                        break;
                    case 10:
                        if(oFunctions.Count > 13 || oSubFunctions.Count == 13)
                        {
                            subPageNumber++;
                            ChangePage();
                        }
                        break;
                    case 11:
                        break;
                    case 12:
                        break;
                    case 13:
                        break;
                    case 14:
                        break;
                }
            }
        }

        private List<FunctionModel> CreateSubList()
        {
            int maxValue = (13 * subPageNumber) - 1;
            int minValue = maxValue = 13;

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