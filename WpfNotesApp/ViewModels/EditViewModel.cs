using System.ComponentModel;
using System.Windows.Input;
using WpfNotesApp.Classes;

namespace WpfNotesApp.ViewModels
{
    public class EditViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        private int _id;
        private string _title;
        private string _description;
        private bool _isReady;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

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

        public bool IsReady
        {
            get { return _isReady; }
            set
            {
                _isReady = value;
                OnPropertyChanged(nameof(IsReady));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            var selectedNote = _mainWindowViewModel.SelectedNote;
            if (selectedNote != null)
            {
                Id = selectedNote.Id;
                Title = selectedNote.Title;
                Description = selectedNote.Description;
                IsReady = selectedNote.IsReady;
            }

            SaveCommand = new RelayCommand(o => Save(), o => !string.IsNullOrEmpty(Title));
            CancelCommand = new RelayCommand(o => Cancel(), o => true);
        }

        private void Save()
        {
            if (_mainWindowViewModel.Notes != null)
            {
                var noteToEdit = _mainWindowViewModel.Notes.FirstOrDefault(note => note.Id == Id);
                if (noteToEdit != null)
                {
                    noteToEdit.Title = Title;
                    noteToEdit.Description = Description;
                    noteToEdit.IsReady = IsReady;
                    _mainWindowViewModel.SaveNotes();
                }
            }
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
