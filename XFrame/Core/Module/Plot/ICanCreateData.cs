
namespace XFrame.Modules.Plots
{
    internal interface ICanCreateData
    {
        /// <summary>
        /// 创建一个数据提供器
        /// </summary>
        /// <returns>数据提供器</returns>
        IPlotDataProvider CreateDataProvider(IStory story);
    }
}
