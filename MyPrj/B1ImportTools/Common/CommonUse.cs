namespace B1ImportTools
{
    public class CommonUse
    {

        public static SAPbobsCOM.Company curCompany;
        public static void Login()
        {
            curCompany = new SAPbobsCOM.Company();
            curCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
            //curCompany.LicenseServer = "192.168.3.222:30000";
            //
            curCompany.Server = "LYQ-ACE9616F909";
            curCompany.CompanyDB = "Olin_Test5";
            curCompany.UserName = "manager";
            curCompany.Password = "avatech";

            int conRetVal = curCompany.Connect();
            string conRetValErrorMsg = curCompany.GetLastErrorDescription();
        }
    }
}
