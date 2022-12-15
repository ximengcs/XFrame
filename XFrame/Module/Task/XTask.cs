
using System;

namespace XFrame.Modules
{
    /// <summary>
    /// 通用任务
    /// </summary>
    public partial class XTask : TaskBase
    {
        private Type m_HandleType = typeof(XTask);

        public override Type HandleType => m_HandleType;

        public override void OnInit()
        {
            base.OnInit();
            AddStrategy(new TaskStrategy());
        }
    }
}
