﻿using System;
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
        private int _mathExp = 10000 / 2;
        private int _count;

        internal override int IterationsCount => _count;

        public LeaderboardLLEnhanced()
        {
            _entityNode = _entities.First;
            _prevNode = _entities.First;
        }

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

            if (_prevNode == null)
                _prevNode = _entityNode;
            else if(Math.Abs(_mathExp - _prevNode.Value.Score) > Math.Abs(_mathExp - score)) 
                _prevNode = _entityNode;
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