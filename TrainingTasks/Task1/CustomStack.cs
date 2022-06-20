using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTasks.Task1
{
    internal sealed class CustomStack<T>
    {
        private Queue<T> _mainQueue = new Queue<T>();
        private Queue<T> _secondQueue = new Queue<T>();
        private Queue<T> _queueTmp;

        public void Push(T item)
        {
            _mainQueue.Enqueue(item);
        }

        public T Pop()
        {
            var size = _mainQueue.Count;
            for(int i = 0; i < size - 1; i++)
                _secondQueue.Enqueue(_mainQueue.Dequeue());

            var result = _mainQueue.Dequeue();

            _queueTmp = _mainQueue;
            _mainQueue = _secondQueue;
            _secondQueue = _queueTmp;

            return result;
        }
    }
}
