﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CncModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Materiel_NoScan_V2Entities : DbContext
    {
        public Materiel_NoScan_V2Entities()
            : base("name=Materiel_NoScan_V2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<JDJS_WMS_Device_Brand_Info> JDJS_WMS_Device_Brand_Info { get; set; }
        public virtual DbSet<JDJS_WMS_Device_Info> JDJS_WMS_Device_Info { get; set; }
        public virtual DbSet<JDJS_WMS_Device_ProgState_Info> JDJS_WMS_Device_ProgState_Info { get; set; }
        public virtual DbSet<JDJS_WMS_Device_RealTime_Data> JDJS_WMS_Device_RealTime_Data { get; set; }
        public virtual DbSet<JDJS_WMS_Device_Type_Info> JDJS_WMS_Device_Type_Info { get; set; }
        public virtual DbSet<JDJS_WMS_Device_Alarm_History_Table> JDJS_WMS_Device_Alarm_History_Table { get; set; }
    }
}
