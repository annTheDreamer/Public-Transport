using System;
using System.Collections.Generic;
using System.Text;

namespace Public_Transport.UI
{
    public class ConsoleUI
    {
        private Dictionary<int, string> _menuOptions = new Dictionary<int, string>
        {
            { 1, "Add Vehicle" },
            { 2, "See All Vehicles" },
            { 3, "Refuel Vehicle" },
            { 4, "Charge Vehicle" },
            { 5, "Exit" },
        };

        public string GetUserInput() => Console.ReadLine() ?? "";

        public void PrintWelcomeMessage() =>
            Console.WriteLine("Welcome to the Fleet Management Project.");

        public void PrintMenuOptions()
        {
            Console.WriteLine(
                "Please select one of the options by typing the number before the option you'd like to select:"
            );
            foreach (var menuOption in _menuOptions)
            {
                Console.WriteLine($"{menuOption.Key}. {menuOption.Value}");
            }
        }
    }
}
