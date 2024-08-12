using System.Windows.Controls;
using WpfNotesApp.ViewModels;

namespace WpfNotesApp.Views
{
    /// <summary>
    /// Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : UserControl
    {
        public EditView(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = new EditViewModel(mainWindowViewModel);
        }
    }
}
