//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UKBeautyLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string SellerAccount_ID_fk { get; set; }
        public string Account_Name { get; set; }
        public Nullable<int> Enabled { get; set; }
    }
}
