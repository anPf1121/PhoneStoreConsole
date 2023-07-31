using MySqlConnector;

namespace DAL
{
    public class DbConfig
    {
        private static MySqlConnection connection = new MySqlConnection();
        private DbConfig() { }
        public static MySqlConnection GetDefaultConnection()
        {
            return GetConnection("server=localhost;user id=root;password=123456;port=3306;database=phonestore;IgnoreCommandTransaction=true;");
        }
        public static MySqlConnection GetConnection()
        {

            string delimiter = "$$"; // Replace with your custom delimiter
            string sqlFilePath = "../phonestore.sql";
            string connectionString = "server=localhost;user id=root;password=123456;port=3306;database=phonestore;IgnoreCommandTransaction=true;";
            try
            {
                string sqlCommands = File.ReadAllText(sqlFilePath);
                string[] statements = sqlCommands.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (string statement in statements)
                    {
                        using (MySqlCommand command = new MySqlCommand(statement, connection))
                        {
                            command.ExecuteNonQuery();
                            Console.WriteLine("Statement executed successfully.");
                        }
                    }

                    Console.WriteLine("All statements executed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            string connectionStringNotHaveDB = "server=localhost;user=root;password=123456;";

            try
            {
                string sqlCommands = File.ReadAllText(sqlFilePath);

                using (MySqlConnection connection = new MySqlConnection(connectionStringNotHaveDB))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(sqlCommands, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            try
            {

                string conString;
                using (System.IO.FileStream fileStream = System.IO.File.OpenRead("DbConfig.txt"))
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    conString = reader.ReadLine() ?? "server=localhost;user id=root;password=123456;port=3306;database=phonestore;IgnoreCommandTransaction=true;";
                }

                if (!conString.Contains("IgnoreCommandTransaction=true"))
                {
                    conString += "IgnoreCommandTransaction=true;";
                }
                Console.ReadLine();
                return GetConnection(conString);
            }
            catch
            {
                return GetDefaultConnection();
            }
        }
        public static MySqlConnection GetConnection(string connectionString)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.ConnectionString = connectionString;

            }
            return connection;
        }
    }
}