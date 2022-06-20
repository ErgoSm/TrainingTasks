using System.Collections.Generic;
using TrainingTasks.Task1;
using Xunit;

namespace TrainingTask.Tests.Task1
{
    public class Task1Test
    {
        [Fact]
        public void Custom_stack_simple_test()
        {
            var array = new int[] { 7, 3, 2, 5, -10, 17, 0, 4 };
            var customStack = new CustomStack<int>();
            var stack = new Stack<int>();

            for(int i = 0; i < 4; i++)
            {
                stack.Push(array[i]);
                customStack.Push(array[i]);
            }

            for (int i = 4; i < array.Length; i++)
            {
                stack.Push(array[i]);
                customStack.Push(array[i]);

                Assert.Equal(stack.Pop(), customStack.Pop());
            }
        }
    }
}
