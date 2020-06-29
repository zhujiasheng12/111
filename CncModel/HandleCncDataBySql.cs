using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterFaceLib;

namespace CncModel
{
    public class HandleCncDataBySql : IHandleCncData
    {

        public bool CncDataSave(Dictionary<int, IMonitorRealTimeModel> models)
        {
            using (Materiel_NoScan_V2Entities CIE = new Materiel_NoScan_V2Entities ())
            {
                using (System.Data.Entity.DbContextTransaction mytran = CIE.Database.BeginTransaction())
                {
                    try
                    {
                       
                        foreach (var mode in models.ToArray())
                        {
                            var device = CIE.JDJS_WMS_Device_Info.Where(r => r.ID == mode.Key);
                            if (device.Count() > 0)
                            {
                                JDMonitorRealTimeModel mo = mode.Value as JDMonitorRealTimeModel;
                                //保存实时数据
                                if (!(mo.MachCoord == null))
                                {
                                    var id = CIE.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == mode.Key);
                                    if (id.Count() < 1)
                                    {
                                        JDJS_WMS_Device_RealTime_Data jDMachRealTimeData = new JDJS_WMS_Device_RealTime_Data()
                                        {
                                            CncID = mode.Key,
                                            MachCoord_X = mo.MachCoord[0],
                                            MachCoord_Y = mo.MachCoord[1],
                                            MachCoord_Z = mo.MachCoord[2],
                                            MachCoord_A = mo.MachCoord[3],
                                            MachCoord_B = mo.MachCoord[4],
                                            MachCoord_C = mo.MachCoord[5],
                                            AbsCoord_X = mo.AbsCoord[0],
                                            AbsCoord_Y = mo.AbsCoord[1],
                                            AbsCoord_Z = mo.AbsCoord[2],
                                            AbsCoord_A = mo.AbsCoord[3],
                                            AbsCoord_B = mo.AbsCoord[4],
                                            AbsCoord_C = mo.AbsCoord[5],
                                            RelCoord_X = mo.RelCoord[0],
                                            RelCoord_Y = mo.RelCoord[1],
                                            RelCoord_Z = mo.RelCoord[2],
                                            RelCoord_A = mo.RelCoord[3],
                                            RelCoord_B = mo.RelCoord[4],
                                            RelCoord_C = mo.RelCoord[5],
                                            ProgState = mo.ProgState,
                                            CurrWCoord = mo.CurrWCoord,
                                            FeedRate = mo.FeedRate,
                                            SpindleSpeed = mo.SpindleSpeed,
                                            ToolNo = mo.ToolNO,
                                            MachTime = mo.MachTime,
                                            CurrO = mo.CurrO,
                                            CurrOMainO = mo.CurrMainO,
                                            CountNum = mo.WorkCount,
                                            SpindleRate = mo.Rates[0],
                                            Rate = mo.Rates[1],
                                            SInfo_A = mo.Sinfo[0],
                                            SInfo_NM = mo.Sinfo[1],
                                            SInfo_W = mo.Sinfo[2],
                                            CurLineNo = mo.CurLineNo[0],
                                            //CurLineText =mo.CurrLineText ,
                                            Time_Y = mo.time[0],
                                            Time_M = mo.time[1],
                                            Time_D = mo.time[2],
                                            Time_H = mo.time[3],
                                            Time_MIT = mo.time[4],
                                            Time_S = mo.time[5],
                                            Time_zone = mo.time[6],
                                            Mode = mo.Mode
                                        };
                                        CIE.JDJS_WMS_Device_RealTime_Data.Add(jDMachRealTimeData);
                                        //CIE.SaveChanges();
                                    }
                                    else//已存在则修改
                                    {
                                        foreach (var item in id)
                                        {
                                            item.MachCoord_X = mo.MachCoord[0];
                                            item.MachCoord_Y = mo.MachCoord[1];
                                            item.MachCoord_Z = mo.MachCoord[2];
                                            item.MachCoord_A = mo.MachCoord[3];
                                            item.MachCoord_B = mo.MachCoord[4];
                                            item.MachCoord_C = mo.MachCoord[5];
                                            item.AbsCoord_X = mo.AbsCoord[0];
                                            item.AbsCoord_Y = mo.AbsCoord[1];
                                            item.AbsCoord_Z = mo.AbsCoord[2];
                                            item.AbsCoord_A = mo.AbsCoord[3];
                                            item.AbsCoord_B = mo.AbsCoord[4];
                                            item.AbsCoord_C = mo.AbsCoord[5];
                                            item.RelCoord_X = mo.RelCoord[0];
                                            item.RelCoord_Y = mo.RelCoord[1];
                                            item.RelCoord_Z = mo.RelCoord[2];
                                            item.RelCoord_A = mo.RelCoord[3];
                                            item.RelCoord_B = mo.RelCoord[4];
                                            item.RelCoord_C = mo.RelCoord[5];
                                            item.ProgState = mo.ProgState;
                                            item.CurrWCoord = mo.CurrWCoord;
                                            item.FeedRate = mo.FeedRate;
                                            item.SpindleSpeed = mo.SpindleSpeed;
                                            item.ToolNo = mo.ToolNO;
                                            item.MachTime = mo.MachTime;
                                            item.CurrO = mo.CurrO;
                                            item.CurrOMainO = mo.CurrMainO;
                                            item.CountNum = mo.WorkCount;
                                            item.SpindleRate = mo.Rates[0];
                                            item.Rate = mo.Rates[1];
                                            item.SInfo_A = mo.Sinfo[0];
                                            item.SInfo_NM = mo.Sinfo[1];
                                            item.SInfo_W = mo.Sinfo[2];
                                            item.CurLineNo = mo.CurLineNo[0];
                                            item.CurLineText = mo.CurrLineText;
                                            item.Time_Y = mo.time[0];
                                            item.Time_M = mo.time[1];
                                            item.Time_D = mo.time[2];
                                            item.Time_H = mo.time[3];
                                            item.Time_MIT = mo.time[4];
                                            item.Time_S = mo.time[5];
                                            item.Time_zone = mo.time[6];
                                            item.Mode = mo.Mode;
                                            //CIE.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    var id = CIE.JDJS_WMS_Device_RealTime_Data.Where(r => r.CncID == mode.Key);
                                    if (id.Count() < 1)
                                    {
                                        JDJS_WMS_Device_RealTime_Data jDMachRealTimeData = new JDJS_WMS_Device_RealTime_Data()
                                        {
                                            CncID = mode.Key,
                                            MachCoord_X = -1,
                                            MachCoord_Y = -1,
                                            MachCoord_Z = -1,
                                            MachCoord_A = -1,
                                            MachCoord_B = -1,
                                            MachCoord_C = -1,
                                            AbsCoord_X = -1,
                                            AbsCoord_Y = -1,
                                            AbsCoord_Z = -1,
                                            AbsCoord_A = -1,
                                            AbsCoord_B = -1,
                                            AbsCoord_C = -1,
                                            RelCoord_X = -1,
                                            RelCoord_Y = -1,
                                            RelCoord_Z = -1,
                                            RelCoord_A = -1,
                                            RelCoord_B = -1,
                                            RelCoord_C = -1,
                                            ProgState = -1,
                                            CurrWCoord = -1,
                                            FeedRate = -1,
                                            SpindleSpeed = -1,
                                            ToolNo = -1,
                                            MachTime = -1,
                                            CurrO = -1,
                                            CurrOMainO = -1,
                                            CountNum = -1,
                                            SpindleRate = -1,
                                            Rate = -1,
                                            SInfo_A = -1,
                                            SInfo_NM = -1,
                                            SInfo_W = -1,
                                            CurLineNo = -1,
                                            //CurLineText =mo.CurrLineText ,
                                            Time_Y = -1,
                                            Time_M = -1,
                                            Time_D = -1,
                                            Time_H = -1,
                                            Time_MIT = -1,
                                            Time_S = -1,
                                            Time_zone = -1,
                                            Mode = -1
                                        };
                                        CIE.JDJS_WMS_Device_RealTime_Data.Add(jDMachRealTimeData);
                                        //CIE.SaveChanges();
                                    }
                                    else//已存在则修改
                                    {
                                        foreach (var item in id)
                                        {
                                            item.MachCoord_X = -1;
                                            item.MachCoord_Y = -1;
                                            item.MachCoord_Z = -1;
                                            item.MachCoord_A = -1;
                                            item.MachCoord_B = -1;
                                            item.MachCoord_C = -1;
                                            item.AbsCoord_X = -1;
                                            item.AbsCoord_Y = -1;
                                            item.AbsCoord_Z = -1;
                                            item.AbsCoord_A = -1;
                                            item.AbsCoord_B = -1;
                                            item.AbsCoord_C = -1;
                                            item.RelCoord_X = -1;
                                            item.RelCoord_Y = -1;
                                            item.RelCoord_Z = -1;
                                            item.RelCoord_A = -1;
                                            item.RelCoord_B = -1;
                                            item.RelCoord_C = -1;
                                            item.ProgState = -1;
                                            item.CurrWCoord = -1;
                                            item.FeedRate = -1;
                                            item.SpindleSpeed = -1;
                                            item.ToolNo = -1;
                                            item.MachTime = -1;
                                            item.CurrO = -1;
                                            item.CurrOMainO = -1;
                                            item.CountNum = -1;
                                            item.SpindleRate = -1;
                                            item.Rate = -1;
                                            item.SInfo_A = -1;
                                            item.SInfo_NM = -1;
                                            item.SInfo_W = -1;
                                            item.CurLineNo = -1;
                                            //item.CurLineText = -1;
                                            item.Time_Y = -1;
                                            item.Time_M = -1;
                                            item.Time_D = -1;
                                            item.Time_H = -1;
                                            item.Time_MIT = -1;
                                            item.Time_S = -1;
                                            item.Time_zone = -1;
                                            item.Mode = -1;
                                            //CIE.SaveChanges();
                                        }
                                    }
                                }


     
                            }
                        }
                        CIE.SaveChanges();
                        mytran.Commit();

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
                        mytran.Rollback();
                        return false;
                    }
                }
            }
        }

        public List<CncInfo> GetCncInfo()
        {
            List<CncInfo> infos = new List<CncInfo>();
            try
            {
                using (Materiel_NoScan_V2Entities model = new Materiel_NoScan_V2Entities())
                {
                    var dataList = from cnc in model.JDJS_WMS_Device_Info
                                   join type in model.JDJS_WMS_Device_Type_Info on cnc.MachType equals type.ID
                                   join brand in model.JDJS_WMS_Device_Brand_Info on type.BrandID equals brand.ID
                                   select new
                                   {
                                       cnc.ID,
                                       cnc.IP,
                                       cnc.MachNum,
                                       cnc.MachState,
                                       cnc.MachType,
                                       cnc.Position,
                                       type.Type,
                                       type.BrandID ,
                                       brand.Brand
                                   };
                    if (dataList.Count() > 0)
                    {
                        foreach (var item in dataList)
                        {
                            switch (item.BrandID)//判断机床品牌
                            {
                                case 1:
                                    JDInfo jd = new JDInfo();
                                    jd.ID = item.ID;
                                    jd.MachNum = item.MachNum;
                                    jd.MachBrand = item.Brand;
                                    jd.MachType = item.Type;
                                    jd.IP = item.IP;
                                    jd.MachState = item.MachState;
                                    infos.Add(jd);
                                    break;
                                case 2:
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    break;
                                case 5:
                                    break;
                                case 6:
                                    break;
                            }
                        }
                    }
                }

                return infos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
