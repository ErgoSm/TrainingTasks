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

    internal sealed class Map
    {
        private readonly int _fieldSize = 1000;
        private readonly int _flatsNumber = 20000;

        private Square _square;
        private Dictionary<int, Flat> _flats = new Dictionary<int, Flat>();

        private Func<Flat, int, int> _diff => (flat, price) => Math.Abs(flat.Price - price);

        internal Map()
        {
            var rnd = new Random();

            for (int i = 0; i < _flatsNumber; i++)
                _flats.Add(i, new Flat(rnd.Next(1000), rnd.Next(1000), rnd.Next(1000000, 40000000)));

            _square = new Square(new SquareParameters(0, 0, 1000, 50), _flats.Values);
        }

        //Returns the maxObjects-most relevant Flats
        internal IEnumerable<Flat> GetView(int x, int y, byte zoomLevel, int targetPrice, int maxObjects)
        {
            maxObjects = maxObjects > _flatsNumber ? _flatsNumber : maxObjects;
            var relevantFlats = new List<Flat>();

            //Find squares
            var squareHalfSize = (_fieldSize - zoomLevel * 100) / 2;
            var visibleSquares = GetVisibleSquares(y - squareHalfSize >= 0 ? y - squareHalfSize : 0,
                                                   x - squareHalfSize >= 0 ? x - squareHalfSize : 0,
                                                   y + squareHalfSize <= _fieldSize ? y + squareHalfSize : _fieldSize,
                                                   x + squareHalfSize <= _fieldSize ? x + squareHalfSize : _fieldSize).ToList();

            //Sort
            var mergedList = visibleSquares.First().Flats.ToList();
            for (int i = 1; i < visibleSquares.Count(); i++)
                mergedList = MergeSorting(mergedList, visibleSquares[i].Flats.ToList());


            //Find nearest
            var list = mergedList.ToList();
            var startIndex = FindIndexOfNearest(list, targetPrice);


            //Find N-elements
            relevantFlats.Add(list[startIndex]);
            int n = 1;
            int upIndex = startIndex - 1 >= 0 ? startIndex - 1 : 0;
            int downIndex = startIndex + 1 < list.Count ? startIndex + 1 : list.Count - 1;
            bool upSelected;
            while(n < maxObjects)
            {
                if (_diff(list[upIndex], targetPrice) < _diff(list[downIndex], targetPrice))
                {
                    relevantFlats.Add(list[upIndex]);
                    upSelected = true;
                }
                else
                {
                    relevantFlats.Add(list[downIndex]);
                    upSelected = false;
                }

                if (upSelected)
                {
                    if (upIndex - 1 >= 0)
                        upIndex--;
                    else
                        downIndex++;
                }
                else
                {
                    if (downIndex + 1 < list.Count)
                        downIndex++;
                    else
                        upIndex--;
                }

                n++;
            }

            return relevantFlats;
        }

        private IEnumerable<Square> GetVisibleSquares(int top, int left, int bottom, int right)
        {
            return _square.GetIntersections(left, right, top, bottom);
        }

        private List<Flat> MergeSorting(List<Flat> listOne, List<Flat> listTwo)
        {
            var resultList = new List<Flat>();

            int lastIndexOne = listOne.Count - 1;
            int lastIndexTwo = listTwo.Count - 1;
            int indexOne = 0;
            int indexTwo = 0;

            while (indexOne < lastIndexOne && indexTwo < lastIndexTwo)
            {
                if(indexOne != lastIndexOne && listOne[indexOne].Price < listTwo[indexTwo].Price || indexTwo == lastIndexTwo)
                {
                    resultList.Add(listOne[indexOne]);
                    indexOne++;
                }
                else
                {
                    resultList.Add(listTwo[indexTwo]);
                    indexTwo++;
                }
            }


            return resultList;
        }

        //TODO: implement the function
        internal void UpdatePrice(int x, int y, int newPrice)
        {

        }

        //Minimization with the golden ratio method
        private int FindIndexOfNearest(List<Flat> flats, int target)
        {
            int start = 0; ;
            int end = flats.Count - 1;
            int current = 0;

            while (end - start > 2)
            {
                if (_diff(flats[start], target) < _diff(flats[end], target))
                {
                    current = start;
                    end = (int) (start + (end - start) * 0.618);
                }
                else
                {
                    current = end;
                    start = (int) Math.Ceiling(start + (end - start) * 0.382);
                }
            }

            return current;
        }


    }

    internal sealed class Square
    {
        private double _xLeft;
        private double _yTop;
        private double _size;

        //private Square _parent;
        private ICollection<Square> _children;

        public bool IsLeaf { get; private set; }
        public List<Flat> Flats { get; private set; }

        public Square(SquareParameters parameters, IEnumerable<Flat> flats)
        {
            _xLeft = parameters.X;
            _yTop = parameters.Y;
            _size = parameters.Size;

            if(flats.Count() <= parameters.NumberOnLevel)
            {
                Flats = flats.ToList();
                Flats.Sort();
                IsLeaf = true;
            }
            else
            {
                _children = new List<Square>();
                for(int i = 0; i < 4; i++)
                {
                    var newSize = _size / 2;
                    var newParam = new SquareParameters(i % 2 == 0 ? _xLeft : _xLeft + newSize,
                        i < 2 ? _yTop : _yTop + newSize, newSize, parameters.NumberOnLevel);

                    _children.Add(new Square(newParam, flats.Where(f => f.X >= newParam.X && f.X <= newParam.X + newParam.Size &&
                                                                        f.Y >= newParam.Y && f.Y <= newParam.Y + newParam.Size)));
                }
            }
        }

        //TODO: add function IsSubfield to minimize IsIntersections calls
        public IEnumerable<Square> GetIntersections(int left, int right, int top, int bottom)
        {
            var squares = new List<Square>();

            foreach(var child in _children)
            {
                if (child.IsLeaf)
                {
                    if(child.IsIntersected(left, right, top, bottom))
                        squares.Add(child);
                }
                else
                    squares.AddRange(child.GetIntersections(left, right, top, bottom));
            }

            return squares;
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
