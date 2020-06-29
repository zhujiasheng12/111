using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace API.JDCNC
{
   public static class Function
    {
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool AutoWarmup(IntPtr JdMon, float WarnWaitTime, bool bBuzzer, bool bFlash);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool BuildTask(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool ChangeMode(IntPtr JdMon, int DestMode);
        /// <summary>
        /// 连接机床
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <param name="servername">机床Ip</param>
        /// <param name="RPCPort">RPC端口默认Ox59</param>
        /// <param name="CallbackPort">回调端口</param>
        /// <param name="FileUploadPort">文件上传端口</param>
        /// <param name="FileDownloadPort">文件下发端口</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool ConnectJDMach(IntPtr JdMon, string servername, ushort RPCPort = 0x59, ushort CallbackPort = 0x1ba8, ushort FileUploadPort = 0x1ba9, ushort FileDownloadPort = 0x1baa);
        /// <summary>
        /// 创造机床监控指针
        /// </summary>
        /// <returns>机床监控指针</returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern IntPtr CreateJDMachMon();

        /// <summary>
        /// 删除机床监控指针
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern void DeleteJDMachMon(ref IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool DeleteNcProg(IntPtr JdMon, string NcProgName);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool DelMachFile(IntPtr JdMon, string DestDir, string FileName);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool DisconnectJDMach(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool EmptyTask(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetAppDataBuf(IntPtr JDMachMon, byte[] AppDataBuf);
        /// <summary>
        /// 获得机床相关模态信息
        /// </summary>
        /// <param name="JdMon">机床监控对象</param>
        /// <param name="CurrWCoord">当前使用的坐标系G54</param>
        /// <param name="Feedrate">进给速度Feedrate</param>
        /// <param name="SpindleSpeed">主轴转速SpindleSpeed</param>
        /// <param name="ToolNo">主轴刀号CurrentTool</param>
        /// <param name="MachTime">当前加工时间</param>
        /// <param name="CurrO">正在运行的程序号</param>
        /// <param name="CurrMainO">正在运行的主程序号</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetBasicModalInfo(IntPtr JdMon, ref int CurrWCoord, ref float Feedrate, ref int SpindleSpeed, ref int ToolNo, ref float MachTime, ref int CurrO, ref int CurrMainO);
        /// <summary>
        /// 获取机床告警
        /// </summary>
        /// <param name="a">机床监控指针</param>
        /// <param name="ErrCount">错误数目</param>
        /// <param name="ErrCodeList">告警编号AlarmNumber</param>
        /// <param name="ErrDescList">告警详情AlarmMessage</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetCncErrList(IntPtr a, ref int ErrCount, int[] ErrCodeList, byte[,] ErrDescList);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetConnectionTimeout(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetCurLineNo(IntPtr JdMon, int[] CurLineNo);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetCurrLineText(IntPtr JdMon, StringBuilder CurrLineText);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetCurrUserName(IntPtr JDMachMon, byte[] UserName);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetEQPVersion(IntPtr JDMachMon, byte[] HwVer, byte[] SoftVer);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetLastErr(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachAlmInfo(IntPtr JdMon, ref int AlmInfo);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachAlmInfoEx(IntPtr JdMon, int ErrNo, string ErrDesc, string ErrModule);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachFileList(IntPtr JdMon, string DestDir, int StrBufSize, byte[] FileList);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachineDI(IntPtr JdMon, int Index, ref int Di);
        /// <summary>
        /// 获取CNC产能计数
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <param name="count">机台累计产能(含数据重置操作)Count</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachinedWorkpieceCount(IntPtr JdMon, ref int count);
        /// <summary>
        /// 获取当前机床坐标
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <param name="MachCoord">机械坐标MachinePos</param>
        /// <param name="AbsCoord">绝对坐标AbsolutePos</param>
        /// <param name="RelCoord">相对坐标RelativePos</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachPos(IntPtr JdMon, double[] MachCoord, double[] AbsCoord, double[] RelCoord);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachRcvFolder(IntPtr JdMon, string MachRcvFolder);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachScreen(IntPtr JdMon, ref int Len, byte[] Bmp, byte[] ScreenData);
        /// <summary>
        /// 获取机床时间
        /// </summary>
        /// <param name="JDMachMon"></param>
        /// <param name="nTime"></param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachTime(IntPtr JDMachMon, int[] nTime);
        /// <summary>
        /// 获取宏变量
        /// </summary>
        /// <param name="JdMon"></param>
        /// <param name="VarCount"></param>
        /// <param name="VarNo"></param>
        /// <param name="VarVal"></param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMacroVarValue(IntPtr JdMon, int VarCount, int[] VarNo, double[] VarVal);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetOprationMode(IntPtr JdMon, ref int Mode);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetOrigin(IntPtr JdMon, int OrgNo, float[] OrgCoord);
        /// <summary>
        /// 获取当前机床状态
        /// </summary>
        /// <param name="JdMon">通过CreateJDMachMon创建的机床指针</param>
        /// <param name="NcProgState">机床当前状态</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetProgState(IntPtr JdMon, ref int NcProgState);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetRate(IntPtr JdMon, int[] rate);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetRPCTimeout(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetSpindleInfo(IntPtr JdMon, double[] sinfo);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetTask(IntPtr JdMon, byte[] FileName, byte[] TaskName, byte[] MainRunProg, byte[] MainProgs, byte[] SubProgs);
        /// <summary>
        /// 获取机台时间分布
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <param name="time">开机总时间PowerOnTime，所有工作时间包括切割时间在内（不包括保持暂停状态时间）OperatingTime，切割时间CuttingTime</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetTime(IntPtr JdMon, double[] time);
        /// <summary>
        /// 获取刀具偏移信息
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <param name="ToolNo">刀具编号ToolNumber</param>
        /// <param name="ToolLHRD">刀具外形LR，刀具磨损HD LengthWearOffset(长度) RadiusWearOffset（半径）</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetToolInfo(IntPtr JdMon, int ToolNo, double[] ToolLHRD);
        /// <summary>
        /// 获取刀具寿命
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <param name="toolNo">刀具编号Tool</param>
        /// <param name="TotalLife">刀具寿命ToolMaxLife</param>
        /// <param name="CurrLife">刀具当前使用ToolUsedLife</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetToolLife(IntPtr JdMon, int toolNo, double TotalLife, double CurrLife);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetWorkInfoDaily(IntPtr JdMon, byte[] WorkInfoDaily);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetWorkpieceMachTimeRecord(IntPtr JdMon, int ArrSize, ref int WorkpieceCount, int[] ProgNo, float[] MachTime);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool IsConnect(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool IsRcvingFile(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool IsSendingFile(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool PauseProg(IntPtr JdMon);
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <param name="SrcFileName">要下载的文件名</param>
        /// <param name="DestFileName">要另存为的文件名</param>
        /// <param name="pProgCtrl"></param>
        /// <param name="ProgPosPtr"></param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool ReceiveFile(IntPtr JdMon, string SrcFileName, string DestFileName, IntPtr pProgCtrl, SetProgPosFunPtr ProgPosPtr);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool RenameMachFile(IntPtr JdMon, string SrcFileName, string DestFileName);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool RenameNcProg(IntPtr JdMon, string SrcNcProgName, string DestNcProgName);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool ResetMach(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SaveTask(IntPtr JdMon, string FileName);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SaveTaskAs(IntPtr JdMon, string FileName);
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <param name="AddToTsk">是否自动加载为当前任务</param>
        /// <param name="FileName">要发送的文件名</param>
        /// <param name="pProgCtrl"></param>
        /// <param name="ProgPosPtr"></param>
        /// <param name="FileThread">是否开线程发送文件</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SendFile(IntPtr JdMon, bool AddToTsk, string FileName, IntPtr pProgCtrl, SetProgPosFunPtr ProgPosPtr, bool FileThread = true);
        /// <summary>
        /// 发送NC文件
        /// </summary>
        /// <param name="JdMon"></param>
        /// <param name="FileName">发送的文件名</param>
        /// <param name="AddToTsk">是否自动加入到任务列表</param>
        /// <param name="SetMainProgram">是否作为主程序</param>
        /// <param name="pProgCtrl"></param>
        /// <param name="ProgPosPtr"></param>
        /// <param name="FileThread">是否开线程发送文件</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SendNcFile(IntPtr JdMon, string FileName, bool AddToTsk, bool SetMainProgram, IntPtr pProgCtrl, SetProgPosFunPtr ProgPosPtr, bool FileThread = true);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SetCallFuncMaxTimeout(IntPtr JdMon, int Timeout);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SetConnectionTimeout(IntPtr JdMon, int millisecond);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern void SetEventCallback(IntPtr JdMon, CallBackDelegate DestMode);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SetMachRcvFolder(IntPtr JdMon, string MachRcvFolder);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SetMachTime(IntPtr JDMachMon, int[] nTime);
        /// <summary>
        /// 写入宏变量
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <param name="VarCount">变量个数</param>
        /// <param name="VarNo">变量号数组</param>
        /// <param name="VarVal">变量值数组</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SetMacroVarValue(IntPtr JdMon, int VarCount, int[] VarNo, double[] VarVal);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SetMainRunProg(IntPtr JdMon, string NcMainProgName);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern void SetNetworkBrkCallback(IntPtr JdMon, CallBackDelegate p);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SetRPCTimeout(IntPtr JdMon, int Timeout);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool SetSignalTower(IntPtr JdMon, int ColorType, int bFlash, int bBuzzer);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool ShowTerminalMsg(IntPtr JdMon, string MsgToShow);
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool StartProg(IntPtr JdMon);
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="JdMon">机床监控指针</param>
        /// <returns></returns>
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool StopProg(IntPtr JdMon);
        [DllImport("JDXA.DNC.Framework.dll")]
        public static extern bool GetMachPosMultiAxis(IntPtr JdMon, double[] MachCoord, double[] AbsCoord, double[] RelCoord);

        public delegate int CallBackDelegate(IntPtr info);

        public delegate void SetProgPosFunPtr(IntPtr pProgCtrl, int ProgPos);
        //CNC_Alarm:CNC告警类别的实时状态描述，是布尔变量，若某一类告警发生，其值为1，反之，为0
        //Parameter switch on (SW)
        //Power off parameter set(PW)
        //I/O error(IO)
        //Foreground P/S(PS)
        //Overtravel,External data(OT)
        //Overheat alarm(OH)
        //Servo alarm(SV)
        //Data I/O error(SR)
        //Macro alarm(MC)
        //Spindle alarm(SP)
        //Other alarm(DS) (DS)
        //Alarm concerning Malfunction prevent functions(IE)
        //Background P/S(BG)
        //Syncronized error(SN)
        //External alarm message(EX)
        //PMC error(PC)

        //CNC_ProductCount:CNC产能计数。TotalCount：机台累计产能(不含数据重置操作)

        //CNC_SpindleAlarm:主轴告警。Spindle：主轴号，同一时间统一机台只能是一根主轴在工作，Spindle=1。Alarm：主轴告警，当前收集到的数据所有的Alarm=none，是因为windowsCNC DC有问题，还是机台本身就没有主轴告警，需要确认。

        //CNC_ToolLifeData：机台告警。type：告警类别。Axis：告警发生在哪个轴。

        //CNC_AxisPosition：轴实时坐标位置。Axis：轴编号。DistanceToGo：相对坐标原点需要移动的距离(但不确定是否是轴移动的距离，如果是，那么轴的移动是先回归原点，后进行下一次移动)。EXCEL中需要输入轴的编号来获取坐标信息。

        //CNC_AxisSkippedPos：进给轴跳过位置。Axis：进给轴编号。SkippedPos：跳过位置。

        //CNC_ConstantSurfaceSpeedControl：ConvertedSpindleSpeed：转换后主轴转速。SpecifiedSurfaceSpeed：指定的线速度。ClampOfMaxSpindleSpeed：最大主轴转速。

        //CNC_ServoInfo：机台伺服器负载、延迟、进给等。
        //Axis	轴编号
        //gain 伺服增益
        //current 伺服电流
        //speed 伺服运转速度
        //LoadMeter 伺服负载率
        //DelayAmount 伺服延迟量
        //AcceDeceDelayAmount 伺服加减速延迟量

        //CNC_SpindleAdjCtrlModeSyncError	主轴调整同步错误
        //CNC_SpindleAdjTappingModeSyncError 主轴调整刚性攻丝模式下的同步错误
        //CNC_SpindleAdjPosDeflectionS 主轴调整S偏移
        //CNC_SpindleAdjPosDeflectionZ 主轴调整Z偏移
        //字段 字段描述
        //SyncError 主轴调整同步错误布尔变量
        //SyncError 主轴调整刚性攻丝模式下的同步错误布尔变量
        //spindle 主轴
        //PosDeflectionS 主轴调整S偏移量
        //posDeflectionZ 主轴调整Z偏移量

        //CNC_SpindleInfo：主轴相关数据，包括：主轴负载率、马达速度、负载、进给速率、速度、操作模式、当前刀具。
        //Spindle	主轴号，同一时间统一机台只能是一根主轴在工作，Spindle=1
        //SpindleLoadMeter 主轴负载率
        //SpindleMotorSpeed 主轴马达速度
        //LoadInfo 主轴负载
        //OpMode 主轴当前操作模式

        //CNC_TimerData	机台时间分布
        //CycleTime	制作一个工件时间周期
        //FreePurpose 空闲时间

        //CNC_ToolOffsetData:刀偏
        //LengthGeometryOffset	长度几何偏移
        //RadiusGeometryOffset	半径几何偏移
        //这两种偏移在方法 bool GetBasicModalInfoExCSH(ref int CurrWCoord, ref float Feedrate, ref int SpindleSpeed, ref int ToolNo, ref float MachTime, ref int CurrO, ref int CurrMainO,ref object dH);

        //CNC_MacroHistory	宏变量历史记录
        //字段 字段描述
        //MacroNumber 宏变量编号
        //Time 时间
        //PathNumber 路径编号
        //MacroValueOld 该宏变量的上一次变量值
        //MacroValueNew 该宏变量的当前变量值

        //CNC_MDIKeyHistory 按键历史信息
        //字段 字段描述
        //KeyCode 键盘键编号
        //PowerOnFlag 机台是否接电源布尔变量
        //PathNumber 路径编号
        //ExternalMdiKeyFlag 外部键布尔变量
        //Time 时间

        //CNC_OpMsgHistory 机台操作信息历史
        //字段 字段描述
        //DisplayFlag 是否展示机台告警布尔变量
        //MessageNumber 机台操作信息编号
        //Time 时间
        //Message 机台操作信息

        //CNC_ParamHistory 参数历史信息
        //字段 字段描述
        //ParameterType 参数类型
        //ParameterNumber 参数编号
        //Time 时间
        //ParameterValueOld 参数上一次变量值
        //ParameterValueNew 参数当前变量值

        //CNC_SigHistory 机器信号历史信息
        //字段 字段描述
        //SignalName 信号名，采集回来的数据值都为F
        //SignalNumber    信号编号
        //BitPatternBefore    变化前二进制位
        //BitPatternAfter 变化后二进制位
        //PMCNumber   PMC编号
        //Time    时间

        //CNC_ToolOffsetHistory 刀具偏移量历史信息
        //字段 字段描述
        //ToolOffsetType 刀具偏移类型(长度或半径)
        //ToolOffsetNumber 刀偏号
        //Time 时间
        //PathNumber 路径编号
        //ToolOffsetOld 前刀偏
        //ToolOffsetNew 调整至刀偏值

        //CNC_ToolOffsetHistory 刀具偏移量历史信息
        //字段 字段描述
        //WorkOffsetType
        //WorkOffsetNumber
        //Time
        //PathNumber
        //AxisNumber
        //WorkOffsetOld
        //WorkOffsetNew

        //读取/写入刀具几何补偿/磨损补偿			

















































    }
}
