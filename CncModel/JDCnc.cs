using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.JDCNC;
using InterFaceLib;

namespace CncModel
{
    public class JDCnc:CncMachine
    {
        private IntPtr ptr;
        public JDCnc()
        {
            ptr = Function.CreateJDMachMon();
        }
        public override bool Connect()
        {
            return Function.ConnectJDMach(ptr, this.cncInfo.IP);
        }

        public override bool DisConnect()
        {
            return Function.DisconnectJDMach(ptr);
        }

        public override bool GetData(IMonitorRealTimeModel model)
        {
            try
            {
                
                JDMonitorRealTimeModel monitorRealTimeModel = model as JDMonitorRealTimeModel;

                //机床坐标
                double[] machCorrd = new double[6];
                double[] absCorrd = new double[6];
                double[] relCorrd = new double[6];
                if (Function.GetMachPos(ptr, machCorrd, absCorrd, relCorrd))
                {
                    monitorRealTimeModel.MachCoord = machCorrd;
                    monitorRealTimeModel.AbsCoord = absCorrd;
                    monitorRealTimeModel.RelCoord = relCorrd;
                }
                else
                { return false; }
                //机床程序状态
                int progState = -1;
                int almInfo = -1;
                if (Function.GetMachAlmInfo(ptr, ref almInfo))
                {
                    if (almInfo == 0)
                    {
                        if (Function.GetProgState(ptr, ref progState))
                        {
                            monitorRealTimeModel.ProgState = progState;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        progState = 4;
                        monitorRealTimeModel.ProgState = progState;
                        int errCount = -1;
                        int[] errCodeList = new int[50];
                        byte[,] errDescList = new byte[50, 128];
                        List<string> errDesc = new List<string>();

                        if (Function.GetCncErrList(ptr, ref errCount, errCodeList, errDescList))
                        {
                            byte[] buff = new byte[128];
                            for (int i = 0; i < 50; i++)
                            {
                                for (int j = 0; j < 128; j++)
                                {
                                    buff[j] = errDescList[i, j];
                                }
                                string str = Encoding.Default.GetString(buff).Replace("\0", "").Replace("\r", "").Replace("\n", "");
                                errDesc.Add(str);
                            }
                            {
                                using (Materiel_NoScan_V2Entities cie = new Materiel_NoScan_V2Entities())
                                {
                                    var alarminfo = cie.JDJS_WMS_Device_Alarm_History_Table.Where(r => r.CncID == this.cncInfo.ID && r.EndTime == null);
                                    if (alarminfo.Count() < 1)
                                    {
                                        for (int i = 0; i < errCount; i++)
                                        {
                                            JDJS_WMS_Device_Alarm_History_Table alarms = new JDJS_WMS_Device_Alarm_History_Table()
                                            {
                                                CncID = this.cncInfo.ID,
                                                ErrCode = errCodeList[i].ToString(),
                                                ErrDesc = errDesc[i],
                                                StartTime = DateTime.Now,

                                            };
                                            cie.JDJS_WMS_Device_Alarm_History_Table.Add(alarms);
                                        }
                                        cie.SaveChanges();
                                    }
                                }
                            }

                        }
                        else
                        {
                            errCount = -1; errCodeList = null; errDescList = null;
                        }
                        monitorRealTimeModel.ErrCount = errCount;
                        monitorRealTimeModel.ErrCodeList = errCodeList;
                        monitorRealTimeModel.ErrDescList = errDesc.ToArray();

                    }
                }
                else
                { return false; }
                //机床模态
                int currwCorrd = -1;
                float feedrate = -1;
                int spindlespeed = -1;
                int toolno = -1;
                float machTime = -1;
                int curro = -1;
                int curroMaino = -1;
                if (Function.GetBasicModalInfo(ptr, ref currwCorrd, ref feedrate, ref spindlespeed, ref toolno, ref machTime, ref curro, ref curroMaino))
                {
                    monitorRealTimeModel.CurrWCoord = currwCorrd;
                    monitorRealTimeModel.FeedRate = feedrate;
                    monitorRealTimeModel.SpindleSpeed = spindlespeed;
                    monitorRealTimeModel.ToolNO = toolno;
                    monitorRealTimeModel.MachTime = machTime;
                    monitorRealTimeModel.CurrO = curro;
                    monitorRealTimeModel.CurrMainO = curroMaino;
                }
                else { return false; }

                //机床时间信息
                double[] times = new double[3];
                if (Function.GetTime(ptr, times))
                {
                    //model.Times = times;
                    monitorRealTimeModel.Times = times;
                }
                else
                {
                    return false;
                }
                //工件计数
                int count = -1;
                if (Function.GetMachinedWorkpieceCount(ptr, ref count))
                {
                    monitorRealTimeModel.WorkCount = count;
                }
                else { return false; }
                //倍率信息
                int[] rates = new int[2];
                if (Function.GetRate(ptr, rates))
                {
                    monitorRealTimeModel.Rates = rates;
                }
                else { return false; }
                //主轴信息
                double[] sinfo = new double[3];
                if (Function.GetSpindleInfo(ptr, sinfo))
                {
                    monitorRealTimeModel.Sinfo = sinfo;
                }
                else { return false; }
                //当前运行行号
                int[] curlineno = new int[3];
                if (Function.GetCurLineNo(ptr, curlineno))
                {
                    monitorRealTimeModel.CurLineNo = curlineno;
                }
                else { return false; }
                //当前运行行文本
                StringBuilder curlinetext = new StringBuilder();
                curlinetext.Capacity = 200000;
                if (Function.GetCurrLineText(ptr, curlinetext))
                {
                    monitorRealTimeModel.CurrLineText = curlinetext.ToString();
                }
                else { return false; }

                //机床系统时间
                int[] time = new int[7];
                if (Function.GetMachTime(ptr, time))
                {

                    //model.time = time;
                    monitorRealTimeModel.time = time;
                }
                else
                { return false; }
                //当前xyzabc剩余量没有相关函数
                //当前工作模式 获取机床的操作模式：
                //1、回参考点是128  
                //2、打再寸动上是32 
                //3、又打到回参考点之后又是128   
                //3、打到手轮上，是64    
                //4、程序运行 是 2    
                //5、MDI是4   
                //6、编辑是1   
                //7、点动是16
                int modell = -1;
                int mode = -1;
                if (Function.GetOprationMode(ptr, ref mode))
                {
                    switch (mode)
                    {
                        case 1:
                            modell = 1;//编辑
                            break;
                        case 2:
                            modell = 2;//程序运行
                            break;
                        case 4:
                            modell = 3;//mdi
                            break;
                        case 8:
                            modell = 8;//程序自动远程运行
                            break;
                        case 16:
                            modell = 4;//点动
                            break;
                        case 32:
                            modell = 5;//寸动
                            break;
                        case 64:
                            modell = 6;//手轮
                            break;
                        case 128:
                            modell = 7;//回参考点
                            break;
                        case 256:
                            modell = 9;
                            break;

                    }
                    monitorRealTimeModel.Mode = modell;
                }
                else { return false; }
                model = monitorRealTimeModel as IMonitorRealTimeModel;
                return true;
            }
            catch (Exception ex)
            {

                if (!Directory.Exists("log"))
                {
                    Directory.CreateDirectory("log");
                }
                using (StreamWriter write = new StreamWriter("log/" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".log", true))
                {

                    write.WriteLine(DateTime.Now.ToString() + " " + ex);
                }
                return false;
            }
        }

        public override bool IsConnect()
        {
            return Function.IsConnect(ptr);
        }
    }
}
