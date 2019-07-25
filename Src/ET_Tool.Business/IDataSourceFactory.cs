namespace ET_Tool.Business
{
    public interface IDataSourceFactory
    {
        IDataSource GetDataSource(string name);
    }
}