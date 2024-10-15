// ----------------------------------------------------------------------
// <copyright file="MainModel.cs" company="noName">
//     Copyright (c) noName s. r. o..  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------




namespace noName.WpfApp
{
    using noName.WpfApp.ViewModel;
    using System.Runtime.InteropServices;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Importování funkce AllocConsole z knihovny kernel32.dll
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            // Přidání konzole do WPF - pouze in debug mode
            AllocConsole();
#endif

        }

        #region Methods
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (viewModel != null)
            {
                viewModel.GridWidth = e.NewSize.Width;
                viewModel.GridHeight = e.NewSize.Height;
            }
        }
        #endregion
    }
}