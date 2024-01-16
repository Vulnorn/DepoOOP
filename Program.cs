using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DepoOOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ControlRoom controlRoom = new ControlRoom();
        }
    }

    class ControlRoom
    {
        protected List<Van> Vans = new List<Van>();
    }

    class Train
    {
        private List<Van> _vans = new List<Van>();
        private static Random _random = new Random();

        public Train (int passengers)
        {
            Passengers = passengers;

        }

        public int Passengers { get; private set; }

        public void CreateNew()
        {
            _vans = CreateVans();
            ShowVans();

            while (Passengers>0)
            {
                Console.WriteLine($"Разместите {Passengers} по вогонам выбрав доступный из списка.");

            }

        }
          
        private List<Van> CreateVans ()
        {
            int randomIndex;
            int minimumNumberSeats = 40;
            int maximumNumberSeats = 201;
            List<Van > vans = new List<Van>();
            string[] name = new string[] { "Купе", "Плацкарт", "Сидачие", "2х этажный плацкарт" };

            for (int i = 0; i<name.Length; i++)
            {
                randomIndex = _random.Next(minimumNumberSeats,maximumNumberSeats);
                vans.Add(new Van(name[i],randomIndex));
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
            Console.Write($"Вагон {Name}, имеет {Seating} посадочных мест");
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