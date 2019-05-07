using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RClone_Anime.RClone
{
    public class LsParser
    {
        private const string LsRegex = @"^\s*(\d+)\s+(?:([^/]+)[/])?(.+)$";

        private LsParser()
        {
        }

        public static IEnumerable<LsItem> parse(string lsOutput)
        {
            var result = new List<LsItem>();
            using (StringReader reader = new StringReader(lsOutput))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    var matches = Regex.Match(line, LsRegex);
                    if (!matches.Success)
                        continue;

                    var item = new LsItem
                    {
                        fileId = long.Parse(matches.Groups[1].Value),
                        dirName = !string.IsNullOrEmpty(matches.Groups[2].Value) ? matches.Groups[2].Value : null,
                        fileName = matches.Groups[3].Value
                    };

                    result.Add(item);
                }
            }

            return result;
        }
    }
}