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
            string sqlFilePath = "../phonestore.sql";
            // Đọc nội dung của tệp .sql vào chuỗi
            string sqlQuery = File.ReadAllText(sqlFilePath);
            string connectionString = "server=localhost;user id=root;password=123456;port=3306;database=phonestore;IgnoreCommandTransaction=true;";
            try
            {
                // Kết nối đến cơ sở dữ liệu MySQL
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Thực thi các câu lệnh SQL từ chuỗi đã đọc
                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Successfully executed the SQL script.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
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