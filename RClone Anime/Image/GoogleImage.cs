using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RClone_Anime.Configuiration;

namespace RClone_Anime.Image
{
    public class GoogleImage
    {
        private const string ImageSearchUrl = "https://www.google.com/search?q={0}&tbm=isch";
        private const int MaxParallelRequests = 3;

        public static string getFirst(string phrase)
        {
            var url = getImageSearchUrl(phrase);

            var web = new HtmlWeb();
            var doc = web.Load(url);

            return doc.DocumentNode.SelectSingleNode("//table[@class='images_table']//img").Attributes["src"].Value;
        }

        public static IEnumerable<Task> AddImages(IEnumerable<Anime> anime, bool force = false)
        {
            var semaphore = new Semaphore(MaxParallelRequests, MaxParallelRequests);
            return anime.Where(a => force || a.Image == null).Select(a => Task.Run(() =>
            {
                semaphore.WaitOne();
                a.RefreshImage();
                semaphore.Release();
            }));
        }

        private static string getImageSearchUrl(string phrase)
        {
            var encodedPhrase = WebUtility.UrlEncode(phrase);
            return string.Format(ImageSearchUrl, encodedPhrase);
        }
    }
}