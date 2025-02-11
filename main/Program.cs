using System;
using System.IO;
using System.Text;

namespace MineSweeper
{

    class MineSweeper
    {
        public const int field_size = 11;
        public static int number_of_mines = 10;
        public static int lives = 100;
        public static int round = 1;
        private static int lrows;
        private static int lcolumn;

        static void Main()
        {
            // Mine finder
            // Mines are hid on a square cordination field
            // You have a a device which able to detect precisly any mine but it cant give u the direction of the mine.
            // You must find all the mines or else you lose.

            try
            {
                // MineField created
                Field f = new Field(field_size, number_of_mines);

                // Cursor
                Console.CursorVisible = false;

                // Game loop
                // Every iteration is a round. Each round the user has to guess a cordinate on the board.
                Console.Clear();
                while (lives > 0)
                {
                    // Listening to Specific keypresses as long as its true
                    bool loop = true;
                    while (loop)
                    {
                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey(intercept: true).Key;
                            switch (key)
                            {
                                case ConsoleKey.Spacebar:
                                    if (f.SwipeMine() && f.Mines.Count < number_of_mines)
                                    {
                                        Console.WriteLine("You have diactivated a mine. Noice!");
                                        number_of_mines = f.Mines.Count;
                                        loop = false;
                                        break;
                                    }
                                    Console.WriteLine("You missed! Try again.");
                                    MineSweeper.lives--;
                                    loop = false;
                                    break;
                                case ConsoleKey.DownArrow:
                                    if (f.Y > field_size * -1 / 2) f.Y -= 1;
                                    break;
                                case ConsoleKey.UpArrow:
                                    if (f.Y < field_size / 2) f.Y += 1;
                                    break;
                                case ConsoleKey.LeftArrow:
                                    if (f.X > field_size * -1 / 2) f.X -= 1;
                                    break;
                                case ConsoleKey.RightArrow:
                                    if (f.X < field_size / 2) f.X += 1;
                                    break;
                            }
                            // Draw call
                            DrawGrid(f.GetCords, f.FoundMines, f.ConstMines);
                        }
                    }
                    // End of the round
                    number_of_mines = f.Mines.Count;
                    if (number_of_mines == 0) break;
                    DrawDinstance(f.GetCords, f.Mines);
                    round++;
                }
                Console.SetCursorPosition(0, 19);
                if (lives > 0)
                    Console.WriteLine("Congrat u won!!");
                else Console.WriteLine("You lost!");
                Console.CursorVisible = true;
                Console.ReadKey();
            }
            catch (Exception e)
            {   
                Console.Clear();
                Console.CursorVisible = true;
                Console.WriteLine(e);

                Console.WriteLine("\nPress any <key> to continue.");
                Console.ReadLine();

            }
        }
        static int Abs(int x) => (x ^ (x >> 31)) - (x >> 31);

        static string CordsToCharacter(Field.Cords cord, Field.Cords player, HashSet<Field.Cords> foundMines, HashSet<Field.Cords> mines)
        {
            return cord switch
            {
                _ when cord.x == player.x && cord.y == player.y => "\u001b[38;5;82m@\u001b[0m",
                _ when cord.x == 0 && cord.y == 0 => "\u001b[38;5;62mO\u001b[0m",
                _ when foundMines.Contains(cord) => mines.Contains(cord) ? "\u001b[31mM\u001b[0m" : "\u001b[38;5;62mX\u001b[0m",
                _ => "\u001b[37m.\u001b[0m"
            };
        }
        static void DrawGrid(Field.Cords player, HashSet<Field.Cords> found_mines, HashSet<Field.Cords> mines)
        {
            const int start = -1 * field_size / 2;
            const int end = field_size / 2;
            int columnWidth = Math.Max(end.ToString().Length, (-start).ToString().Length) + 2;
            int left_padding = 1 * columnWidth;
            int right_padding = 0;
            //    -5 -4 -3 -2 -1 0 1 2 3 4 5 
            for (int i = start; i <= end; i++)
            {
                int position = (i + 6) * columnWidth;
                if (i >= 0) position += 1;
                Console.Write($"\u001b[4;{left_padding + position}H{i}");
            }
            for (int i = end ; i >= start; i--)
            {
                if (i != Abs(i)) right_padding = 1;
                Console.Write($"\u001b[{10 - i};{1 * left_padding - right_padding}H{i}");
            }
            int y = end;
            int x = start;
            while (y != start - 1) {
                int position = (x + 6) * columnWidth + 1;
                string character = CordsToCharacter(new Field.Cords(x, y), player, found_mines, mines);
                Console.Write($"\u001b[{10 - y};{left_padding + position}H{character}");

                if (x == end) {x = start; y--; }
                else x++;
            }
        }
        static void DrawDinstance(Field.Cords player, HashSet<Field.Cords> mines)
        {

            const int start = -1 * field_size / 2;
            const int end = field_size / 2;
            int columnWidth = Math.Max(end.ToString().Length, (-start).ToString().Length) + 1;
            int left_padding = 17;

            Console.SetCursorPosition(left_padding + field_size * columnWidth, 4);
            Console.Write("Rows: ");

            int cord_y = start + 1;
            var y_mines = mines.Where(m => m.y == player.y).ToHashSet();
            if (y_mines.Count < lrows)
            {
                for (int i = 0; i < lrows; i++)
                {
                    Console.SetCursorPosition(left_padding + field_size * columnWidth, cord_y + 9);
                    Console.Write(new String(' ', 20));
                    cord_y++;
                }
            }
            cord_y = start + 1;
            for (int i = 0; i < y_mines.Count; i++)
            {

                Console.SetCursorPosition(left_padding + field_size * columnWidth, cord_y + 9);
                double distance = Field.Veclength(player, y_mines.ElementAt(i));
                Console.Write($"Mine in a Dist: {distance:F2} ");
                cord_y++;
            }
            lrows = y_mines.Count;

            Console.SetCursorPosition(left_padding + field_size * columnWidth, 10);
            Console.Write("Columns: ");

            int cord_x = start + 1;
            var x_mines = mines.Where(m => m.x == player.x).ToHashSet();
            if (y_mines.Count < lcolumn)
            {
                for (int i = 0; i < lcolumn; i++)
                {
                    Console.SetCursorPosition(left_padding + field_size * columnWidth, cord_x + 15);
                    Console.Write(new String(' ', 20));
                    cord_x++;
                }
            }
            cord_x = start + 1;
            for (int i = 0; i < x_mines.Count; i++)
            {

                Console.SetCursorPosition(left_padding + field_size * columnWidth, cord_x + 15);
                double distance = Field.Veclength(player, x_mines.ElementAt(i));
                Console.Write($"Mine in a Dist: {distance:F2} ");
                cord_x++;
            }
            lcolumn = x_mines.Count;

        }
    }
}
