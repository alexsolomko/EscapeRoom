using System;
using System.Collections.Generic;
using System.Threading;

namespace EscapeRoom
{
    internal class Program
    {
        #region Variables

        #region Colors
        // List of console colors
        static List<ConsoleColor> colors = new List<ConsoleColor>
        {
            ConsoleColor.Red,
            ConsoleColor.Green,
            ConsoleColor.Blue,
            ConsoleColor.Yellow,
            ConsoleColor.White,
            ConsoleColor.Gray
        };

        // ANSI colors
        static string colorRed = "\u001b[31;1m"; // Red color
        static string colorYellow = "\u001b[33;1m"; // Yellow color
        static string colorReset = "\u001b[0m"; // Reset color
        #endregion

        #region Map Chars
        // Map characters
        const char wall = '█'; // Wall piece
        const char player = 'P'; // Player
        const char key = 'K'; // Key
        static char door; // Door
        const char doorHorizontal = '-'; // Horizontal door
        const char doorVertical = '|'; // Vertical door
        const char emptySpace = ' '; // Empty space
        #endregion

        #region Labels
        // Game labels
        static string labelName = "Escape Room by Honey Sky"; // Console window title
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
        static string studioName = "by Honey Sky"; // Studio name
        static string pressAnyKey = "Press any key to start the game"; // Press any key prompt
        static string welcome = "Welcome!\n\n"; // Welcome message
        static string leerFeld = "           "; // Empty field for formatting
        static string gameInstructions = $"{leerFeld}You are the player ({colorYellow}{player}{colorReset})\n" +
                                         $"{leerFeld}Your goal is to find the key ({colorRed}{key}{colorReset})\n" +
                                         $"{leerFeld}and open the door\n" +
                                         $"{leerFeld}Move using the arrow keys";
        static string sliderRegelAnfang = $"Use the arrow keys to adjust the ";
        static string sliderRegelEnde = $" of the room:";
        static string enterKey = $"Press Enter to confirm";
        static string win = @"

                    __          __  _           _ 
                    \ \        / / (_)         | |
                     \ \  /\  / /   _   _ __   | |
                      \ \/  \/ /   | | | '_ \  | |
                       \  /\  /    | | | | | | |_|
                        \/  \/     |_| |_| |_| (_)
                                                  
                                                  
";
        #endregion

        #region Room Property
        // Room properties
        const int sizeWidth = 70; // Window/Buffer Width
        const int sizeHeight = 20; // Window/Buffer Height 
        static int miniumSliderSize = 10; // Minimum slider size
        static int maximumRoomHeight = 20; // Maximum room height for slider property
        static int maximumRoomWidth = 40; // Maximum room width for slider property
        static int roomWidth; // Width of the room
        static int roomHeight; // Height of the room
        static char[,] room; // 2D array representing the room layout
        static int playerX; // X-coordinate of the player
        static int playerY; // Y-coordinate of the player
        static int keyX; // X-coordinate of the key
        static int keyY; // Y-coordinate of the key
        static int doorX; // X-coordinate of the door
        static int doorY; // Y-coordinate of the door
        static bool hasKey = false; // Flag indicating whether the player has the key

        static Random rand = new Random(); // Random number generator

        static int numberOfFlashes = 3; // Number of flashes for effects
        static int flashDelay = 400; // Delay between flashes

        static int startX; // Starting X-coordinate for rendering
        static int startY; // Starting Y-coordinate for rendering
        #endregion

        #endregion

        /// <summary>
        /// Entry point of the program
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        static void Main(string[] args)
        {
            WindowProperties(); // Set window properties
            ShowStartLabel(); // Show start label
            GameInstructions(); // Show game instructions
            GetRoomDimensions(); // Get room dimensions
            InitializeRoom(); // Initialize room
            PlayGame(); // Start the game
        }

        #region Window Methods

        /// <summary>
        /// Sets the properties of the console window
        /// </summary>
        static void WindowProperties()
        {
            Console.WindowWidth = sizeWidth;
            Console.WindowHeight = sizeHeight;

            Console.BufferWidth = sizeWidth;
            Console.BufferHeight = sizeHeight;

            Console.CursorVisible = false;
        }

        /// <summary>
        /// Displays the start label
        /// </summary>
        static void ShowStartLabel()
        {
            Console.Title = labelName; // Set console window title

            for (int i = 0; i < numberOfFlashes; i++)
            {
                Console.Clear();
                Thread.Sleep(flashDelay);
                Console.ForegroundColor = colors[3];
                Console.WriteLine(startLabel);
                Thread.Sleep(flashDelay);
            }
            Console.ForegroundColor = colors[4];
            CenterText(studioName); // Center the studio name
            Console.ResetColor();
            Console.Write("\n\n\n\n");

            Console.ForegroundColor = colors[1];
            CenterText(pressAnyKey); // Center the press any key message
            Console.ResetColor();
            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Displays the game instructions
        /// </summary>
        static void GameInstructions()
        {
            Console.WriteLine("\n\n\n");
            CenterText(welcome); // Center the welcome message
            Console.WriteLine(gameInstructions);
            Console.WriteLine("\n\n");
            Console.ForegroundColor = colors[1];
            CenterText(pressAnyKey); // Center the press any key message
            Console.ReadKey(true);
            Console.ResetColor();
            Console.Clear();
        }
        #endregion

        #region Slider

        /// <summary>
        /// Gets the dimensions of the room using sliders
        /// </summary>
        static void GetRoomDimensions()
        {
            roomWidth = GetSliderValue("Width", miniumSliderSize, maximumRoomWidth); // Get the width of the room
            roomHeight = GetSliderValue("Height", miniumSliderSize, maximumRoomHeight); // Get the height of the room
        }

        /// <summary>
        /// Gets the value from a slider control
        /// </summary>
        static int GetSliderValue(string _label, int _minValue, int _maxValue)
        {
            int optionVal = _minValue; // Initialize the option value
            int optionStep = 1; // Step value for the option

            Console.Clear();

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("\n\n\n\n\n");

                CenterText($"{sliderRegelAnfang} {_label} {sliderRegelEnde}\n\n"); // Display slider instructions

                CenterText(RenderSettingSlider(optionVal, _minValue, _maxValue)); // Display the slider
                Console.WriteLine('\n');
                Console.ForegroundColor = colors[3];
                CenterText(enterKey);
                Console.ResetColor();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

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

        /// <summary>
        /// Renders the slider control
        /// </summary>
        static string RenderSettingSlider(int _optionVal, int _minValue, int _maxValue)
        {
            string optionValStr; // String representation of the option value
            if (_optionVal < 10)
            {
                optionValStr = $"0{_optionVal}";
            }
            else
            {
                optionValStr = $"{_optionVal}";
            }

            string sliderSec = "═"; // Slider section
            string sliderStr = "";

            sliderStr += "■"; // Start of the slider

            // Create the slider
            for (int i = _minValue; i <= _maxValue; i++)
            {
                if (i == _optionVal)
                {
                    sliderStr += $"╣{optionValStr}╠"; // Add thumb to the slider
                }
                else
                {
                    sliderStr += sliderSec; // Fill the rest of the slider with sections
                }
            }

            sliderStr += "■"; // End of the slider

            return sliderStr;
        }
        #endregion Slider

        #region Initialization of Room, Door, Player, Key

        /// <summary>
        /// Initializes the room, door, player, and key positions
        /// </summary>
        static void InitializeRoom()
        {
            room = new char[roomWidth, roomHeight];

            // Set room boundaries
            for (int x = 0; x < roomWidth; x++)
            {
                room[x, 0] = wall; // Top wall
                room[x, roomHeight - 1] = wall; // Bottom wall
            }
            for (int y = 0; y < roomHeight; y++)
            {
                room[0, y] = wall; // Left wall
                room[roomWidth - 1, y] = wall; // Right wall
            }

            while (true)
            {
                // Generate new positions for player and key
                playerX = rand.Next(1, roomWidth - 1);
                playerY = rand.Next(1, roomHeight - 1);
                room[playerX, playerY] = player;

                keyX = rand.Next(1, roomWidth - 1);
                keyY = rand.Next(1, roomHeight - 1);
                room[keyX, keyY] = key;

                // Check if player and key positions are not equal
                if (playerX != keyX || playerY != keyY)
                {
                    break;
                }
            }

            // Place door
            int doorSide = rand.Next(4);
            switch (doorSide)
            {
                case 0: // Top
                    doorX = rand.Next(1, roomWidth - 1);
                    doorY = 0;
                    door = doorHorizontal;
                    break;
                case 1: // Bottom
                    doorX = rand.Next(1, roomWidth - 1);
                    doorY = roomHeight - 1;
                    door = doorHorizontal;
                    break;
                case 2: // Left
                    doorX = 0;
                    doorY = rand.Next(1, roomHeight - 1);
                    door = doorVertical;
                    break;
                case 3: // Right
                    doorX = roomWidth - 1;
                    doorY = rand.Next(1, roomHeight - 1);
                    door = doorVertical;
                    break;
            }
            room[doorX, doorY] = door;
        }

        /// <summary>
        /// Starts the game loop
        /// </summary>
        static void PlayGame()
        {
            Console.Clear();
            DisplayRoom();

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = false;

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                HandleInput(keyInfo.Key);

                DisplayRoom();

                if (hasKey && playerX == doorX && playerY == doorY)
                {
                    WinSound();
                    for (int i = 0; i < numberOfFlashes; i++)
                    {
                        Console.Clear();
                        Thread.Sleep(flashDelay);
                        Console.ForegroundColor = colors[1];
                        Console.WriteLine(win);
                        Thread.Sleep(flashDelay);
                    }
                    Console.ReadKey(true);
                    Console.ResetColor();
                    Console.Clear();
                    break;
                }

            }
        }
        #endregion

        #region Display Room, Draw Room

        /// <summary>
        /// Displays the room on the console
        /// </summary>
        static void DisplayRoom()
        {
            startX = (Console.WindowWidth - roomWidth) / 2;
            startY = (Console.WindowHeight - roomHeight) / 2;

            for (int y = 0; y < roomHeight; y++)
            {
                Console.SetCursorPosition(startX, startY + y);
                for (int x = 0; x < roomWidth; x++)
                {
                    if (room[x, y] == wall)
                    {
                        if (!hasKey)
                        {
                            Console.ForegroundColor = colors[2];
                            Console.Write(wall);
                        }
                        else
                        {
                            Console.ForegroundColor = colors[1];
                            Console.Write(wall);
                        }
                    }
                    else if (room[x, y] == player)
                    {
                        Console.ForegroundColor = colors[3];
                        Console.Write(player);
                    }
                    else if (room[x, y] == key)
                    {
                        if (!hasKey)
                        {
                            Console.ForegroundColor = colors[0];
                            Console.Write(key);
                        }
                        else
                        {
                            Console.ForegroundColor = colors[5];
                            Console.Write(emptySpace);
                        }
                    }
                    else if (room[x, y] == door)
                    {
                        if (!hasKey)
                        {
                            Console.ForegroundColor = colors[1];
                            Console.Write(door);
                        }
                        else
                        {
                            Console.ForegroundColor = colors[5];
                            Console.Write(emptySpace);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = colors[5];
                        Console.Write(emptySpace);
                    }
                }
            }
            Console.ResetColor();
        }
        #endregion

        #region Controls

        /// <summary>
        /// Handles player input
        /// </summary>
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
                room[playerX, playerY] = emptySpace;
                playerX = newX;
                playerY = newY;
                room[playerX, playerY] = player;

                if (playerX == keyX && playerY == keyY)
                {
                    hasKey = true;
                    KeySound();
                }
            }
            else
            {
                WallTouchSound();
            }
        }

        /// <summary>
        /// Checks if the move is valid
        /// </summary>
        static bool IsValidMove(int _x, int _y)
        {
            char destination = room[_x, _y];
            return destination != wall && (destination != door || hasKey);
        }
        #endregion

        #region Center Text

        /// <summary>
        /// Centers text on the console
        /// </summary>
        static void CenterText(string _text)
        {
            Console.Write(new string(' ', (Console.WindowWidth - _text.Length) / 2));
            Console.WriteLine(_text);
        }
        #endregion

        #region Game Sound Effects

        /// <summary>
        /// Plays a sound effect for picking up the key
        /// </summary>
        static void KeySound() => Console.Beep(500, 500);

        /// <summary>
        /// Plays a sound effect for touching the wall
        /// </summary>
        static void WallTouchSound() => Console.Beep(1000, 300);

        /// <summary>
        /// Plays a sound effect for winning the game
        /// </summary>
        static void WinSound()
        {
            Console.Beep(659, 125); // E
            Console.Beep(659, 125); // E
            Thread.Sleep(125);
            Console.Beep(659, 250); // E
            Console.Beep(523, 125); // C
            Console.Beep(659, 125); // E
            Thread.Sleep(250);

            Console.Beep(784, 125); // G
            Thread.Sleep(500);
            Console.Beep(392, 125); // G
        }
        #endregion
    }
}
