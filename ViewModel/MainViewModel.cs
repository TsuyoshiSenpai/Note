using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Note.Models;
using Note.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Services;

namespace Note.ViewModel
{
    class MainViewModel : Observer
    {
        #region properties
        private string _Login = String.Empty;

        public string Login
        {
            get { return _Login; }
            set { _Login = value;
                OnPropertyChanged();
            }
        }
        private string _Password = String.Empty;

        public string Password
        {
            get { return _Password; }
            set { _Password = value;
                OnPropertyChanged();
            }
        }

        private string _RepeatPassword = String.Empty;

        public string RepeatPassword
        {
            get { return _RepeatPassword; }
            set { _RepeatPassword = value;
                OnPropertyChanged();
            }
        }

        private string _RepeatLogin = String.Empty;

        public string RepeatLogin
        {
            get { return _RepeatLogin; }
            set { _RepeatLogin = value;
                OnPropertyChanged();
            }
        }

        #endregion
        /*public List<Item> ItemList { get; set; }*/
        #region commands
        public RelayCommand CloseMainWindowCommand
        { get; set; }

        public RelayCommand CloseRegisterWindowCommand
        { get; set; }

        public RelayCommand OpenRegisterWindowCommand
        { get; set; }

        public RelayCommand RegisterCommand
        { get; set; }

        public RelayCommand CloseErrorWindowCommand
        { get; set; }

        public RelayCommand CloseRegisterCompleteCommand
        { get; set; }

        public RelayCommand LoginCommand
        { get; set; }

        #endregion
        public MainViewModel()
        {
            /*ItemList = new List<Item>();*/
            CloseMainWindowCommand = new RelayCommand(o =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "MainWindow")
                    {
                        window.Close();
                    }
                }
            });
            CloseRegisterWindowCommand = new RelayCommand(o =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "MainWindow")
                    {
                        window.Show();
                    }
                    else if (window.Title == "RegisterWindow")
                    {
                        window.Close();
                    }
                    else if (window.Title == "RegisterComplete")
                    {
                        window.Close();
                    }
                }
            });
            OpenRegisterWindowCommand = new RelayCommand(o =>
            {
                RegisterWindow registerWindow = new RegisterWindow();
                registerWindow.Owner = Application.Current.MainWindow;
                registerWindow.Show();
                Application.Current.MainWindow.Hide();
            });
            CloseErrorWindowCommand = new RelayCommand(o =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "ErrorWindow")
                    {
                        window.Close();
                    }
                }
            });
            CloseRegisterCompleteCommand = new RelayCommand(o =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "RegisterComplete")
                    {
                        window.Close();
                    }
                }
            });
            RegisterCommand = new RelayCommand(o =>
            {
                string FileDirectPath = $"{Environment.CurrentDirectory}\\{Login}";
                if (Password == String.Empty || Login == String.Empty || Directory.Exists(FileDirectPath) || Login.Contains(" ") || RepeatLogin != Login || RepeatPassword != Password)
                {
                    ErrorWindowOpen();
                }
                else
                {
                    Directory.CreateDirectory(FileDirectPath);
                    File.Create(FileDirectPath + "\\Login.txt").Close();
                    File.Create(FileDirectPath + "\\Password.txt").Close();
                    File.WriteAllText(FileDirectPath + "\\Login.txt", Login);
                    File.WriteAllText(FileDirectPath + "\\Password.txt", Password);
                    RegisterComplete registerWindow = new RegisterComplete();
                    registerWindow.Owner = Application.Current.MainWindow;
                    registerWindow.Show();
                    Login = String.Empty;
                    Password = String.Empty;
                    RepeatLogin = String.Empty;
                    RepeatPassword = String.Empty;
                }
            });
            LoginCommand = new RelayCommand(o =>
            {
                string FileDirectPath = $"{Environment.CurrentDirectory}\\{Login}";
                if (!Directory.Exists(FileDirectPath) || Login != File.ReadAllText(FileDirectPath + "\\Login.txt") || Password != File.ReadAllText(FileDirectPath + "\\Password.txt"))
                {
                    ErrorWindowOpen();
                }
                else
                {
                    File.Create($"{Environment.CurrentDirectory}\\TempLogin.txt").Close();
                    File.WriteAllText($"{Environment.CurrentDirectory}\\TempLogin.txt", Login);
                    TaskWindow taskWindow = new TaskWindow();
                    taskWindow.Owner = Application.Current.MainWindow;
                    taskWindow.Show();
                    Application.Current.MainWindow.Hide();
                }
            });
        }

        public void ErrorWindowOpen()
        {
            ErrorWindow errorWindow = new ErrorWindow();
            errorWindow.Owner = Application.Current.MainWindow;
            errorWindow.Show();
        }

    }
}
