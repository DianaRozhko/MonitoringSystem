
using System;
using MySql.Data.MySqlClient;

class Program
{
    static void Main(string[] args)
    {
        // Рядок підключення (змініть відповідно до вашої конфігурації)
        string connectionString = "Server=127.0.0.1;Database=monitoring_system;User ID=root;Password=root;SslMode=none;";

        try
        {
            // Створення з'єднання
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Успішне підключення до бази даних!");

                // Виконання простого запиту для перевірки бази даних
                string query = "SELECT DATABASE();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    string databaseName = (string)command.ExecuteScalar();
                    Console.WriteLine($"Поточна база даних: {databaseName}");
                }

                // Запит для отримання списку таблиць
                string tableQuery = "SHOW TABLES;";
                using (MySqlCommand command = new MySqlCommand(tableQuery, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nСписок таблиць у базі даних:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"- {reader[0]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }
}

