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
    
    public partial class tbl_captcha
    {
        public long cp_id { get; set; }
        public string cp_cleartext { get; set; }
        public string cp_ciphertext { get; set; }
        public Nullable<System.DateTime> cp_expiration { get; set; }
        public bool cp_check { get; set; }
        public Nullable<System.DateTime> cp_date { get; set; }
    }
}
