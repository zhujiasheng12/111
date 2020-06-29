using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterFaceLib
{
    public interface IHandleCncData
    {
        /// <summary>
        /// 获取机床数据
        /// </summary>
        /// <returns></returns>
        List<CncInfo> GetCncInfo();
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="models">机床数据所在对象</param>
        /// <returns></returns>
        bool CncDataSave(Dictionary<int, IMonitorRealTimeModel> models);
    }
}
