using System;
using System.Collections.Generic;

namespace DepoOOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ControlRoom ControlRoom = new ControlRoom();

            ControlRoom.Work();
        }
    }

    class ControlRoom
    {
        private static Random s_random = new Random();

        private Train _train;
        private bool _isReadyToDeparture;

        public ControlRoom()
        {
            _isReadyToDeparture = false;
        }

        public void Work()
        {
            const string CommandCreateDirection = "1";
            const string CommandSendTrain = "2";
            const string CommandExit = "3";

            bool isWork = true;

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать");
                Console.WriteLine($"Выберите пункт в меню:");
                Console.WriteLine($"{CommandCreateDirection} - Создать направление");
                Console.WriteLine($"{CommandSendTrain} - Отправить поезд");
                Console.WriteLine($"{CommandExit} - Выход");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandCreateDirection:
                        CreateDirection();
                        break;

                    case CommandSendTrain:
                        SendTrain();
                        break;

                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Ошибка ввода команды.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void CreateDirection()
        {
            if (_isReadyToDeparture == true)
            {
                Console.WriteLine("Направление создано ожидается отправка поезда");
            }
            else
            {
                TryGetDirectioin();
            }

            Console.ReadKey();
        }

        private bool TryGetDirectioin()
        {
            int lowerLimitRandom = 300;
            int upperLimitRandom = 1000;
            Console.Clear();
            Console.WriteLine("Введите пункт отправление");
            string departurePoint = Console.ReadLine();

            Console.WriteLine("Введите пункт прибытия");
            string arrivaPoint = Console.ReadLine();

            int passengers = s_random.Next(lowerLimitRandom, upperLimitRandom);
            _train = new Train(passengers);

            Console.WriteLine($"Вы создали направление {departurePoint} - {arrivaPoint} ожидается поезд на {passengers} пассажиров");

            return _isReadyToDeparture = true;
        }

        private void SendTrain()
        {
            if (_isReadyToDeparture == false)
            {
                Console.WriteLine("Нет поезда для отправки.");
            }
            else
            {
                _train.CreateNew();
                _isReadyToDeparture = false;
            }

            Console.ReadKey();
        }
    }

    class Train
    {
        private List<Van> _vans = new List<Van>();
        private static Random _random = new Random();

        public Train(int passengers)
        {
            Passengers = passengers;
        }

        public int Passengers { get; private set; }

        public void CreateNew()
        {
            _vans = CreateVans();
            ShowVans();

            while (Passengers > 0)
            {
                Console.WriteLine($"Разместите {Passengers} по вогонам выбрав доступный из списка.");
                Complete();

            }

            Console.WriteLine("Поезд укомплектован и отправлен по текущему направлению");
            Console.ReadKey();
        }

        private void Complete()
        {
            if (TryGetVan() == false)
                Console.WriteLine("Не было добавлено вагона");
            else
                Console.WriteLine("Вы добавили новый вагон");
        }

        private bool TryGetVan()
        {
            Console.WriteLine("Введите номер вагона для добавления его к поезду");

            if (GetNumberVan(out int numberVan) == false)
                return false;

            AccommodatePassengers(numberVan);

            return true;
        }

        private void AccommodatePassengers(int numberVan)
        {
            Passengers = Passengers - _vans[numberVan].Seating;

            if (Passengers < 0)
                Passengers = 0;
        }

        private bool GetNumberVan(out int numberVan)
        {
            int lowerLimit = 0;
            numberVan = Utilite.GetNumberInRange(lowerLimit);
            numberVan--;
            return true;
        }

        private List<Van> CreateVans()
        {
            int randomIndex;
            int minimumNumberSeats = 40;
            int maximumNumberSeats = 201;
            List<Van> vans = new List<Van>();
            string[] name = new string[] { "Купе", "Плацкарт", "Сидачие", "2х этажный плацкарт" };

            for (int i = 0; i < name.Length; i++)
            {
                randomIndex = _random.Next(minimumNumberSeats, maximumNumberSeats);
                vans.Add(new Van(name[i], randomIndex));
            }

            return vans;
        }

        private void ShowVans()
        {
            int sequenceNumber;
            Console.WriteLine("Доступные вагоны:");

            for (int i = 0; i < _vans.Count; i++)
            {
                sequenceNumber = i + 1;

                Console.Write($"{sequenceNumber}) ");

                _vans[i].ShowInfo();
            }

            Console.WriteLine();
        }
    }

    class Van
    {
        public Van(string name, int seating)
        {
            Name = name;
            Seating = seating;
        }

        public string Name { get; private set; }
        public int Seating { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Вагон {Name}, имеет {Seating} посадочных мест");
        }
    }

    class Utilite
    {
        public static int GetNumberInRange(int lowerLimitRangeNumbers = Int32.MinValue, int upperLimitRangeNumbers = Int32.MaxValue)
        {
            bool isEnterNumber = true;
            int enterNumber = 0;
            string userInput;

            while (isEnterNumber)
            {
                Console.WriteLine($"Введите число.");

                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out enterNumber) == false)
                    Console.WriteLine("Не корректный ввод.");
                else if (VerifyForAcceptableNumber(enterNumber, lowerLimitRangeNumbers, upperLimitRangeNumbers))
                    isEnterNumber = false;
            }

            return enterNumber;
        }

        private static bool VerifyForAcceptableNumber(int number, int lowerLimitRangeNumbers, int upperLimitRangeNumbers)
        {
            if (number < lowerLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за нижний предел допустимого значения.");
                return false;
            }
            else if (number > upperLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за верхний предел допустимого значения.");
                return false;
            }

            return true;
        }
    }
}