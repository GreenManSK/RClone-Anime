using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using RClone_Anime.Encrypt;

namespace RClone_Anime.Configuiration
{
    public class Config
    {
        private static Config _instance;
        private const string ConfigFile = "config";

        public string RclonePath { get; set; }
        public string OutputPath { get; set; }
        public ObservableCollection<Drive> Drives { get; set; }

        private Config()
        {
            Drives = new ObservableCollection<Drive>();
        }

        private void AfterDeserialization()
        {
            foreach (var drive in Drives)
            {
                drive.AfterDeserialization();
            }
        }

        public void Save(PasswordStore password)
        {
            using (var sw = new StreamWriter(ConfigFile))
            {
                sw.WriteLine(
                    StringCipher.Encrypt(JsonConvert.SerializeObject(this), password.Get())
                );
            }
        }

        public static Config Get(PasswordStore password)
        {
            return _instance ?? (_instance = Deserialize(password));
        }

        private static Config Deserialize(PasswordStore password)
        {
            if (!File.Exists(ConfigFile))
            {
                return new Config();
            }

            var content = File.ReadAllText(ConfigFile);
            var settings = JsonConvert.DeserializeObject<Config>(StringCipher.Decrypt(content, password.Get()));
            settings.AfterDeserialization();
            return settings;
        }
    }
}