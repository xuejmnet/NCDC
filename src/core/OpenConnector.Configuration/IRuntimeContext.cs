namespace OpenConnector.Configuration;

public interface IRuntimeContext
{
    IMergeComparer GetMergeComparer();
}