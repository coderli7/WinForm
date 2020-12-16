using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiCheDS
{
    public class Case_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string organ_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("案件编号")]
        public string case_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string return_flag { get; set; }
    }

    public class Accident_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string insured_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_place { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_result { get; set; }
    }

    public class Base_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apply_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string social_security { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apply_certi_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apply_certi_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apply_cellphone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apply_tel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apply_pas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apply_mail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apply_certi_validate { get; set; }
    }

    public class Surgery_recordItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string surgery_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string surgery_code { get; set; }
    }

    /// <summary>
    /// 手术信息
    /// </summary>
    public class Surgery_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string record_counts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Surgery_recordItem> surgery_record { get; set; }
    }

    //public class Deduct_sum
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string deduct1 { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string deduct2 { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string deduct3 { get; set; }
    //}

    public class Medical_fee_info
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("西药费")]
        public string west_medicine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("中成药费")]
        public string china_medicine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("中草药费")]
        public string herbal_medicine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("诊察费")]
        public string examination { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("检查费")]
        public string inspection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("化验费")]
        public string laboratory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("特殊检查费")]
        public string special_inspection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("治疗费")]
        public string treatment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("手术费")]
        public string surgery { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("其他")]
        public string other { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("材料费")]
        public string material { get; set; }
    }

    public class Fee_detailItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item_tradename { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item_amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string item_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string medical_class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string self_pay_part { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string self_pay_money { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string flag_on_image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string note1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string note2 { get; set; }
    }

    public class Fee_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string fee_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fee_detailItem> fee_detail { get; set; }
    }

    //public class Fee_info
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string sum_amount { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string calc_amount { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string Note { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Deduct_sum deduct_sum { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Medical_fee_info medical_fee_info { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Fee_details fee_details { get; set; }
    //}

    public class Clinic_recordItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clinic_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hospital_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string first_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string end_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Surgery_info surgery_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("费用信息")]
        public Fee_info fee_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gender { get; set; }
    }

    public class Clinic_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string record_counts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Clinic_recordItem> clinic_record { get; set; }
    }

    public class Surgery_info1
    {
        /// <summary>
        /// 
        /// </summary>
        public string record_counts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> surgery_record { get; set; }
    }

    public class Patient_info
    {
        /// <summary>
        /// 陈^
        /// </summary>
        /// 
        [Description("住院病人姓名")]
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("住院病人性别")]
        public string gender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string age { get; set; }
    }

    public class Deduct_sum
    {
        /// <summary>
        /// 
        /// </summary>
        public string deduct1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deduct2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deduct3 { get; set; }
    }

    public class Medical_fee_info1
    {
        /// <summary>
        /// 
        /// </summary>
        public string west_medicine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string china_medicine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string herbal_medicine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string examination { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inspection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string laboratory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string special_inspection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string treatment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string surgery { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nursing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string blood_transfusion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string other { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string material { get; set; }
    }

    public class Fee_details1
    {
        /// <summary>
        /// 
        /// </summary>
        public string fee_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> fee_detail { get; set; }
    }

    public class Fee_info
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("门诊合计费用")]
        public string sum_amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string calc_amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Deduct_sum deduct_sum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("费用分类")]
        public Medical_fee_info medical_fee_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fee_details fee_details { get; set; }
    }

    public class Inpatient_recordItem
    {


        /// <summary>
        /// 
        /// </summary>
        [Description("发票号")]
        public string seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("住院号")]
        public string operation_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("医院代码")]
        public string hospital_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("科别")]
        public string operation_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hospital_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("住院日期")]
        public string in_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("出院日期")]
        public string out_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Surgery_info surgery_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Patient_info patient_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("费用信息")]
        public Fee_info fee_info { get; set; }
    }

    /// <summary>
    /// 住院信息
    /// </summary>
    /// 
    [Description("住院信息")]
    public class Inpatient_info
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("记录数")]
        public string record_counts { get; set; }

        [Description("票据明细")]
        public List<Inpatient_recordItem> inpatient_record { get; set; }
    }

    public class Pay_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string pay_mode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string acco_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string acco_certi_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bank_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bank_account { get; set; }
    }

    public class Extension_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string confirm_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accident_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deformity_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scald_part { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scald_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scald_degree { get; set; }
    }

    public class Agency_info
    {
        /// <summary>
        /// 
        /// </summary>
        public string agency_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string agency_certi_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string agency_certi_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string agency_tel { get; set; }
    }

    public class Questions
    {
        /// <summary>
        /// 
        /// </summary>
        public string QuestionCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Question { get; set; }
    }

    public class Root_PiaoJu
    {
        /// <summary>
        /// 
        /// </summary>
        public string myversion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("案件信息")]
        public Case_info case_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Accident_info accident_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Base_info base_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Clinic_info clinic_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("住院信息")]
        public Inpatient_info inpatient_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Pay_info pay_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Extension_info extension_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Agency_info agency_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Questions Questions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IMG_XMLDATA { get; set; }
    }
}
