using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TrainingTasks;
using Xunit;

namespace TrainingTask.Tests.Task3
{
    public class Task3Test
    {
        private Leaderboard _leaderboard = new Leaderboard();

        [Fact]
        public void Leaderboard_returns_sorted_list()
        {
            var sorted = new List<Entity>();
            var leaderboardList = new List<Entity>();
            var rnd = new Random();
            var newEntity = new Entity();


            for(int i = 0; i < 1000; i++)
            {
                newEntity = new Entity { Id = i.ToString(), Score = rnd.Next(10000) };
                sorted.Add(newEntity);
                _leaderboard.Update(newEntity.Id, newEntity.Score);
            }
            sorted = sorted.OrderByDescending(x => x.Score).ToList();
            leaderboardList = _leaderboard.GetAll().ToList();


            for(int i = 0; i < sorted.Count; i++)
                Assert.Equal(sorted[i].Score, leaderboardList[i].Score);
        }

        [Fact]
        public void Leaderboard_update()
        {
            var sorted = new List<int>();
            var leaderboardList = new List<Entity>();


            for (int i = 0; i < 10; i++)
            {
                _leaderboard.Update(i.ToString(), i);
            }

            sorted = _leaderboard.GetAll().Select(x => x.Score).ToList();
            sorted.RemoveAt(4);
            sorted.Insert(0, 100);
            sorted.RemoveAt(3);
            sorted.Insert(1, 9);

            _leaderboard.Update("5", 100);
            _leaderboard.Update("7", 9);
            leaderboardList = _leaderboard.GetAll().ToList();


            for (int i = 0; i < sorted.Count; i++)
                Assert.Equal(sorted[i], leaderboardList[i].Score);
        }

        [Fact]
        public void Leaderboard_additional_update()
        {
            var sorted = new List<int>() { 100, 10, 0 };
            var leaderboardList = new List<Entity>();


            _leaderboard.Update("x", 100);
            _leaderboard.Update("y", 10);
            _leaderboard.Update("z", 1);
            _leaderboard.Update("z", 0);
            leaderboardList = _leaderboard.GetAll().ToList();


            for (int i = 0; i < sorted.Count; i++)
                Assert.Equal(sorted[i], leaderboardList[i].Score);
        }

        [Fact]
        public void Simple_benchmark_test()
        {
            var sorted = new List<Entity>();
            var rnd = new Random();
            var sw = new Stopwatch();
            var option1 = 0.0;
            var option2 = 0.0;


            sw.Start();
            for (var i = 0; i < 10000; i++)
            {
                _leaderboard.Update(i.ToString(), rnd.Next());
                _leaderboard.GetAll().ToArray();
            }
            sw.Stop();
            option1 = sw.Elapsed.TotalMilliseconds;

            sw.Start();
            for (var i = 0; i < 10000; i++)
            {
                sorted.Add(new Entity { Id = i.ToString(), Score = rnd.Next() });
                sorted.ToArray();
            }
            sw.Stop();
            option2 = sw.Elapsed.TotalMilliseconds;

            Assert.True(option1 < option2);
        }
    }
}
