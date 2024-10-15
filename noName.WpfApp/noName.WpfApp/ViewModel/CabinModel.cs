// ----------------------------------------------------------------------
// <copyright file="RectangleModel" company="dataPartner">
//     Copyright (c) dataPartner s. r. o.  All rights reserved.
// </copyright>
// <author>
//     eva.valentova
// </author>
// ----------------------------------------------------------------------




namespace noName.WpfApp.ViewModel
{
    using noName.WpfApp.Model;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class CabinModel : ViewModelBase
    {

        #region Members
        /// <summary>
        /// 
        /// </summary>

        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public int Capacity { get; set; }
        public int Occupancy { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int Order { get; set; }
        public float AgeAvarage { get; set; }
        public int Odd { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Border? Rectangle { get; set; }
        public TextBlock? CabinTextBlock { get; set; }
        public List<CabinListKid> ListOfKids { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public CabinModel()
        {
            ListOfKids = new List<CabinListKid>();

        }

        #endregion

        #region Methods

        private void CreateList()
        {
            for (int i = 0; i < 6; i++)
            {
                var list = new CabinListKid();
                list.Name = "Eva" + Order;
                list.Surname = "Jahodová" + Capacity;
                list.Age = 11 + i;

                ListOfKids.Add(list);
            }
        }
        /// <summary>
        /// Vytvoření textu k jednotlivým chatkám
        /// </summary>
        public void CreateText()
        {
            // Vytvoření TextBlocku pro zobrazení textu uvnitř Border
            CabinTextBlock = new TextBlock
            {
                Text = "č." + Order.ToString() + "\n Kap." + Capacity.ToString() + "\n Obsazenost " + Occupancy.ToString()
                + "\nPohl. " + Gender + "\n Prům.věk " + AgeAvarage + "\n Oddíl " + Odd,
                Foreground = new SolidColorBrush(Colors.Black), // Nastavení barvy textu
                FontSize = 12, // Nastavení velikosti textu
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
            };
        }

        /// <summary>
        /// vytvoření jednotlivých chatek do vizualizace s postelemi
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InizializeRectangle(SolidColorBrush color, int width, int height)
        {

            int bedWidth = 15;
            int bedHeight = 20;

            // Vytvoření kontejneru Grid pro postelé v chatkách
            Grid grid = new Grid();

            for (int i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                {
                    var columnDefinition = new ColumnDefinition();
                    //columnDefinition.Width = new GridLength(100);
                    columnDefinition.Width = new GridLength();  // Nastavení šířky sloupce
                    columnDefinition.Width = GridLength.Auto;
                    columnDefinition.MinWidth = 1;
                    grid.HorizontalAlignment = HorizontalAlignment.Center;
                    grid.VerticalAlignment = VerticalAlignment.Center;
                    grid.ColumnDefinitions.Add(columnDefinition);
                }
                else
                {
                    var columnDefinition = new ColumnDefinition();
                    //columnDefinition.Width = new GridLength(100);
                    columnDefinition.Width = new GridLength();  // Nastavení šířky sloupce
                    columnDefinition.Width = GridLength.Auto;
                    columnDefinition.MinWidth = 1;
                    grid.HorizontalAlignment = HorizontalAlignment.Center;
                    grid.VerticalAlignment = VerticalAlignment.Center;
                    grid.ColumnDefinitions.Add(columnDefinition);
                }

            }


            if (Capacity % 2 != 0) //pokud je Capacity liché
            {
                // Vytvoření row mřížky pro chatky s lichou kapacitou
                for (int i = 0; i < Capacity + 2; i++)
                {
                    var rowDefinition = new RowDefinition();
                    //rowDefinition.Height = new GridLength(100);
                    rowDefinition.Height = new GridLength();
                    rowDefinition.Height = GridLength.Auto; // Nastavení výšky řádku
                    rowDefinition.MinHeight = 2;
                    grid.HorizontalAlignment = HorizontalAlignment.Center;
                    grid.VerticalAlignment = VerticalAlignment.Center;
                    grid.RowDefinitions.Add(rowDefinition);
                }
            }
            else
            {

                // Vytvoření rows mřížky pro chatky s sudou kapacitou
                for (int i = 0; i < Capacity + 1; i++)
                {
                    var rowDefinition = new RowDefinition();
                    //rowDefinition.Height = new GridLength(100);
                    rowDefinition.Height = new GridLength();
                    rowDefinition.Height = GridLength.Auto; // Nastavení výšky řádku
                    rowDefinition.MinHeight = 2;
                    grid.HorizontalAlignment = HorizontalAlignment.Center;
                    grid.VerticalAlignment = VerticalAlignment.Center;
                    grid.RowDefinitions.Add(rowDefinition);
                }
            }

            //if (Capacity % 2 != 0) //pokud je Capacity liché
            //{
            //    // Vytvoření row mřížky pro chatky s lichou kapacitou
            //    for (int i = 0; i < Capacity + 2; i++)
            //    {
            //        double freeSpace = ((double)height - (double)bedHeight * ((double)height - 1) / 2.0 - bedWidth) / ((int)height / 2 + 2);

            //        var rowDefinition = new RowDefinition();

            //        if (i % 2 == 0)
            //        {
            //            rowDefinition.Height = new GridLength(freeSpace);
            //        }
            //        else if (i % 2 != 0 && i != (Capacity))
            //        {
            //            rowDefinition.Height = new GridLength(bedHeight);
            //        }
            //        else
            //        {
            //            rowDefinition.Height = new GridLength(bedWidth);
            //        }
            //        grid.RowDefinitions.Add(rowDefinition);

            //    }
            //}
            //else
            //{

            //    // Vytvoření rows mřížky pro chatky s sudou kapacitou
            //    for (int i = 0; i < Capacity + 1; i++)
            //    {
            //        double freeSpace = ((double)Capacity * 12.0 - (double)bedHeight * (double)Capacity / 2.0) / ((double)Capacity / 2.0 + 1);

            //        var rowDefinition = new RowDefinition();

            //        if (i % 2 == 0)
            //        {
            //            rowDefinition.Height = new GridLength(freeSpace);
            //        }
            //        else
            //        {
            //            rowDefinition.Height = new GridLength(bedHeight);
            //        }
            //        grid.RowDefinitions.Add(rowDefinition);
            //    }

            //}
            //// Vytvoření column mřížky
            //for (int i = 0; i < 4; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        double freeSpace = (50.0 - (double)bedWidth * 2.0) / 3.0;

            //        var columnDefinition = new ColumnDefinition();
            //        columnDefinition.Width = new GridLength(freeSpace);
            //        grid.ColumnDefinitions.Add(columnDefinition);
            //    }
            //    else
            //    {
            //        var columnDefinition = new ColumnDefinition();
            //        columnDefinition.Width = new GridLength(bedWidth);
            //        grid.ColumnDefinitions.Add(columnDefinition);
            //    }

            //}


            // Vytvoření a přidání postelý do chatek pro vizualizaci
            for (int i = 0; i < grid.RowDefinitions.Count / 2; i++)
            {
                int row = 2 * i + 1;

                if (Capacity % 2 == 0)
                {
                    for (int j = 0; j < grid.ColumnDefinitions.Count / 2; j++)
                    {
                        int col = 2 * j + 1;
                        Border childRectangle = new Border
                        {
                            BorderThickness = new Thickness(1),
                            BorderBrush = new SolidColorBrush(Colors.Black),
                            Background = new SolidColorBrush(Colors.LightGray),
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            Width = bedWidth,
                            Height = bedHeight,
                        };


                        // Nastavení pozice obdélníku v mřížce
                        Grid.SetRow(childRectangle, row);
                        Grid.SetColumn(childRectangle, col);

                        // Přidání obdélníku do mřížky
                        grid.Children.Add(childRectangle);
                    }
                }
                else if (Capacity % 2 != 0)
                {
                    int col = 0;
                    for (int j = 0; j < grid.ColumnDefinitions.Count / 2; j++)
                    {
                        col = 2 * j + 1;


                        if (row != Capacity)
                        {
                            Border childRectangle = new Border
                            {
                                BorderThickness = new Thickness(1),
                                BorderBrush = new SolidColorBrush(Colors.Black),
                                Background = new SolidColorBrush(Colors.LightGray),
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Width = bedWidth,
                                Height = bedHeight,
                            };

                            // Nastavení pozice obdélníku v mřížce
                            Grid.SetRow(childRectangle, row);
                            Grid.SetColumn(childRectangle, col);

                            // Přidání obdélníku do mřížky
                            grid.Children.Add(childRectangle);
                        }

                    }

                    col = 1;
                    if (row == Capacity)
                    {
                        Border childRectangle = new Border
                        {
                            BorderThickness = new Thickness(1),
                            BorderBrush = new SolidColorBrush(Colors.Black),
                            Background = new SolidColorBrush(Colors.LightGray),
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            Width = bedHeight,
                            Height = bedWidth,
                        };
                        // Nastavení pozice obdélníku v mřížce
                        Grid.SetRow(childRectangle, row);
                        Grid.SetColumn(childRectangle, col);
                        Grid.SetColumnSpan(childRectangle, 3);
                        // Přidání obdélníku do mřížky
                        grid.Children.Add(childRectangle);

                    }
                }
            }

            // Nastavení Border a vložení TextBlocku jako Child element
            Rectangle = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.Black),
                Width = width,
                Height = height,
                Background = color,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Child = grid, // Přidání TextBlocku do Border

            };
            CreateList();
        }




        #endregion
    }
}
