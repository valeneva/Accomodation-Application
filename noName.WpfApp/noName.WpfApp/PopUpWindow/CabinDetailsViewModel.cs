// ----------------------------------------------------------------------
// <copyright file="CabinDetailsViewModel" company="dataPartner">
//     Copyright (c) dataPartner s. r. o.  All rights reserved.
// </copyright>
// <author>
//     eva.valentova
// </author>
// ----------------------------------------------------------------------

using noName.WpfApp.Model;
using noName.WpfApp.ViewModel;
using System;

namespace noName.WpfApp.PopUpWindow
{
    public class CabinDetailsViewModel : ViewModelBase
    {

        #region Members
        /// <summary>
        /// 
        /// </summary>
        private Cabin? _cabin;

        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public Cabin? Cabin
        {
            get => _cabin;
            set => SetPropertyValue(ref _cabin, value);
            
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
