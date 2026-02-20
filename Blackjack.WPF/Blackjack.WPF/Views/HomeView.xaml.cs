using Blackjack.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Blackjack.WPF.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();

            // Loaded fires every time the view is reloaded.
            // So navigating back and forward between GameView and HomeView,
            // will trigger HomeView_Loaded.
            Loaded += HomeView_Loaded;
        }

        private async void HomeView_Loaded(object sender, RoutedEventArgs e)
        {
            // Prevents errors in Design Mode
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            if (DataContext is HomeViewModel vm)
            {
                await vm.LoadPlayersAsync();
                await vm.InitializeStateAsync();
            }
        }
    }
}
