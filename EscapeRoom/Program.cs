using System.Reflection.Metadata.Ecma335;
using static System.Console;

namespace EscapeRoom
{
    internal class Program
    {
        static string startLabel;               // Start Label
        static string studioName = "by Honey Sky";
        static string pressAnyKey = "(press any key to play)";
        static int roomWidth;                   // Breite des Raums
        static int roomHeight;                  // Höhe des Raums
        static char[,] room;                    // Das Spielfeld
        static int playerX;                     // X-Position der Spielfigur
        static int playerY;                     // Y-Position der Spielfigur
        static int keyX;                        // X-Position des Schlüssels
        static int keyY;                        // Y-Position des Schlüssels
        static int doorX;                       // X-Position der Tür
        static int doorY;                       // Y-Position der Tür
        static bool hasKey = false;             // Gibt an, ob die Spielfigur den Schlüssel hat
        static Random rand = new Random();

        // Soundeffekte
        static void BeepSound() => Beep(400, 500);             // Hz für ms
        static void BeepSoundWall() => Beep(1000, 300);        // Hz für ms
        #region Program start
        static void Main(string[] args)
        {
            Console.SetWindowSize(70, 20);
            Console.SetBufferSize(70, 20);

            CursorVisible = false;

            #region Start Label
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
            
            Title = "Escape Room by Honey Sky";         // Label text
            ForegroundColor = ConsoleColor.Yellow;
            Write(startLabel);
            centerText(studioName);                     //Zentrirte Text
            centerText(pressAnyKey);                    //Zentrierte Text
            ResetColor();
            ReadKey();
            Clear();
            #endregion Start Label

            WriteLine("Willkommen!\n\n");
            WriteLine("Du bist die Spielfigur (@). Dein Ziel ist es, den Schlüssel ($) zu finden und die Tür (+) zu öffnen.");
            WriteLine("Bewege dich mit den Pfeiltasten. \nDrücke Enter, um das Spiel zu starten.");

            ReadKey();
            Clear();

            // Raumgröße festlegen
            Write("Geben Sie die Breite des Raums ein: ");      // Breite x
            roomWidth = int.Parse(ReadLine());

            Write("Geben Sie die Höhe des Raums ein: ");        // Höhe y
            roomHeight = int.Parse(ReadLine());


            InitializeRoom();

            // Spiel start
            while (true)
            {
                Clear();
                CursorVisible = false;
                DisplayRoom();
                if (hasKey && playerX == doorX && playerY == doorY)
                {
                    Clear();
                    WriteLine("Win");
                    ReadKey();
                    break;
                }

                ConsoleKeyInfo keyInfo = ReadKey();
                HandleInput(keyInfo.Key);

                // Programmabbruch bei Escape Taste betätigung
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }

            }

        }

        #endregion

        static void centerText(String text)
        {
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
        }

        #region InitializeRoom()
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

            // Spieler platzieren
            playerX = rand.Next(1, roomWidth - 1);
            playerY = rand.Next(1, roomHeight - 1);
            room[playerX, playerY] = '@';

            // Schlüssel platzieren
            keyX = rand.Next(1, roomWidth - 1);
            keyY = rand.Next(1, roomHeight - 1);
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
                        Write('@');
                    }
                    else if (room[x, y] == '$')
                    {
                        if (!hasKey)
                        {
                            ForegroundColor = ConsoleColor.Red;
                            Write('$');
                        }
                        else
                        {
                            Write('·');
                        }
                    }
                    else if (room[x, y] == '+')
                    {
                        if (!hasKey)
                        {
                            ForegroundColor = ConsoleColor.Green;
                            Write('+');
                        }
                        else
                        {
                            Write(' ');
                        }

                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Blue;
                        Write('·');
                    }
                }
            }
            ResetColor();
        }
        #endregion

        #region Key controller
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
                room[playerX, playerY] = '·';
                playerX = newX;
                playerY = newY;
                room[playerX, playerY] = '@';

                if (playerX == keyX && playerY == keyY)
                {
                    hasKey = true;
                    BeepSound();                            // Spielsound für das gesammelte Schlüssel
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
            return destination != '#' && (destination != '+' || hasKey);
        }
        #endregion
    }
}