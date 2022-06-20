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
        private List<T> _list = new List<T> ();

        public void Push(T item)
        {
            _mainQueue.Enqueue(item);
        }

        public T Pop()
        {
            _list.Clear ();
            var size = _mainQueue.Count;
            for(int i = 0; i < size - 1; i++)
                _list.Add(_mainQueue.Dequeue());

            var result = _mainQueue.Dequeue();

            foreach (var item in _list)
                _mainQueue.Enqueue(item);


            return result;
        }
    }
}
