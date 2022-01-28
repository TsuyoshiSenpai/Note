using Note.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note.Models
{
    class Item : Observer
    {
        private string _TaskName;

        public string TaskName
        {
            get { return _TaskName; }
            set { _TaskName = value;
                OnPropertyChanged();
            }
        }

        private DateTime _Date;

        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value;
                OnPropertyChanged();
            }
        }

        private string _TaskStatus;

        public string TaskStatus
        {
            get { return _TaskStatus; }
            set { _TaskStatus = value;
                OnPropertyChanged();
            }
        }

    }
}
