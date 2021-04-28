using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace ShardingConnector.DataStructure.RangeStructure
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/28 15:09:48
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    internal class Cut
    {
        internal static Cut<C> BelowAll<C>() where C : IComparable
        {
            return Cut<C>.BelowAll.INSTANCE;
        }

        internal static Cut<C> AboveAll<C>() where C : IComparable
        {
            return Cut<C>.AboveAll.INSTANCE;
        }

        internal static Cut<C> BelowValue<C>(C endpoint) where C : IComparable
        {
            return new Cut<C>.BelowValue(endpoint);
        }

        internal static Cut<C> AboveValue<C>(C endpoint) where C : IComparable
        {
            return new Cut<C>.AboveValue(endpoint);
        }
    }

    /// <summary>
    /// Implementation detail for the internal structure of <seealso cref="Range{C}"/> instances. Represents a unique
    /// way of "cutting" a "number line" (actually of instances of type <typeparamref name="C"/>, not necessarily
    /// "numbers") into two sections; this can be done below a certain value, above a certain value,
    /// below all values or above all values. With this object defined in this way, an interval can
    /// always be represented by a pair of <seealso cref="Cut{C}"/> instances.
    /// </summary>
    [Serializable]
    internal abstract class Cut<C> : IComparable<Cut<C>> where C : IComparable
    {
        internal readonly C _endpoint;

        internal Cut(C endpoint)
        {
            _endpoint = endpoint;
        }

        internal abstract bool IsLessThan(C value);

        internal abstract BoundType TypeAsLowerBound();

        internal abstract BoundType TypeAsUpperBound();

        internal abstract void DescribeAsLowerBound(StringBuilder sb);

        internal abstract void DescribeAsUpperBound(StringBuilder sb);

        // note: overridden by {BELOW,ABOVE}_ALL
        public virtual int CompareTo(Cut<C> that)
        {
            if (that == BelowAll.INSTANCE)
            {
                return 1;
            }
            if (that == AboveAll.INSTANCE)
            {
                return -1;
            }
            int result = _endpoint.CompareTo(that._endpoint);
            if (result != 0)
            {
                return result;
            }
            // same value. below comes before above
            return (this is AboveValue).CompareTo(that is AboveValue);
        }

        internal virtual C Endpoint()
        {
            return _endpoint;
        }

        public override bool Equals(object obj)
        {
            if (obj is Cut<C> that)
            {
                int compareResult = CompareTo(that);
                return compareResult == 0;
            }
            return false;
        }

        // Prevent "missing hashCode" warning by explicitly forcing subclasses implement it
        public override abstract int GetHashCode();


        [Serializable]
        public sealed class BelowAll : Cut<C>
        {
            public static readonly BelowAll INSTANCE = new BelowAll();

            internal BelowAll() : base(default(C))
            {
            }
            internal override C Endpoint()
            {
                throw new InvalidOperationException("range unbounded on this side");
            }
            internal override bool IsLessThan(C value)
            {
                return true;
            }
            internal override BoundType TypeAsLowerBound()
            {
                throw new InvalidOperationException();
            }
            internal override BoundType TypeAsUpperBound()
            {
                throw new InvalidOperationException("this statement should be unreachable");
            }
            internal override void DescribeAsLowerBound(StringBuilder sb)
            {
                sb.Append("(-\u221e");
            }
            internal override void DescribeAsUpperBound(StringBuilder sb)
            {
                throw new InvalidOperationException();
            }
            public override int CompareTo(Cut<C> o)
            {
                return (o is BelowAll) ? 0 : -1;
            }
            public override int GetHashCode()
            {
                return RuntimeHelpers.GetHashCode(this);
            }
            public override string ToString()
            {
                return "-\u221e";
            }
        }

        [Serializable]
        public sealed class AboveAll : Cut<C>
        {
            internal static readonly AboveAll INSTANCE = new AboveAll();

            internal AboveAll() : base(default(C))
            {
            }
            internal override C Endpoint()
            {
                throw new InvalidOperationException("range unbounded on this side");
            }
            internal override bool IsLessThan(C value)
            {
                return false;
            }
            internal override BoundType TypeAsLowerBound()
            {
                throw new InvalidOperationException("this statement should be unreachable");
            }
            internal override BoundType TypeAsUpperBound()
            {
                throw new InvalidOperationException();
            }
            internal override void DescribeAsLowerBound(StringBuilder sb)
            {
                throw new InvalidOperationException();
            }
            internal override void DescribeAsUpperBound(StringBuilder sb)
            {
                sb.Append("+\u221e)");
            }
            public override int CompareTo(Cut<C> o)
            {
                return (o is AboveAll) ? 0 : 1;
            }
            public override int GetHashCode()
            {
                return RuntimeHelpers.GetHashCode(this);
            }
            public override string ToString()
            {
                return "+\u221e";
            }
        }

        [Serializable]
        public sealed class BelowValue : Cut<C>
        {
            internal BelowValue(C endpoint) : base(endpoint)
            {
            }
            internal override bool IsLessThan(C value)
            {
                return _endpoint.CompareTo(value) <= 0;
            }
            internal override BoundType TypeAsLowerBound()
            {
                return BoundType.CLOSED;
            }
            internal override BoundType TypeAsUpperBound()
            {
                return BoundType.OPEN;
            }
            internal override void DescribeAsLowerBound(StringBuilder sb)
            {
                sb.Append('[').Append(_endpoint);
            }
            internal override void DescribeAsUpperBound(StringBuilder sb)
            {
                sb.Append(_endpoint).Append(')');
            }
            public override int GetHashCode()
            {
                return _endpoint.GetHashCode();
            }
            public override string ToString()
            {
                return "\\" + _endpoint + "/";
            }
        }

        [Serializable]
        public sealed class AboveValue : Cut<C>
        {
            internal AboveValue(C endpoint) : base(endpoint)
            {
            }
            internal override bool IsLessThan(C value)
            {
                return _endpoint.CompareTo(value) < 0;
            }
            internal override BoundType TypeAsLowerBound()
            {
                return BoundType.OPEN;
            }
            internal override BoundType TypeAsUpperBound()
            {
                return BoundType.CLOSED;
            }
            internal override void DescribeAsLowerBound(StringBuilder sb)
            {
                sb.Append('(').Append(_endpoint);
            }
            internal override void DescribeAsUpperBound(StringBuilder sb)
            {
                sb.Append(_endpoint).Append(']');
            }
            public override int GetHashCode()
            {
                return ~_endpoint.GetHashCode();
            }
            public override string ToString()
            {
                return "/" + _endpoint + "\\";
            }
        }
    }
}