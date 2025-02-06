namespace MineSweeper
{
    internal class Field
    {
        private Cords _cords;
        private readonly HashSet<Cords> _const_mines;
        private HashSet<Cords> _mines;
        private HashSet<Cords> _found_mines;

        public Field(int size, int number_of_mines)
        {
            // Sets the current pos
            _cords = new Cords(0, 0);

            // Random
            Random _rand = new Random();
            _mines = new HashSet<Cords>();
            _found_mines = new HashSet<Cords>();

            for (int i = 0; i < number_of_mines; i++)
            {
                int x = _rand.Next(-1 * (size / 2), size / 2);
                int y = _rand.Next(-1 * (size / 2), size / 2);

                if (!_mines.Add(new Field.Cords(x, y)))
                    i--;
            }
            _const_mines = new HashSet<Cords>(_mines);
        }
        public struct Cords
        {
            public int x;
            public int y;

            public Cords(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public bool SwipeMine()
        {
            if (_found_mines.Add(_cords))
            {
                _mines.ExceptWith(_found_mines);
                return true;
            }
            return false;
        }

        public static Cords LineProjection(Cords origin, Cords point)
        {
            // calc the direction of the vector, from the bomb cords, the origin cords.
            int x_direction = point.x - origin.x;
            int y_direction = point.y - origin.y;

            int x_projected_line = point.x + 1 * x_direction;
            int y_projected_line = point.y + 1 * y_direction;
            return new Cords(x_projected_line, y_projected_line);

        }

        public static double Veclength(Cords origin, Cords point)
        {
            int x_dir = point.x - origin.x;
            int y_dir = point.y - origin.y;

            return 1 * Math.Sqrt(x_dir * x_dir + y_dir * y_dir);
        }

        public Cords GetCords { get => _cords; }
        public int X { get => _cords.x; set => _cords.x = value; }
        public int Y { get => _cords.y; set => _cords.y = value; }
        internal HashSet<Cords> Mines { get => _mines; set => _mines = value; }
        internal HashSet<Cords> FoundMines { get => _found_mines; set => _found_mines = value; }
        internal HashSet<Cords> ConstMines { get => _const_mines; }

    }
}
