using System.Reflection;
using System.Text;

namespace NCDC.Protocol.Common;

public abstract class Enumeration<TValue,TEnum> : IEquatable<Enumeration<TValue,TEnum>>
 where TValue:Enum
    where TEnum : Enumeration<TValue,TEnum>
{
    private static readonly Dictionary<TValue, TEnum> Enumerations = CreateEnumerations();
    public TValue Value { get; }
    public Encoding Encoding { get; }
    public string Name { get; }

    protected Enumeration(TValue value, Encoding encoding, string name)
    {
        Value = value;
        Encoding = encoding;
        Name = name;
    }

    public static TEnum? FromValue(TValue value)
    {
        return Enumerations.TryGetValue(value, out var enumeration) ? enumeration : default;
    }


    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.SingleOrDefault(o=>o.Name==name);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Enumeration<TValue,TEnum>)obj);
    }

    public bool Equals(Enumeration<TValue,TEnum>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    private static Dictionary<TValue, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);
        var fieldsForType = enumerationType.GetFields(
                BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.FlattenHierarchy)
            .Where(fieldinfo => enumerationType.IsAssignableFrom(fieldinfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);
        return fieldsForType.ToDictionary(o => o.Value);
    }
}