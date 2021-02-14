﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackLight
{
    class Program
    {
        const double initialMoney = 100.00;

        static string[] cardSuits = { "♥", "♦", "♣", "♠" };
        static string[] playingCards = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };

        static bool isGameRunning = true;

        static List<int> playerCardScores = new List<int>();
        static List<int> dealerCardScores = new List<int>();

        static double playerMoney = initialMoney;
        static string name = "Unnamed";
        static int age = 0;
        static string playerRole = "Player";
        static string playerSkillLevel = "Beginner";
        static string favoriteCard = "Ace of Hearts";
        static int totalGamesPlayed = 0;
        static string playerNickname = "";

        static int currentWinningStreak = 0;
        static int bestWinningStreak = 0;

        static int playerTotalCardScore = 0;
        static int dealerTotalCardScore = 0;

        static int bettingAmount = 0;

        static void Main(string[] args)
        {
            SetInitialPlayerInformation(true);

            if (playerNickname == "")
            {
                playerNickname = "No nickname";
            }

            Console.Title = "BlackJackLight";

            while(isGameRunning)
            {
                PrintLogo();
                PrintPlayerMenuInfo();
                PrintMenu();
                HandleGame();

                Console.Clear();
            }
        }

        private static void HandleGame()
        {
            Console.WriteLine("\nPlease type in menu option number and press <Enter>");
            string selectedMenuOption = Console.ReadLine();

            switch (selectedMenuOption)
            {
                case "1":
                    HandleNewRound();
                    break;
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
                    isGameRunning = false;
                    break;
            }
        }

        private static void HandleNewRound()
        {
            PrepareNewRound();
            SetBetAmount();
            PrintNewGameMessage();

            if (!IsBetValid())
            {
                PrintInvalidBetMessage();
            }

            HitCard("Dealer");

            bool canHit = true;

            while(canHit)
            {
                HitCard();
                canHit = CanHitAgain();
            }

            while(dealerTotalCardScore < 17)
            {
                HitCard("Dealer");
            }

            PrintTotalScore();
            PrintTotalScore("Dealer");
            CalculateRoundResult();
        }

        private static bool CanHitAgain()
        {
            if(playerTotalCardScore < 21)
            {
                Console.WriteLine("Do you want to hit again?\n1. Yes 2. No");
                var hitAgain = Console.ReadLine();

                if(hitAgain == "1")
                {
                    return true;
                }
            }

            return false;
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
                currentWinningStreak = 0;
                playerMoney -= bettingAmount;
                PrintRoundLost();
            }
            else
            {
                double wonBonusAmount = bettingAmount * 0.05 * currentWinningStreak;
                double wonAmount = bettingAmount + wonBonusAmount;

                currentWinningStreak++;

                if (bestWinningStreak < currentWinningStreak)
                {
                    bestWinningStreak = currentWinningStreak;
                }

                playerMoney += wonAmount;
                PrintRoundWon(wonAmount);
            }
        }

        private static void PrintRoundWon(double wonAmount)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Congratulations!!\nYou won {wonAmount}$!!\nYour current money: {playerMoney}$\n\nPress any key to continue..");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        private static void PrintRoundLost()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Dealer won! You lost {bettingAmount}$..\nYour current money: {playerMoney}$\n\nPress any key to continue..");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        /// <summary>
        /// This methods prints out the total score
        /// </summary>
        /// <param name="pullerRole">This is a parameter meant to indicate the role of the puller</param>
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
            playerCardScores.Clear();
            dealerCardScores.Clear();
            playerTotalCardScore = 0;
            dealerTotalCardScore = 0;
            bettingAmount = 0;
        }

        private static void PrintNewGameMessage()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Shuffling the deck..");
            Console.WriteLine("Done shuffling the deck.");
            Console.WriteLine("Serving the cards");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void HitCard(string pullerRole = "Player")
        {
            var randomGenerator = new Random();
            var cardSuit = cardSuits[randomGenerator.Next(cardSuits.Length)];

            var playingCardIndex = randomGenerator.Next(cardSuits.Length);
            var playingCard = playingCards[playingCardIndex];
            int cardScore;

            if(playingCardIndex == 0)
            {
                cardScore = 11;
            }
            else if(playingCardIndex < 9)
            {
                cardScore = playingCardIndex + 1;
            }
            else
            {
                cardScore = 10;
            }

            if(pullerRole == "Player")
            {
                playerCardScores.Add(cardScore);
                Console.ForegroundColor = ConsoleColor.Green;
                CalculateCardHit();
            }
            else
            {
                dealerCardScores.Add(cardScore);
                Console.ForegroundColor = ConsoleColor.Red;
                CalculateCardHit("Dealer");
            }

            
            Console.WriteLine($"{pullerRole} is drawing a card.. {playingCard} of {cardSuit} was drawn");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void CalculateCardHit(string pullerRole = "Player")
        {
            if (pullerRole == "Player")
            {
                playerTotalCardScore = CalculateCurrentTotalCardScore(playerCardScores);
            }
            else
            {
                dealerTotalCardScore = CalculateCurrentTotalCardScore(dealerCardScores);
            }
        }

        private static int CalculateCurrentTotalCardScore(List<int> cardScores)
        {
            var totalCardScore = cardScores.Sum();

            if(totalCardScore > 21)
            {
                var aceCard = cardScores.FirstOrDefault(cs => cs == 11);
                cardScores.Remove(aceCard);
                cardScores.Add(1);
                totalCardScore = cardScores.Sum();
            }

            return totalCardScore;
        }

        private static void ResetPlayerStats()
        {
            totalGamesPlayed = 0;
            currentWinningStreak = 0;
            bestWinningStreak = 0;
            playerMoney = initialMoney;
            playerSkillLevel = "Beginner";

            Console.WriteLine("Stats were reset");
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
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

        private static void SetInitialPlayerInformation(bool setDefaultValues = false)
        {
            if(!setDefaultValues)
            {
                Console.WriteLine("Please insert your name and press <Enter>:");
                name = Console.ReadLine();

                Console.WriteLine("Please insert your age and press <Enter>:");
                age = int.Parse(Console.ReadLine());

                Console.WriteLine("Please insert your nickname and press <Enter>:");
                playerNickname = Console.ReadLine();

                SetPlayerSkillLevel();
            }
            else
            {
                name = "Edvinas";
                age = 26;
                playerNickname = "DeveloperJourney";
                playerSkillLevel = "Intermediate";
            }
        }

        private static void PrintCredits()
        {
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine($"Game developer: Edvinas (DeveloperJourney)");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
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
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }

        private static void PrintMenu()
        {
            Console.WriteLine("1. New round");
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
            Console.WriteLine($"| Current winning streak: {currentWinningStreak} (+{currentWinningStreak*5}% bonus) | Best winning streak: {bestWinningStreak} |");
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
