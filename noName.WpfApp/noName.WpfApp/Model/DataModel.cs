// ----------------------------------------------------------------------
// <copyright file="MainModel.cs" company="noName">
//     Copyright (c) noName s. r. o..  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------


namespace noName.WpfApp.Model
{
    public class DataModel
    {
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public string Gender { get; set; } = String.Empty;
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Age { get; set; }
        public string FriendName { get; set; } = String.Empty;
        public string Note { get; set; } = String.Empty;
    }
}