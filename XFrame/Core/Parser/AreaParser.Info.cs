using System.Collections.Generic;
using System.Linq;

namespace XFrame.Core
{
    public partial class AreaParser
    {
        private struct Info
        {
            public bool IsRange => Min != Max;
            public bool IsNum;
            public int Min;
            public int Max;
            public string Origin;

            public Info(string value)
            {
                IsNum = IntParser.TryParse(value, out Min);
                Max = Min;
                Origin = value;
            }

            public Info(string value, char split)
            {
                Origin = value;

                string[] vList = value.Split(split);
                if (vList.Length == 1)
                {
                    IsNum = IntParser.TryParse(vList[0], out Min);
                    Max = Min;
                }
                else
                {
                    IsNum = true;
                    IntParser.TryParse(vList[0], out Min);
                    IntParser.TryParse(vList[1], out Max);
                }
            }

            public void Calcalute(HashSet<string> result)
            {
                if (IsNum)
                {
                    if (IsRange)
                    {
                        for (int i = Min; i <= Max; i++)
                        {
                            string str = i.ToString();
                            if (!result.Contains(str))
                                result.Add(str);
                        }
                    }
                    else
                    {
                        string str = Min.ToString();
                        if (!result.Contains(str))
                            result.Add(str);
                    }
                }
                else
                {
                    if (!result.Contains(Origin))
                        result.Add(Origin);
                }
            }
        }
    }
}
