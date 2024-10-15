// ----------------------------------------------------------------------
// <copyright file="CabinsLayoutCreater" company="dataPartner">
//     Copyright (c) dataPartner s. r. o.  All rights reserved.
// </copyright>
// <author>
//     eva.valentova
// </author>
// ----------------------------------------------------------------------



namespace noName.WpfApp.LayoutViewModel
{
    using Emgu.CV;
    using Emgu.CV.Structure;
    using noName.WpfApp.Model;
    using noName.WpfApp.PopUpWindow;
    using noName.WpfApp.ViewModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes; // Pro Rectangle ve WPF

    public class CabinsLayoutCreater : CabinsCreater
    {


        #region Members

        private List<Cabin>? _cabins;
        private Image<Gray, byte>? _image;
        #endregion

        #region Properties
        private Canvas? _layoutCanvas;

        private double _gridWidth;
        private double _gridHeight;
        public double GridWidth
        {
            get => _gridWidth;
            set
            {
                SetPropertyValue(ref _gridWidth, value);
                if (_cabins is not null && _image is not null)
                {
                    CreateLayout(_cabins, _image);
                }
            }
        }
        public double GridHeight
        {
            get => _gridHeight;
            set
            {
                SetPropertyValue(ref _gridHeight, value);
                if (_cabins is not null && _image is not null)
                {
                    CreateLayout(_cabins, _image);
                }
            }
        }
        /// <summary>
        /// properta s layoutem chatek
        /// </summary>
        public Canvas? LayoutCanvas
        {
            get => _layoutCanvas;
            set => SetPropertyValue(ref _layoutCanvas, value);
        }

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public CabinsLayoutCreater() : base()
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// Vytvoří canvas pro gui s chatkami na základě souřadnic z .png
        /// </summary>
        /// <param name="cabins"></param>
        protected override void CreateLayout(List<Cabin> cabins, Image<Gray, byte> image)
        {
            //
            _cabins = cabins;

            //získání velikosti .png
            _image = image;
            double picWidth = image.Width;
            double picHeight = image.Height;



            LayoutCanvas = new Canvas();
            LayoutCanvas.VerticalAlignment = VerticalAlignment.Top;
            LayoutCanvas.HorizontalAlignment = HorizontalAlignment.Left;

            // Zobrazení chatek ve WPF Canvas
            foreach (var cabin in _cabins)
            {
                double width = (double)cabin.BoundingBox.Width / picWidth * GridWidth;
                double height = (double)cabin.BoundingBox.Height / picHeight * GridHeight;
                double posX = (double)cabin.BoundingBox.Location.X / picWidth * GridWidth;
                double posY = (double)cabin.BoundingBox.Location.Y / picHeight * GridHeight;

                if (width < picWidth - 10 && height < picHeight - 10)
                {

                    Rectangle rect = new Rectangle
                    {
                        Width = width, // Příklad procenta, upravte dle potřeby
                        Height = height, // Příklad procenta, upravte dle potřeby
                        Stroke = Brushes.Black,
                        Fill = Brushes.Transparent, // Aby byl obdélník viditelný při kliknutí
                        Tag = cabin // Uložení odkazu na chatku do Tagu
                    };

                    // Vytvoření šipky prpo popUpwindow
                    var smallRectWidth = 15;
                    var smallRectHeight = 15;
                    Rectangle smallRect = new Rectangle
                    {
                        Width = smallRectWidth,
                        Height = smallRectHeight,
                        Fill = Brushes.Gray, // Barva šipky
                        Stroke = Brushes.Red,
                        Tag = cabin // Uložení odkazu na chatku do Tagu
                    };

                    // Vytvoření šipky prpo popUpwindow
                    TextBlock ikonI = new TextBlock
                    {
                        Text = "i",
                        FontSize = 15,
                        TextAlignment = TextAlignment.Center,
                        Width = smallRectWidth,
                        Height = smallRectHeight,
                        Tag = cabin // Uložení odkazu na chatku do Tagu
                    };
                    // Přidání obsluhy události kliknutí na obdélník
                    rect.MouseDown += Rect_MouseDown;
                    // Přidání obsluhy události kliknutí na šipku
                    smallRect.MouseDown += Rect_MouseDown;
                    ikonI.MouseDown += Rect_MouseDown;

                    cabin.Rectangle = rect;

                    Canvas.SetLeft(rect, posX); // Opět upravte dle potřeby
                    Canvas.SetTop(rect, posY); // Opět upravte dle potřeby

                    Canvas.SetLeft(smallRect, posX + width - smallRectWidth); // Umístění šipky vpravo
                    Canvas.SetTop(smallRect, posY + height - smallRectHeight); // Umístění šipky dolů

                    Canvas.SetLeft(ikonI, posX + width - smallRectWidth); // Umístění šipky vpravo
                    Canvas.SetTop(ikonI, posY + height- smallRectHeight); // Umístění šipky dolů

                    LayoutCanvas.Children.Add(rect);
                    LayoutCanvas.Children.Add(smallRect);
                    LayoutCanvas.Children.Add(ikonI);
                }
            }

        }

        //private void Arrow_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (sender is Polygon arrow && arrow.Tag is Cabin cabin)
        //    {
        //        // Otevření nebo zavření okna s detaily
        //        var detailsWindow = Application.Current.Windows
        //            .OfType<CabinDetailsWindow>()
        //            .FirstOrDefault(w => (w.DataContext as CabinDetailsViewModel)?.Cabin == cabin);

        //        if (detailsWindow == null)
        //        {
        //            // Otevři nové okno, pokud neexistuje
        //            var cabinDetailsViewModel = new CabinDetailsViewModel { Cabin = cabin };
        //            detailsWindow = new CabinDetailsWindow(cabinDetailsViewModel);
        //            detailsWindow.Owner = Application.Current.MainWindow; // Nastav vlastníka
        //            detailsWindow.Show();
        //        }
        //        else
        //        {
        //            // Zavři okno, pokud existuje
        //            detailsWindow.Close();
        //        }
        //    }
        //}

        private void Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rect && rect.Tag is Cabin cabin)
            {
                // Otevření nebo zavření okna s detaily
                var detailsWindow = Application.Current.Windows
                    .OfType<CabinDetailsWindow>()
                    .FirstOrDefault(w => (w.DataContext as CabinDetailsViewModel)?.Cabin == cabin);

                if (detailsWindow == null)
                {
                    // Otevři nové okno, pokud neexistuje
                    var cabinDetailsViewModel = new CabinDetailsViewModel { Cabin = cabin };
                    detailsWindow = new CabinDetailsWindow(cabinDetailsViewModel);
                    detailsWindow.Owner = Application.Current.MainWindow; // Nastav vlastníka

                    // Získání pozice obdélníku na obrazovce
                    var transform = rect.TransformToAncestor(Application.Current.MainWindow);
                    var rectPosition = transform.Transform(new Point(0, 0));
                    var rectBottomRight = new Point(rectPosition.X + rect.ActualWidth, rectPosition.Y + rect.ActualHeight);
                    var screenPosition = Application.Current.MainWindow.PointToScreen(rectBottomRight);

                    // Nastavení pozice okna s detaily
                    detailsWindow.Left = screenPosition.X;
                    detailsWindow.Top = screenPosition.Y;

                    // Zobrazení okna
                    detailsWindow.Show();

                    // Připojení události pro pohyb hlavního okna
                    Application.Current.MainWindow.LocationChanged += (s, args) =>
                    {
                        // Vypočti novou pozici detaily okna relativně k hlavnímu oknu
                        var newScreenPosition = Application.Current.MainWindow.PointToScreen(rectBottomRight);
                        detailsWindow.Left = newScreenPosition.X;
                        detailsWindow.Top = newScreenPosition.Y;
                    };
                }
                else
                {
                    // Zavři okno, pokud existuje
                    detailsWindow.Close();
                }
            }
        }

        #endregion
    }
}
