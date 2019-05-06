using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using RClone_Anime.Encrypt;

namespace RClone_Anime.RClone
{
    public class RCloneRunner
    {
        public delegate void CopyStatusUpdate(string status);
        
        private const int RefreshConstant = 1; // Number of seconds between stats about copy are send to output
        
        private readonly PasswordStore _password;
        private readonly string _rclonePath;

        public RCloneRunner(string path, PasswordStore password)
        {
            _rclonePath = path;
            _password = password;
        }

        public Task<string> Ls(string disk, string path)
        {
            return Task.Run(() =>
            {
                var process = CreateProcess(LsArgument(disk, path));
                process.Start();

                process.StandardInput.WriteLine(_password.Get());
                var output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                return output;
            });
        }
        
        public Task Copy(string disk, string path, string dest, CopyStatusUpdate callback)
        {
            return Task.Run(() =>
            {
                var process = CreateProcess(CopyArgument(disk, path, dest));
                process.Start();

                process.StandardInput.WriteLine(_password.Get());

                var sb = new StringBuilder();
                while (!process.StandardError.EndOfStream)
                {
                    var line = process.StandardError.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        callback(sb.ToString());
                        sb = new StringBuilder();
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }

                process.WaitForExit();
            });
        }

        private Process CreateProcess(string arguments)
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _rclonePath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                }
            };
        }

        private static string LsArgument(string disk, string path)
        {
            return $"ls \"{disk}\":\"{path}\"";
        }

        private static string CopyArgument(string disk, string path, string dest)
        {
            return $"copy \"{disk}\":\"{path}\" \"{dest}\" -v --stats {RefreshConstant}s";
        }
    }
}