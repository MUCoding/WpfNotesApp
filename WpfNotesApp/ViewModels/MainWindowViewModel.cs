using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WpfNotesApp.Classes;
using WpfNotesApp.Views;

namespace WpfNotesApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public static NoteContext NoteContext { get; set; }

        private object? _currentView;
        private NotesSortOption _selectedSortOption;
        private bool _hideReady;
        private Note? _selectedNote;
        private ObservableCollection<Note> _notes;

        public object? CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        
        public NotesSortOption SelectedSortOption
        {
            get { return _selectedSortOption; }
            set
            {
                _selectedSortOption = value;
                OnPropertyChanged(nameof(SelectedSortOption));
                SortNotes();
            }
        }
        
        public bool HideReady
        {
            get { return _hideReady; }
            set
            {
                _hideReady = value;
                OnPropertyChanged(nameof(HideReady));
                if (Notes != null)
                {
                    if (_hideReady)
                    {
                        Notes = new ObservableCollection<Note>(NoteContext.Notes.Where(note => !note.IsReady));
                    }
                    else
                    {
                        Notes = new ObservableCollection<Note>(NoteContext.Notes);
                    }
                }
            }
        }
        
        public Note? SelectedNote
        {
            get => _selectedNote;
            set
            {
                _selectedNote = value;
                OnPropertyChanged(nameof(SelectedNote));
                if (_selectedNote != null)
                {
                    ShowEditViewCommand.Execute(this);
                }
            }
        }
        
        public ObservableCollection<Note> Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        public ICommand ShowAddViewCommand { get; set; }
        public ICommand ShowEditViewCommand { get; set; }
        public ICommand HideCurrentViewCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }


        public MainWindowViewModel()
        {
            ShowAddViewCommand = new RelayCommand(o => ShowAddView(), o => true);
            ShowEditViewCommand = new RelayCommand(o => ShowEditView(), o => SelectedNote != null);
            HideCurrentViewCommand = new RelayCommand(o => HideCurrentView(), o => CurrentView != null);
            DeleteNoteCommand = new RelayCommand(o => DeleteNote(), o => SelectedNote != null);

            NoteContext = new NoteContext();
            ListNotes();

            // Sort by Default automatically when the app starts
            SelectedSortOption = NotesSortOption.Default;
            HideReady = false;
        }

        public void ShowAddView()
        {
            SelectedNote = null;
            CurrentView = new AddView(this);
        }

        public void ShowEditView()
        {
            CurrentView = new EditView(this);
        }

        public void HideCurrentView()
        {
            CurrentView = null;
            SelectedNote = null;
        }

        public void ListNotes()
        {
            Notes = new ObservableCollection<Note>(NoteContext.Notes);
            if (Notes != null)
            {
                SortNotes();
            }
            CurrentView = null;
            SelectedNote = null;
        }

        public void SaveNotes(Note newNote = null)
        {
            if (newNote != null)
            {
                NoteContext.Notes.Add(newNote);
            }
            NoteContext.SaveChanges();
            ListNotes();
        }

        public void DeleteNote()
        {
            if (SelectedNote != null)
            {
                var noteToRemove = NoteContext.Notes.FirstOrDefault(note => note.Id == SelectedNote.Id);
                if (noteToRemove != null)
                {
                    NoteContext.Notes.Remove(SelectedNote);
                    NoteContext.SaveChanges();
                }
                ListNotes();
            }
        }

        private void SortNotes()
        {
            switch (SelectedSortOption)
            {
                case NotesSortOption.Title:
                    Notes = [.. Notes.OrderBy(note => note.Title)];
                    break;
                case NotesSortOption.LastModified:
                    Notes = [.. Notes.OrderBy(note => note.LastModified)];
                    break;
                default:
                    Notes = [.. Notes.OrderBy(note => note.Id)];
                    break;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
