
namespace XFrame.Modules.Datas
{
    internal interface ICanInitialize
    {
        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="data">数据</param>
        void OnInit(object data);
    }
}
