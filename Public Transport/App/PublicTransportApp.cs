using System;
using System.Collections.Generic;
using System.Text;
using Public_Transport.UI;

namespace Public_Transport.App
{
    public class PublicTransportApp
    {
        private ConsoleUI _consoleUi;
        private Dictionary<int, string> _menuOptions;

        public PublicTransportApp(ConsoleUI consoleUi)
        {
            _consoleUi = consoleUi;
            _menuOptions = _consoleUi.GetMenuOptions();
        }

        public void Start()
        {
            var userInput = string.Empty;
            _consoleUi.PrintWelcomeMessage();
            while (true)
            {
                Console.WriteLine();
                _consoleUi.PrintMenuOptions();
                Console.WriteLine();
                userInput = _consoleUi.GetUserInput();
                var userChoice = SelectedMenuOption(userInput);

                if (userChoice is null || !_menuOptions.ContainsKey(userChoice.Value))
                    Console.WriteLine(
                        "Invalid choice. Please choose one of the options printed above by selecting its number."
                    );
                else
                {
                    switch (userChoice.Value)
                    {
                        case 1:
                            _consoleUi.AddVehicle();
                            break;
                        case 2:
                            Console.WriteLine("User wants to see all vehicles");
                            break;
                        case 3:
                            Console.WriteLine("User wants to refuel a vehicle");
                            break;
                        case 4:
                            Console.WriteLine("User wants to recharge a vehicle");
                            break;
                        case 5:
                            Console.WriteLine("Goodbye, see you soon!");
                            return;
                    }
                }
            }
        }

        public int? SelectedMenuOption(string userInput)
        {
            bool isParsable = int.TryParse(userInput, out int result);
            if (isParsable)
                return result;
            else
                return null;
        }
    }
}
