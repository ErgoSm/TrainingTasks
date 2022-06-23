namespace TrainingTasks
{
    internal sealed class LeaderboardLL : LeaderboardClass
    {
        private Dictionary<string, Entity> _entityLinks = new Dictionary<string, Entity>();
        private LinkedList<Entity> _entities = new LinkedList<Entity>();
        private LinkedListNode<Entity> _entityNode;

        internal override IEnumerable<Entity> GetAll()
        {
            return _entities;
        }

        internal override void Update(string id, int score)
        {
            if (_entityLinks.ContainsKey(id))
            {
                var entity = _entityLinks[id];

                _entities.Remove(entity);

                entity.Score = score;
                _entityNode = GetNearesSmallerEntity(entity);

                if (_entityNode == null)
                    _entities.AddLast(entity);
                else
                    _entities.AddBefore(_entityNode, entity);
            }
            else
            {
                var newEntity = new Entity { Id = id, Score = score };
                _entityNode = GetNearesSmallerEntity(newEntity);

                if (_entityNode == null)
                    _entities.AddLast(newEntity);
                else
                    _entities.AddBefore(_entityNode, newEntity);

                _entityLinks.Add(id, newEntity);
            }
        }

        private LinkedListNode<Entity> GetNearesSmallerEntity(Entity newEntity)
        {
            foreach (var entity in _entities)
                if (entity.Score < newEntity.Score)
                    return _entities.Find(entity);

            return null;
        }
    }
}
