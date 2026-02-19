using Blackjack.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Blackjack.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreatePlayerWindow.xaml
    /// </summary>
    public partial class CreatePlayerWindow : Window
    {
        public CreatePlayerWindow()
        {
            InitializeComponent();
            Loaded += CreatePlayerWindow_Loaded;
        }

        private void CreatePlayerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreatePlayerViewModel vm)
            {
                vm.CloseRequested += result =>
                {
                    DialogResult = result;
                    Close();
                };
            }
        }
    }
}
