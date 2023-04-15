using System;

namespace XFrame.Modules.Local
{
    public partial class LocalizeModule
    {
        private class FormatterProvider : IFormatProvider
        {
            private Type m_Type;
            private ICustomFormatter m_Formatter;

            public void SetFormatter(ICustomFormatter formatter)
            {
                m_Type = typeof(ICustomFormatter);
                m_Formatter = formatter;
            }

            public object GetFormat(Type formatType)
            {
                if (formatType == m_Type)
                    return m_Formatter;
                return null;
            }
        }
    }
}
