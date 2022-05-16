using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
* @Author: xjm
* @Description:
* @Date: DATE
* @Email: 326308290@qq.com
*/
namespace ShardingConnector.DataStructure
{
    public class LinkedDictionary<K, V> : IDictionary<K, V>, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>
    {
        private List<K> list = new List<K>();
        private Dictionary<K, V> dictionary = new Dictionary<K, V>();

        public LinkedDictionary()
        {
        }

        public V this[K key]
        {
            get { return this.dictionary[key]; }
            set
            {
                this.dictionary[key] = value;
                if (!this.list.Contains(key))
                {
                    this.list.Add(key);
                }
            }
        }

        public int Count => this.dictionary.Count;

        public bool IsReadOnly => false;

        ICollection<K> IDictionary<K, V>.Keys => this.list;

        ICollection<V> IDictionary<K, V>.Values
        {
            get
            {
                List<V> values = new List<V>(this.dictionary.Count);
                foreach (K key in this.list)
                {
                    V value = default(V);
                    this.dictionary.TryGetValue(key, out value);
                    values.Add(value);
                }

                return values;
            }
        }

        public void Add(KeyValuePair<K, V> item)
        {
            this.dictionary.Add(item.Key, item.Value);
            if (!this.list.Contains(item.Key))
            {
                this.list.Add(item.Key);
            }
        }

        public void Add(K key, V value)
        {
            this.dictionary.Add(key, value);
            if (!this.list.Contains(key))
            {
                this.list.Add(key);
            }
        }

        public void Clear()
        {
            this.dictionary.Clear();
            this.list.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return this.dictionary.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            if (this.Contains(item))
            {
                this.list.Remove(item.Key);
                return this.dictionary.Remove(item.Key);
            }
            else
            {
                return false;
            }
        }

        public bool Remove(K key)
        {
            if (this.dictionary.ContainsKey(key))
            {
                this.list.Remove(key);
                return this.dictionary.Remove(key);
            }
            else
            {
                return false;
            }
        }

        public bool TryGetValue(K key, out V value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public V Get(K key)
        {
            V value = default(V);
            this.dictionary.TryGetValue(key, out value);
            return value;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (K key in this.list)
            {
                V value = default(V);
                this.dictionary.TryGetValue(key, out value);
                yield return new KeyValuePair<K, V>(key, value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class LinkedDictionaryIterator<K, V> : IEnumerator<V>
        {
            private int i;
            private readonly Dictionary<K, V> dictionary;
            private readonly List<K> list;

            public LinkedDictionaryIterator(Dictionary<K, V> dictionary, List<K> list)
            {
                this.dictionary = dictionary;
                this.list = list;
                this.i = 0;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return this.i < this.dictionary.Count;
            }

            public void Reset()
            {
                this.i = 0;
            }

            public KeyValuePair<K, V> Current
            {
                get
                {
                    int ii = this.i;
                    ++this.i;
                    V value = default(V);
                    K key = this.list[ii];
                    this.dictionary.TryGetValue(key, out value);
                    return new KeyValuePair<K, V>(key, value);
                }
            }

            V IEnumerator<V>.Current
            {
                get
                {
                    int ii = this.i;
                    ++this.i;
                    V value = default(V);
                    K key = this.list[ii];
                    this.dictionary.TryGetValue(key, out value);
                    return value;
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}