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
    
    public partial class tbl_equipment_detail
    {
        public int id { get; set; }
        public string e_id { get; set; }
        public Nullable<System.TimeSpan> e_testing_time { get; set; }
        public Nullable<int> e_avg_hr { get; set; }
        public Nullable<int> e_loan_day { get; set; }
        public string e_lab_location { get; set; }
        public Nullable<System.DateTime> e_date { get; set; }
    }
}
