using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using RClone_Anime.Configuiration;
using RClone_Anime.Encrypt;
using RClone_Anime.RClone;

namespace RClone_Anime.Windows
{
    /// <summary>
    /// Interaction logic for DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        private Anime _anime;
        private RCloneRunner _runner;
        private string _outputPath;

        private Process _process;
        
        public DownloadWindow(Anime anime, PasswordStore password, Config config)
        {
            _anime = anime;
            _runner = new RCloneRunner(config.RclonePath, password);
            _outputPath = config.OutputPath;
            Closing += OnWindowClosing;
            InitializeComponent();
            StartDownload();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e) 
        {
            Debug.WriteLine("Killing:" + _process);
            _process?.Kill();
        }

        private void StartDownload()
        {
            _runner.Copy(_anime.Drive.DriveName, _anime.GetPath(), _outputPath, out _process, (log) =>
            {
                Application.Current.Dispatcher.Invoke(() => { ProgressBox.Text = log; });
            });
        }
    }
}
