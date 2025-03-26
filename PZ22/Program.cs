using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ22
{
    // Структура Изделие
    public struct Product
    {
        public string Group1 { get; private set; }
        public string Group2 { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public double Weight { get; private set; }
        public string Configuration { get; private set; }
        public double Cost { get; private set; }

        public static double ExchangeRate { get; set; } = 75.0;

        public Product(string name, string code, double weight, string configuration, double cost)
        {
            Name = name;
            Code = code;
            Weight = weight;
            Configuration = configuration;
            Cost = cost;
            Group1 = DetermineGroup1(cost);
            Group2 = DetermineGroup2(weight);
        }

        private static string DetermineGroup1(double cost)
        {
            if (cost < 10)
                return "Малоценка";
            else if (cost < 1000)
                return "Средней цены";
            else if (cost < 10000)
                return "Повышенной стоимости";
            else
                return "Значительной стоимости";
        }

        private static string DetermineGroup2(double weightKg)
        {
            double weightGrams = weightKg * 1000;
            if (weightGrams <= 500)
                return "Легкое";
            else if (weightGrams <= 5000)
                return "Средней тяжести";
            else if (weightGrams <= 500000)
                return "Значительной тяжести";
            else
                return "Очень тяжелое";
        }

        public double GetCostInRubles()
        {
            return Cost * ExchangeRate;
        }

        public string GetCertificate(string destinationCountry)
        {
            return "Сертификат изделия:\n" +
                   "Название: " + Name + "\n" +
                   "Шифр: " + Code + "\n" +
                   "Вес: " + Weight + " кг\n" +
                   "Комплектация: " + Configuration + "\n" +
                   "Стоимость: " + Cost + " у.е. (" + GetCostInRubles() + " руб.)\n" +
                   "Группа 1: " + Group1 + "\n" +
                   "Группа 2: " + Group2 + "\n" +
                   "Страна назначения: " + destinationCountry + "\n";
        }

        public static void SetExchangeRate(double newRate)
        {
            ExchangeRate = newRate;
        }

        public static Product operator +(Product p1, Product p2)
        {
            string newName = p1.Name + " & " + p2.Name;
            string newCode = p1.Code + "-" + p2.Code;
            double newWeight = (p1.Weight + p2.Weight) / 2;
            string newConfiguration = p1.Configuration + " / " + p2.Configuration;
            double newCost = p1.Cost + p2.Cost;
            return new Product(newName, newCode, newWeight, newConfiguration, newCost);
        }

        public static bool operator >(Product p1, Product p2)
        {
            return p1.Cost > p2.Cost;
        }

        public static bool operator <(Product p1, Product p2)
        {
            return p1.Cost < p2.Cost;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите текущий курс обмена (1 у.е. в рублях): ");
            double rate;
            while (!double.TryParse(Console.ReadLine(), out rate))
            {
                Console.Write("Некорректный ввод. Введите число: ");
            }
            Product.SetExchangeRate(rate);

            Console.Write("Введите количество изделий: ");
            int count;
            while (!int.TryParse(Console.ReadLine(), out count) || count <= 0)
            {
                Console.Write("Некорректный ввод. Введите целое положительное число: ");
            }

            Product[] products = new Product[count];

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"\nВведите данные для изделия {i + 1}:");
                Console.Write("Название: ");
                string name = Console.ReadLine();
                Console.Write("Шифр: ");
                string code = Console.ReadLine();
                Console.Write("Вес (в кг): ");
                double weight;
                while (!double.TryParse(Console.ReadLine(), out weight))
                {
                    Console.Write("Некорректный ввод. Введите число: ");
                }
                Console.Write("Комплектация: ");
                string configuration = Console.ReadLine();
                Console.Write("Стоимость (у.е.): ");
                double cost;
                while (!double.TryParse(Console.ReadLine(), out cost))
                {
                    Console.Write("Некорректный ввод. Введите число: ");
                }

                products[i] = new Product(name, code, weight, configuration, cost);
            }

            for (int i = 0; i < count; i++)
            {
                Console.Write($"\nВведите страну назначения для изделия {i + 1}: ");
                string destination = Console.ReadLine();
                Console.WriteLine("\n" + products[i].GetCertificate(destination));
                Console.WriteLine(new string('-', 50));
            }

            if (count >= 2)
            {
                Console.WriteLine("\nДемонстрация объединения двух изделий:");
                Console.Write("Введите индекс первого изделия (от 1 до " + count + "): ");
                int idx1;
                while (!int.TryParse(Console.ReadLine(), out idx1) || idx1 < 1 || idx1 > count)
                {
                    Console.Write("Некорректный ввод. Введите число от 1 до " + count + ": ");
                }
                Console.Write("Введите индекс второго изделия (от 1 до " + count + "): ");
                int idx2;
                while (!int.TryParse(Console.ReadLine(), out idx2) || idx2 < 1 || idx2 > count)
                {
                    Console.Write("Некорректный ввод. Введите число от 1 до " + count + ": ");
                }
                Product combined = products[idx1 - 1] + products[idx2 - 1];
                Console.Write("Введите страну назначения для объединённого изделия: ");
                string destCombined = Console.ReadLine();
                Console.WriteLine("\nСертификат объединённого изделия:");
                Console.WriteLine(combined.GetCertificate(destCombined));
            }

            if (count >= 2)
            {
                Console.WriteLine("\nДемонстрация сравнения стоимости двух изделий:");
                Console.Write("Введите индекс первого изделия (от 1 до " + count + "): ");
                int idx1;
                while (!int.TryParse(Console.ReadLine(), out idx1) || idx1 < 1 || idx1 > count)
                {
                    Console.Write("Некорректный ввод. Введите число от 1 до " + count + ": ");
                }
                Console.Write("Введите индекс второго изделия (от 1 до " + count + "): ");
                int idx2;
                while (!int.TryParse(Console.ReadLine(), out idx2) || idx2 < 1 || idx2 > count)
                {
                    Console.Write("Некорректный ввод. Введите число от 1 до " + count + ": ");
                }

                if (products[idx1 - 1] > products[idx2 - 1])
                    Console.WriteLine($"Изделие {idx1} стоит дороже изделия {idx2}");
                else if (products[idx1 - 1] < products[idx2 - 1])
                    Console.WriteLine($"Изделие {idx2} стоит дороже изделия {idx1}");
                else
                    Console.WriteLine("Оба изделия имеют одинаковую стоимость");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
