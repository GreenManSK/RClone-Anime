using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using RClone_Anime.Anidb;
using RClone_Anime.Configuiration;
using RClone_Anime.Encrypt;
using RClone_Anime.Image;
using RClone_Anime.Windows;

namespace RClone_Anime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PasswordStore _password;
        private Config _config;
        private List<Anime> _anime;

        public MainWindow()
        {
            InitializeComponent();
            GetPassword();
            InitValues();
        }

        private void InitValues()
        {
            if (_config == null)
                return;
            OutputPathInput.Text = _config.GetOutputPath();
            LoadAnimeFromConfig();
        }

        private void LoadAnimeFromConfig(bool reloadAnime = true)
        {
            var showSeen = SeenCheckbox != null && SeenCheckbox.IsChecked.HasValue && SeenCheckbox.IsChecked.Value;
            var showNotSeen = NotSeenCheckbox != null && NotSeenCheckbox.IsChecked.HasValue &&
                              NotSeenCheckbox.IsChecked.Value;
            var nameFilter = FilterInput.Text;
            Task.Run(() =>
            {
                if (reloadAnime)
                {
                    LoadAnime();
                }
                UpdateAnimeGrid(showSeen, showNotSeen, nameFilter);
            });
        }

        private void LoadAnime()
        {
            _anime = new List<Anime>();
            foreach (var drive in _config.Drives)
            {
                _anime.AddRange(drive.Anime);
            }

            _anime.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        }

        private void UpdateAnimeGrid(bool showSeen, bool showNotSeen, string nameFilter)
        {
            if (_anime == null)
                return;
            IEnumerable anime = _anime
                .Where(a => ((a.Drive.Watched && showSeen) || (!a.Drive.Watched && showNotSeen)) 
                            && Regex.IsMatch(a.Name, nameFilter, RegexOptions.IgnoreCase))
                .Select(a => a);

            Application.Current.Dispatcher.Invoke(() => { AnimeGrid.ItemsSource = anime; });
        }

        ~MainWindow()
        {
            _config?.Save(_password);
        }

        private void GetPassword()
        {
            var passwordWindow = new PasswordWindow();
            passwordWindow.ShowDialog();
            if (passwordWindow.Password == null || passwordWindow.Config == null)
            {
                Close();
            }

            _password = passwordWindow.Password;
            _config = passwordWindow.Config;
        }

        private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new SettingsWindow(_password, _config);
            window.ShowDialog();
            LoadAnimeFromConfig();
        }

        private void OnOutputPathInputClick(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;
            openFileDialog.InitialDirectory = _config.GetOutputPath();
            if (openFileDialog.ShowDialog() != CommonFileDialogResult.Ok) return;
            OutputPathInput.Text = openFileDialog.FileName;
            _config.OutputPath = openFileDialog.FileName;
        }

        private void OnImageButtonClick(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                await Task.WhenAll(GoogleImage.AddImages(_anime, true));
                AnimeGrid.Items.Refresh();
            });
        }

        private void SeenCheckboxChanged(object sender, RoutedEventArgs e)
        {
            if (SeenCheckbox == null || NotSeenCheckbox == null)
            {
                return;
            }

            LoadAnimeFromConfig(false);
        }

        private void OnFilterInputChange(object sender, TextChangedEventArgs e)
        {
            LoadAnimeFromConfig(false);
        }

        private void OnAnidbButtonClick(object sender, RoutedEventArgs e)
        {
            if (AnimeGrid.SelectedItem == null) return;
            var anime = AnimeGrid.SelectedItem as Anime;
            Process.Start(AnidbHelper.SearchLink(anime.Name));
        }

        private void OnDownloadButtonClick(object sender, RoutedEventArgs e)
        {
            if (AnimeGrid.SelectedItem == null) return;
            var anime = AnimeGrid.SelectedItem as Anime;
            var window = new DownloadWindow(anime, _password, _config);
            window.Show();
            window.StartDownload();
        }

        private void OnFolderButtonClick(object sender, RoutedEventArgs e)
        {
            if (AnimeGrid.SelectedItem == null) return;
            var anime = AnimeGrid.SelectedItem as Anime;
            var window = new FileListWindow(anime, _password, _config);
            window.ShowDialog();
        }
    }
}