﻿namespace NCDC.Base.PriorityQueues {
    /// <summary>
    /// 泛型优先队列 https://www.cnblogs.com/skyivben/archive/2009/04/18/1438731.html 优化T类型不需要实现IComparable
    /// </summary>
    /// <typeparam name="T">实现IComparable&lt;T&gt;的类型</typeparam>
    public class PriorityQueue<T>:IDisposable {
        private const int defaultCapacity = 0x10;//默认容量为16

        private IComparer<T> comparer;
        public PriorityQueue()
            : this(defaultCapacity) {
        }
        public PriorityQueue(int initCapacity,bool ascending = true,IComparer<T> comparer=null) {
            buffer = new T[initCapacity];
            heapLength = 0;
            descending = ascending;
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        public bool IsEmpty() {
            return heapLength == 0;
        }

        public T Top() {
            if (heapLength == 0) throw new OverflowException("queu is empty no element can return");
            return buffer[0];
        }


        public void Push(T obj) {
            if (IsFull()) expand();
            buffer[heapLength] = obj;
            Heap<T>.heapAdjustFromBottom(buffer, heapLength, descending,comparer);
            heapLength++;
        }

        public void Pop() {
            if (heapLength == 0) throw new OverflowException("优先队列为空时无法执行出队操作");
            --heapLength;
            swap(0, heapLength);
            Heap<T>.heapAdjustFromTop(buffer, 0, heapLength, descending,this.comparer);
        }
        /// <summary>
        /// 集合是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return heapLength == buffer.Length;
        }

        private void expand() {
            Array.Resize<T>(ref buffer, buffer.Length * 2);
        }

        private void swap(int a, int b) {
            T tmp = buffer[a];
            buffer[a] = buffer[b];
            buffer[b] = tmp;
        }

        private bool descending;
        private int heapLength;
        private T[] buffer;

        public void Dispose()
        {
            if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
            {
                for (var i = 0; i < buffer.Length; i++)
                {
                    ((IDisposable)buffer[i]).Dispose();
                }
            }
        }
    }
}
