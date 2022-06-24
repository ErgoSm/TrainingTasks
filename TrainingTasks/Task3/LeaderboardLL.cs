namespace TrainingTasks
{
    internal sealed class LeaderboardLL : LeaderboardClass
    {
        private Dictionary<string, LinkedListNode<Entity>> _entityLinks = new Dictionary<string, LinkedListNode<Entity>>();
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
                entity.Value.Score = score;

                Insert(_entities.First, entity.Value, score);
            }
            else
            {
                var newEntity = new Entity { Id = id, Score = score };
                _entityLinks.Add(id, Insert(_entities.First, newEntity, score));
            }
        }

        private LinkedListNode<Entity> Insert(LinkedListNode<Entity> startNode, Entity entity, int score)
        {

            _entityNode = GetNearestSmallerEntity(startNode, score);

            if (_entityNode == null)
                _entityNode = _entities.AddLast(entity);
            else
                _entityNode = _entities.AddBefore(_entityNode, entity);

            return _entityNode;
        }

        private LinkedListNode<Entity> GetNearestSmallerEntity(LinkedListNode<Entity> startEntity, int score)
        {
            while (startEntity != null && startEntity.Value.Score > score)
                startEntity = startEntity.Next;
                

            return startEntity;
        }
    }
}
