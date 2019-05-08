using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
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
        public List<AnimeFile> Files { get; set; }

        private readonly Anime _anime;
        private readonly RCloneRunner _runner;
        private readonly string _outputPath;

        private Process _process;

        public DownloadWindow(Anime anime, PasswordStore password, Config config)
        {
            _anime = anime;
            _runner = new RCloneRunner(config.RclonePath, password);
            _outputPath = config.OutputPath;
            Closing += OnWindowClosing;
            InitializeComponent();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (_process != null && !_process.HasExited)
            {
                Debug.WriteLine("Killing:" + _process);
                _process?.Kill();
            }
        }

        public void StartDownload()
        {
            if (_process != null)
                return;
            if (Files == null)
            {
                StartDownloadAll();
            }
            else
            {
                StartDownloadFileList();
            }
        }

        private void StartDownloadFileList()
        {
            Task.Run(async () =>
            {
                foreach (var file in Files)
                {
                    await _runner.Copy(
                        _anime.Drive.DriveName,
                        file.GetPath(),
                        _anime.GetOutputPath(_outputPath),
                        out _process,
                        (log) => { Application.Current.Dispatcher.Invoke(() => { ProgressBox.Text = log; }); });
                }
            });
        }

        private void StartDownloadAll()
        {
            _runner.Copy(_anime.Drive.DriveName, _anime.GetPath(), _anime.GetOutputPath(_outputPath), out _process,
                (log) => { Application.Current.Dispatcher.Invoke(() => { ProgressBox.Text = log; }); });
        }
    }
}