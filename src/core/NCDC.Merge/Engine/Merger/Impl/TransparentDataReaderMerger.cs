using System;
using System.Collections.Generic;
using System.Text;
using NCDC.CommandParser.Abstractions;
using NCDC.CommandParser.Command;
using NCDC.Basic.Parser.Command;
using NCDC.Basic.Parser.MetaData.Schema;
using OpenConnector.Merge.Reader.Transparent;
using OpenConnector.StreamDataReaders;

namespace OpenConnector.Merge.Engine.Merger.Impl
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/5/5 10:15:26
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class TransparentDataReaderMerger:IDataReaderMerger
    {
        public IStreamDataReader Merge(List<IStreamDataReader> streamDataReaders, ISqlCommandContext<ISqlCommand> sqlCommandContext, SchemaMetaData schemaMetaData)
        {
            return new TransparentMergedDataReader(streamDataReaders[0]);
        }
    }
}
