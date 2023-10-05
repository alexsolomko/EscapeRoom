using System;
using static System.Console;

namespace EscapeRoom
{
    internal class Program
    {
        const char Wall = '#';
        const char Player = '@';
        const char Key = '$';
        const char Door = '+';
        const char EmptySpace = '·';

        static string startLabel;
        static string studioName = "by Honey Sky";
        static string pressAnyKey = "(press any key to play)";
        static int roomWidth;
        static int roomHeight;
        static char[,] room;
        static int playerX;
        static int playerY;
        static int keyX;
        static int keyY;
        static int doorX;
        static int doorY;
        static bool hasKey = false;
        static Random rand = new Random();

        static void Main(string[] args)
        {
            Console.SetWindowSize(70, 20);
            Console.SetBufferSize(70, 20);
            CursorVisible = false;

            ShowStartLabel();

            WriteLine("Willkommen!\n\n");
            WriteLine("Du bist die Spielfigur (@). Dein Ziel ist es, den Schlüssel ($) zu finden und die Tür (+) zu öffnen.");
            WriteLine("Bewege dich mit den Pfeiltasten. \nDrücke Enter, um das Spiel zu starten.");

            ReadKey();
            Clear();

            GetRoomDimensions();

            InitializeRoom();

            PlayGame();
        }

        static void ShowStartLabel()
        {
            startLabel = @"
     
      ______                            _____                       
     |  ____|                          |  __ \                      
     | |__   ___  ___ __ _ _ __   ___  | |__) |___   ___  _ __ ___  
     |  __| / __|/ __/ _` | '_ \ / _ \ |  _  // _ \ / _ \| '_ ` _ \ 
     | |____\__ \ (_| (_| | |_) |  __/ | | \ \ (_) | (_) | | | | | |
     |______|___/\___\__,_| .__/ \___| |_|  \_\___/ \___/|_| |_| |_|
                          | |                                       
                          |_|                                       
";

            Title = "Escape Room by Honey Sky";
            ForegroundColor = ConsoleColor.Yellow;
            Write(startLabel);
            CenterText(studioName);
            CenterText(pressAnyKey);
            ResetColor();
            ReadKey();
            Clear();
        }

        static void CenterText(string text)
        {
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
        }

        static void GetRoomDimensions()
        {
            Write("Geben Sie die Breite des Raums ein: ");
            roomWidth = int.Parse(ReadLine());

            Write("Geben Sie die Höhe des Raums ein: ");
            roomHeight = int.Parse(ReadLine());
        }

        static void InitializeRoom()
        {
            room = new char[roomWidth, roomHeight];

            // Raumgrenzen festlegen
            for (int x = 0; x < roomWidth; x++)
            {
                room[x, 0] = Wall;                   // Obere Wand
                room[x, roomHeight - 1] = Wall;      // Untere Wand
            }
            for (int y = 0; y < roomHeight; y++)
            {
                room[0, y] = Wall;                   // Linke Wand
                room[roomWidth - 1, y] = Wall;       // Rechte Wand
            }

            // Spieler platzieren
            playerX = rand.Next(1, roomWidth - 1);
            playerY = rand.Next(1, roomHeight - 1);
            room[playerX, playerY] = Player;

            // Schlüssel platzieren
            keyX = rand.Next(1, roomWidth - 1);
            keyY = rand.Next(1, roomHeight - 1);
            room[keyX, keyY] = Key;

            int doorSide = rand.Next(4);
            switch (doorSide)
            {
                case 0: // Oben
                    doorX = rand.Next(1, roomWidth - 1);
                    doorY = 0;
                    break;
                case 1: // Unten
                    doorX = rand.Next(1, roomWidth - 1);
                    doorY = roomHeight - 1;
                    break;
                case 2: // Links
                    doorX = 0;
                    doorY = rand.Next(1, roomHeight - 1);
                    break;
                case 3: // Rechts
                    doorX = roomWidth - 1;
                    doorY = rand.Next(1, roomHeight - 1);
                    break;
            }
            room[doorX, doorY] = Door;
        }

        static void PlayGame()
        {
            while (true)
            {
                Clear();
                CursorVisible = false;
                DisplayRoom();
                if (hasKey && playerX == doorX && playerY == doorY)
                {
                    Clear();
                    WriteLine("Gewonnen!");
                    ReadKey();
                    break;
                }

                ConsoleKeyInfo keyInfo = ReadKey();
                HandleInput(keyInfo.Key);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }

        static void DisplayRoom()
        {
            for (int y = 0; y < roomHeight; y++)
            {
                for (int x = 0; x < roomWidth; x++)
                {
                    SetCursorPosition(x, y);

                    if (room[x, y] == Wall)
                    {
                        ForegroundColor = ConsoleColor.Blue;
                        Write(Wall);
                    }
                    else if (room[x, y] == Player)
                    {
                        ForegroundColor = ConsoleColor.Yellow;
                        Write(Player);
                    }
                    else if (room[x, y] == Key)
                    {
                        if (!hasKey)
                        {
                            ForegroundColor = ConsoleColor.Red;
                            Write(Key);
                        }
                        else
                        {
                            Write(EmptySpace);
                        }
                    }
                    else if (room[x, y] == Door)
                    {
                        if (!hasKey)
                        {
                            ForegroundColor = ConsoleColor.Green;
                            Write(Door);
                        }
                        else
                        {
                            Write(EmptySpace);
                        }
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Blue;
                        Write(EmptySpace);
                    }
                }
            }
            ResetColor();
        }

        static void HandleInput(ConsoleKey key)
        {
            int newX = playerX;
            int newY = playerY;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    newY--;
                    break;
                case ConsoleKey.DownArrow:
                    newY++;
                    break;
                case ConsoleKey.LeftArrow:
                    newX--;
                    break;
                case ConsoleKey.RightArrow:
                    newX++;
                    break;
            }

            if (IsValidMove(newX, newY))
            {
                room[playerX, playerY] = EmptySpace;
                playerX = newX;
                playerY = newY;
                room[playerX, playerY] = Player;

                if (playerX == keyX && playerY == keyY)
                {
                    hasKey = true;
                    BeepSound();
                }
            }
            else
            {
                BeepSoundWall();
            }
        }

        static bool IsValidMove(int x, int y)
        {
            char destination = room[x, y];
            return destination != Wall && (destination != Door || hasKey);
        }

        static void BeepSound() => Beep(400, 500);
        static void BeepSoundWall() => Beep(1000, 300);
    }
}
