using System.Windows.Controls;
using WpfNotesApp.ViewModels;

namespace WpfNotesApp.Views
{
    /// <summary>
    /// Interaction logic for AddView.xaml
    /// </summary>
    public partial class AddView : UserControl
    {
        public AddView(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = new AddViewModel(mainWindowViewModel);
        }
    }
}
