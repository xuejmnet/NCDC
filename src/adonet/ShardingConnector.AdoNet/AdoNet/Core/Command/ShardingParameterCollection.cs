using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ShardingConnector.AdoNet.AdoNet.Core.Command
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/7/23 15:41:38
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ShardingParameterCollection:DbParameterCollection
    {
        private readonly ICollection<ShardingMethodInvocation> shardingMethodInvocations =
            new LinkedList<ShardingMethodInvocation>();

        private readonly List<ShardingParameter> @params = new List<ShardingParameter>();

        public void ReplyMethodsInvocation(object target)
        {
            foreach (var shardingMethodInvocation in shardingMethodInvocations)
            {
                shardingMethodInvocation.Inovke(target);
            }
        }
        public override int Add(object value)
        {
            shardingMethodInvocations.Add(new ShardingMethodInvocation(typeof(DbParameterCollection).GetMethod("Add"),new object[]{value}));
             
            @params.Add((ShardingParameter)value);
            return @params.Count - 1;
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public override void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            throw new NotImplementedException();
        }

        public override int Count { get; }
        public override object SyncRoot { get; }

        public override int IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string value)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(int index)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override void AddRange(Array values)
        {
            throw new NotImplementedException();
        }
    }
}
