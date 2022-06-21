namespace TrainingTasks
{
    internal sealed class Leaderboard
    {
        private Dictionary<string, Entity> _entityLinks = new Dictionary<string, Entity>();
        private List<Entity> _entities = new List<Entity>();
        private int _index;

        internal IEnumerable<Entity> GetAll()
        {
            return _entities;
        }

        internal void Update(string id, int score)
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

    

    internal sealed class Entity
    {
        public string Id { get; set; }
        public int Score { get; set; }
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