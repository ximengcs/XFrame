
using XFrame.Core;

namespace XFrame.Modules.Tasks
{
    public static class TaskUtility
    {
        public static void Delete(this ITask task)
        {
            XModule.Task.Remove(task);
        }

        public static ITask AutoDelete(this ITask task)
        {
            return task.OnCompleteAfter(Delete);
        }

        public static void StartWithDelete(this ITask task)
        {
            task.AutoDelete().Start();
        }
    }
}
