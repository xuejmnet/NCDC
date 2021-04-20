namespace ShardingConnector.CommandParser.Segment.Generic
{
/*
* @Author: xjm
* @Description:
* @Date: Monday, 12 April 2021 22:05:13
* @Email: 326308290@qq.com
*/
    public sealed class DataTypeLengthSegment:ISqlSegment
    {
        
        private int startIndex;
    
        private int stopIndex;
    
        private int precision;
    
        private int scale;
    
        /// <summary>
        /// get secondNumber.
        /// </summary>
        /// <returns></returns>
        public int GetScale() {
            return scale;
        }

        public int GetStartIndex()
        {
            return startIndex;
        }

        public int GetStopIndex()
        {
            return stopIndex;
        }

        public int GetPrecision()
        {
            return precision;
        }

        public void SetScale(int scale)
        {
            this.scale = scale;
        }

        public void SetStartIndex(int startIndex)
        {
            this.startIndex = startIndex;
        }

        public void SetStopIndex(int stopIndex)
        {
            this.stopIndex = stopIndex;
        }

        public void SetPrecision(int precision)
        {
            this.precision = this.precision;
        }
    }
}