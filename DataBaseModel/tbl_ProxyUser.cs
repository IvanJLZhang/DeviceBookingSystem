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
    
    public partial class tbl_ProxyUser
    {
        public int id { get; set; }
        public Nullable<bool> IsFinished { get; set; }
        public string UID { get; set; }
        public string ProxyUID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
    }
}
