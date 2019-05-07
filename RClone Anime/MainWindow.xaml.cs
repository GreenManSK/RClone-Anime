﻿using System.Diagnostics;
using RClone_Anime.Windows;
using System.Windows;
using RClone_Anime.Configuiration;
using RClone_Anime.Encrypt;

namespace RClone_Anime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PasswordStore _password;
        private Config _config;
        
        public MainWindow()
        {
            InitializeComponent();
            GetPassword();
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
        }
    }
}