using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using RClone_Anime.Configuiration;
using RClone_Anime.Encrypt;
using RClone_Anime.RClone;

namespace RClone_Anime.Windows
{
    /// <summary>
    /// Interaction logic for RefreshWindow.xaml
    /// </summary>
    public partial class RefreshWindow : Window
    {
        private Config _config;
        private Drive _drive;
        private PasswordStore _password;

        public RefreshWindow(Drive drive, Config config, PasswordStore password)
        {
            _config = config;
            _drive = drive;
            _password = password;
            InitializeComponent();
            Task.Run(() => RefreshAnime());
        }

        ~RefreshWindow()
        {
            _config.Save(_password);
        }

        private async void RefreshAnime()
        {
            var rclone = new RCloneRunner(_config.RclonePath, _password);
            var result = await rclone.Ls(_drive.DriveName, _drive.Path);
            var files = LsParser.parse(result);

            var animeMap = CreateAnimeMap(files);
            AddAnimeToDrive(animeMap);
            Application.Current.Dispatcher.Invoke(() => { Close(); });
        }

        private Dictionary<string, Anime> CreateAnimeMap(IEnumerable<LsItem> files)
        {
            var result = new Dictionary<string, Anime>();
            foreach (LsItem file in files)
            {
                if (file.dirName == null)
                    continue;

                Anime anime;
                if (result.ContainsKey(file.dirName))
                {
                    anime = result[file.dirName];
                }
                else
                {
                    anime = new Anime(file.dirName);
                    result.Add(file.dirName, anime);
                }

                anime.AddFile(new AnimeFile(file.fileName));
            }

            return result;
        }

        private void AddAnimeToDrive(Dictionary<string, Anime> animeMap)
        {
            _drive.Anime.Clear();
            foreach (var anime in animeMap)
            {
                _drive.AddAnime(anime.Value);
            }
        }
    }
}