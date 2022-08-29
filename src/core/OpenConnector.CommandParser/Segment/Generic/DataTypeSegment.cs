namespace OpenConnector.CommandParser.Segment.Generic
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:04:27
* @Email: 326308290@qq.com
*/
    public sealed class DataTypeSegment:ISqlSegment
    {
        private int startIndex;
    
        private int stopIndex;
    
        private string dataTypeName;
    
        private DataTypeLengthSegment dataLength;

        public string GetDataTypeName()
        {
            return dataTypeName;
        }

        public void SetDataTypeName(string dataTypeName)
        {
            this.dataTypeName = dataTypeName;
        }

        public DataTypeLengthSegment GetDataLength()
        {
            return dataLength;
        }

        public void SetDataLength(DataTypeLengthSegment dataLength)
        {
            this.dataLength = dataLength;
        }
        public int GetStartIndex()
        {
            return startIndex;
        }
        public int GetStopIndex()
        {
            return stopIndex;
        }

        public void SetStartIndex(int startIndex)
        {
            this.startIndex = startIndex;
        }

        public void SetStopIndex(int stopIndex)
        {
            this.stopIndex = stopIndex;
        }
    }
}