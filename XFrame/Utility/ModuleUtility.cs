using XFrame.Modules.Archives;
using XFrame.Modules.Conditions;
using XFrame.Modules.Containers;
using XFrame.Modules.Crypto;
using XFrame.Modules.Datas;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Download;
using XFrame.Modules.Entities;
using XFrame.Modules.Event;
using XFrame.Modules.ID;
using XFrame.Modules.Local;
using XFrame.Modules.Plots;
using XFrame.Modules.Pools;
using XFrame.Modules.Procedure;
using XFrame.Modules.Rand;
using XFrame.Modules.Resource;
using XFrame.Modules.Serialize;
using XFrame.Modules.StateMachine;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;
using XFrame.Modules.XType;

namespace XFrame.Core
{
    public partial class ModuleUtility
    {
        private static ITypeModule m_Type;
        public static ITypeModule Type => m_Type != null ? m_Type : m_Type = Entry.GetModule<ITypeModule>();

        private static IArchiveModule m_Archive;
        public static IArchiveModule Archive => m_Archive != null ? m_Archive : m_Archive = Entry.GetModule<IArchiveModule>();

        private static IConditionModule m_Condition;
        public static IConditionModule Condition => m_Condition != null ? m_Condition : m_Condition = Entry.GetModule<IConditionModule>();

        private static IContainerModule m_Container;
        public static IContainerModule Container => m_Container != null ? m_Container : m_Container = Entry.GetModule<IContainerModule>();

        private static ICryptoModule m_Crypto;
        public static ICryptoModule Crypto => m_Crypto != null ? m_Crypto : m_Crypto = Entry.GetModule<ICryptoModule>();

        private static IDataModule m_Data;
        public static IDataModule Data => m_Data != null ? m_Data : m_Data = Entry.GetModule<IDataModule>();

        private static IDownloadModule m_Download;
        public static IDownloadModule Download => m_Download != null ? m_Download : m_Download = Entry.GetModule<IDownloadModule>();

        private static IEntityModule m_Entity;
        public static IEntityModule Entity => m_Entity != null ? m_Entity : m_Entity = Entry.GetModule<IEntityModule>();

        private static IEventModule m_Event;
        public static IEventModule Event => m_Event != null ? m_Event : m_Event = Entry.GetModule<IEventModule>();

        private static IFsmModule m_Fsm;
        public static IFsmModule Fsm => m_Fsm != null ? m_Fsm : m_Fsm = Entry.GetModule<IFsmModule>();

        private static IIdModule m_Id;
        public static IIdModule Id => m_Id != null ? m_Id : m_Id = Entry.GetModule<IIdModule>();

        private static ILocalizeModule m_Local;
        public static ILocalizeModule Local => m_Local != null ? m_Local : m_Local = Entry.GetModule<ILocalizeModule>();

        private static ILogModule m_Log;
        public static ILogModule Log => m_Log != null ? m_Log : m_Log = Entry.GetModule<ILogModule>();

        private static IPlotModule m_Plot;
        public static IPlotModule Plot => m_Plot != null ? m_Plot : m_Plot = Entry.GetModule<IPlotModule>();

        private static IPoolModule m_Pool;
        public static IPoolModule Pool => m_Pool != null ? m_Pool : m_Pool = Entry.GetModule<IPoolModule>();

        private static IProcedureModule m_Procedure;
        public static IProcedureModule Procedure => m_Procedure != null ? m_Procedure : m_Procedure = Entry.GetModule<IProcedureModule>();

        private static IRandModule m_Rand;
        public static IRandModule Rand => m_Rand != null ? m_Rand : m_Rand = Entry.GetModule<IRandModule>();

        private static IResModule m_Res;
        public static IResModule Res => m_Res != null ? m_Res : m_Res = Entry.GetModule<IResModule>();

        private static ISerializeModule m_Serialize;
        public static ISerializeModule Serialize => m_Serialize != null ? m_Serialize : m_Serialize = Entry.GetModule<ISerializeModule>();

        private static ITaskModule m_Task;
        public static ITaskModule Task => m_Task != null ? m_Task : m_Task = Entry.GetModule<ITaskModule>();

        private static ITimeModule m_Time;
        public static ITimeModule Time => m_Time != null ? m_Time : m_Time = Entry.GetModule<ITimeModule>();

        public static void Refresh()
        {
            m_Type = null;
            m_Archive = null;
            m_Condition = null;
            m_Container = null;
            m_Crypto = null;
            m_Data = null;
            m_Download = null;
            m_Entity = null;
            m_Event = null;
            m_Fsm = null;
            m_Id = null;
            m_Local = null;
            m_Log = null;
            m_Plot = null;
            m_Pool = null;
            m_Procedure = null;
            m_Rand = null;
            m_Res = null;
            m_Serialize = null;
            m_Task = null;
            m_Time = null;
        }
    }
}
