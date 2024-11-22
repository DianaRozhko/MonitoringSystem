using System;
using System.Threading;

namespace AirRadiationMonitoring
{
	// Клас для зберігання даних моніторингу
	public class MonitoringData
	{
		// Поле для якості повітря (вимірюється у мікрограмах на кубічний метр, PM2.5)
		public float AirQuality { get; set; }

		// Поле для рівня радіації (вимірюється у мікрозівертах на годину)
		public float Radiation { get; set; }
	}

	class Program
	{
		/// <summary>
		/// Симулює отримання даних із датчиків.
		/// Генерує випадкові значення для якості повітря та радіаційного фону.
		/// </summary>
		/// <returns>Об'єкт типу MonitoringData з випадковими значеннями</returns>
		static MonitoringData GetSensorData()
		{
			Random rand = new Random(); // Ініціалізація генератора випадкових чисел

			return new MonitoringData
			{
				AirQuality = 10 + rand.Next(91), // Якість повітря від 10 до 100
				Radiation = 0.1f + (float)(rand.NextDouble() * 1.9) // Радіація від 0.1 до 2.0
			};
		}

		/// <summary>
		/// Реалізує авторизацію користувача.
		/// </summary>
		/// <returns>Повертає true, якщо логін і пароль правильні, інакше false</returns>
		static bool Login()
		{
			Console.WriteLine("=== Вхід у систему ===");
			Console.Write("Введіть ім'я користувача: ");
			string username = Console.ReadLine(); // Зчитуємо ім'я користувача

			Console.Write("Введіть пароль: ");
			string password = Console.ReadLine(); // Зчитуємо пароль

			// Перевірка логіна і пароля (статичні значення для прикладу)
			if (username == "admin" && password == "1234")
			{
				Console.WriteLine("Авторизація успішна!");
				return true; // Успішний вхід
			}
			else
			{
				Console.WriteLine("Невірне ім'я користувача або пароль.");
				return false; // Невдалий вхід
			}
		}

		/// <summary>
		/// Запускає моніторинг у режимі реального часу.
		/// Дані відображаються кожні 2 секунди.
		/// </summary>
		static void StartMonitoring()
		{
			Console.WriteLine("\n=== Запуск моніторингу ===");
			Console.WriteLine("Натисніть Ctrl+C для завершення.\n");

			while (true) // Безкінечний цикл для постійного відображення даних
			{
				MonitoringData data = GetSensorData(); // Отримання даних із датчиків

				// Виведення даних у консоль
				Console.WriteLine($"Якість повітря (PM2.5): {data.AirQuality:F2} мкг/м³ | Радіація: {data.Radiation:F2} мкЗв/год");

				Thread.Sleep(2000); // Затримка на 2 секунди перед наступним оновленням
			}
		}

		static void Main(string[] args)
		{
			// Встановлення кодування UTF-8 для коректного відображення української мови
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			// Авторизація користувача
			if (!Login())
			{
				Console.WriteLine("Вихід із програми."); // Повідомлення про вихід
				return; // Завершення програми, якщо авторизація не вдалася
			}

			// Запуск моніторингу
			StartMonitoring();
		}
	}
}
