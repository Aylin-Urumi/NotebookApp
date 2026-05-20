using Avalonia.Controls;
using NotebookApp.ViewModels;
 
namespace NotebookApp.Views;
 
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}