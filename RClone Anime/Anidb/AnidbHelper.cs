using System.Net;

namespace RClone_Anime.Anidb
{
    public class AnidbHelper
    {
        private const string SearchLinkUrl =
            "http://anidb.net/perl-bin/animedb.pl?show=search&do=fulltext&adb.search={0}&entity.animetb=1&field.titles=1&h=0&do.fsearch=Search";

        private AnidbHelper()
        {
        }

        public static string SearchLink(string text)
        {
            text = WebUtility.UrlEncode(text);
            return string.Format(SearchLinkUrl, text);
        }
    }
}