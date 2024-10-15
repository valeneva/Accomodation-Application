namespace noName.WpfApp.Database
{
    using System.Data.SQLite;
    using Microsoft.Extensions.Logging;

    public class Database
    {
        
        private ILogger _logger;
        public Database(ILogger logger)
        {
_logger = logger;
        
        // Vytvoření nové databáze v paměti
        using (var connection = new SQLiteConnection("Data Source=mydatabase2.db"))
        {
            connection.Open();

            // Vytvoření tabulky
            string sql = "CREATE TABLE people (id INTEGER PRIMARY KEY, name TEXT, age INTEGER)";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            // Vložení dat
            sql = "INSERT INTO people (name, age) VALUES ('Alice', 30), ('Bob', 25)";
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            // Načtení dat
            sql = "SELECT * FROM people";
            using (var command = new SQLiteCommand(sql, connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["id"]}: {reader["name"]}, {reader["age"]} let");
                }
            }
        }
        }

    }

}