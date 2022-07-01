using System.Linq;
using TrainingTasks.Task4;
using Xunit;

namespace TrainingTask.Tests.Task4
{

    public class Task4Test
    {
        
        [Fact]
        public void Simple_map_test()
        {
            var map = new Map(new MapInitializer());


            var flats = map.GetView(500, 500, 7, 5000000, 50);


            Assert.True(map != null);
        }

        [Fact]
        public void Simple_updating_test()
        {
            var initializer = new MapInitializer();
            int x = 400;
            int y = 400;
            while (!initializer.TryAddFlat(x, y, 5000000))
            {
                x++;
                if (x > 599)
                {
                    x = 400;
                    y++;
                }
            }


            var map = new Map(initializer);
            var flats = map.GetView(500, 500, 7, 5000000, 1).ToList();


            Assert.Equal(flats[0].X, x);
            Assert.Equal(flats[0].Y, y);
            Assert.Equal(flats[0].Price, 5000000);
        }
    }
}
