using System.IO;
using Newtonsoft.Json;

namespace RClone_Anime.Configuiration
{
    public class AnimeFile
    {
        public string Name { get; set; }

        [JsonIgnore] public Anime Anime { get; set; }

        [JsonIgnore]
        public string Ext => Path.GetExtension(Name);

        public AnimeFile(string name)
        {
            Name = name;
        }

        public string GetPath()
        {
            return $"{Anime.GetPath()}/{Name}";
        }
    }
}