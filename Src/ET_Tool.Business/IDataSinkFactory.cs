namespace ET_Tool.Business
{
    public interface IDataSinkFactory
    {
        IDataSink GetDataSink(string name, string outConfiguration);
    }
}