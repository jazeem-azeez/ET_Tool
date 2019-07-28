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
            int markerStartPos = 0;
            string subString = string.Empty;
            List<string> detectedFields = new List<string>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    char temp = line[i];
                    int nextOccurence = line.IndexOf(temp, i + 1 >= line.Length ? i : i + 1);
                    if (nextOccurence >= line.Length)
                    {
                        nextOccurence = line.Length - 1;
                    }
                    if (nextOccurence >= 0)
                    {
                        i = nextOccurence;
                    }
                }
                if (line[i] == ',')
                {

                    subString = line.Substring(markerStartPos, i - markerStartPos); 
                    markerStartPos = i + 1;
                    detectedFields.Add("" + subString);
                }

            }
            if (markerStartPos < line.Length || line.EndsWith(','))
            {

                subString = line.Substring(markerStartPos, line.Length - markerStartPos);
                detectedFields.Add(subString);
            }

            return detectedFields.ToArray();
        }

    }
}
