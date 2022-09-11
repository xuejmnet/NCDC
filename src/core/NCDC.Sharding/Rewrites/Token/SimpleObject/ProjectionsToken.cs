using System.Text;
using NCDC.Sharding.Rewrites.Sql.Token.SimpleObject;

namespace NCDC.Sharding.Rewrites.Token.SimpleObject
{
    /*
    * @Author: xjm
    * @Description:
    * @Date: 2021/4/27 16:43:32
    * @Ver: 1.0
    * @Email: 326308290@qq.com
    */
    public sealed class ProjectionsToken:SqlToken,IAttachable
    {
        private readonly ICollection<string> projections;
        public ProjectionsToken(int startIndex, ICollection<string> projections) : base(startIndex)
        {
            this.projections = projections;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var projection in projections)
            {
                result.Append(", ");
                result.Append(projection);
            }
            return result.ToString();
        }
    }
}
