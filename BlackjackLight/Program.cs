﻿using System;

namespace BlackjackLight
{
    class Program
    {
        const double initialMoney = 100.00;

        static double playerMoney = initialMoney;
        static string name = "Unnamed";
        static int age = 0;
        static string playerRole = "Player";
        static string playerSkillLevel = "Beginner";
        static string favoriteCard = "Ace of Hearts";
        static int totalGamesPlayed = 0;
        static string playerNickname = "";

        static int playerTotalCardScore = 0;
        static int dealerTotalCardScore = 0;

        static int bettingAmount = 0;

        static void Main(string[] args)
        {
            SetInitialPlayerInformation();

            if (playerNickname == "")
            {
                playerNickname = "No nickname";
            }

            SetPlayerSkillLevel();

            Console.Title = "BlackJackLight";

            PrintLogo();
            PrintPlayerMenuInfo();
            PrintMenu();

            Console.WriteLine("\nPlease type in menu option number and press <Enter>");
            string selectedMenuOption = Console.ReadLine();

            switch (selectedMenuOption)
            {
                case "1":
                    PrepareNewRound();
                    SetBetAmount();

                    if(!IsBetValid())
                    {
                        PrintInvalidBetMessage();
                        return;
                    }

                    var firstCardScore = HitCard();
                    var secondCardScore = HitCard();
                    var thirdCardScore = 0;

                    var firstDealerCard = HitCard("Dealer");
                    var secondDealerCard = HitCard("Dealer");


                    Console.WriteLine($"Would like to get served another card?\n1. Yes 2. No");
                    var shouldDeal = Console.ReadLine();

                    if (shouldDeal == "1")
                    {
                        thirdCardScore = HitCard();
                    }

                    PrintTotalScore();
                    PrintTotalScore("Dealer");
                    CalculateRoundResult();

                    return;
                case "2":
                    Console.WriteLine("Are you sure you want to reset your stat?\n1. Yes\n2. No");
                    string promptAnswer = Console.ReadLine();
                    if (promptAnswer == "1")
                    {
                        ResetPlayerStats();
                    }
                    break;
                case "3":
                    PrintStats();
                    break;
                case "4":
                    PrintCredits();
                    break;
                case "5":
                    Console.WriteLine("Exiting Blackjack");
                    return;
            }

            Console.ReadKey();
        }

        private static void PrintInvalidBetMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nI am sorry, but you have insufficient funds..");
            Console.WriteLine("Press any key to quit..");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        private static bool IsBetValid()
        {
            return bettingAmount <= playerMoney;
        }

        private static void SetBetAmount()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\nType in how much you are willing to bet?");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"(You currently have: {playerMoney}$)");
            bettingAmount = int.Parse(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void CalculateRoundResult()
        {
            if (playerTotalCardScore > 21 || playerTotalCardScore <= dealerTotalCardScore)
            {
                playerMoney -= bettingAmount;
                PrintRoundLost();
            }

            playerMoney += bettingAmount;
            PrintRoundWon();
        }

        private static void PrintRoundWon()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Congratulations!!\nYou won {bettingAmount}$!!\nYour current money: {playerMoney}$\n\nPress any key to quit");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        private static void PrintRoundLost()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Dealer won! You lost {bettingAmount}$..\nYour current money: {playerMoney}$\n\nPress any key to quit");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        private static void PrintTotalScore(string pullerRole = "Player")
        {
            if (pullerRole == "Player")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{pullerRole} total card score: {playerTotalCardScore}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{pullerRole} total card score: {dealerTotalCardScore}");
            }
            
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PrepareNewRound()
        {
            PrintNewGameMessage();

            playerTotalCardScore = 0;
            dealerTotalCardScore = 0;
            bettingAmount = 0;
        }

        private static void PrintNewGameMessage()
        {
            Console.WriteLine("Shuffling the deck..");
            Console.WriteLine("Done shuffling the deck.");
            Console.WriteLine("Serving the cards");
        }

        private static int HitCard(string pullerRole = "Player")
        {
            var randomGenerator = new Random();
            var cardScore = randomGenerator.Next(1, 10);

            if(pullerRole == "Player")
            {
                playerTotalCardScore += cardScore;
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                dealerTotalCardScore += cardScore;
                Console.ForegroundColor = ConsoleColor.Red;
            }

            
            Console.WriteLine($"{pullerRole} is drawing a card.. Card score is: {cardScore}");
            Console.ForegroundColor = ConsoleColor.White;

            return cardScore;
        }

        private static void ResetPlayerStats()
        {
            totalGamesPlayed = 0;
            playerMoney = initialMoney;
            playerSkillLevel = "Beginner";

            Console.WriteLine("Stats were reset");
        }

        private static void SetPlayerSkillLevel()
        {
            Console.WriteLine("Enter how many games you have played so far and press <Enter>:");
            totalGamesPlayed = int.Parse(Console.ReadLine());

            if (totalGamesPlayed < 50)
            {
                playerSkillLevel = "Beginner";
            }
            else if (totalGamesPlayed < 100)
            {
                playerSkillLevel = "Intermediate";
            }
            else if (totalGamesPlayed < 150)
            {
                playerSkillLevel = "Advanced";
            }
            else
            {
                playerSkillLevel = "Expert";
            }
        }

        private static void SetInitialPlayerInformation()
        {
            Console.WriteLine("Please insert your name and press <Enter>:");
            name = Console.ReadLine();

            Console.WriteLine("Please insert your age and press <Enter>:");
            age = int.Parse(Console.ReadLine());

            Console.WriteLine("Please insert your nickname and press <Enter>:");
            playerNickname = Console.ReadLine();
        }

        private static void PrintCredits()
        {
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine($"Game developer: Edvinas (DeveloperJourney)");
            Console.WriteLine("--------------------------------------------------------------------------------------");
        }

        private static void PrintStats()
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"| Player skill level/group: {playerSkillLevel}");
            Console.WriteLine($"| Player role: {playerRole}");
            Console.WriteLine($"| Player name: {name}");
            Console.WriteLine($"| Player age: {age}");
            Console.WriteLine($"| Player nickname: {playerNickname}");
            Console.WriteLine($"| Player money: {playerMoney}");
            Console.WriteLine($"| Player favorite card: {favoriteCard}");
            Console.WriteLine("---------------------------------------------");
        }

        private static void PrintMenu()
        {
            Console.WriteLine("1. New game");
            Console.WriteLine("2. Reset stats");
            Console.WriteLine("3. Stats");
            Console.WriteLine("4. Credits");
            Console.WriteLine("5. Exit");
        }

        private static void PrintPlayerMenuInfo()
        {
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine($"| {playerSkillLevel} | {playerRole} | {name} {age} |  {playerNickname} |");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine($"Hello {name}");
            Console.WriteLine($"{name}, your money count is: {playerMoney}$");
        }

        private static void PrintLogo()
        {
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
        }
    }
}
