using MySql.Data.MySqlClient;
using Microsoft.Data.Sqlite;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using System.Windows;
using System.IO;

namespace WinFormsApp1
{
    public class DatabaseHelper
    {
        private String serverName = "127.0.0.1";
        private String port = "3306";
        private String databaseName = "lms";
        private String databaseUser = "root";
        private String databasePassword = "1234567890-=";

        private DatabaseHelper() { }

        private static DatabaseHelper? _instance;
        public static DatabaseHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatabaseHelper();
                return _instance;
            }
        }
        public SqliteConnection getConnection()
        {
            try
            {
                string dbInAppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "data.sqlite");

                // Target writable location in user's LocalAppData
                string dbUserFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyApp");
                string dbUserCopy = Path.Combine(dbUserFolder, "data.sqlite");

                // First-time copy if needed
                if (!File.Exists(dbUserCopy))
                {
                    if (!File.Exists(dbInAppPath))
                        throw new FileNotFoundException("Original database file not found.", dbInAppPath);

                    Directory.CreateDirectory(dbUserFolder);
                    File.Copy(dbInAppPath, dbUserCopy, overwrite: false);
                }

                // Create SQLite connection string
                string connectionString = $"Data Source={dbUserCopy};Cache=Shared;Mode=ReadWriteCreate;";
                var connection = new SqliteConnection(connectionString);
                connection.Open();

                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public SqliteDataReader getData(string query)
        {
            var connection = getConnection(); // keep this open
            var command = new SqliteCommand(query, connection);

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public DataTable GetTable(string query)
        {
            using (var connection = getConnection())
            using (var command = new SqliteCommand(query, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }
        public int Update(string query)
        {

            using (var connection = getConnection())
            {
                using (var command = new SqliteCommand(query, connection)) 
                {
                    return command.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(string query)
        {
            using (var connection = getConnection())
            {
                using (var command = new SqliteCommand(query, connection))
                {
                    return command.ExecuteScalar();
                }
            }
        }

    }
}