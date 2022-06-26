using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTasks
{
    internal sealed class LeaderboardLLEnhanced : LeaderboardClass
    {
        private Dictionary<string, LinkedListNode<Entity>> _entityLinks = new Dictionary<string, LinkedListNode<Entity>>();
        private LinkedList<Entity> _entities = new LinkedList<Entity>();
        private LinkedListNode<Entity> _entityNode;
        private LinkedListNode<Entity> _prevNode;
        private int _sum = 0;
        private int _mathExp = 0; // 10000 / 2;
        private int _currentDiff = 10000 / 2;

        internal override IEnumerable<Entity> GetAll()
        {
            return _entities;
        }

        internal override void Update(string id, int score)
        {
            if (_entityLinks.ContainsKey(id))
            {
                var entity = _entityLinks[id];

                entity.Value.Score = score;

                Insert(_prevNode, entity.Value, score);
                _entities.Remove(entity);
            }
            else
            {
                var newEntity = new Entity { Id = id, Score = score };
                _entityLinks.Add(id, Insert(_prevNode, newEntity, score));
            }

            _sum += score;
            _mathExp = _sum / _entities.Count;

            if(_currentDiff > Math.Abs(_mathExp - score))
            {
                _prevNode = _entityNode;
                _currentDiff = Math.Abs(_mathExp - score);
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
            if (startEntity == null)
                return null;

            if(startEntity.Value.Score > score)
                while (startEntity != null && startEntity.Value.Score > score)
                {
                    startEntity = startEntity.Next;
                    _count++;
                }
            else
            {
                while (startEntity != null && startEntity.Value.Score <= score)
                {
                    startEntity = startEntity.Previous;
                    _count++;
                }   

                startEntity = startEntity?.Next ?? _entities.First;
            }

            return startEntity;
        }
    }
}
