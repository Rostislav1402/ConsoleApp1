using System;
using System.Globalization;
using System.Text;

namespace MyDelivery
{
    abstract class Delivery
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        protected Delivery(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название не должно быть пустым.");

            if (name.Length > 20)
                throw new ArgumentException("Максимум 20 символов.");

            Name = name;
        }

        public void SetPrice(decimal basePrice)
        {
            Price = basePrice * GetCoefficient();
        }

        protected abstract decimal GetCoefficient();

        public override string ToString()
        {
            return $"{Name,-20} Стоимость: {Price:F2}";
        }
    }

    class Standard : Delivery
    {
        public Standard(string name) : base(name) { }

        protected override decimal GetCoefficient()
        {
            return 1.0m;
        }
    }

    class Express : Delivery
    {
        public Express(string name) : base(name) { }

        protected override decimal GetCoefficient()
        {
            return 2.5m;
        }
    }

    class International : Delivery
    {
        public International(string name) : base(name) { }

        protected override decimal GetCoefficient()
        {
            return 4.0m;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Служба доставки\n");

            decimal basePrice;

            while (true)
            {
                Console.Write("Введите базовую стоимость: ");
                string input = Console.ReadLine();

                if ((decimal.TryParse(input, NumberStyles.Number,
                    CultureInfo.CurrentCulture, out basePrice)
                    || decimal.TryParse(input, NumberStyles.Number,
                    CultureInfo.InvariantCulture, out basePrice))
                    && basePrice >= 0)
                {
                    break;
                }

                Console.WriteLine("Ошибка! Введите число не меньше нуля.");
            }

            Delivery[] deliveries =
            {
                CreateDelivery("Стандартная доставка", 1),
                CreateDelivery("Экспресс-доставка", 2),
                CreateDelivery("Международная доставка", 3)
            };

            Console.WriteLine("\nРезультаты:");

            foreach (Delivery item in deliveries)
            {
                item.SetPrice(basePrice);
                Console.WriteLine(item);
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода.");
            Console.ReadKey();
        }

        static Delivery CreateDelivery(string title, int type)
        {
            while (true)
            {
                Console.Write($"Введите название ({title}): ");
                string name = Console.ReadLine();

                try
                {
                    switch (type)
                    {
                        case 1:
                            return new Standard(name);
                        case 2:
                            return new Express(name);
                        case 3:
                            return new International(name);
                        default:
                            throw new ArgumentException("Неверный тип.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
