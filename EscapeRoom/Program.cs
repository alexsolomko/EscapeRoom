﻿using static System.Console;

namespace EscapeRoom
{
    internal class Program
    {
        static int roomWidth = 22;
        static int roomHeight = 11;
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
            InitializeRoom();
            DisplayRoom();
        }
        #region InitializeRoom
        static void InitializeRoom()
        {
            room = new char[roomWidth, roomHeight];
        
            // Raumgrenzen festlegen
            for (int x = 0; x < roomWidth; x++)
            {
                room[x, 0] = '#';                   // Obere Wand
                room[x, roomHeight - 1] = '#';      // Untere Wand
            }
            for (int y = 0; y < roomHeight; y++)
            {
                room[0, y] = '#';                   // Linke Wand
                room[roomWidth - 1, y] = '#';       // Rechte Wand
            }

            // Boden festlegen
            for (int x = 1; x < roomWidth - 1; x++)
            {
                for (int y = 1; y < roomHeight - 1; y++)
                {
                    room[x, y] = ' ';
                }
            }

            // Spieler platzieren
            playerX = rand.Next(1, roomWidth - 1);
            playerY = rand.Next(1, roomHeight - 1);
            room[playerX, playerY] = '@';

            // Schlüssel platzieren
            keyX = rand.Next(1, roomWidth -1);
            keyY = rand.Next(1, roomHeight -1);
            room[keyX, keyY] = '$';

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
            room[doorX, doorY] = '+';
        }
        #endregion

        #region DisplayRoom()
        static void DisplayRoom()
        {
            for (int y = 0; y < roomHeight; y++)
            {
                for (int x = 0; x < roomWidth; x++)
                {
                    SetCursorPosition(x, y);

                    if (room[x, y] == '#')
                    {
                        ForegroundColor = ConsoleColor.Blue;
                        Write('#');
                    }
                    else if (room[x, y] == '@')
                    {
                        ForegroundColor = ConsoleColor.Yellow;
                        WriteLine('@');
                    }
                    else if (room[x, y] == '$')
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine('$');
                    }
                    else if (room[x, y] == '+')
                    {
                        ForegroundColor = ConsoleColor.Green;
                        WriteLine('+');
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Blue;
                        Write(' ');
                    }
                }
            }
            ResetColor();
        }
        #endregion

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

        }

    }
}