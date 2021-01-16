using System;

namespace BlackjackLight
{
    class Program
    {
        static void Main(string[] args)
        {
            const double initialMoney = 100.00;

            double playerMoney = initialMoney;
            string name = "Unnamed";
            int age = 0;
            string playerRole = "Player";
            string playerSkillLevel = "Beginner";
            string favoriteCard = "Ace of Hearts";
            int totalGamesPlayed = 0;
            double playerEarnedMoney = 0.00;
            string playerNickname = "";

            int luckLevel = 1;
            double betSum = 0;
            int betPrizeCoeficient;

            Console.WriteLine("Please insert your name and press <Enter>:");
            name = Console.ReadLine();

            Console.WriteLine("Please insert your age and press <Enter>:");
            age = int.Parse(Console.ReadLine());

            Console.WriteLine("Please insert your nickname and press <Enter>:");
            playerNickname = Console.ReadLine();

            if (playerNickname == "")
            {
                playerNickname = "No nickname";
            }

            Console.WriteLine("Enter how many games you have played so far and press <Enter>:");
            totalGamesPlayed = int.Parse(Console.ReadLine());

            if (totalGamesPlayed < 50)
            {
                playerSkillLevel = "Beginner";
                betPrizeCoeficient = 4;
            }
            else if (totalGamesPlayed < 100)
            {
                playerSkillLevel = "Intermediate";
                betPrizeCoeficient = 3;
            }
            else if (totalGamesPlayed < 150)
            {
                playerSkillLevel = "Advanced";
                betPrizeCoeficient = 2;
            }
            else
            {
                playerSkillLevel = "Expert";
                betPrizeCoeficient = 2;
            }

            Console.Title = "BlackJackLight";
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  .-----------. ");
            Console.WriteLine(" /------------/|");
            Console.WriteLine("/.-----------/||");
            Console.WriteLine("| ♥       ♥  |||");
            Console.WriteLine("| BlackJack  |||");
            Console.WriteLine("|            |||");
            Console.WriteLine("|     ♥      |||");
            Console.WriteLine("|            |||");
            Console.WriteLine("| The Game   |||");
            Console.WriteLine("| ♥       ♥  ||/");
            Console.WriteLine("\\-----------./  ");
            Console.WriteLine("");
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine($"| {playerSkillLevel} | {playerRole} | {name} {age} |  {playerNickname} |");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine($"Hello {name}");
            Console.WriteLine($"{name}, your money count is: {playerMoney}$");
            Console.WriteLine("1. New game");
            Console.WriteLine("2. Reset stats");
            Console.WriteLine("3. Options");
            Console.WriteLine("4. Stats");
            Console.WriteLine("5. Credits");
            Console.WriteLine("6. Exit");

            Console.WriteLine("\nPlease type in menu option number and press <Enter>");
            string selectedMenuOption = Console.ReadLine();

            switch (selectedMenuOption)
            {
                case "1":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("How much $ would you like to bet?");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("");
                    betSum = double.Parse(Console.ReadLine());
                    
                    if(betSum > playerMoney)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Insufficient money..");
                        Console.WriteLine("Exiting the game..");
                        return;
                    }

                    playerMoney = playerMoney - betSum;

                    Console.WriteLine("Shuffling the deck..");
                    Console.WriteLine("Done shuffling the deck.");
                    Console.WriteLine("Serving the cards");

                    var randomGenerator = new Random();
                    var firstCardScore = randomGenerator.Next(1, 10);
                    var secondCardScore = randomGenerator.Next(1, 10);
                    var thirdCardScore = 0;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Your first card score is: {firstCardScore}");
                    Console.WriteLine($"Your second card score is: {secondCardScore}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Would like to get served another card?\n1. Yes 2. No");
                    var shouldDeal = Console.ReadLine();

                    if (shouldDeal == "1")
                    {
                        thirdCardScore = randomGenerator.Next(1, 10);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Your third card score is: {thirdCardScore}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    var totalCardScore = firstCardScore + secondCardScore + thirdCardScore;

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Total card score: {totalCardScore}");
                    Console.ForegroundColor = ConsoleColor.White;

                    if (totalCardScore > 21)
                    {
                        Console.WriteLine($"You lost {betSum}$\nYour current balance: {playerMoney}$");
                        Console.WriteLine("Game over..\n\nPress any key to quit");
                        Console.ReadKey();
                        return;
                    }

                    var dealerHand = randomGenerator.Next(10, 21);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Your dealer total card score: {dealerHand}");

                    if (totalCardScore <= dealerHand)
                    {
                        Console.WriteLine($"You lost {betSum}$\nYour current balance: {playerMoney}$");
                        Console.WriteLine("Dealer won! Game over..\n\nPress any key to quit");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                        return;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    playerMoney = playerMoney + betSum * betPrizeCoeficient;
                    Console.WriteLine($"You won {betSum * betPrizeCoeficient}\nYour current balance: {playerMoney}");
                    Console.WriteLine("Congratulations!!\nYou won!!\n\nPress any key to quit");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                    return;
                case "2":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Are you sure you want to reset your stat?\n1. Yes\n2. No");
                    Console.ForegroundColor = ConsoleColor.White;
                    string promptAnswer = Console.ReadLine();
                    if (promptAnswer == "1")
                    {
                        totalGamesPlayed = 0;
                        playerMoney = initialMoney;
                        playerSkillLevel = "Beginner";

                        Console.WriteLine("Stats were reset");
                    }
                    break;
                case "3":
                    Console.WriteLine("-----------");
                    Console.WriteLine("| Options |");
                    Console.WriteLine("-----------");
                    Console.WriteLine("1. Luck modifier");
                    Console.WriteLine("2. Favorite card");

                    Console.WriteLine("\nPlease input option setting you want to change and press <Enter>:");
                    var optionSelected = Console.ReadLine();

                    switch(optionSelected)
                    {
                        case "1":
                            var oldLuckModifier = luckLevel;
                            Console.WriteLine($"Your current luck modifier: {luckLevel}");
                            Console.WriteLine("Please insert your luck modifier you want and press <Enter>:");
                            luckLevel = int.Parse(Console.ReadLine());
                            Console.WriteLine($"Luck modifier changed from: {oldLuckModifier} to: {luckLevel}");
                            break;
                        case "2":
                            var oldFavoriteCard = favoriteCard;
                            Console.WriteLine($"Your current favorite card: {favoriteCard}");
                            Console.WriteLine("Please insert your favorite card and press <Enter>:");
                            favoriteCard = Console.ReadLine();
                            Console.WriteLine($"Favorite card changed from: {oldFavoriteCard} to: {favoriteCard}");
                            break;
                    }
                    break;
                case "4":
                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine($"| Player skill level/group: {playerSkillLevel}");
                    Console.WriteLine($"| Player role: {playerRole}");
                    Console.WriteLine($"| Player name: {name}");
                    Console.WriteLine($"| Player age: {age}");
                    Console.WriteLine($"| Player nickname: {playerNickname}");
                    Console.WriteLine($"| Player money: {playerMoney}");
                    Console.WriteLine($"| Player money: {playerEarnedMoney}");
                    Console.WriteLine($"| Player favorite card: {favoriteCard}");
                    Console.WriteLine("---------------------------------------------");
                    break;
                case "5":
                    Console.WriteLine("--------------------------------------------------------------------------------------");
                    Console.WriteLine($"Game developer: Edvinas (DeveloperJourney)");
                    Console.WriteLine("--------------------------------------------------------------------------------------");
                    break;
                case "6":
                    Console.WriteLine("Exiting Blackjack");
                    return;
            }

            Console.WriteLine("Exiting Blackjack");
            Console.ReadKey();
        }
    }
}
