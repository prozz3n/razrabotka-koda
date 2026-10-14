using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Todo.Entities
{
    public class TaskModel : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _category = string.Empty;
        private DateTime _date = DateTime.Now;
        private bool _isDone;

        public Guid Id { get; set; } = Guid.NewGuid();

        public string OwnerLogin { get; set; } = string.Empty;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Status));
            }
        }

        public string Status => IsDone ? "Выполнено" : "В работе";

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
