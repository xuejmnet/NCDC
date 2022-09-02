namespace OpenConnector.CommandParserBinder.MetaData.Index
{
/*
* @Author: xjm
* @Description:
* @Date: Thursday, 08 April 2021 22:08:49
* @Email: 326308290@qq.com
*/
    public sealed class IndexMetaData
    {
        public IndexMetaData(string name)
        {
            Name = name;
        }

        private bool Equals(IndexMetaData other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is IndexMetaData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public string Name { get; }
    }
}