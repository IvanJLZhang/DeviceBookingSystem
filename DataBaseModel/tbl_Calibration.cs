//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Calibration
    {
        public string C_ID { get; set; }
        public string Device_ID { get; set; }
        public Nullable<System.DateTime> Calibration_Date { get; set; }
        public Nullable<double> Calibration_Cost { get; set; }
        public Nullable<int> Reminding_day { get; set; }
        public Nullable<int> Calibration_Duration { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
    }
}
