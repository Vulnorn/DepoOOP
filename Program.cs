using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DepoOOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dispatcher ControlRoom = new Dispatcher();

            ControlRoom.Work();
        }
    }

    class Dispatcher
    {
        Direction Direction;

        private List<Train> _trains = new List<Train>();

        private int _tickets;
        private bool _isCreateDirectionExpected = false;
        private bool _isSalesTicketExpected = false;

        public void Work()
        {
            const string CommandCreateDirection = "1";
            const string CommandSellTickets = "2";
            const string CommandCreateTrain = "3";
            const string CommandExit = "4";

            bool isWork = true;

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать");
                Console.WriteLine($"Выберите пункт в меню:");
                Console.WriteLine($"{CommandCreateDirection} - Создать направление");
                Console.WriteLine($"{CommandSellTickets} - Продать билеты");
                Console.WriteLine($"{CommandCreateTrain} - Создать поезд");
                Console.WriteLine($"{CommandExit} - Выход\n\n");

                ShoySchedule();

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandCreateDirection:
                        CreateDirection();
                        break;

                    case CommandSellTickets:
                        SellTickets();
                        break;

                    case CommandCreateTrain:
                        CreateTrain();
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
            if (_isCreateDirectionExpected == false && _isSalesTicketExpected == false)
            {
                Direction = DirectionFactory.Create();
                _isCreateDirectionExpected = true;
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Есть созданное направление и продайте билеты и отправте поезд");
            }
        }

        private void SellTickets()
        {
            if (_isSalesTicketExpected == false)
            {
                int lowerLimitRandom = 300;
                int upperLimitRandom = 1000;
                _tickets = Utilite.GenerateRandomNumber(lowerLimitRandom, upperLimitRandom);

                Console.WriteLine($"На это направление было продано {_tickets}.");
                _isSalesTicketExpected = true;
            }
            else
            {
                Console.WriteLine($"Билеты уже проданы отправьте поезд.");
            }
        }

        private void CreateTrain()
        {
            if (_isSalesTicketExpected == true && _isCreateDirectionExpected == true)
            {
                _trains.Add(TrainFactory.Create(_tickets, Direction));
                _isSalesTicketExpected = false;
                _isCreateDirectionExpected = false;
            }
            else
            {
                Console.WriteLine($"Нет билетов или направления.");
            }
        }

        private void ShoySchedule()
        {
                for (int i = 0; i < _trains.Count; i++)
                {
                    Console.WriteLine($"Отправление - {_trains[i].Direction.Departure}, Прибытие - {_trains[i].Direction.Arrival}, Количество пассажиров - {_trains[i].Passengers}");
                }
        }
    }

    class TrainFactory
    {
        public static Train Create(int tickets, Direction direction)
        {
            List<Van> vans = VanFactory.Create();
            List<Van> composition = new List<Van>();

            int allPassenger = tickets;
            Console.WriteLine($"Разместите {allPassenger} по вогонам выбрав доступный из списка.");

            while (allPassenger != 0)
            {
                GetVan(vans, composition);
                allPassenger = AccommodatePassengers(composition, allPassenger);
            }

            Console.WriteLine("Поезд укомплектован");
            Console.ReadKey();
            return new Train(direction, tickets, composition);
        }

        private static void GetVan(List<Van> vans, List<Van> structure)
        {
            int upperLimit = vans.Count;
            int lowerLimit = 0;

            Console.WriteLine("Введите номер вагона для добавления его к поезду");
            int numberVan = Utilite.GetNumberInRange(lowerLimit, upperLimit);

            structure.Add(vans[numberVan]);
            Console.WriteLine("Вы добавили новый вагон");
        }

        private static int AccommodatePassengers(List<Van> structure, int allPassengers)
        {
            int passengers = 0;

            for (int i = 0; i < structure.Count; i++)
            {
                passengers += structure[i].SeatsCount;
            }

            if (passengers >= allPassengers)
                allPassengers = 0;

            return allPassengers;
        }
    }

    class VanFactory
    {
        public static List<Van> Create()
        {
            List<Van> vans = new List<Van>();

            int randomIndex;
            int minimumNumberSeats = 40;
            int maximumNumberSeats = 201;

            string[] name = new string[] { "Купе", "Плацкарт", "Сидачие", "2х этажный плацкарт" };

            for (int i = 0; i < name.Length; i++)
            {
                randomIndex = Utilite.GenerateRandomNumber(minimumNumberSeats, maximumNumberSeats);
                vans.Add(new Van(name[i], randomIndex));
            }

            Show(vans);

            return vans;
        }

        private static void Show(List<Van> vans)
        {
            int sequenceNumber;
            Console.WriteLine("Доступные вагоны:");

            for (int i = 0; i < vans.Count; i++)
            {
                sequenceNumber = i + 1;

                Console.Write($"{sequenceNumber}) ");

                vans[i].ShowInfo();
            }

            Console.WriteLine();
        }
    }

    class DirectionFactory
    {
        public static Direction Create()
        {
            string departurePoint;
            string arrivaPoint;

            do
            {
                Console.Clear();
                Console.WriteLine($"Пункты не должны совподать");
                Console.WriteLine("Введите пункт отправление");
                departurePoint = Console.ReadLine();

                Console.WriteLine("Введите пункт прибытия");
                arrivaPoint = Console.ReadLine();

            }
            while (departurePoint == arrivaPoint);

            return new Direction(departurePoint, arrivaPoint);
        }
    }

    class Direction
    {
        public Direction(string departure, string arrival)
        {
            Departure = departure;
            Arrival = arrival;
        }

        public string Departure { get; private set; }
        public string Arrival { get; private set; }
    }

    class Van
    {
        public Van(string name, int seating)
        {
            Name = name;
            SeatsCount = seating;
        }

        public string Name { get; private set; }
        public int SeatsCount { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Вагон {Name}, имеет {SeatsCount} посадочных мест");
        }
    }

    class Train
    {
        public Train(Direction direction, int passengers, List<Van> vans)
        {
            Passengers = passengers;
            Direction = direction;
            Vans = vans;
        }

        public Direction Direction { get; private set; }

        public List<Van> Vans { get; private set; }

        public int Passengers { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"");
        }
    }

    class Utilite
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int lowerLimitRangeRandom, int upperLimitRangeRandom)
        {
            int numberRandom = s_random.Next(lowerLimitRangeRandom, upperLimitRangeRandom);
            return numberRandom;
        }

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