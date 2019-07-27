using System.Collections.Generic;

namespace ET_Tool.Business
{
    public class RuntimeArgs
    {
        public bool AutoBuild { get; set; }
        public string LookUpFilePattern { get; set; }
        public string SourceDataFolder { get; set; }
        public string DataSourceFileName { get; set; }
        public string DataSinkFileName { get; set; }
        public string OutConfigFileName { get; set; }
        public Dictionary<string,string> DegreeToDecimalLatLongMapperSettings { get; set; }
        public string DefaultCleanerConfig { get; set; }
    }
}