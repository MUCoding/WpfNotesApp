using System.Windows;
using System.Windows.Controls;
using WpfNotesApp.Classes;

namespace WpfNotesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static NoteContext NoteContext { get; set; }
        public static List<Note>? Notes { get; set; }
        public static Note? SelectedNote { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            NoteContext = new NoteContext();
            ListNotes();
        }

        public void ListNotes()
        {
            RefreshNotesFromDatabase();
            if (Notes != null)
            {
                if (SortByTitleRadioButton.IsChecked == true)
                {
                    // Automatically sort by title
                    Notes = Notes.OrderBy(note => note.Title).ToList();
                }
                else if (SortByLastModifiedRadioButton.IsChecked == true)
                {
                    // Automatically sort by LastModified
                    Notes = Notes.OrderByDescending(note => note.LastModified).ToList();
                }
                NotesListBox.ItemsSource = HideReadyCheckBox.IsChecked == true ? Notes.Where(note => !note.IsReady).ToList() : Notes;
            }        
            NotesListBox.SelectedItem = null;
            SelectedNote = null;
            HideEditingView();
        }

        private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (listBox.SelectedItem != null)
            {
                SelectedNote = listBox.SelectedItem as Note;
                ShowEditingView();
                DeleteButton.IsEnabled = true;
            }
        }

        #region Sorting
        private void SortByDefaultRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Notes != null)
            {
                NotesListBox.ItemsSource = Notes;
            }
        }

        private void SortByTitleRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Notes != null)
            {
                var sortedNotes = Notes.OrderBy(note => note.Title).ToList();
                NotesListBox.ItemsSource = sortedNotes;
            }
        }

        private void SortByLastModifiedRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Notes != null)
            {
                var sortedNotes = Notes.OrderByDescending(note => note.LastModified).ToList();
                NotesListBox.ItemsSource = sortedNotes;
            }
        }
        #endregion

        private void HideReadyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var hideReadyCheckBox = (CheckBox)sender;
            if (hideReadyCheckBox.IsChecked == true)
            {
                if (Notes != null)
                {
                    NotesListBox.ItemsSource = Notes.Where(note => !note.IsReady).ToList();
                    if (SelectedNote != null && SelectedNote.IsReady)
                    {
                        // Clear selected note and hide editing view
                        SelectedNote = null;
                        HideEditingView();
                    }
                }
            }
            else
            {
                NotesListBox.ItemsSource = Notes;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear SelectedNote
            SelectedNote = null;
            NotesListBox.SelectedItem = null;
            ShowEditingView();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNote != null)
            {
                var noteToRemove = NoteContext.Notes.FirstOrDefault(n => n.Id == SelectedNote.Id);
                if (noteToRemove != null)
                {
                    NoteContext.Remove(noteToRemove);
                    NoteContext.SaveChanges();
                }
            }
            ListNotes();
        }

        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var titleTextBox = (TextBox)sender;
            if (titleTextBox != null)
            {
                var text = titleTextBox.Text;
                SaveButton.IsEnabled = !string.IsNullOrEmpty(text);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNote != null)
            {
                // Edit existing Note
                var noteToEdit = NoteContext.Notes.FirstOrDefault(n => n.Id == SelectedNote.Id);
                if (noteToEdit != null)
                {
                    noteToEdit.Title = TitleTextBox.Text;
                    noteToEdit.Description = DescriptionTextBox.Text;
                    noteToEdit.IsReady = IsReadyCheckBox.IsChecked.HasValue && IsReadyCheckBox.IsChecked.Value;
                    noteToEdit.LastModified = DateTime.Now;
                    NoteContext.Notes.Update(noteToEdit);
                }
            }
            else
            {
                // Save into database
                var newNote = new Note
                {
                    Title = TitleTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    IsReady = IsReadyCheckBox.IsChecked.HasValue && IsReadyCheckBox.IsChecked.Value,
                    LastModified = DateTime.Now,
                };
                NoteContext.Notes.Add(newNote);
            }

            NoteContext.SaveChanges();
            ListNotes();
        }

        private static void RefreshNotesFromDatabase()
        {
            Notes = [.. NoteContext.Notes];
        }

        private void ShowEditingView()
        {
            if (SelectedNote != null)
            {
                // Fill in data
                TitleTextBox.Text = SelectedNote.Title;
                DescriptionTextBox.Text = SelectedNote.Description;
                IsReadyCheckBox.IsChecked = SelectedNote.IsReady;
            }
            else
            {
                // Clear data
                TitleTextBox.Text = "";
                DescriptionTextBox.Text = "";
                IsReadyCheckBox.IsChecked = false;
            }
            TitleTextBlock.Visibility = Visibility.Visible;
            TitleTextBox.Visibility = Visibility.Visible;
            DescriptionTextBlock.Visibility = Visibility.Visible;
            DescriptionTextBox.Visibility = Visibility.Visible;
            IsReadyCheckBox.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Visible;
            SaveButton.IsEnabled = !string.IsNullOrEmpty(TitleTextBox.Text);
        }

        private void HideEditingView()
        {
            // Clear data
            TitleTextBox.Text = "";
            DescriptionTextBox.Text = "";
            IsReadyCheckBox.IsChecked = false;

            // Hide elements
            TitleTextBlock.Visibility = Visibility.Hidden;
            TitleTextBox.Visibility = Visibility.Hidden;
            DescriptionTextBlock.Visibility = Visibility.Hidden;
            DescriptionTextBox.Visibility = Visibility.Hidden;
            IsReadyCheckBox.Visibility = Visibility.Hidden;
            SaveButton.Visibility = Visibility.Hidden;
            SaveButton.IsEnabled = false;
        }
    }
}