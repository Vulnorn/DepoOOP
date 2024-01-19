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
        private Train _train;
        private static Random _random = new Random();
        public ControlRoom()
        {
            IsWork = true;
            DepartureExpected = false;
        }

        public bool IsWork { get; private set; }
        public bool DepartureExpected { get; private set; }

        public void Work()
        {
            while (IsWork)
            {
                ShowMenu();
            }
        }

        private void ShowMenu()
        {
            const string CreateDirectionMenu = "1";
            const string SendTrainMenu = "2";
            const string ExitMenu = "3";

            Console.Clear();
            Console.WriteLine("Добро пожаловать");
            Console.WriteLine($"Выберите пункт в меню:");
            Console.WriteLine($"{CreateDirectionMenu} - Создать направление");
            Console.WriteLine($"{SendTrainMenu} - Отправить поезд");
            Console.WriteLine($"{ExitMenu} - Выход");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case CreateDirectionMenu:
                    CreateDirection();
                    break;

                case SendTrainMenu:
                    SendTrain();
                    break;

                case ExitMenu:
                    Exit();
                    break;

                default:
                    Console.WriteLine("Ошибка ввода команды.");
                    Console.ReadKey();
                    break;
            }
        }

        private void CreateDirection()
        {
            if (DepartureExpected == true)
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

            int passengers = _random.Next(lowerLimitRandom, upperLimitRandom);
            _train = new Train(passengers);

            Console.WriteLine($"Вы создали направление {departurePoint} - {arrivaPoint} ожидается поезд на {passengers} пассажиров");

            return DepartureExpected = true;
        }

        private void SendTrain()
        {
            if (DepartureExpected == false)
            {
                Console.WriteLine("Нет поезда для отправки.");
            }
            else
            {
                TryGetTrain();
            }

            Console.ReadKey();
        }

        private bool TryGetTrain()
        {
            _train.CreateNew();
            return DepartureExpected = false;
        }

        private void Exit()
        {
            IsWork = false;
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
            if (Utilite.TryGetPositiveNumber(out numberVan) == false)
                return false;

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
        public static bool TryGetPositiveNumber(out int numder)
        {
            string userInput;

            do
            {
                userInput = Console.ReadLine();
            }
            while (GetInputValue(userInput, out numder));

            if (GetNumberRange(numder))
            {
                Console.WriteLine("Хорошая попытка.");
                return false;
            }

            return true;
        }

        private static bool GetInputValue(string input, out int number)
        {
            if (int.TryParse(input, out number) == false)
            {
                Console.WriteLine("Не корректный ввод.");
                return true;
            }

            return false;
        }

        private static bool GetNumberRange(int number)
        {
            int positiveValue = 0;

            if (number < positiveValue)
                return true;

            return false;
        }
    }
}