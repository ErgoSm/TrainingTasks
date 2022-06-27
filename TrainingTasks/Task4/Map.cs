namespace TrainingTasks.Task4
{
    internal readonly record struct Flat(int X, int Y, int Price);

    internal sealed class Map
    {
        private Dictionary<int, Flat> _flats = new Dictionary<int, Flat>();

        internal Map()
        {
            var rnd = new Random();

            for (int i = 0; i < 20000; i++)
                _flats.Add(i, new Flat(rnd.Next(1000), rnd.Next(1000), rnd.Next(1000000, 40000000)));
        }

        internal IEnumerable<Flat> GetView(int x, int y, byte zoomLevel, int targetPrice, int maxObjects)
        {
            var relevantFlats = new List<Flat>();



            return relevantFlats;
        }

        internal void UpdatePrice(int x, int y, int newPrice)
        {

        }

        private Flat FindMostAppropriate()
        {
            return new Flat();
        }
    }

    internal class Square
    {
        private int _x;
    }
}
