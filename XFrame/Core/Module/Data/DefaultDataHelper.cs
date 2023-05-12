using System;
using XFrame.Utility;
using XFrame.Modules.Serialize;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Modules.XType;

namespace XFrame.Modules.Datas
{
    internal class DefaultDataHelper : IDataHelper
    {
        private class TypeInfo
        {
            public Type TableType;
            public Type JsonType;

            public TypeInfo(Type tableType, Type jsonType = null)
            {
                TableType = tableType;
                JsonType = jsonType;
            }
        }

        private Dictionary<int, TypeInfo> m_TableTypes;
        private Dictionary<Type, List<IDataTable>> m_Tables;

        void IDataHelper.OnInit()
        {
            m_TableTypes = new Dictionary<int, TypeInfo>();
            m_Tables = new Dictionary<Type, List<IDataTable>>();
        }

        void IDataHelper.AddTableType(Type type)
        {
            if (!type.IsGenericType)
            {
                Log.Debug("XFrame", "Data table type error");
                return;
            }

            TableAttribute attr = TypeModule.Inst.GetAttribute<TableAttribute>(type);
            if (attr != null)
            {
                Type jsonType = attr != null ? attr.JsonType : null;
                m_TableTypes.Add(attr.TableType, new TypeInfo(type, jsonType));
            }
        }

        public IDataTable Add(string json, Type dataType)
        {
            if (InnerFindType(dataType, out Type tbType, out Type jsonType))
            {
                return InnerAdd(tbType, jsonType, json);
            }
            else
            {
                Log.Debug("XFrame", $"Add data error. data type not has handler class.");
                return default;
            }
        }

        public bool TryGet(Type dataType, out List<IDataTable> list)
        {
            if (InnerFindType(dataType, out Type tbType, out Type jsonType))
            {
                return m_Tables.TryGetValue(tbType, out list);
            }
            else
            {
                Log.Debug("XFrame", $"Get data error. data type not has handler class.");
                list = null;
                return false;
            }
        }

        private bool InnerFindType(Type dataType, out Type tbType, out Type jsonType)
        {
            tbType = null;
            jsonType = null;

            DataAttribute attr = TypeModule.Inst.GetAttribute<DataAttribute>(dataType);
            int tableType = attr != null ? attr.TableType : TableType.List;
            if (m_TableTypes.TryGetValue(tableType, out TypeInfo info))
            {
                tbType = info.TableType;
                jsonType = info.JsonType;

                if (tbType != null)
                    tbType = tbType.MakeGenericType(dataType);

                if (jsonType != null)
                    jsonType = jsonType.MakeGenericType(dataType);
                else
                    jsonType = dataType;

                return true;
            }

            return false;
        }

        private IDataTable InnerAdd(Type tbType, Type jsonType, string json)
        {
            object data = SerializeModule.Inst.DeserializeJsonToObject(json, jsonType);
            IDataTable table = (IDataTable)Activator.CreateInstance(tbType);
            table.OnInit(data);

            List<IDataTable> list;
            if (!m_Tables.TryGetValue(tbType, out list))
            {
                list = new List<IDataTable>();
                m_Tables[tbType] = list;
            }
            list.Add(table);
            return table;
        }
    }
}
