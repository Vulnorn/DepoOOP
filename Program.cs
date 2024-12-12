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
        private List<Train> _train = new List<Train>();

        private bool _isReadyToDeparture;

        public Dispatcher()
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
                Console.WriteLine($"{CommandExit} - Выход\n\n");

                ShoySchedule();

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
                Console.WriteLine("Направление создано ожидается отправка поезда");
            else
                TryGetDirectioin();

            Console.ReadKey();
        }

        private bool TryGetDirectioin()
        {
            int lowerLimitRandom = 300;
            int upperLimitRandom = 1000;
            string departurePoint;
            string arrivaPoint;
            Console.Clear();

          

            int passengers = Utilite.GenerateRandomNumber(lowerLimitRandom, upperLimitRandom);
            _train = new Train(passengers, departurePoint, arrivaPoint);

            Console.WriteLine($"Вы создали направление {_train.DeparturePoint} - {_train.ArrivaPoint} ожидается поезд на {_train.Passengers} пассажиров");

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
                _train.Create();
                _isReadyToDeparture = false;
                _schedule.Add(_train);
            }

            Console.ReadKey();
        }

        private void ShoySchedule()
        {
            for (int i = 0; i < _schedule.Count; i++)
            {
                Console.WriteLine($"От {_schedule[i].DeparturePoint} Куда {_schedule[i].ArrivaPoint} Количество пассажиров - {_schedule[i].Passengers}");
            }
        }
    }

    

   class TrainFactory
    {
        private VanFactory _vanFactory = new VanFactory();

        private List<Van> _vans = new List<Van>();

        private int _passengers;
        private int _numberVan;

        public TrainFactory(int passengers)
        {
            _passengers = passengers;
            Create();
        }

        private void Create()
        {
            while (_passengers > 0)
            {
                Console.WriteLine($"Разместите {_passengers} по вогонам выбрав доступный из списка.");
                Complete();
            }

            Console.WriteLine("Поезд укомплектован");
            Console.ReadKey();
            
        }

        private void Complete()
        {
            if (TryGetVan() == false)
            {
                Console.WriteLine("Не было добавлено вагона");
            }
            else
            {
                Console.WriteLine("Вы добавили новый вагон");
                
            }
        }

        private bool TryGetVan()
        {
            Console.WriteLine("Введите номер вагона для добавления его к поезду");
            _numberVan = GetNumberVan();

            if (_vanFactory.Vans.Count < _numberVan)
                return false;

            AccommodatePassengers();
            return true;
        }

        private void AccommodatePassengers()
        {
            _numberVan--;
            _passengers = _passengers - _vanFactory.Vans[_numberVan].SeatsCount;

            if (_passengers < 0)
                _passengers = 0;
        }

        private int GetNumberVan()
        {
            int lowerLimit = 0;
            int numberVan = Utilite.GetNumberInRange(lowerLimit);
            return numberVan;
        }
    }

    class VanFactory
    {
        public VanFactory() 
        {
            Create(Vans);
            Show();
        } 

        public List<Van> Vans {  get; private set; } 

        private void Show()
        {
            int sequenceNumber;
            Console.WriteLine("Доступные вагоны:");

            for (int i = 0; i < Vans.Count; i++)
            {
                sequenceNumber = i + 1;

                Console.Write($"{sequenceNumber}) ");

                Vans[i].ShowInfo();
            }

            Console.WriteLine();
        }

        private List<Van> Create(List<Van> vans)
        {
            int randomIndex;
            int minimumNumberSeats = 40;
            int maximumNumberSeats = 201;
            string[] name = new string[] { "Купе", "Плацкарт", "Сидачие", "2х этажный плацкарт" };

            for (int i = 0; i < name.Length; i++)
            {
                randomIndex = Utilite.GenerateRandomNumber(minimumNumberSeats, maximumNumberSeats);
                vans.Add(new Van(name[i], randomIndex));
            }

            return vans;
        }
    }

    class DirectionFactory
    {
        private Direction Create ()
        {
            string departurePoint;
            string arrivaPoint;

            do
            {
                Console.Clear();
                Console.WriteLine("Введите пункт отправление");
               departurePoint = Console.ReadLine();

                Console.WriteLine("Введите пункт прибытия");
               arrivaPoint = Console.ReadLine();

                Console.WriteLine($"Пункты не должны совподать");
            }
            while (departurePoint == arrivaPoint);

            return new Direction(departurePoint, arrivaPoint);
        }
    }

    class Direction
    {
        public  Direction(string departure, string arrival)
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
        public Train(Direction direction, string name, int passengers, List<Van> vans)
        {
            Passengers = passengers;
            Name = name;
            Direction = direction;
            Vans= vans;
        }

        public Direction Direction { get; private set; }

        public List<Van> Vans { get; private set; }

        public int Passengers { get; private set; }
        public string Name { get; private set; }  

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