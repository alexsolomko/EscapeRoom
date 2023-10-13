using System;
using System.Reflection;
using System.Reflection.Emit;

namespace EscapeRoom
{
    internal class Program
    {
        const char Wall = '#';         // Wand Teil
        const char Player = 'P';       // Spieler
        const char Key = 'K';          // Schlüssel
        const char Door = 'D';         // Tür
        const char EmptySpace = '·';   // Leerfläche Teil
        const int sizeWidth = 70;      // Window/Buffer Width
        const int sizeHeight = 20;     // Window/Buffer Height

        static string labelName = "Escape Room von Honey Sky"; // Titel des Konsolenfensters
        static string startLabel = @"
     ______                            _____                       
    |  ____|                          |  __ \                      
    | |__   ___  ___ __ _ _ __   ___  | |__) |___   ___  _ __ ___  
    |  __| / __|/ __/ _` | '_ \ / _ \ |  _  // _ \ / _ \| '_ ` _ \ 
    | |____\__ \ (_| (_| | |_) |  __/ | | \ \ (_) | (_) | | | | | |
    |______|___/\___\__,_| .__/ \___| |_|  \_\___/ \___/|_| |_| |_|
                         | |                                       
                         |_|              

";
        static string studioName = "von Honey Sky";
        static string pressAnyKey = "Drücke eine beliebige Taste, um das Spiel zu starten";
        static string welcome = "Willkommen!\n\n";
        static string enterText = $"           Du bist die Spielfigur ({Player})" + '\n' + $"           Dein Ziel ist es, den Schlüssel ({Key}) zu finden" + '\n' + $"           und die Tür ({Door}) zu öffnen" + '\n' + $"           Bewege dich mit den Pfeiltasten";
        static string sliderRegelAnfang = $"Verwende die Pfeiltasten, um die ";
        static string sliderRegelEnde = $" des Raums auszuwählen:";
        static string enterKey = "Drücken Sie zur Bestätigung die Eingabetaste";
        static string win = @"





                    __          __  _           _ 
                    \ \        / / (_)         | |
                     \ \  /\  / /   _   _ __   | |
                      \ \/  \/ /   | | | '_ \  | |
                       \  /\  /    | | | | | | |_|
                        \/  \/     |_| |_| |_| (_)
                                                  
                                                  
";

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
        static Random rand = new Random(); // Gott des Zufalls

        static void Main(string[] args)
        {
            WindowProperties();         // Window/Buffer Eigenschaften
            ShowStartLabel();           // Titelgrafik
            HelloText();                // Hello Text
            GetRoomDimensions();        // Ruft die Methode auf, um die Raumabmessungen festzulegen
            InitializeRoom();           // Ruft die Methode auf, um den Raum zu initialisieren
            PlayGame();                 // Ruft die Methode auf, um das Spiel zu starten
        }
        static void WindowProperties()
        {
            Console.WindowWidth = sizeWidth;
            Console.WindowHeight = sizeHeight;

            Console.BufferWidth = sizeWidth;
            Console.BufferHeight = sizeHeight;

            Console.CursorVisible = false;
        }
        static void ShowStartLabel()
        {
            Console.Title = labelName;                      // Titel des Konsolenfensters
            Console.ForegroundColor = ConsoleColor.Yellow;  // Textfarbe auf Gelb setzen
            Console.Write(startLabel);
            Console.ForegroundColor= ConsoleColor.White;
            CenterText(studioName);                         // Zentriert den Text "von Honey Sky"
            Console.ResetColor();
            Console.Write("\n\n\n\n\n");
            Console.ForegroundColor = ConsoleColor.Green;
            CenterText(pressAnyKey);                        // Zentriert den Text "(Drücken Sie eine beliebige Taste, um zu spielen)"
            Console.ResetColor();                           // Zurücksetzen der Textfarbe auf Standard
            Console.ReadKey();
            Console.Clear();
        }
        static void HelloText()
        {
            Console.WriteLine("\n\n\n");
            CenterText(welcome);
            Console.WriteLine(enterText);
            Console.WriteLine("\n\n");
            CenterText(pressAnyKey);
            Console.ReadKey();
            Console.Clear();
        }
        static void GetRoomDimensions()
        {
            roomWidth = GetSliderValue("Breite", 10, 40); // Ruft die Methode auf, um die Breite auszuwählen
            roomHeight = GetSliderValue("Höhe", 10, 20); // Ruft die Methode auf, um die Höhe auszuwählen
        }
        static int GetSliderValue(string _label, int _minValue, int _maxValue)
        {
            int optionVal = _minValue; // Anfangswert der Option
            int optionStep = 1; // Schrittweite der Option

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n\n\n\n\n");
                CenterText(sliderRegelAnfang + $"{_label}"  + sliderRegelEnde);                                                           //$"Verwende die Pfeiltasten, um die {_label} des Raums auszuwählen:"
                Console.WriteLine('\n');
                CenterText(RenderSettingSlider(optionVal, _minValue, _maxValue));        // Ruft die Methode auf, um den Schieberegler anzuzeigen
                Console.WriteLine('\n');
                CenterText(enterKey);
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.LeftArrow && optionVal > _minValue)
                {
                    optionVal -= optionStep;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow && optionVal < _maxValue)
                {
                    optionVal += optionStep;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return optionVal;
                }
            }
        }
        static string RenderSettingSlider(int _optionVal, int _minValue, int _maxValue)
        {
            string optionValStr; // Fügt eine Null vor Zahlen kleiner als 10 hinzu
            if (_optionVal < 10)
            {
                optionValStr = $"0{_optionVal}";
            }
            else
            {
                optionValStr = $"{_optionVal}";
            }

            string sliderSec = "═"; // Slider-Abschnitt
            string sliderStr = "";
            
            sliderStr += "■"; // Setzt "Rahmen" am Anfang des Schiebereglers

            // Schieberegler erstellen
            for (int i = _minValue; i <= _maxValue; i++)
            {
                if (i == _optionVal)
                { 
                    sliderStr += $"╣{optionValStr}╠";       // Fügt den Daumen zum Schieberegler hinzu
                }
                else
                { // Füllt den Rest des Schiebereglers mit Slider-Abschnitten
                    sliderStr += sliderSec;
                }
            }

            sliderStr += "■"; // Setzt "Rahmen" am Ende des Schiebereglers

            return sliderStr;
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
                Console.Clear();
                Console.CursorVisible = false;
                DisplayRoom();
                if (hasKey && playerX == doorX && playerY == doorY)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(win);
                    Console.ReadKey();
                    Console.ResetColor();
                    Console.Clear();
                    break;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                HandleInput(keyInfo.Key);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
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
                    Console.SetCursorPosition(x, y);

                    if (room[x, y] == Wall)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(Wall);
                    }
                    else if (room[x, y] == Player)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(Player);
                    }
                    else if (room[x, y] == Key)
                    {
                        if (!hasKey)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Key);
                        }
                        else
                        {
                            Console.Write(EmptySpace);
                        }
                    }
                    else if (room[x, y] == Door)
                    {
                        if (!hasKey)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Door);
                        }
                        else
                        {
                            Console.Write(EmptySpace);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(EmptySpace);
                    }
                }
            }
            Console.ResetColor();
        }
        static void HandleInput(ConsoleKey _key)
        {
            int newX = playerX;
            int newY = playerY;

            switch (_key)
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
        static bool IsValidMove(int _x, int _y)
        {
            char destination = room[_x, _y];
            return destination != Wall && (destination != Door || hasKey);
        }
        static void CenterText(string _text)
        {
            Console.Write(new string(' ', (Console.WindowWidth - _text.Length) / 2));
            Console.WriteLine(_text);
        }
        static void BeepSound() => Console.Beep(400, 500);
        static void BeepSoundWall() => Console.Beep(1000, 300);
    }
}
