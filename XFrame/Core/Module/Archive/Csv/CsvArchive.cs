
using XFrame.Collections;

namespace XFrame.Modules.Archives
{
    [Archive("csv")]
    public class CsvArchive : IArchive
    {
        //private string m_Path;
        //private Csv m_Csv;

        void IArchive.OnInit(string path)
        {
            //m_Path = path;
        }

        public void Delete()
        {

        }

        public void Save()
        {

        }
    }
}
