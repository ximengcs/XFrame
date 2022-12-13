
using System;

namespace XFrame.Modules
{
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
