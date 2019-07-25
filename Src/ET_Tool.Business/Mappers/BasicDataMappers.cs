using System.Collections.Generic;

namespace ET_Tool.Business.Mappers
{
    public class BasicDataMappers : IDataMapper
    {
        private readonly Dictionary<string, Dictionary<string, string>> _keyValuePairs = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, string> MappingRules = new Dictionary<string, string>();
        public string ResolveFor(string fieldKey) => string.Empty;
    }
}
