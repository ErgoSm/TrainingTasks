namespace TrainingTasks.Task1
{
    internal sealed class CustomStack<T>
    {
        private Queue<T> _mainQueue = new Queue<T>();

        public void Push(T item)
        {
            _mainQueue.Enqueue(item);
        }

        public T Pop()
        {
            var size = _mainQueue.Count;
            for(int i = 0; i < size - 1; i++)
                _mainQueue.Enqueue(_mainQueue.Dequeue());

            var result = _mainQueue.Dequeue();


            return result;
        }
    }
}
