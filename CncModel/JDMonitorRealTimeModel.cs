using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterFaceLib;

namespace CncModel
{
    public class JDMonitorRealTimeModel :IMonitorRealTimeModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 机床坐标系
        /// </summary>
        public double[] MachCoord { get; set; }
        /// <summary>
        /// 绝对坐标系
        /// </summary>
        public double[] AbsCoord { get; set; }
        /// <summary>
        /// 相对坐标系
        /// </summary>
        public double[] RelCoord { get; set; }

        /// <summary>
        /// 当前工件坐标系
        /// </summary>
        public int CurrWCoord { get; set; }

        private int State;
        public JDMonitorRealTimeModel(int id)
        {
            this.ID = id;
            State = -20;
            OnProgStateChanged += new MachRealTimeDataChanged(afterProgStateChanged);
        }
        public int ProgState
        {
            get { return State; }
            set
            {
                if (value != State)
                {
                    State = value;
                    whenProgStateChange();
                }
                else
                { State = value; }

            }
        }
        public delegate void MachRealTimeDataChanged(object sender, EventArgs e);
        public event MachRealTimeDataChanged OnProgStateChanged;
        private void afterProgStateChanged(object sender, EventArgs e)
        {
            DateTime now = new DateTime();
            now = DateTime.Now;
            JDMonitorRealTimeModel mode = sender as JDMonitorRealTimeModel;

            using (Materiel_NoScan_V2Entities  CIE = new  Materiel_NoScan_V2Entities ())
            {
                using (System.Data.Entity.DbContextTransaction mytran = CIE.Database.BeginTransaction())
                {
                    try
                    {
                        var StateID = CIE.JDJS_WMS_Device_ProgState_Info.Where(r => r.CncID == mode.ID && r.EndTime == null);
                        if (StateID.Count() < 1)
                        {
                            JDJS_WMS_Device_ProgState_Info state = new JDJS_WMS_Device_ProgState_Info()
                            {
                                CncID = mode.ID,
                                ProgState = mode.ProgState,
                                StartTime = now
                            };
                            CIE.JDJS_WMS_Device_ProgState_Info.Add(state);
                            CIE.SaveChanges();
                        }
                        else
                        {
                            foreach (var item in StateID)
                            {
                                item.EndTime = now;
                            }
                            JDJS_WMS_Device_ProgState_Info state = new JDJS_WMS_Device_ProgState_Info()
                            {
                                CncID = mode.ID,
                                ProgState = mode.ProgState,
                                StartTime = now
                            };
                            CIE.JDJS_WMS_Device_ProgState_Info.Add(state);
                            CIE.SaveChanges();

                        }
                        var alarmstate = CIE.JDJS_WMS_Device_Alarm_History_Table.Where(r => r.CncID == mode.ID && r.EndTime == null);
                        foreach (var item in alarmstate)
                        {
                            item.EndTime = now;
                        }
                        CIE.SaveChanges();


                        mytran.Commit();
                        CIE.SaveChanges();
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
                        mytran.Rollback();

                    }
                }
            }
        }
        private void whenProgStateChange()
        {
            if (OnProgStateChanged != null)
            {
                OnProgStateChanged(this, null);
            }
        }
        /// <summary>
        /// 当前进给速度
        /// </summary>
        public float FeedRate { get; set; }
        /// <summary>
        /// 主轴转速
        /// </summary>
        public int SpindleSpeed { get; set; }
        /// <summary>
        /// 主轴刀具号
        /// </summary>
        public int ToolNO { get; set; }
        /// <summary>
        /// 加工时间
        /// </summary>
        public float MachTime { get; set; }
        /// <summary>
        /// 当前程序号
        /// </summary>
        public int CurrO { get; set; }
        /// <summary>
        /// 当前主程序号
        /// </summary>
        public int CurrMainO { get; set; }
        /// <summary>
        /// 补偿编号（半径补偿编号G41，长度补偿编号G43）
        /// </summary>
        public int[] dH { get; set; }
        /// <summary>
        ///  时间信息（系统启动以来运行时间，系统启动以来程序累计执行时间，当前程序启动以来执行时间）
        /// </summary>
        public double[] Times { get; set; }
        /// <summary>
        /// 工件计数
        /// </summary>
        public int WorkCount { get; set; }
        /// <summary>
        /// 倍率信息（主轴倍率，进给倍率）
        /// </summary>
        public int[] Rates { get; set; }
        /// <summary>
        /// 主轴信息（主轴电流，主轴扭矩。主轴功率）
        /// </summary>
        public double[] Sinfo { get; set; }
        /// <summary>
        /// 当前程序运行行号
        /// </summary>
        public int[] CurLineNo { get; set; }
        /// <summary>
        /// 当前程序运行行文本
        /// </summary>
        public string  CurrLineText { get; set; }
        /// <summary>
        /// 机床系统时间（年，月，日，时，分，秒，时区）
        /// </summary>
        public int[] time { get; set; }
        /// <summary>
        /// 当前机床XYZABC剩余量
        /// </summary>
        public double[] Dist { get; set; }
        /// <summary>
        /// 机床程控状态（空运行，单步运行，机床锁住，选择停止，程序段跳过，MST锁住，手轮试切）
        /// </summary>
        public int ProgCtrlState { get; set; }
        /// <summary>
        /// 实际获得的位图数据数据部分长度
        /// </summary>
        public int Len { get; set; }
        /// <summary>
        /// 远程机床截图后保存的bitmap数据信息
        /// </summary>
        public object bmp { get; set; }
        /// <summary>
        /// 实际截图获得的像素信息
        /// </summary>
        public object ScreenData { get; set; }
        /// <summary>
        /// 各轴负载
        /// </summary>
        public object Svld { get; set; }
        /// <summary>
        /// 机床工作模式（程序编辑，程序自动内存运行，MDI运行，程序自动远程运行，增量进给，手动连续，手轮，参考点，示教）
        /// </summary>
        public int Mode { get; set; }
        //机床开机时间
        public DateTime StartTime { get; set; }
        //机床关机时间
        public DateTime EndTime { get; set; }
        //运行时间
        public double RunTime { get; set; }
        public int flag { get; set; }
        //报修状态
        public double RepairStatus;
        //报修状态确认
        public double RepairStatusOn;
        //问题描述编号
        public double AlarmDesc;
        //问题描述编号确认
        public double AlarmDescOn;

        //刀具信息
        public string toolList { get; set; }
        /// <summary>
        /// 错误数目
        /// </summary>
        public int ErrCount { get; set; }
        /// <summary>
        /// 错误代号
        /// </summary>
        public int[] ErrCodeList { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string[] ErrDescList { get; set; }

        public List<int> toolNum { get; set; }
    }

    public class ToolInfo
    {
        public int toolID;
        public double toolL;
        public double toolH;
        public double toolR;
        public double toolD;
        public double toolMaxTime;
        public double toolCurrTime;
        public double toolMaxNum;
        public double toolCurrNum;
        public double toolMaxDistance;
        public double toolCurrDistance;


    }

}
