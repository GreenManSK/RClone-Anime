using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using RClone_Anime.Configuiration;
using RClone_Anime.Encrypt;

namespace RClone_Anime.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private const string RCloneFileFilter = "rclone.exe|rclone.exe";

        private PasswordStore _password;
        private Config _config;

        public SettingsWindow(PasswordStore password, Config config)
        {
            _password = password;
            _config = config;
            InitializeComponent();
            InitVlaues();
        }

        private void InitVlaues()
        {
            RClonePathInput.Text = _config.RclonePath;
            DrivesGrid.ItemsSource = _config.Drives;
        }

        ~SettingsWindow()
        {
            _config.Save(_password);
        }

        private void OnRClonePathInputClick(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog {Filter = RCloneFileFilter};
            if (openFileDialog.ShowDialog() != true) return;
            RClonePathInput.Text = openFileDialog.FileName;
            _config.RclonePath = openFileDialog.FileName;
        }

        private void OnRemoveClick(object sender, RoutedEventArgs e)
        {
            if (DrivesGrid.SelectedItem == null) return;
            var drive = DrivesGrid.SelectedItem as Drive;
            _config.Drives.Remove(drive);
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new DriveWindow(_config);
            window.ShowDialog();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (DrivesGrid.SelectedItem == null) return;
            var drive = DrivesGrid.SelectedItem as Drive;
            var window = new DriveWindow(_config, drive);
            window.ShowDialog();
        }

        private void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            if (DrivesGrid.SelectedItem == null) return;
            var drive = DrivesGrid.SelectedItem as Drive;
            var window = new RefreshWindow(drive, _config, _password);
            window.ShowDialog();
            DrivesGrid.Items.Refresh();
        }
    }
}