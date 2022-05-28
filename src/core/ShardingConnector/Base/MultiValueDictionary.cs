﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ShardingConnector.Base
{
	/*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/24 14:49:06
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
	/// <summary>
	/// Extension to the normal Dictionary. This class can store more than one value for every key. It keeps a HashSet for every Key value.
	/// Calling Add with the same Key and multiple values will store each value under the same Key in the Dictionary. Obtaining the values
	/// for a Key will return the HashSet with the Values of the Key. It can also merge with other instances of MultiValueDictionary, as long
	/// as the TKey and TValue types are equal.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	[Serializable]
	public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, HashSet<TValue>>
	{
		#region Class Member Declarations
		private IEqualityComparer<TValue> _valueComparer;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiValueDictionary&lt;TKey, TValue&gt;"/> class.
		/// </summary>
		public MultiValueDictionary()
			: this(null)
		{
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="MultiValueDictionary&lt;TKey, TValue&gt;"/> class.
		/// </summary>
		/// <param name="valueComparer">The IEqualityComparer&lt;TValue&gt; which is used for the HashSet objects created for each TKey instance. 
		/// Can be null, in which case the default EqualityComparer is used.</param>
		public MultiValueDictionary(IEqualityComparer<TValue> valueComparer) : base()
		{
			_valueComparer = valueComparer;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="MultiValueDictionary&lt;TKey, TValue&gt;"/> class.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		protected MultiValueDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			try
			{
				_valueComparer = info.GetValue("_valueComparer", typeof(IEqualityComparer<TValue>)) as IEqualityComparer<TValue>;
			}
			catch
			{
				// ignore. Versioning issue -> data doesn't contain the comparer. 
			}
		}


		/// <summary>
		/// Clones this instance using shallow copy
		/// </summary>
		/// <returns></returns>
		public MultiValueDictionary<TKey, TValue> Clone()
		{
			return this.MemberwiseClone() as MultiValueDictionary<TKey, TValue>;
		}


		/// <summary>
		/// Gets the object data.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_valueComparer", _valueComparer);
		}


		/// <summary>
		/// Adds the specified value under the specified key
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void Add(TKey key, TValue value)
		{


            ShardingAssert.ShouldBeNotNull(key, "key");

			HashSet<TValue> container;
			if (!this.TryGetValue(key, out container))
			{
				container = new HashSet<TValue>(_valueComparer);
				this.Add(key, container);
			}
			container.Add(value);
		}


		/// <summary>
		/// Adds the range of values under the key specified.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="values">The values.</param>
		public void AddRange(TKey key, IEnumerable<TValue> values)
		{
			if (values == null)
			{
				return;
			}

			foreach (TValue value in values)
			{
				this.Add(key, value);
			}
		}


		/// <summary>
		/// Determines whether this dictionary contains the specified value for the specified key 
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns>true if the value is stored for the specified key in this dictionary, false otherwise</returns>
		public bool ContainsValue(TKey key, TValue value)
		{
            ShardingAssert.ShouldBeNotNull(key, "key");
			bool toReturn = false;
			HashSet<TValue> values;
			if (this.TryGetValue(key, out values))
			{
				toReturn = values.Contains(value);
			}
			return toReturn;
		}


		/// <summary>
		/// Removes the specified value for the specified key. It will leave the key in the dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void Remove(TKey key, TValue value)
		{
            ShardingAssert.ShouldBeNotNull(key, "key");

			HashSet<TValue> container;
			if (this.TryGetValue(key, out container))
			{
				container.Remove(value);
				if (container.Count <= 0)
				{
					this.Remove(key);
				}
			}
		}


		/// <summary>
		/// Merges the specified multivaluedictionary into this instance.
		/// </summary>
		/// <param name="toMergeWith">To merge with.</param>
		/// <remarks>If this instance has an EqualityComparer set for the values, it is used when merging toMergeWith</remarks>
		public void Merge(MultiValueDictionary<TKey, TValue> toMergeWith)
		{
			if (toMergeWith == null)
			{
				return;
			}

			foreach (KeyValuePair<TKey, HashSet<TValue>> pair in toMergeWith)
			{
				foreach (TValue value in pair.Value)
				{
					this.Add(pair.Key, value);
				}
			}
		}


		/// <summary>
		/// Gets the values for the key specified. This method is useful if you want to avoid an exception for key value retrieval and you can't use TryGetValue
		/// (e.g. in lambdas)
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="returnEmptySet">if set to true and the key isn't found, an empty hashset is returned, otherwise, if the key isn't found, null is returned</param>
		/// <returns>
		/// This method will return null (or an empty set if returnEmptySet is true) if the key wasn't found, or
		/// the values if key was found.
		/// </returns>
		public HashSet<TValue> GetValues(TKey key, bool returnEmptySet)
		{
			HashSet<TValue> toReturn;
			if (!this.TryGetValue(key, out toReturn) && returnEmptySet)
			{
				toReturn = new HashSet<TValue>(_valueComparer);
			}
			return toReturn;
		}
	}
}
