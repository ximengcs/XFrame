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
        public static ITypeModule Type => m_Type != null ? m_Type : m_Type = Entry.GetModule<ITypeModule>();
        public static IArchiveModule Archive => m_Archive != null ? m_Archive : m_Archive = Entry.GetModule<IArchiveModule>();
        public static IConditionModule Condition => m_Condition != null ? m_Condition : m_Condition = Entry.GetModule<IConditionModule>();
        public static IContainerModule Container => m_Container != null ? m_Container : m_Container = Entry.GetModule<IContainerModule>();
        public static ICryptoModule Crypto => m_Crypto != null ? m_Crypto : m_Crypto = Entry.GetModule<ICryptoModule>();
        public static IDataModule Data => m_Data != null ? m_Data : m_Data = Entry.GetModule<IDataModule>();
        public static IDownloadModule Download => m_Download != null ? m_Download : m_Download = Entry.GetModule<IDownloadModule>();
        public static IEntityModule Entity => m_Entity != null ? m_Entity : m_Entity = Entry.GetModule<IEntityModule>();
        public static IEventModule Event => m_Event != null ? m_Event : m_Event = Entry.GetModule<IEventModule>();
        public static IFsmModule Fsm => m_Fsm != null ? m_Fsm : m_Fsm = Entry.GetModule<IFsmModule>();
        public static IIdModule Id => m_Id != null ? m_Id : m_Id = Entry.GetModule<IIdModule>();
        public static ILocalizeModule I18N => m_I18N != null ? m_I18N : m_I18N = Entry.GetModule<ILocalizeModule>();
        public static ILogModule Log => m_Log != null ? m_Log : m_Log = Entry.GetModule<ILogModule>();
        public static IPlotModule Plot => m_Plot != null ? m_Plot : m_Plot = Entry.GetModule<IPlotModule>();
        public static IPoolModule Pool => m_Pool != null ? m_Pool : m_Pool = Entry.GetModule<IPoolModule>();
        public static IProcedureModule Procedure => m_Procedure != null ? m_Procedure : m_Procedure = Entry.GetModule<IProcedureModule>();
        public static IRandModule Rand => m_Rand != null ? m_Rand : m_Rand = Entry.GetModule<IRandModule>();
        public static IResModule Res => m_Res != null ? m_Res : m_Res = Entry.GetModule<IResModule>();
        public static ISerializeModule Serialize => m_Serialize != null ? m_Serialize : m_Serialize = Entry.GetModule<ISerializeModule>();
        public static ITaskModule Task => m_Task != null ? m_Task : m_Task = Entry.GetModule<ITaskModule>();
        public static ITimeModule Time => m_Time != null ? m_Time : m_Time = Entry.GetModule<ITimeModule>();
    }
}
