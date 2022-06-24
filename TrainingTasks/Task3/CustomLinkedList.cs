using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTasks.Task3
{
    internal sealed class CustomLinkedList<T>
    {
        private List<CustomItem<T>> _items;

        public void AddFirst(T item)
        {
            /*if(_items.Count == 0)
                _items.Insert(0, new CustomItem<T>(item));
            else
                _items.Insert(0, new CustomItem<T>(item), );*/
        }

        public void AddLast(T item)
        {

        }

        public void AddAfter(T item)
        {

        }

        public void AddBefore(T item)
        {

        }
    }

    internal sealed class CustomItem<T>
    {
        public readonly CustomItem<T> Next;
        public readonly CustomItem<T> Prev;

        private T _item { get; set; }


        internal CustomItem(T item)
        {
            _item = item;
        }

        internal CustomItem(T item, CustomItem<T> next, CustomItem<T> prev = null) : this(item)
        {
            Next = next;
            Prev = prev;
        }

        internal CustomItem(T item, CustomItem<T> prev) : this(item)
        {
            Prev = prev;
        }

    }
}
