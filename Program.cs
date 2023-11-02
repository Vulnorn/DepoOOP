using System;
using System.Collections;
using System.Collections.Generic;

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

    class Train
    {
        protected List<Van> Vans = new List<Van>();

        public Train (int passengers)
        {
            Passengers = passengers;
        }

        public int Passengers { get; private set; }


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