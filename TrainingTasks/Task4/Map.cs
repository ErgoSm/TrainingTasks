namespace TrainingTasks.Task4
{
    internal sealed class Map
    {
        private readonly MapInitializer _initializer;

        private Square _square;
        private Dictionary<int, Flat> _flats = new Dictionary<int, Flat>();

        private Func<Flat, Flat, int, bool> _less => (flatOne, flatTwo, price) => Math.Abs(flatOne.Price - price) < Math.Abs(flatTwo.Price - price);

        internal Map(MapInitializer initializer)
        {
            _initializer = initializer;

            _square = new Square(new SquareParameters(0, 0, 1000, 50), _initializer.GetFlats());
        }

        //Returns the maxObjects-most relevant Flats
        internal IEnumerable<Flat> GetView(int x, int y, byte zoomLevel, int targetPrice, int maxObjects)
        {
            maxObjects = maxObjects > _initializer.FlatsNumber ? _initializer.FlatsNumber : maxObjects;
            var relevantFlats = new List<Flat>();

            //Find squares
            var squareHalfSize = (_initializer.FieldSize - zoomLevel * 100) / 2;
            var visibleSquares = GetVisibleSquares(y - squareHalfSize >= 0 ? y - squareHalfSize : 0,
                                                   x - squareHalfSize >= 0 ? x - squareHalfSize : 0,
                                                   y + squareHalfSize <= _initializer.FieldSize ? y + squareHalfSize : _initializer.FieldSize,
                                                   x + squareHalfSize <= _initializer.FieldSize ? x + squareHalfSize : _initializer.FieldSize).ToList();

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
                if (_less(list[upIndex], list[downIndex], targetPrice))
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

        internal bool UpdatePrice(int x, int y, int newPrice)
        {
            return _square.Update(x, y, newPrice);
        }

        //Minimization with the golden ratio method
        private int FindIndexOfNearest(List<Flat> flats, int target)
        {
            int start = 0; ;
            int end = flats.Count - 1;
            int current = 0;

            while (end - start > 2)
            {
                if (_less(flats[start], flats[end], target))
                {
                    current = start;
                    end = (int) (start + (end - start) * 0.618);
                }
                else
                {
                    current = end;
                    start = (int) (start + (end - start) * 0.382);
                }
            }

            current = _less(flats[start], flats[end], target) ?
                _less(flats[start], flats[start + 1], target) ? start : start + 1 :
                _less(flats[start + 1], flats[end], target) ? start + 1 : end;

            return current;
        }
    }
}
