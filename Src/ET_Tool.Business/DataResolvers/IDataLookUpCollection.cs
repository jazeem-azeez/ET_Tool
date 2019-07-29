namespace ET_Tool.Business.Mappers
{
    public interface IDataLookUpCollection
    {
        string[] Columns { get; }

        string LookUp(string keyColumn, string valueColumn, string key);
    }
}