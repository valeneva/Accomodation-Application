
// ----------------------------------------------------------------------
// <copyright file="MainModel.cs" company="noName">
//     Copyright (c) noName s. r. o..  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------


//Main View model - propojení modelu s view

namespace noName.WpfApp.ViewModel
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Win32;
    using noName.WpfApp.Commands;
    using noName.WpfApp.LayoutViewModel;
    using noName.WpfApp.Model;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using noName.WpfApp.Database;
    public class MainViewModel : ViewModelBase
    {

        #region Members
        private readonly MainModel _model;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private List<DataModel> _dataModel;

        private Database _database;

        #endregion

        #region WpfProperties

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        public static string WindowTitle => $"noName {Assembly.GetAssembly(typeof(MainViewModel)).GetName().Version}";
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        //Cabins
        public CabinModel? FourChildCabin { get; set; }
        public CabinModel? FiveChildCabin { get; set; }
        public CabinModel? SixChildCabin { get; set; }
        public CabinModel? SevenChildCabin { get; set; }
        public CabinModel? EightChildCabin { get; set; }
        public CabinModel? NineChildCabin { get; set; }
        public CabinModel? TenChildCabin { get; set; }
        public CabinModel? ElevenChildCabin { get; set; }
        public CabinModel? TwelveChildCabin { get; set; }

        private int _result;
        /// <summary>
        /// Pokusná proměnná do GUI
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public int Result
        {
            get => _result;
            set => SetPropertyValue(ref _result, value);

        }

        private double _gridWidth;
        private double _gridHeight;
        public double GridWidth
        {
            get => _gridWidth;
            set
            {
                SetPropertyValue(ref _gridWidth, value);
                _layout.GridWidth = value;
            }

        }
        public double GridHeight
        {
            get => _gridHeight;
            set
            {
                SetPropertyValue(ref _gridHeight, value);
                _layout.GridHeight = value;
            }
        }

        public List<DataModel> DataModel
        {
            get => _dataModel;
            set => SetPropertyValue(ref _dataModel, value);
        }

        // Počet řádků a sloupců
        private int _rowsS;
        public int NumOfRowsS
        {
            get => _rowsS;
            set => SetPropertyValue(ref _rowsS, value);
        }

        private int _columnsS;
        public int NumOfColumnsS
        {
            get => _columnsS;
            set => SetPropertyValue(ref _columnsS, value);
        }

        private int _rowsB;
        public int NumOfRowsB
        {
            get => _rowsB;
            set => SetPropertyValue(ref _rowsB, value);
        }

        private int _columnsB;
        public int NumOfColumnsB
        {
            get => _columnsB;
            set => SetPropertyValue(ref _columnsB, value);
        }

        private Grid _cabinGridS;
        public Grid CabinGridS
        {
            get => _cabinGridS;
            set => SetPropertyValue(ref _cabinGridS, value);
        }


        private Grid _cabinGridB;
        public Grid CabinGridB
        {
            get => _cabinGridB;
            set => SetPropertyValue(ref _cabinGridB, value);
        }

        private bool _isPopupOpen;
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => SetPropertyValue(ref _isPopupOpen, value);
        }

        private string _popupText = String.Empty;
        public string PopupText
        {
            get => _popupText;
            set => SetPropertyValue(ref _popupText, value);
        }

        private List<CabinListKid> _pokSeznam;


        public List<CabinListKid> PokSeznam
        {
            get => _pokSeznam;
            set => SetPropertyValue(ref _pokSeznam, value);
        }

        public string NumOfGirls { get; private set; } = String.Empty;
        public string NumOfBoys { get; private set; } = String.Empty;
        public string AvrgAge { get; private set; } = String.Empty;


        private CabinsLayoutCreater _layout;
        public CabinsLayoutCreater Layout
        {
            get => _layout;
            set => SetPropertyValue(ref _layout, value);
        }


        #endregion

        #region ObservableCollections
        // Kolekce obdélníků
        public ObservableCollection<CabinModel> CabinLayout { get; set; }

        #endregion

        #region ICommandsProperties
        public ICommand CalculateCommand { get; }
        public ICommand ImportDataCommand { get; }

        #endregion

        #region Constructor
        /// <summary>
        /// Inizializace MainModelu
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public MainViewModel(ILoggerFactory loggerFactory, MainModel model)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<MainViewModel>();
            _model = model;


            Layout = new CabinsLayoutCreater();
            _layout = Layout;

            CalculateCommand = new RelayCommand(CalculateAsync);
            ImportDataCommand = new RelayCommand(ImportData);
            Result = 0;

            DataModel = new List<DataModel>();
            _dataModel = DataModel;

            CabinGridS = new Grid();
            _cabinGridS = CabinGridS;

            CabinGridB = new Grid();
            _cabinGridB = CabinGridB;

            CabinLayout = new ObservableCollection<CabinModel>();
            PokSeznam = new List<CabinListKid>();
            _pokSeznam = PokSeznam;

            CreateCabinLayoutGrid();
            AddCabinsToCollection();
            AddCabinsToGrid();

            _database = new Database(_logger);
        }



        #endregion

        #region Methods


        /// <summary>
        /// Vytvoření gridu pro umístění obdelníků dle rozložení chatek
        /// </summary>
        private void CreateCabinLayoutGrid()
        {
            NumOfRowsS = 7;
            NumOfColumnsS = 17;

            NumOfRowsB = 7;
            NumOfColumnsB = 17;

            // vytvoření sloupců gridu pro cabin layout
            for (int i = 0; i < NumOfColumnsS; i++)
            {
                var columnDefinition = new ColumnDefinition();
                //columnDefinition.Width = new GridLength(100);
                columnDefinition.Width = new GridLength();  // Nastavení šířky sloupce
                columnDefinition.Width = GridLength.Auto;
                columnDefinition.MinWidth = 80;
                CabinGridS.HorizontalAlignment = HorizontalAlignment.Stretch;
                CabinGridS.ColumnDefinitions.Add(columnDefinition);
            }

            // Vytvoření řádků gridu pro cabin layout
            for (int i = 0; i < NumOfRowsS; i++)
            {
                var rowDefinition = new RowDefinition();
                //rowDefinition.Height = new GridLength(100);
                rowDefinition.Height = new GridLength();
                rowDefinition.Height = GridLength.Auto; // Nastavení výšky řádku
                rowDefinition.MinHeight = 100;
                CabinGridS.VerticalAlignment = VerticalAlignment.Stretch;
                CabinGridS.RowDefinitions.Add(rowDefinition);
            }
        }

        /// <summary>
        /// Přidání chatek do kolekce na základě počtu chatek s dnaou kapacitou
        /// </summary>
        private void AddCabinsToCollection()
        {

            //počet chatek dané kapacity
            int numOfFourChildCabin = 10;
            int numOfFiveChildCabin = 15;
            int numOfSixChildCabin = 0;
            int numOfSevenChildCabin = 0;
            int numOfEightChildCabin = 3;

            int order = 0;
            int size2 = 50;

            for (int i = 0; i < numOfFourChildCabin; i++)
            {
                order++;

                FourChildCabin = new CabinModel()
                {
                    Capacity = 4,
                    Order = order
                };
                int size = FourChildCabin.Capacity * 12;
                FourChildCabin.InizializeRectangle(new SolidColorBrush(Colors.Blue), size2, size);
                FourChildCabin.CreateText();
                CabinLayout.Add(FourChildCabin);
            }

            for (int i = 0; i < numOfFiveChildCabin; i++)
            {
                order++;

                FiveChildCabin = new CabinModel()
                {
                    Capacity = 5,
                    Order = order
                };
                int size = FiveChildCabin.Capacity * 14;
                FiveChildCabin.InizializeRectangle(new SolidColorBrush(Colors.Red), size2, size);
                FiveChildCabin.CreateText();
                CabinLayout.Add(FiveChildCabin);
            }

            for (int i = 0; i < numOfSixChildCabin; i++)
            {
                order++;

                SixChildCabin = new CabinModel()
                {
                    Capacity = 6,
                    Order = order
                };
                int size = SixChildCabin.Capacity * 12;
                SixChildCabin.InizializeRectangle(new SolidColorBrush(Colors.Brown), size2, size);
                SixChildCabin.CreateText();
                CabinLayout.Add(SixChildCabin);

            }

            for (int i = 0; i < numOfSevenChildCabin; i++)
            {
                order++;

                SevenChildCabin = new CabinModel()
                {
                    Capacity = 7,
                    Order = order
                };
                int size = SevenChildCabin.Capacity * 14;
                SevenChildCabin.InizializeRectangle(new SolidColorBrush(Colors.Pink), size2, size);
                SevenChildCabin.CreateText();
                CabinLayout.Add(SevenChildCabin);

            }


            for (int i = 0; i < numOfEightChildCabin; i++)
            {
                order++;

                EightChildCabin = new CabinModel()
                {
                    Capacity = 8,
                    Order = order
                };
                int size = EightChildCabin.Capacity * 12;
                EightChildCabin.InizializeRectangle(new SolidColorBrush(Colors.Green), size2, size);
                EightChildCabin.CreateText();
                CabinLayout.Add(EightChildCabin);

            }

            foreach (CabinModel cabin in CabinLayout)
            {
                // Přidání obsluhy události
                if (cabin.Rectangle is not null)
                {
                    cabin.Rectangle.MouseDown += PopCabinInfo_MouseDown;

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddCabinsToGrid()
        {
            int sumCapacity = 0;
            TextBlock sumCapacityTextBlock = new TextBlock();

            int i = 0;
            int j = 0;
            int k = 0;
            foreach (CabinModel cabin in CabinLayout)
            {
                sumCapacity += cabin.Capacity;


                if (cabin.Capacity == 4)
                {
                    //Logika umístění chatek na layout
                    i++;
                    cabin.Row = 1;
                    cabin.Column = i + 1;

                    // Nastavení pozice chatky v gridu
                    Grid.SetColumn(cabin.Rectangle, cabin.Column);
                    Grid.SetRow(cabin.Rectangle, cabin.Row);

                    // Přidání chatky do gridu
                    CabinGridS.Children.Add(cabin.Rectangle);

                    Grid.SetColumn(cabin.CabinTextBlock, cabin.Column);
                    Grid.SetRow(cabin.CabinTextBlock, cabin.Row - 1);
                    CabinGridS.Children.Add(cabin.CabinTextBlock);
                }
                else if (cabin.Capacity == 5)
                {
                    //Logika umístění chatek na layout
                    j++;
                    cabin.Row = 5;
                    cabin.Column = j + 1;

                    // Nastavení pozice chatky v gridu
                    Grid.SetColumn(cabin.Rectangle, cabin.Column);
                    Grid.SetRow(cabin.Rectangle, cabin.Row);

                    // Přidání chatky do gridu
                    CabinGridS.Children.Add(cabin.Rectangle);

                    Grid.SetColumn(cabin.CabinTextBlock, cabin.Column);
                    Grid.SetRow(cabin.CabinTextBlock, cabin.Row - 1);
                    CabinGridS.Children.Add(cabin.CabinTextBlock);

                }
                else if (cabin.Capacity == 8)
                {
                    //Logika umístění chatek na layout
                    k++;
                    cabin.Row = k + 1;
                    cabin.Column = 1;

                    // Nastavení pozice chatky v gridu
                    Grid.SetColumn(cabin.Rectangle, cabin.Column);
                    Grid.SetRow(cabin.Rectangle, cabin.Row);

                    // Přidání chatky do gridu
                    CabinGridS.Children.Add(cabin.Rectangle);

                    Grid.SetColumn(cabin.CabinTextBlock, cabin.Column - 1);
                    Grid.SetRow(cabin.CabinTextBlock, cabin.Row);
                    CabinGridS.Children.Add(cabin.CabinTextBlock);
                }
                else
                {
                    throw new NotImplementedException();
                    //nutné doplnit funkce: AddCabinsToGrid,AddCabinsToCollection... popř. CabinModel
                }
            }

            sumCapacityTextBlock.Text = "Celková kapacita: " + sumCapacity.ToString()
                + "\n Počet dívek: " + NumOfGirls + "\n Počet chlapců: " + NumOfBoys + "\n Průměrný věk: " + AvrgAge;

            Grid.SetColumn(sumCapacityTextBlock, NumOfColumnsS / 2);
            Grid.SetRow(sumCapacityTextBlock, NumOfRowsS / 2);
            CabinGridS.Children.Add(sumCapacityTextBlock);
        }

        /// <summary>
        /// Import .csv data do programu
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ImportData(object? obj)
        {
            // Vytvoření instance OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Nastavení filtrů pro CSV a Excel soubory
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";
            openFileDialog.Title = "Vyberte soubor";

            // Otevření dialogového okna a ověření, zda uživatel soubor vybral
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                // Získání cesty k vybranému souboru
                string filePath = openFileDialog.FileName;
                string fileExtension = Path.GetExtension(filePath);

                if (fileExtension == ".csv")
                {
                    DataModel = _model.ReadCsvFile(filePath);
                }
                else if (fileExtension == ".xlsx" || fileExtension == ".xls")
                {
                    DataModel = _model.ReadExcelFile(filePath);
                }

                MessageBox.Show("Vybrali jste soubor: " + filePath);
            }
            else
            {
                MessageBox.Show("Žádný soubor nebyl vybrán.");
            }
        }

        /// <summary>
        /// Pokusná metoda 
        /// </summary>
        private void CalculateAsync(object? parameter)
        {
            Result = 15;
            _ = _model.LogAnimalsAsync();// Testovací hodnoty
        }



        /// <summary>
        /// Vyskakovací okno nad chatkou se jmény
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PopCabinInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border clickedCabin)
            {
                // Najde CabinModel podle obdélníku, na který bylo kliknuto
                var clickedModel = CabinLayout.FirstOrDefault(c => c.Rectangle == clickedCabin);

                if (clickedModel != null)
                {
                    // Aktualizuje text popupu podle pořadí chatky
                    PopupText = "Chatka: " + clickedModel.Order.ToString();

                    PokSeznam = clickedModel.ListOfKids;

                }
            }


            if (!IsPopupOpen)
            {
                // Otevře popup
                IsPopupOpen = true;

            }
            else
            {
                // Zavírá popup, pokud byl již otevřený
                IsPopupOpen = false;
            }

            // Zabraňuje zavření popup okamžitě po otevření
            e.Handled = true;

        }


        #endregion
    }
}
