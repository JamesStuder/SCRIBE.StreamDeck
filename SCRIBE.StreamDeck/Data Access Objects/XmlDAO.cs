using SCRIBE.StreamDeck.Models;
using SCRIBE.StreamDeck.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SCRIBE.StreamDeck.Data_Access_Objects
{
    public class XmlDAO
    {
        public List<FunctionModel> GetFunctions(string elementName, string xmlFile, NotifyIcon notifyIcon)
        {
            List<FunctionModel> oFunctionModel = new List<FunctionModel>();
            try
            {
                XDocument xmlDoc = XDocument.Load(xmlFile);
                oFunctionModel = (from xml in xmlDoc.Root.Element(elementName).Descendants("function")
                                  select new FunctionModel
                                  {
                                      Name = xml.Attribute("name").Value,
                                      Value = xml.Attribute("value").Value,
                                      RootName = elementName,
                                      ImageLocation = SetImnageFileLocation(elementName, xml.Attribute("name").Value)
                                  }).ToList();
            }
            catch (Exception ex)
            {
                NotificationService notification = new NotificationService();
                notification.ToastMessage(notifyIcon, "XML Error - Reading", ex.Message);
                return null;
            }
            return oFunctionModel;
        }

        public void OpenXmlFile(string xmlFile, NotifyIcon notifyIcon)
        {
            try
            {
                Process.Start(xmlFile);
            }
            catch (Exception ex)
            {
                NotificationService notification = new NotificationService();
                notification.ToastMessage(notifyIcon, "XML Error - Opening", ex.Message);
            }
        }

        private string SetImnageFileLocation(string elementName, string attributeName)
        {
            ImageService image = new ImageService();
            string imageFile = image.SetBaseFolder(elementName) + "\\" + attributeName + ".png";
            return imageFile;
        }
    }
}