namespace MineSweeper
{
    internal class Field
    {

        private int _x;
        private int _y;
        private readonly HashSet<(int, int)> _const_mines;
        private HashSet<(int, int)> _mines;
        private HashSet<(int, int)> _found_mines;

        public Field(int size, int number_of_mines)
        {
            // Sets the current pos
            _x = 0;
            _y = 0;
            // Random
            Random _rand = new Random();
            _mines = new HashSet<(int, int)>();
            _found_mines = new HashSet<(int, int)>();

            for (int i = 0; i < number_of_mines; i++)
            {
                int x = _rand.Next(-1 * (size / 2), size / 2);
                int y = _rand.Next(-1 * (size / 2), size / 2);

                if (!_mines.Add((x, y)))
                    i--;
            }
            _const_mines = new HashSet<(int, int)>(_mines);
        }
        public bool SwipeMine()
        {
            if (_found_mines.Add((_x, _y)))
            {
                _mines.ExceptWith(_found_mines);
                return true;
            }
            return false;
        }

        public static (int, int) LineProjection((int, int) origin, (int, int) point)
        {
            // calc the direction of the vector, from the bomb cords, the origin cords.
            int x_direction = point.Item1 - origin.Item1;
            int y_direction = point.Item2 - origin.Item2;

            int x_projected_line = point.Item1 + 1 * x_direction;
            int y_projected_line = point.Item2 + 1 * y_direction;
            return (x_projected_line, y_projected_line);

        }

        public static double Veclength((int, int) origin, (int, int) point)
        {
            int x_dir = point.Item1 - origin.Item1;
            int y_dir = point.Item2 - origin.Item2;

            return 1 * Math.Sqrt(x_dir * x_dir + y_dir * y_dir);
        }
        //       private bool HasDuplicates(out int[] indexes)
        //       {
        //
        //           bool has = false;
        //           (int x, int y)[] cords = new (int x, int y)[_mine_arr.Length];
        //           List<int> indexbuffer = new List<int>();
        //
        //           for (int i = 0; i < _mine_arr.Length; i++)
        //           {
        //               var bomb = (_mine_arr[i].X, _mine_arr[i].Y);
        //
        //               if (!cords.Contains(bomb))
        //               {
        //                   cords.Append(bomb);
        //               }
        //
        //               indexbuffer.Add(i);
        //               has = true;
        //           }
        //           indexes = has ? indexbuffer.ToArray() : null;
        //           return has;
        //       }

        public (int, int) Cords { get => (_x, _y); set => (_x, _y) = value; }
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        internal HashSet<(int, int)> Mines { get => _mines; set => _mines = value; }
        internal HashSet<(int, int)> FoundMines { get => _found_mines; set => _found_mines = value; }
        internal HashSet<(int, int)> ConstMines { get => _const_mines; }

    }
}
