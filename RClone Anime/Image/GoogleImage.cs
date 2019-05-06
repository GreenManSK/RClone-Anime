using System.Net;
using HtmlAgilityPack;

namespace RClone_Anime.Image
{
    public class GoogleImage
    {

        private const string ImageSearchUrl = "https://www.google.com/search?q={0}&tbm=isch";
        
        public static string getFirst(string phrase)
        {
            var url = getImageSearchUrl(phrase);
            
            var web = new HtmlWeb();
            var doc = web.Load(url);
            
            return doc.DocumentNode.SelectSingleNode("//table[@class='images_table']//img").Attributes["src"].Value;
        }

        private static string getImageSearchUrl(string phrase)
        {
            var encodedPhrase = WebUtility.UrlEncode(phrase);
            return string.Format(ImageSearchUrl, encodedPhrase);
        }
    }
}