using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlackjackLight
{
    class Program
    {
        const string PlayerDataFileName = "PlayerData.txt";
        const string DataSeparatorChar = ";";

        const double initialMoney = 100.00;

        const int playingCardsSize = 13;
        const int playingCardSuitsSize = 4;

        static string[][] deckCards = new string[][]
        {
            new string[] { "♥", "♦", "♣", "♠" },
            new string[] { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" },
        };
        static List<string> unavailableCardsPositions = new List<string>();

        static bool isGameRunning = true;

        static List<int> playerCardScores = new List<int>();
        static List<int> dealerCardScores = new List<int>();

        static double playerMoney = initialMoney;
        static string name = "Unnamed";
        static int age = 0;
        static string playerRole = "Player";
        static string playerSkillLevel = "Beginner";
        static string favoriteCard = "Ace of Hearts";
        static string playerNickname = "";
        static int totalGamesPlayed = 0;

        static int currentWinningStreak = 0;
        static int bestWinningStreak = 0;

        static int playerTotalCardScore = 0;
        static int dealerTotalCardScore = 0;

        static int bettingAmount = 0;

        static void Main(string[] args)
        {
            SetInitialPlayerInformation();

            Console.Title = "BlackJackLight";

            while (isGameRunning)
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

            while (canHit)
            {
                HitCard();
                canHit = CanHitAgain();
            }

            while (dealerTotalCardScore < 17)
            {
                HitCard("Dealer");
            }

            PrintTotalScore();
            PrintTotalScore("Dealer");
            CalculateRoundResult();
            EvaluatePlayerSkillLevel();
            UpdateStoragePlayerData();
        }

        private static bool CanHitAgain()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            if (playerTotalCardScore < 21)
            {
                Console.WriteLine("Do you want to hit again?\n1. Yes 2. No");
                var hitAgain = Console.ReadLine();

                if (hitAgain == "1")
                {
                    return true;
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
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
            totalGamesPlayed++;

            if (playerTotalCardScore == 21 && dealerTotalCardScore == 21)
            {
                playerMoney += bettingAmount;
                PrintRoundDraw();
            }
            else if (playerTotalCardScore > 21 || (playerTotalCardScore <= dealerTotalCardScore && dealerTotalCardScore <= 21))
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

        private static void PrintRoundDraw()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"It's a draw! {bettingAmount}$ was returned to your bank..\nYour current money: {playerMoney}$\n\nPress any key to continue..");
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

            var cardSuitIndex = 0;
            var playingCardIndex = 0;
            var cardPos = "";

            while(unavailableCardsPositions.Contains(cardPos))
            {
                cardSuitIndex = randomGenerator.Next(playingCardSuitsSize);
                playingCardIndex = randomGenerator.Next(playingCardsSize);
                cardPos = string.Concat(cardSuitIndex.ToString(), " ", playingCardIndex.ToString());
            }

            unavailableCardsPositions.Add(cardPos);

            var playingCard = string.Concat(deckCards[0][cardSuitIndex], deckCards[1][playingCardIndex], deckCards[0][cardSuitIndex]);

            int cardScore;
            int totalCardScore;
            List<int> cardScores;

            if (playingCardIndex == 0)
            {
                cardScore = 11;
            }
            else if (playingCardIndex < 9)
            {
                cardScore = playingCardIndex + 1;
            }
            else
            {
                cardScore = 10;
            }

            if (pullerRole == "Player")
            {
                playerCardScores.Add(cardScore);
                Console.ForegroundColor = ConsoleColor.Green;
                CalculateCardHit();
                totalCardScore = playerTotalCardScore;
                cardScores = playerCardScores;
            }
            else
            {
                dealerCardScores.Add(cardScore);
                Console.ForegroundColor = ConsoleColor.Red;
                CalculateCardHit("Dealer");
                totalCardScore = dealerTotalCardScore;
                cardScores = dealerCardScores;
            }

            Console.WriteLine($"\n{pullerRole} is drawing a card..");
            Console.Write("Current card scores: |");
            foreach (var card in cardScores)
            {
                Console.Write($" {card} |");
            }

            Console.WriteLine($"\n{pullerRole} drew - | {playingCard} | ({cardScore}).");
            Console.WriteLine($"[{pullerRole}] -> Current hand score: {totalCardScore}\n");
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

            if (totalCardScore > 21)
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

            File.Delete(PlayerDataFileName);
            SetInitialPlayerInformation();

            Console.WriteLine("Stats were reset");
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }

        private static void EvaluatePlayerSkillLevel()
        {
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
            if (!File.Exists(PlayerDataFileName))
            {
                SetNewPlayerInitialValues();
                SetupPlayerDataStorage();
            }
            else
            {
                SetPlayerDataFromStorage();
            }
        }

        private static void SetupPlayerDataStorage()
        {
            using (StreamWriter sw = File.CreateText(PlayerDataFileName))
            {
                var playerLineData = string.Join(DataSeparatorChar, name, age, playerSkillLevel, favoriteCard, playerNickname, playerMoney, totalGamesPlayed, currentWinningStreak, bestWinningStreak);

                sw.WriteLine("NAME AGE PLAYERROLE PLAYERSKILLLEVEL FAVORITECARD PLAYERNICKNAME PLAYERMONEY TOTALGAMESPLAYED CURRENTWINNINGSTREAK BESTWINNINGSTREAK");
                sw.WriteLine(playerLineData);
            }
        }

        private static void UpdateStoragePlayerData()
        {
            File.Delete(PlayerDataFileName);
            SetupPlayerDataStorage();
        }

        private static void SetNewPlayerInitialValues()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

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

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void SetPlayerDataFromStorage()
        {
            using (StreamReader sr = File.OpenText(PlayerDataFileName))
            {
                string headerLine = sr.ReadLine();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var separatedData = line.Split(DataSeparatorChar);

                    name = separatedData[0];
                    age = int.Parse(separatedData[1]);
                    playerSkillLevel = separatedData[2];
                    favoriteCard = separatedData[3];
                    playerNickname = separatedData[4];
                    playerMoney = double.Parse(separatedData[5]);
                    totalGamesPlayed = int.Parse(separatedData[6]);
                    currentWinningStreak = int.Parse(separatedData[7]);
                    bestWinningStreak = int.Parse(separatedData[8]);
                }
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
            Console.WriteLine($"| Player total games played: {totalGamesPlayed}");
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
            Console.WriteLine("\n1. New round");
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
            Console.WriteLine($"| Current winning streak: {currentWinningStreak} (+{currentWinningStreak * 5}% bonus) | Best winning streak: {bestWinningStreak} |");
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
