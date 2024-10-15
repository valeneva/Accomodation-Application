// ----------------------------------------------------------------------
// <copyright file="Cabins" company="dataPartner">
//     Copyright (c) dataPartner s. r. o.  All rights reserved.
// </copyright>
// <author>
//     eva.valentova
// </author>
// ----------------------------------------------------------------------



using noName.WpfApp.LayoutViewModel;

namespace noName.WpfApp.ViewModel
{
    using Emgu.CV;
    using Emgu.CV.Structure;
    using System.Drawing;
    using noName.WpfApp.LayoutViewModel;
    using noName.WpfApp.Model;

    public abstract class CabinsCreater : LayoutPictureExtractor
    {

        #region Members

        private List<Cabin>? _cabins;


        #endregion

        #region Properties
        public List<Cabin>? Cabins
        {
            get => _cabins;
            set => SetPropertyValue(ref _cabins, value);
        }

        #endregion

        #region Constructor
        public CabinsCreater() : base()
        {


        }

        #endregion

        #region Methods
        /// <summary>
        /// Vytvoří list chatek s propertamy
        /// </summary>
        /// <param name="cabinsCoordinates"></param>
        protected override void CreateCabins(List<Rectangle> cabinsRectangles, Image<Gray, byte> image)
        {
            Cabins = new List<Cabin>();

            for (int i = 0; i < cabinsRectangles.Count; i++)
            {


                Cabins.Add(new Cabin
                {
                    Title = "Detail chatky " + i.ToString(),
                    Order = i,
                    Capacity = 6, // Například pevně stanovená kapacita
                    AvrgAge = 12, // Například pevně stanovená věková skupina
                    Gender = "Male", // Pevně stanovené nebo náhodně
                    BoundingBox = cabinsRectangles[i] // Souřadnice chatky
                });
            }

            CreateLayout(Cabins, image);
        }

        protected abstract void CreateLayout(List<Cabin> cabins, Image<Gray, byte> image);

        #endregion
    }
}
