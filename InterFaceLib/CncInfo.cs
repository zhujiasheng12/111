using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterFaceLib
{
    public abstract class CncInfo//抽象类，设备信息
    {
        public int ID;
        /// <summary>
        /// 机床编号
        /// </summary>
        public string MachNum;
        /// <summary>
        /// 机床型号
        /// </summary>
        public string MachType;
        /// <summary>
        /// 机床品牌
        /// </summary>
        public string MachBrand;
        public string IP;
        /// <summary>
        /// 机床状态
        /// </summary>
        public string MachState;
        public abstract CncMachine GetCncMachine();
    }
}
