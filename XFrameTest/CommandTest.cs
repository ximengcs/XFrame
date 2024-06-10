
using System.Collections;
using System.Text;

namespace XFrameTest
{
    public class CommandLineBatch : IEnumerable
    {
        private CommandLine[] Lines;

        public CommandLineBatch(string origin)
        {
            InnerAnalyze(origin);
        }

        public IEnumerator GetEnumerator()
        {
            return Lines.GetEnumerator();
        }

        private void InnerAnalyze(string param)
        {
            string[] listStr = param.Split('\n');
            Lines = new CommandLine[listStr.Length];
            for (int i = 0; i < listStr.Length; i++)
                Lines[i] = new CommandLine(listStr[i]);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (CommandLine line in Lines)
                sb.AppendLine(line.ToString());
            return sb.ToString();
        }
    }

    internal class CommandLine
    {
        private string m_Name;
        private string[] m_Params;

        public string Name => m_Name;

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= m_Params.Length)
                    return default;
                return m_Params[index];
            }
        }

        public CommandLine(string origin)
        {
            InnerAnalyze(origin);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Name);
            foreach (string str in m_Params)
            {
                sb.Append(" [");
                sb.Append(str);
                sb.Append(']');
            }
            return sb.ToString();
        }

        private void InnerAnalyze(string param)
        {
            StringBuilder cur = null;
            List<StringBuilder> list = new List<StringBuilder>();
            bool hasAdd = false;
            bool valid = false;
            int index = 0;
            bool isString = false;
            foreach (char c in param)
            {
                if (list.Count <= index && cur == null)
                {
                    cur = new StringBuilder();
                }

                if (c == '\"')
                {
                    if (cur.Length == 0)
                    {
                        valid = true;
                        isString = true;
                    }
                    else
                    {
                        index++;
                        cur = null;
                        isString = false;
                        valid = false;
                        hasAdd = false;
                    }
                }
                else
                {
                    if (c == ' ' && !isString)
                    {
                        index++;
                        cur = null;
                        isString = false;
                        valid = false;
                        hasAdd = false;
                    }
                    else
                    {
                        valid = true;
                        cur.Append(c);
                    }
                }

                if (valid && !hasAdd)
                {
                    hasAdd = true;
                    list.Add(cur);
                }
            }

            m_Params = new string[list.Count - 1];
            m_Name = list[0].ToString();
            for (int i = 1; i < list.Count; i++)
                m_Params[i - 1] = list[i].ToString();
        }
    }

    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void Test1()
        {
            string param = "clear -o test \"comment test\" test2\n" +
                "cleardata -i";
            CommandLineBatch line = new CommandLineBatch(param);
            Console.Write(line);
        }
    }
}
