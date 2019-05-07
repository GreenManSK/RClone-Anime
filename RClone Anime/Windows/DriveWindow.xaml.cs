using System.Windows;
using RClone_Anime.Configuiration;

namespace RClone_Anime.Windows
{
    /// <summary>
    /// Interaction logic for DriveWindow.xaml
    /// </summary>
    public partial class DriveWindow : Window
    {
        private Config _config;
        private Drive _drive;

        public DriveWindow(Config config, Drive drive = null)
        {
            _config = config;
            _drive = drive;
            InitializeComponent();
            InitValues();
        }

        private void InitValues()
        {
            if (_drive == null) return;
            NameInput.Text = _drive.DriveName;
            PathInput.Text = _drive.Path;
            WatchedCheckbox.IsChecked = _drive.Watched;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (_drive == null)
            {
                _drive = new Drive();
            }

            _drive.DriveName = NameInput.Text;
            _drive.Path = PathInput.Text;
            _drive.Watched = WatchedCheckbox.IsChecked.HasValue && WatchedCheckbox.IsChecked.Value;

            _config.Drives.Remove(_drive);
            _config.Drives.Add(_drive);
            Close();
        }
    }
}