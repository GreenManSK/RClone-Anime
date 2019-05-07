using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace RClone_Anime.Configuiration
{
    public class Anime
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public Collection<AnimeFile> Files { get; set; }
        
        [JsonIgnore]
        public Drive Drive { get; set; }

        public Anime(string name)
        {
            Name = name;
            Files = new Collection<AnimeFile>();
        }

        public void AddFile(AnimeFile file)
        {
            file.Anime = this;
            Files.Add(file);
        }

        public void AfterDeserialization()
        {
            foreach (var file in Files)
            {
                file.Anime = this;
            }
        }
    }
}