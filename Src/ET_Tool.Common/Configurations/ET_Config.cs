using System.Collections.Generic;

namespace ET_Tool.Common.Configurations
{
    public class ET_Config
    {
        /*
         * key, pattern/fileName
         */
        public Dictionary<string, string> LookUpInfoSourceUriCollection { get; set; }

        /*
         * format is destination Key : [sequence of {key,TransformerActorKey}]
         */
        public Dictionary<string, List<KeyValuePair<string, string>>> TransformationSequenceInfo { get; set; }
        public bool AutoDetect { get; set; }

    }
}
