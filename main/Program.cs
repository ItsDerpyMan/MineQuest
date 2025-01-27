using System;
using System.IO;
using System.Text;

namespace MineSweeper
{

    class MineSweeper
    {
        public const int field_size = 11;
        public static int number_of_mines = 10;
        public static int lives = 9;
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
                while (lives > 1)
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
                                case ConsoleKey.UpArrow:
                                    if (f.Y > field_size * -1 / 2) f.Y -= 1;
                                    break;
                                case ConsoleKey.DownArrow:
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
                            DrawGrid(f.Cords, f.FoundMines, f.ConstMines);
                        }
                    }
                    // End of the round
                    number_of_mines = f.Mines.Count;
                    if (number_of_mines == 0) break;
                    DrawDinstance(f.Cords, f.Mines);
                    round++;
                }
                Console.SetCursorPosition(0, 19);
                Console.WriteLine("Congrat u won!!");
                Console.CursorVisible = true;
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                Console.WriteLine("\nPress any <key> to continue.");
                Console.ReadLine();

            }
        }
        static void DrawGrid((int, int) player, HashSet<(int, int)> found_mines, HashSet<(int, int)> mines)
        {
            Console.SetCursorPosition(0, 0);
            var grid = new StringBuilder();

            const int start = -1 * field_size / 2;
            const int end = field_size / 2;

            int columnWidth = Math.Max(end.ToString().Length, (-start).ToString().Length) + 1;

            grid.AppendLine($"{round}. Round")
            .AppendLine($"Guess a number between {field_size * -1 / 2} and {field_size / 2} on the X and Y cordinates.")
            .AppendLine($"Lives: {lives}")
            .AppendLine($"Mines: {number_of_mines} ");

            grid.Append(' ', 3);
            for (int x = start; x <= end; x++)
            {
                grid.Append(x.ToString().PadLeft(columnWidth));
            }
            grid.AppendLine();

            for (int y = start; y <= end; y++)
            {
                grid.Append(y.ToString().PadLeft(3));
                for (int x = start; x <= end; x++)
                {
                    if (x == player.Item1 && y == player.Item2)
                        grid.Append("@".PadLeft(columnWidth));
                    else if (x == 0 && y == 0)
                        grid.Append("O".PadLeft(columnWidth));
                    else if (found_mines.Contains((x, y)))
                    {
                        if (mines.Contains((x, y)))
                        {
                            grid.Append("\x1b[31m").Append("M".PadLeft(columnWidth)).Append("\x1b[0m");
                        }
                        else
                            grid.Append("\x1b[34m").Append("X".PadLeft(columnWidth)).Append("\x1b[0m");
                    }
                    else
                        grid.Append(".".PadLeft(columnWidth));
                }
                grid.AppendLine();
            }

            grid.AppendLine("Press the <arrows> to move. Press the <Space> to mine mine.");
            grid.AppendLine($"Pos: ({player.Item1}, {player.Item2})   ");

            Console.Write(grid.ToString() + "\r");
        }
        static void DrawDinstance((int, int) player, HashSet<(int, int)> mines)
        {

            const int start = -1 * field_size / 2;
            const int end = field_size / 2;
            int columnWidth = Math.Max(end.ToString().Length, (-start).ToString().Length) + 1;

            Console.SetCursorPosition(5 + field_size * columnWidth, 4);
            Console.Write("Rows: ");

            int cord_y = start + 1;
            var y_mines = mines.Where(m => m.Item2 == player.Item2).ToHashSet();
            if (y_mines.Count < lrows)
            {
                for (int i = 0; i < lrows; i++)
                {
                    Console.SetCursorPosition(5 + field_size * columnWidth, cord_y + 9);
                    Console.Write(new String(' ', 20));
                    cord_y++;
                }
            }
            cord_y = start + 1;
            for (int i = 0; i < y_mines.Count; i++)
            {

                Console.SetCursorPosition(5 + field_size * columnWidth, cord_y + 9);
                double distance = Field.Veclength(player, y_mines.ElementAt(i));
                Console.Write($"Mine in a Dist: {distance:F2} ");
                cord_y++;
            }
            lrows = y_mines.Count;

            Console.SetCursorPosition(5 + field_size * columnWidth, 10);
            Console.Write("Columns: ");

            int cord_x = start + 1;
            var x_mines = mines.Where(m => m.Item1 == player.Item1).ToHashSet();
            if (y_mines.Count < lcolumn)
            {
                for (int i = 0; i < lcolumn; i++)
                {
                    Console.SetCursorPosition(5 + field_size * columnWidth, cord_x + 15);
                    Console.Write(new String(' ', 20));
                    cord_x++;
                }
            }
            cord_x = start + 1;
            for (int i = 0; i < x_mines.Count; i++)
            {

                Console.SetCursorPosition(5 + field_size * columnWidth, cord_x + 15);
                double distance = Field.Veclength(player, x_mines.ElementAt(i));
                Console.Write($"Mine in a Dist: {distance:F2} ");
                cord_x++;
            }
            lcolumn = x_mines.Count;

        }
    }
}
