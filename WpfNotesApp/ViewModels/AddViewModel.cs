using System.ComponentModel;
using System.Windows.Input;
using WpfNotesApp.Classes;

namespace WpfNotesApp.ViewModels
{
    public class AddViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        private string _title;
        private string _description;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;

            SaveCommand = new RelayCommand(o => Save(), o => !string.IsNullOrEmpty(Title));
            CancelCommand = new RelayCommand(o => Cancel(), o => true);
        }

        private void Save()
        {
            var newNote = new Note
            {
                Title = Title,
                Description = Description,
                IsReady = false,
            };

            _mainWindowViewModel.SaveNotes(newNote);
        }

        private void Cancel()
        {
            _mainWindowViewModel.HideCurrentViewCommand.Execute(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
