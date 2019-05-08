using System.Collections.ObjectModel;
using Newtonsoft.Json;
using RClone_Anime.Image;

namespace RClone_Anime.Configuiration
{
    public class Anime
    {
        public string Name { get; set; }
        public Collection<AnimeFile> Files { get; set; }
        public string Image { get; set; }

        [JsonIgnore] public Drive Drive { get; set; }

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

        public void RefreshImage()
        {
            Image = GoogleImage.getFirst($"anime {Name}");
        }

        public string GetPath()
        {
            return $"{Drive.Path}/{Name}";
        }

        public string GetOutputPath(string outputDir)
        {
            return $"{outputDir}/{Name}";
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