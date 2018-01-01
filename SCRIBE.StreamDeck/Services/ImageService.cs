using SCRIBE.StreamDeck.Properties;

namespace SCRIBE.StreamDeck.Services
{
    class ImageService
    {
        public string SetBaseFolder(string elementName)
        {
            string baseFolder = "";
            switch(elementName)
            {
                case "Conversion":
                    baseFolder = Resources.ImagesConversion;
                    return baseFolder;
                case "Date":
                    baseFolder = Resources.ImagesDate;
                    return baseFolder;
                case "Logical":
                    baseFolder = Resources.ImagesLogical;
                    return baseFolder;
                case "Lookup":
                    baseFolder = Resources.ImagesLookup;
                    return baseFolder;
                case "Math":
                    baseFolder = Resources.ImagesMath;
                    return baseFolder;
                case "Miscellaneous":
                    baseFolder = Resources.ImagesMiscellaneous;
                    return baseFolder;
                case "Text":
                    baseFolder = Resources.ImagesText;
                    return baseFolder;
                case "UserDefined":
                    baseFolder = Resources.ImagesUserDefined;
                    return baseFolder;
                default:
                    return string.Empty;
            }
        }
    }
}