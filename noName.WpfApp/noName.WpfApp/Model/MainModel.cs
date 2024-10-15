// ----------------------------------------------------------------------
// <copyright file="MainModel.cs" company="noName">
//     Copyright (c) noName s. r. o..  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------


//Main model - propojení python modelu s viewmodel GUI
namespace noName.WpfApp.Model
{
    using ClosedXML.Excel;
    using CsvHelper;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Formats.Asn1;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;


    public class MainModel
    {
        #region Members

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        #endregion

        #region Properties


        #endregion


        #region Constructor
        /// <summary>
        /// Initializes clienta.
        /// </summary>
        public MainModel(ILoggerFactory loggerFactory, HttpClient httpClient)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<MainModel>();
            _httpClient = httpClient;


        }

        #endregion


        #region Methods
        public class Animal
        {
            public string Name { get; set; }
            public string? Weight { get; set; } // Nullable vlastnost

            // Případně inicializace v konstruktoru
            public Animal(string name, string? weight = null)
            {
                Name = name;
                Weight = weight;
            }
        }

        public async Task LogAnimalsAsync()
        {
            try
            {
                // Pošle GET požadavek na endpoint
                var response = await _httpClient.GetAsync("animals");

                // Zkontroluje, zda odpověď je úspěšná
                response.EnsureSuccessStatusCode();


                // Přečte obsah odpovědi
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize JSON na dynamický objekt
                var animals = JsonConvert.DeserializeObject<List<Animal>>(responseContent);




                // Zkontroluje, zda animals není null
                if (animals == null || animals.Count == 0)
                {
                    _logger.LogWarning("Received empty or null response from the server.");
                    return;
                }

                // Logování seznamu zvířátek
                _logger.LogDebug("Retrieved animals list:");

                foreach (var animal in animals)
                {
                    if (animal != null)
                    {
                        var name = animal.Name;
                        var weight = animal.Weight;

                        if (name != null && weight != null)
                        {
                            _logger.LogDebug($"Name: {name}, Weight: {weight}");
                        }
                        else
                        {
                            _logger.LogWarning("Animal data is incomplete or invalid.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while retrieving animal list: {ex.Message}");
            }
        }

        public List<DataModel> ReadCsvFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<DataModel>();
                return new List<DataModel>(records);
            }
        }

        public List<DataModel> ReadExcelFile(string filePath)
        {
            var list = new List<DataModel>();

            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);  // Načti první list v Excel souboru
                var rows = worksheet.RangeUsed().RowsUsed();  // Načti všechny použité řádky

                foreach (var row in rows.Skip(1))
                {
                    var data = new DataModel
                    {
                        Name = row.Cell(1).GetString(),  // Načti hodnotu z prvního sloupce
                        Surname = row.Cell(2).GetString(),  // Načti hodnotu z druhého sloupce
                        Day = row.Cell(3).GetValue<int>(),
                        Month = row.Cell(4).GetValue<int>(),
                        Year = row.Cell(5).GetValue<int>(),
                        Age = row.Cell(6).GetValue<int>(),
                        FriendName = row.Cell(7).GetString(),
                        Note = row.Cell(8).GetString()
                    };
                    list.Add(data);
                }
            }

            return list;
        }

        #endregion
    }
}
