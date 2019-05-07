using System.Collections.ObjectModel;

namespace RClone_Anime.Configuiration
{
    public class Drive
    {
        public string DriveName { get; set; }
        public string Path { get; set; }
        public bool Watched { get; set; }
        public Collection<Anime> Anime { get; set; }

        public Drive()
        {
            Anime = new Collection<Anime>();
        }

        public Drive(string driveName, string path, bool watched): this()
        {
            DriveName = driveName;
            Path = path;
            Watched = watched;
        }

        public void AddAnime(Anime anime)
        {
            anime.Drive = this;
            Anime.Add(anime);
        }

        public void AfterDeserialization()
        {
            foreach (var anime in Anime)
            {
                anime.Drive = this;
            }
        }
    }
}