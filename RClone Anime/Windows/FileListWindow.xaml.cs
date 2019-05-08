using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RClone_Anime.Configuiration;
using RClone_Anime.Encrypt;

namespace RClone_Anime.Windows
{
    /// <summary>
    /// Interaction logic for FileListWindow.xaml
    /// </summary>
    public partial class FileListWindow : Window
    {
        private Anime _anime;
        private PasswordStore _password;
        private Config _config;
        
        public FileListWindow(Anime anime, PasswordStore password, Config config)
        {
            _anime = anime;
            _password = password;
            _config = config;
            InitializeComponent();
            FileGrid.ItemsSource = anime.Files;
        }

        private void OnDownloadButtonClick(object sender, RoutedEventArgs e)
        {
            if (FileGrid.SelectedItems.Count == 0) return;
            var window = new DownloadWindow(_anime, _password, _config);
            window.Files = new List<AnimeFile>();
            foreach (var item in FileGrid.SelectedItems)
            {
                window.Files.Add(item as AnimeFile);
            }
            window.Show();
            window.StartDownload();
        }
    }
}
