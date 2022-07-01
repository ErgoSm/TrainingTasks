using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTasks.Task4
{
    internal sealed class MapInitializer
    {
        public readonly int FieldSize = 1000;
        public readonly int FlatsNumber = 10000;

        private Dictionary<(int, int), Flat> _flats;


        public MapInitializer()
        {
            _flats = new Dictionary<(int, int), Flat>();
            var rnd = new Random();
            int x, y;

            for (int i = 0; i < FlatsNumber; i++)
            {
                x = rnd.Next(FieldSize);
                y = rnd.Next(FieldSize);
                _flats.TryAdd((x, y), new Flat(x, y, rnd.Next(1000000, 40000000)));
            }
                
        }

        public bool TryAddFlat(int x, int y, int price)
        {
            return _flats.TryAdd((x, y), new Flat(x, y, price));
        }

        public IEnumerable<Flat> GetFlats()
        {
            return _flats.Values;
        }
    }
}
