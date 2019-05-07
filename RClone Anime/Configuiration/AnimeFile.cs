using Newtonsoft.Json;

namespace RClone_Anime.Configuiration
{
    public class AnimeFile
    {
        public string Name { get; set; }
        
        [JsonIgnore]
        public Anime Anime { get; set; }

        public AnimeFile(string name)
        {
            Name = name;
        }
    }
}