namespace MineSweeper
{

    public class Mine
    {

        private int _x;
        private int _y;
        private bool _found;

        public Mine(int x, int y)
        {
            _x = x;
            _y = y;
            X = _x;
            Y = _y;

            _found = false;
        }

        public (int, int) Cords { get => (_x, _y); set => (_x, _y) = value; }
        public bool Found { get => _found; set => _found = value; }
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
    }
}
