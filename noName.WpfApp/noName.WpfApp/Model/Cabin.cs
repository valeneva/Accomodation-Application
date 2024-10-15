// ----------------------------------------------------------------------
// <copyright file="Cabin" company="dataPartner">
//     Copyright (c) dataPartner s. r. o.  All rights reserved.
// </copyright>
// <author>
//     eva.valentova
// </author>
// ----------------------------------------------------------------------

namespace noName.WpfApp.Model
{
    public class Cabin
    {

        #region Members
        /// <summary>
        /// 
        /// </summary>


        #endregion
        //TO DO: doplnit property change
        #region Properties
        /// <summary>
        /// název chatky
        /// </summary>
        public string Title { get; set; } = String.Empty;

        /// <summary>
        /// Zobrazovaný obdelních chatky
        /// </summary>
        public System.Windows.Shapes.Rectangle Rectangle { get; set; } = new System.Windows.Shapes.Rectangle();

        /// <summary>
        /// počet míst v chatce
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// počet míst v chatce
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// průměrný věk v chatce
        /// </summary>
        public int AvrgAge { get; set; }

        /// <summary>
        /// Pohlaví dětí na chatce
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Souřadnice chatky v layout gui + rozměry
        /// </summary>
        public System.Drawing.Rectangle BoundingBox { get; set; }


        /// <summary>
        /// Seznam dětí v chatce
        /// </summary>
        public List<DataModel> CabinList { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public Cabin()
        {
            CabinList = new List<DataModel>();

            for (int i = 0; i < 10; i++)
            {
                var dataModel = new DataModel()
                {
                    Name = "Eva" + i,
                    Surname = "Poček" + (2 * i),
                    Age = i,
                    Day = 1,
                    FriendName = "Marta" + i,
                    Gender = "M",
                    Month = 1,
                    Year = 1,
                    Note = "jupí",
                };

                CabinList.Add(dataModel);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param> </param>


        #endregion
    }
}
