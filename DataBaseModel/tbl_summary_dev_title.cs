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
    
    public partial class tbl_summary_dev_title
    {
        public string s_id { get; set; }
        public string s_ownerid { get; set; }
        public string s_name { get; set; }
        public string s_assetid { get; set; }
        public Nullable<int> s_category { get; set; }
        public string s_vender { get; set; }
        public Nullable<double> s_cost { get; set; }
        public Nullable<int> s_status { get; set; }
        public byte[] s_image { get; set; }
        public string s_image_url { get; set; }
        public string s_note { get; set; }
        public Nullable<System.DateTime> s_date { get; set; }
    }
}
