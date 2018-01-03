//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InvoicePOSDATA
{
    using System;
    using System.Collections.Generic;
    
    public partial class CUSTOMER
    {
        public int CUSTOMER_ID { get; set; }
        public string CUSTOMER_CODE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string POSTCODE { get; set; }
        public string COUNTRY { get; set; }
        public string WEBSITE { get; set; }
        public string VAT_TYPE { get; set; }
        public string VAT_NUMBER { get; set; }
        public string PRICE_TYPE { get; set; }
        public Nullable<System.DateTime> DATES_STARTED { get; set; }
        public string DYNAMIC_DISC { get; set; }
        public string CUSTOMER_MAIL { get; set; }
        public string CUSTOMER_STATUS { get; set; }
        public string REGISTERED { get; set; }
        public string DUNS_NO { get; set; }
        public Nullable<int> COMPANY_ID { get; set; }
        public Nullable<System.DateTime> OLDEST_INV_DATE { get; set; }
        public string CONTACT_TYPE { get; set; }
        public string CONTACT_NAME { get; set; }
        public string CONTACT_SALUTATION { get; set; }
        public string CONTACT_PHONE_NO { get; set; }
        public string CONTACT_EXTN_NO { get; set; }
        public string CONTACT_MOBILE_NO { get; set; }
        public string CONTACT_FAX { get; set; }
        public string EMAIL { get; set; }
        public string SKYPE { get; set; }
        public Nullable<System.DateTime> LAST_SALE { get; set; }
        public Nullable<System.DateTime> LAST_PAYMENT { get; set; }
        public Nullable<int> AVG_PMT_DAYS { get; set; }
        public Nullable<bool> IS_SUPPLIER { get; set; }
        public string STATEMENT { get; set; }
        public string SEND_MAIL { get; set; }
        public string SKYPE_ID { get; set; }
        public string OS_BALANCE { get; set; }
        public string OS_ORDERS { get; set; }
        public Nullable<decimal> CR_REMAIN { get; set; }
        public Nullable<bool> IS_DELETE { get; set; }
        public string STATUS { get; set; }
        public Nullable<bool> ON_CREDIT_STOP { get; set; }
        public Nullable<decimal> CREDIT_LIMIT { get; set; }
        public Nullable<int> ON_STOP_AFTER { get; set; }
        public string PUT_ON_STOP_BY { get; set; }
        public Nullable<System.DateTime> STOPPED_ON { get; set; }
    }
}
