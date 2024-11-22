using System;
using System.Threading;

namespace AirRadiationMonitoring
{
	// Клас для зберігання даних моніторингу
	public class MonitoringData
	{
		public float AirQuality { get; set; } // Якість повітря (PM2.5)
		public float Radiation { get; set; }  // Радіаційний фон (мкЗв/год)
	}

	class Program
	{
		// Функція для генерації даних із датчиків
		static MonitoringData GetSensorData()
		{
			Random rand = new Random();
			return new MonitoringData
			{
				AirQuality = 10 + rand.Next(91), // Значення якості повітря від 10 до 100
				Radiation = 0.1f + (float)(rand.NextDouble() * 1.9) // Радіація від 0.1 до 2.0
			};
		}

		// Функція входу в систему
		static bool Login()
		{
			Console.WriteLine("=== Вхід у систему ===");
			Console.Write("Введіть ім'я користувача: ");
			string username = Console.ReadLine();
			Console.Write("Введіть пароль: ");
			string password = Console.ReadLine();

			// Простий контроль (логін: "admin", пароль: "1234")
			if (username == "admin" && password == "1234")
			{
				Console.WriteLine("Авторизація успішна!");
				return true;
			}
			else
			{
				Console.WriteLine("Невірне ім'я користувача або пароль.");
				return false;
			}
		}

		// Функція для запуску моніторингу
		static void StartMonitoring()
		{
			Console.WriteLine("\n=== Запуск моніторингу ===");
			Console.WriteLine("Натисніть Ctrl+C для завершення.\n");

			while (true)
			{
				MonitoringData data = GetSensorData();
				Console.WriteLine($"Якість повітря (PM2.5): {data.AirQuality:F2} мкг/м³ | Радіація: {data.Radiation:F2} мкЗв/год");
				Thread.Sleep(2000); // Очікування 2 секунди перед оновленням
			}
		}

		static void Main(string[] args)
		{
			// Авторизація
			if (!Login())
			{
				Console.WriteLine("Вихід із програми.");
				return;
			}

			// Запуск моніторингу
			StartMonitoring();
		}
	}
}
