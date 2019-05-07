using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using RClone_Anime.Configuiration;
using RClone_Anime.Encrypt;

namespace RClone_Anime.Windows
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordStore Password { get; private set; }
        public Config Config { get; private set; }

        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            CheckPassword();
        }

        private void OnKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckPassword();
            }
        }

        private void CheckPassword()
        {
            Password = new PasswordStore(PasswordInput.Password);
            try
            {
                Config = Config.Get(Password);
                Close();
            }
            catch (Exception e)
            {
                Title = "Invalid password";
                SystemSounds.Hand.Play();
            }
        }
    }
}