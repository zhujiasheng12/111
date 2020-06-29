using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterFaceLib
{
    public abstract class CncMachine//抽象类，设备实体
    {
        public CncInfo cncInfo;
        /// <summary>
        /// 连接机床
        /// </summary>
        /// <returns></returns>
        public abstract bool Connect();
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public abstract bool DisConnect();
        /// <summary>
        /// 是否连接
        /// </summary>
        /// <returns></returns>
        public abstract bool IsConnect();
        /// <summary>
        /// 获取机床数据
        /// </summary>
        /// <param name="model">记录机床数据的对象</param>
        /// <returns></returns>
        public abstract bool GetData(IMonitorRealTimeModel model);
    }
}
