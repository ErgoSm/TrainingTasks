namespace TrainingTasks.Task4
{
    internal record Flat(int X, int Y, int Price) : IComparable
    {
        public int CompareTo(object? obj)
        {
            var flat = obj as Flat;
            return Price == flat?.Price ? 0 : Price > flat.Price ? 1 : -1;
        }
    }

    internal record SquareParameters(double X, double Y, double Size, int NumberOnLevel);

    internal sealed class Square
    {
        private double _xLeft;
        private double _yTop;
        private double _size;
        private Dictionary<(int, int), Flat> _flatLinks;

        public bool IsLeaf { get; private set; }
        public ICollection<Square> Children { get; private set; }
        public List<Flat> Flats { get; private set; }
        
        public Square(SquareParameters parameters, IEnumerable<Flat> flats)
        {
            _xLeft = parameters.X;
            _yTop = parameters.Y;
            _size = parameters.Size;

            if (flats.Count() <= parameters.NumberOnLevel)
            {
                Flats = flats.ToList();
                Flats.Sort();

                //_flatLinks = Flats.ToDictionary(f => (f.X, f.Y));
                _flatLinks = new Dictionary<(int, int), Flat>();
                foreach(var flat in Flats)
                    _flatLinks.TryAdd((flat.X, flat.Y), flat);


                IsLeaf = true;
            }
            else
            {
                Children = new List<Square>();
                for (int i = 0; i < 4; i++)
                {
                    var newSize = _size / 2;
                    var newParam = new SquareParameters(i % 2 == 0 ? _xLeft : _xLeft + newSize,
                        i < 2 ? _yTop : _yTop + newSize, newSize, parameters.NumberOnLevel);

                    Children.Add(new Square(newParam, flats.Where(f => f.X >= newParam.X && f.X <= newParam.X + newParam.Size &&
                                                                        f.Y >= newParam.Y && f.Y <= newParam.Y + newParam.Size)));
                }
            }
        }

        
        public bool Update(int x, int y, int value)
        {
            if(!IsLeaf)
            {
                foreach (var child in Children)
                    if(child.Update(x, y, value))
                        return true;
            }
            else if (_flatLinks.ContainsKey((x, y))) 
            {
                var newFlat = new Flat(x, y, value);

                Flats.Remove(_flatLinks[(x, y)]);
                _flatLinks[(x, y)] = newFlat;

                var index = FindIndex(0, Flats.Count, value);
                Flats.Insert(index, newFlat);

                return true;
            }


            return false;
        }

        private int FindIndex(int start, int end, int value)
        {
            if (start == end)
                return start;

            var i = start + (end - 1 - start) / 2;

            if (Flats[i].Price == value)
                return i;
            if (Flats[i].Price > value)
                return FindIndex(start, i, value);
            else
                return FindIndex(i + 1, end, value);
        }

        public IEnumerable<Square> GetIntersections(int left, int right, int top, int bottom)
        {
            var squares = new List<Square>();

            if (IsSubfield(left, right, top, bottom))
                squares.AddRange(GetChildren(this));
            else
            {
                foreach (var child in Children)
                {
                    if (child.IsLeaf)
                    {
                        if (child.IsIntersected(left, right, top, bottom))
                            squares.Add(child);
                    }
                    else
                        squares.AddRange(child.GetIntersections(left, right, top, bottom));
                }
            }

            return squares;
        }

        private IEnumerable<Square> GetChildren(Square square)
        {
            var children = new List<Square>();

            foreach (var child in square.Children)
            {
                if (child.IsLeaf)
                    children.Add(child);
                else
                    children.AddRange(GetChildren(child));
            }

            return children;
        }

        public bool IsSubfield(int left, int right, int top, int bottom)
        {
            return _xLeft > left && _xLeft + _size < right && _yTop > top && _yTop + _size < bottom;
        }

        private bool IsIntersected(int left, int right, int top, int bottom)
        {
            return IsBeerween(left, right, _xLeft) && IsBeerween(top, bottom, _yTop) ||
                   IsBeerween(left, right, _xLeft + _size) && IsBeerween(top, bottom, _yTop) ||
                   IsBeerween(left, right, _xLeft) && IsBeerween(top, bottom, _yTop + _size) ||
                   IsBeerween(left, right, _xLeft + _size) && IsBeerween(top, bottom, _yTop + _size);
        }

        private bool IsBeerween(int start, int end, double value)
        {
            return start <= value && value <= end;
        }
    }
}
