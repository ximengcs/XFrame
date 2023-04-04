
namespace XFrame.Modules.Event
{
    /// <summary>
    /// 事件
    /// </summary>
    public abstract class XEvent
    {
        /// <summary>
        /// 事件Id
        /// </summary>
        public int Id { get; protected set; }

        public XEvent(int id)
        {
            Id = id;
        }

        public XEvent() { }
    }
}
