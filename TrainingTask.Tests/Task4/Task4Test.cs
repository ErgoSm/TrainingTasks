using TrainingTasks.Task4;
using Xunit;

namespace TrainingTask.Tests.Task4
{

    public class Task4Test
    {
        
        [Fact]
        public void Simple_map_test()
        {
            var map = new Map();


            var flats = map.GetView(500, 500, 7, 5000000, 50);


            Assert.True(map != null);
        }
    }
}
