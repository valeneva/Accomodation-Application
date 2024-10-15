// ----------------------------------------------------------------------
// <copyright file="LayoutPictureExtractor" company="dataPartner">
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
    using Emgu.CV.CvEnum;
    using System.Drawing;
    using Emgu.CV.Util;
    using noName.WpfApp.ViewModel;

    public abstract class LayoutPictureExtractor : ViewModelBase
    {

        #region Members
        
        private Image<Bgr, byte>? _layoutImage;
        private List<Rectangle> _cabinsBoundingBox;
        #endregion

        #region Properties

        #endregion

        #region Constructor
        /// <summary>
        /// Konstructor- práce s .png layoutu chatek
        /// </summary>
        public LayoutPictureExtractor()
        {
            _cabinsBoundingBox = new List<Rectangle>();
            LoadLayoutPicPNG();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Load obrázku layout v .png, upravení pro další praci
        /// </summary>
        public void LoadLayoutPicPNG()
        {
            //cesta k .png
            var path = "C:/Users/eva.valentova/Desktop/noName/layout.png";

            // Načtení obrázku
            _layoutImage = new Image<Bgr, byte>(path);

            // Změna velikosti obrázku na 200x200 pixelů
            //_layoutImage = _layoutImage.Resize(200, 200, Emgu.CV.CvEnum.Inter.Linear);


            // Převod na šedý obraz
            Image<Gray, byte> grayImage = _layoutImage.Convert<Gray, byte>();

            // Aplikace Gaussova rozmazání pro redukci šumu
            CvInvoke.GaussianBlur(grayImage, grayImage, new Size(5, 5), 1.5);

            // Práhování pro nalezení objektů
            CvInvoke.Threshold(grayImage, grayImage, 100, 255, ThresholdType.Binary);

            // Uložení šedého obrázku
            var outputPath = "C:/Users/eva.valentova/Desktop/noName/processed_layout.png";
            grayImage.Save(outputPath);

            DetectOutline(grayImage);
        }


        /// <summary>
        /// Detekce obrysů obrázku
        /// </summary>
        private void DetectOutline(Image<Gray, byte>  grayImage)
        {
            // Detekce obrysů
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(grayImage, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            // Procházení detekovaných obrysů
            for (int i = 0; i < contours.Size; i++)
            {
                // Získání obdélníku, který obklopuje obrys
                Rectangle boundingBox = CvInvoke.BoundingRectangle(contours[i]);

                // Získání listu bodů se souřadnicemi chatek
                _cabinsBoundingBox.Add(boundingBox);
            }

            CreateCabins(_cabinsBoundingBox, grayImage);
        }

        protected abstract void CreateCabins(List<Rectangle> cabinsCoordinates, Image<Gray, byte> image);
        #endregion
    }
}
