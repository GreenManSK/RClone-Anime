using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
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

        private void LoadAnimeFromConfig()
        {
            Task.Run(() =>
            {
                LoadAnime();
                UpdateAnimeGrid();
            });
        }

        private void LoadAnime()
        {
            _anime = new List<Anime>();
            foreach (var drive in _config.Drives)
            {
                _anime.AddRange(drive.Anime);
            }

            _anime.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.Ordinal));
        }

        private void UpdateAnimeGrid()
        {
            Application.Current.Dispatcher.Invoke(() => { AnimeGrid.ItemsSource = _anime; });
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
    }
}