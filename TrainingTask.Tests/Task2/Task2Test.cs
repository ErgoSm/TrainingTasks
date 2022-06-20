using System;
using System.Collections.Generic;
using TrainingTasks.Task2;
using Xunit;

namespace TrainingTask.Tests.Task2
{
    
    public class Task2Test
    {
        private HashSet<User> _set;


        [Theory]
        [InlineData("Bob", "Smith", 123, 1)]
        [InlineData("Bob", "smith", 123, 2)]
        [InlineData("Bob", "Smith", 124, 2)]
        public void Trying_add_in_hashset(string firstName, string lastName, long inn, int size)
        {
            _set = new HashSet<User>();
            var dt = DateTime.Parse("2000-01-01");
            _set.Add(new User { FirstName = "Bob", LastName = "Smith", DateOfBirth = dt, INN = 123 });


            _set.Add(new User { FirstName = firstName, LastName = lastName, DateOfBirth = dt, INN = inn });


            Assert.Equal(size, _set.Count);
        }

        [Fact]
        public void Compare_with_object()
        {
            Assert.False(new User().Equals(new object()));
        }
    }
}
