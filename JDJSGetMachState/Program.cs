using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CncModel;
using InterFaceLib;

namespace JDJSGetMachState
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                IHandleCncData getCncInfo = new HandleCncDataBySql();
                List<CncInfo> cncInfos = getCncInfo.GetCncInfo();
                Dictionary<int, IMonitorRealTimeModel> models = new Dictionary<int, IMonitorRealTimeModel>();
                List<CncMachine> machines = new List<CncMachine>();
                Dictionary<int, CancellationTokenSource> cancelFlag = new Dictionary<int, CancellationTokenSource>();
                Dictionary<int, Task> taskList = new Dictionary<int, Task>();
                cncInfos.ForEach((cnc) =>
                {
                    CncMachine machine = cnc.GetCncMachine();
                    machines.Add(machine);
                    models.Add(machine.cncInfo.ID, new JDMonitorRealTimeModel(machine.cncInfo.ID));
                    cancelFlag .Add (cnc.ID ,new CancellationTokenSource ());
                });
                foreach (var mach in machines)
                {
                    {
                        if (models.ContainsKey(mach.cncInfo .ID))
                        {
                            taskList.Add(mach.cncInfo.ID, new Task(() =>
                            {
                                while (!cancelFlag[mach.cncInfo.ID].IsCancellationRequested)
                                {
                                    if (mach.IsConnect())
                                    {
                                        if (mach.GetData(models[mach.cncInfo .ID]))
                                        {
                                            Thread.Sleep(10);
                                        }
                                        else
                                        {
                                            Console.WriteLine(mach.cncInfo.MachNum.ToString() + "获取信息失败");
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        mach.DisConnect();
                                        models[mach.cncInfo .ID] = InitialMonit(models[mach.cncInfo .ID]);
                                        while (!mach.Connect())
                                        { 
                                            Console.WriteLine(mach.cncInfo.MachNum.ToString() + "连接失败，重新连接中...");
                                        }
                                        continue;
                                    }
                                }

                            },cancelFlag[mach.cncInfo .ID].Token));
                            taskList[mach.cncInfo.ID].Start();
                        }
                    }
                }

                List<CncMachine> machinesAdd = new List<CncMachine>();
                List<CncMachine> machinesDel = new List<CncMachine>();
                List<CncMachine> machinesAlterNew = new List<CncMachine>();
                List<CncMachine> machinesAlterOld = new List<CncMachine>();
                Task alterTask = new Task(() =>
                {
                    while (true)
                    {
                        List<CncMachine> machinesRealTimes = new List<CncMachine>();
                        List<CncInfo> cncInfosRealTime = getCncInfo.GetCncInfo();
                        Dictionary<CncMachine, IMonitorRealTimeModel> modelsRealTime = new Dictionary<CncMachine, IMonitorRealTimeModel>();
                        cncInfosRealTime.ForEach((cnc) =>
                        {
                            CncMachine machineRealTime = cnc.GetCncMachine();
                            machinesRealTimes.Add(machineRealTime);
                            modelsRealTime.Add(machineRealTime, new JDMonitorRealTimeModel(machineRealTime.cncInfo.ID));
                        });
                        machinesAdd.Clear();
                        foreach (var item in machinesRealTimes)
                        {
                            machinesAdd.Add(item);
                        }
                        machinesDel.Clear();
                        foreach (var item in machines)
                        {
                            machinesDel.Add(item);
                        }
                        machinesAlterNew.Clear();
                        machinesAlterOld.Clear();
                        foreach (var item in machines)
                        {
                            foreach (var real in machinesRealTimes)
                            {
                                if ((real.cncInfo.ID == item.cncInfo.ID))
                                {
                                    machinesAdd.Remove(real);
                                }
                            }
                        }
                        foreach (var item in machinesRealTimes)
                        {
                            foreach (var real in machines)
                            {
                                if ((real.cncInfo.ID == item.cncInfo.ID))
                                {
                                    machinesDel.Remove(real);
                                }
                            }
                        }
                        foreach (var item in machinesRealTimes)//修改的
                        {
                            foreach (var real in machines)
                            {
                                if ((real.cncInfo.ID == item.cncInfo.ID) && ((real.cncInfo.MachNum != item.cncInfo.MachNum) || (real.cncInfo.IP != item.cncInfo.IP) || (real.cncInfo.MachState != item.cncInfo.MachState) || (real.cncInfo.MachType != item.cncInfo.MachType)))
                                {
                                    machinesAlterNew.Add(item);
                                    machinesAlterOld.Add(real);
                                }
                            }
                        }
                        foreach (var mach in machinesAdd)
                        {
                            models.Add(mach.cncInfo .ID, new JDMonitorRealTimeModel(mach.cncInfo.ID));
                            cancelFlag.Add(mach.cncInfo.ID, new CancellationTokenSource());                         
                            {
                                if (models.ContainsKey(mach.cncInfo .ID ))
                                {
                                    taskList.Add(mach.cncInfo.ID, new Task(() =>
                                    {
                                        while (!cancelFlag[mach.cncInfo.ID].IsCancellationRequested)
                                        {
                                            if (mach.IsConnect())
                                            {
                                                if (mach.GetData(models[mach.cncInfo .ID]))
                                                {
                                                    Thread.Sleep(10);
                                                }
                                                else
                                                {
                                                    Console.WriteLine(mach.cncInfo.MachNum.ToString() + "获取信息失败");
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                mach.DisConnect();
                                                models[mach.cncInfo .ID] = InitialMonit(models[mach.cncInfo .ID ]);
                                                while (!mach.Connect())
                                                {
                                                    Console.WriteLine(mach.cncInfo.MachNum.ToString() + "连接失败，重新连接中...");
                                                }
                                                continue;
                                            }
                                        }

                                    }, cancelFlag[mach.cncInfo.ID].Token));
                                    taskList[mach.cncInfo.ID].Start();
                                }
                            }
                        }
                        foreach (var mach in machinesDel)
                        {
                            if (taskList.ContainsKey(mach.cncInfo.ID)&&cancelFlag .ContainsKey(mach.cncInfo .ID))
                            {
                                cancelFlag[mach.cncInfo.ID].Cancel();
                                taskList[mach.cncInfo.ID].Wait();
                                taskList.Remove(mach.cncInfo.ID);
                                models.Remove(mach.cncInfo .ID );
                                cancelFlag.Remove(mach.cncInfo.ID);
                                Console.WriteLine(mach.cncInfo.MachNum.ToString() + "删除成功");
                            }
                        }
                        foreach (var mach in machinesAlterOld)
                        {
                            if (taskList.ContainsKey(mach.cncInfo.ID) && cancelFlag.ContainsKey(mach.cncInfo.ID))
                            {
                                cancelFlag[mach.cncInfo.ID].Cancel();
                                taskList[mach.cncInfo.ID].Wait();
                                taskList.Remove(mach.cncInfo.ID);
                                models.Remove(mach.cncInfo .ID );
                                cancelFlag.Remove(mach.cncInfo.ID);
                                Console.WriteLine(mach.cncInfo.MachNum.ToString() + "删除成功");
                            }

                        }
                        foreach (var mach in machinesAlterNew)
                        {
                            models.Add(mach.cncInfo .ID , new JDMonitorRealTimeModel(mach.cncInfo.ID));
                            cancelFlag.Add(mach.cncInfo.ID, new CancellationTokenSource());
                            {
                                if (models.ContainsKey(mach.cncInfo .ID ))
                                {
                                    taskList.Add(mach.cncInfo.ID, new Task(() =>
                                    {
                                        while (!cancelFlag[mach.cncInfo.ID].IsCancellationRequested)
                                        {
                                            if (mach.IsConnect())
                                            {
                                                if (mach.GetData(models[mach.cncInfo .ID ]))
                                                {
                                                    Thread.Sleep(10);
                                                }
                                                else
                                                {
                                                    Console.WriteLine(mach.cncInfo.MachNum.ToString() + "获取信息失败");
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                mach.DisConnect();
                                                models[mach.cncInfo .ID ] = InitialMonit(models[mach.cncInfo .ID ]);
                                                while (!mach.Connect())
                                                {
                                                    Console.WriteLine(mach.cncInfo.MachNum.ToString() + "连接失败，重新连接中...");
                                                }
                                                continue;
                                            }
                                        }

                                    }, cancelFlag[mach.cncInfo.ID].Token));
                                    taskList[mach.cncInfo.ID].Start();
                                }
                            }
                        }
                        cncInfos = cncInfosRealTime;
                        machines = machinesRealTimes;
                        Thread.Sleep(2000);
                    }
                }
                );
                alterTask.Start();

                Thread.Sleep(1000);
                Task taskSave = new Task(() =>
                {
                    while (true)
                    {
                        if (getCncInfo.CncDataSave(models))
                        { 
                            Thread.Sleep(1000); 
                        }
                        else
                        {
                            Console.WriteLine("保存数据失败");
                        }                       

                    }
                });
                taskSave.Start();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (!Directory.Exists("log"))
                {
                    Directory.CreateDirectory("log");
                }
                using (StreamWriter write = new StreamWriter("log/" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".log", true))
                {

                    write.WriteLine(DateTime.Now.ToString() + " " + ex);
                }
            }
        }
        public static IMonitorRealTimeModel InitialMonit(IMonitorRealTimeModel monitorReal)
        {
            JDMonitorRealTimeModel monitorRealTimeModel = monitorReal as JDMonitorRealTimeModel;
            monitorRealTimeModel.ErrCount = -1;
            monitorRealTimeModel.ErrCodeList = new int[] { 0, 0 };
            monitorRealTimeModel.ErrDescList = new string[] { "", "" };
            monitorRealTimeModel.AbsCoord = new double[] { -1, -1, -1, -1, -1, -1 };
            monitorRealTimeModel.MachCoord = new double[] { -1, -1, -1, -1, -1, -1 };
            monitorRealTimeModel.RelCoord = new double[] { -1, -1, -1, -1, -1, -1 };
            monitorRealTimeModel.WorkCount = -1;
            monitorRealTimeModel.CurLineNo = new int[] { -1 };
            monitorRealTimeModel.CurrLineText = "";
            monitorRealTimeModel.CurrMainO = -1;
            monitorRealTimeModel.CurrO = -1;
            monitorRealTimeModel.CurrWCoord = -1;
            int[] _dh = { -1, -1 };
            monitorRealTimeModel.dH = (int[])_dh.Clone();
            monitorRealTimeModel.FeedRate = -1;
            monitorRealTimeModel.MachTime = -1;
            monitorRealTimeModel.Mode = -1;
            monitorRealTimeModel.ProgCtrlState = -1;
            monitorRealTimeModel.ProgState = -1;
            int[] _rate = { -1, -1 };
            monitorRealTimeModel.Rates = (int[])_rate.Clone();
            monitorRealTimeModel.RunTime = -1;
            double[] _sin = { -1, -1, -1 };
            monitorRealTimeModel.Sinfo = (double[])_sin.Clone();
            monitorRealTimeModel.SpindleSpeed = -1;
            int[] time = { -1, -1, -1, -1, -1, -1, -1 };
            monitorRealTimeModel.time = (int[])time.Clone();
            monitorRealTimeModel.Dist = new double[2] { 0, -1 };

            double[] _double = { -1, -1, -1 };

            monitorRealTimeModel.Times = (double[])_double.Clone();
            monitorRealTimeModel.ToolNO = -1;
            monitorRealTimeModel.Rates = new int[] { -1, -1 };
            monitorRealTimeModel.RepairStatus = -1;
            monitorRealTimeModel.RepairStatusOn = -1;
            monitorRealTimeModel.AlarmDesc = -1;
            monitorRealTimeModel.AlarmDescOn = -1;
            monitorReal = monitorRealTimeModel as IMonitorRealTimeModel;
            return monitorReal;
        }
    }
}
