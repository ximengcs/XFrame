using System;

namespace XFrame.Core.Binder
{
    public interface IChangeableValue
    {
        protected internal event Action OnChange;
    }
}
