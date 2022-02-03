using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Note.Models;
using Note.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Services;

namespace Note.ViewModel
{
    class TaskViewModel : Observer
    {
        #region properties
        private string _Login;

        public string Login
        {
            get { return _Login; }
            set { _Login = value;
                OnPropertyChanged();
            }
        }

        private string _TaskText;

        public string TaskText
        {
            get { return _TaskText; }
            set { _TaskText = value;
                OnPropertyChanged();
            }
        }

        private int _Index;

        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }


        public ObservableCollectionEX<Item> TaskList { get; set; }

        #endregion

        #region commands
        public RelayCommand CloseTaskWindowCommand
        { get; set; }

        public RelayCommand AddTaskCommand
        { get; set; }

        public RelayCommand CheckTaskCommand
        { get; set; }

        public RelayCommand DeleteAllTaskCommand
        { get; set; }

        public RelayCommand DeleteTaskCommand
        { get; set; }

        public RelayCommand EditTaskCommand
        { get; set; }

        public RelayCommand ApplyEditTaskCommand
        { get; set; }
        #endregion

        public TaskViewModel()
        {
            Login = File.ReadAllText($"{Environment.CurrentDirectory}\\TempLogin.txt");
            string pathToTasks = $"{Environment.CurrentDirectory}\\{Login}\\Tasks.json";
            TaskList = new ObservableCollectionEX<Item>();
            LoadJsonITEMS(pathToTasks);
            CloseTaskWindowCommand = new RelayCommand(o =>
            {
                File.Delete($"{Environment.CurrentDirectory}\\TempLogin.txt");
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "MainWindow")
                    {
                        window.Show();
                    }
                    else if (window.Title == "TaskWindow")
                    {
                        window.Close();
                    }
                }
            });
            AddTaskCommand = new RelayCommand(o =>
            {
                TaskList.Add(new Item() {TaskName = TaskText, Date = DateTime.Now, TaskStatus = "Выполняется..." });
            });
            CheckTaskCommand = new RelayCommand(o =>
            {
                TaskList[Index].TaskStatus = "Завершено!";
            });
            DeleteAllTaskCommand = new RelayCommand(o =>
            {
                TaskList.Clear();
            });
            DeleteTaskCommand = new RelayCommand(o =>
            {
                TaskList.RemoveAt(Index);
            });
            EditTaskCommand = new RelayCommand(o =>
            {
                TaskText = TaskList[Index].TaskName;
            });
            ApplyEditTaskCommand = new RelayCommand(o =>
            {
                TaskList[Index].TaskName = TaskText;
            });
        }
        public void ErrorWindowOpen()
        {
            ErrorWindow errorWindow = new ErrorWindow();
            errorWindow.Owner = Application.Current.MainWindow;
            errorWindow.Show();
        }
        public static bool IsValidJson(string stringValue)
        {
            if (File.Exists(stringValue))
            {
                var value = File.ReadAllText(stringValue);
                value = value.Trim();
                if ((value.StartsWith("{") && value.EndsWith("}")) ||
                    (value.StartsWith("[") && value.EndsWith("]")))
                {
                    try
                    {
                        JToken obj = JToken.Parse(value);
                        return true;
                    }
                    catch (JsonReaderException)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public void LoadJsonITEMS(string path)
        {
            if (IsValidJson(path))
            {
                string json = File.ReadAllText(path);
                TaskList = JsonConvert.DeserializeObject<ObservableCollectionEX<Item>>(json);
            }
            TaskList.JsonPath = path;
        }
    }
}
