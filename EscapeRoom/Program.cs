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
                room[x, 0] = '#'; // Obere Wand
                room[x, roomHeight - 1] = '#'; // Untere Wand
            }
            for (int y = 0; y < roomHeight; y++)
            {
                room[0, y] = '#'; // Linke Wand
                room[roomWidth - 1, y] = '#'; // Rechte Wand
            }
        
            // Boden
            for (int x = 1; x < roomWidth - 1; x++)
            {
                for (int y = 1; y < roomHeight - 1; y++)
                {
                    room[x, y] = ' ';
                }
            }

            // Spieler platzieren
            Random rand = new Random();
            playerX = rand.Next(1, roomWidth - 1);
            playerY = rand.Next(1, roomHeight - 1);
            room[playerX, playerY] = '@';

        }
        #endregion

        #region DisplayRoom()
        static void DisplayRoom()
        {
            for (int y = 0; y < roomHeight; y++)
            {
                for (int x = 0; x < roomWidth; x++)
                {
                    Console.SetCursorPosition(x, y);

                    if (room[x, y] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write('#');
                    }
                    else if (room[x, y] == '@')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine('@');
                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(' ');
                    }
                }
            }
            Console.ResetColor();
        }
        #endregion

    }
}