using System;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 通用任务
    /// </summary>
    public partial class XTask : TaskBase
    {
        private Type m_HandleType = typeof(XTask);

        public override Type HandleType => m_HandleType;

        protected override void OnInit()
        {
            AddStrategy(new TaskStrategy());
        }
    }
}
