namespace ET_Tool.Business
{
    public interface IDataCleanerFactory
    {
        IDataCleaner GetDataCleaner(string name,IDataCleanerConfig cleanerConfig);
        IDataCleaner GetDataCleaner(string name);
    }
}