namespace EscapeRoom
{
    internal class Program
    {
        static int roomWidth = 11;
        static int roomHeight = 11;
        static char[,] room;
        static int playerX;
        static int playerY;



        static void Main(string[] args)
        {
            InitializeRoom();
            DisplayRoom();
        }

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
        }

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
                    
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(' ');
                    }
                }
            }
            Console.ResetColor();
        }

    }
}