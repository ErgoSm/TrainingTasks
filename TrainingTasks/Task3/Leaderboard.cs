namespace TrainingTasks
{
    internal abstract class LeaderboardClass
    {
        protected int _count;
        internal int IterationsCount => _count;
        internal abstract IEnumerable<Entity> GetAll();
        internal abstract void Update(string id, int score);
    }

    internal sealed class Leaderboard : LeaderboardClass
    {
        private Dictionary<string, Entity> _entityLinks = new Dictionary<string, Entity>();
        private List<Entity> _entities = new List<Entity>();
        private int _index;

        internal override IEnumerable<Entity> GetAll()
        {
            return _entities;
        }

        internal override void Update(string id, int score)
        {
            if (_entityLinks.ContainsKey(id))
            {
                var entity = _entityLinks[id];

                _entities.Remove(_entityLinks[id]);

                _index = FindPlace(0, _entities.Count, score);
                _entityLinks[id].Score = score;
                _entities.Insert(_index, entity);
            }
            else
            {
                if (_entities.Count == 0)
                    _index = 0;
                else
                    _index = FindPlace(0, _entities.Count, score);

                var newEntity = new Entity { Id = id, Score = score };
                _entities.Insert(_index, newEntity);
                _entityLinks.Add(id, newEntity);
            }
        }

        private int FindPlace(int start, int end, int score)
        {
            if(start == end)
                return start;

            var i = start + (end - 1 - start) / 2;

            if (_entities[i].Score == score)
                return i;
            if (_entities[i].Score < score)
                return FindPlace(start, i, score);
            else
                return FindPlace(i + 1, end, score);
        }
    }

    

    internal sealed class Entity : IComparable<Entity>
    {
        public string Id { get; set; }
        public int Score { get; set; }

        public int CompareTo(Entity? other)
        {
            return Score == other?.Score ? 0 : Score > other?.Score ? 1 : -1;
        }
    }

    internal class EntityComparer : IComparer<Entity>
    {
        public int Compare(Entity? x, Entity? y)
        {
            if(x == null || y == null) return 0;

            return x.Score > y.Score ? 1 : x.Score < y.Score ? -1: 0;
        }
    }
}