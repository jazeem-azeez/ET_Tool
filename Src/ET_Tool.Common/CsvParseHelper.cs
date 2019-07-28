using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET_Tool.Common
{
    public class CsvParseHelper
    {

        public string[] GetFields(string line, string separator = "\",\"")
        {


            string[] values = Regex.Split(line, separator);

            for (int i = 0; i < values.Length; i++)
            {
                //Trim values
                values[i] = values[i].Trim('\"');
            }


            return values;
        }
        public static string[] GetAllFields(string line)
        {
            bool fieldStart = true;
            int markerStartPos = 0;
            string subString = string.Empty;
            List<string> detectedFields = new List<string>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '\'' || line[i] == '"')
                {
                    char temp = line[i];
                    int nextOccurence = line.IndexOf(temp, i);
                    if (nextOccurence > line.Length)
                    {
                        nextOccurence = line.Length - 1;
                    }
                    i = nextOccurence;
                }
                if (line[i] == ',')
                {
                    if (i - markerStartPos > 0)
                    {
                        subString = line.Substring(markerStartPos, i - markerStartPos);
                    }

                    detectedFields.Add("" + subString);
                }

            }
            return detectedFields.ToArray();
        }

    }
}
