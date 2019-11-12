using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class PromoCode
    {
        public string Name { get; set; }
        public decimal DiscountValue { get; set; }
        public int DiscountType { get; set; }
        public int AppliesTo { get; set; }
        public int DateStart { get; set; }
        public int DateEnd { get; set; }
        public int UserRestriction { get; set; }
        public int UsageLimit { get; set; }
        public int UsageCurrent { get; set; }
        public decimal DiscountValuea { get; set; }
        public int DiscountTypea { get; set; }

        public PromoCode() { }

        public PromoCode PromocodeGetObj(string pc_name)
        {
            PromoCode p = new PromoCode();
            DataTable dt = new DataTable();
            int r = new PromoCodes().PromoCodeV2_get_byname(pc_name, ref dt);
            if(r>=0 && dt.Rows.Count>0)
            {
                DataRow row = dt.Rows[0];
                p.Name = pc_name;
                p.DiscountValue = Utils.Str2Decimal(row["pc_discountValue"].ToString());
                p.DiscountType = Utils.Str2Int(row["pc_discountType"].ToString());
                p.AppliesTo = Utils.Str2Int(row["pc_appliesTo"].ToString());
                p.DateStart = Utils.Str2Int(row["dateStart"].ToString());
                p.DateEnd = Utils.Str2Int(row["dateEnd"].ToString());
                p.UserRestriction = Utils.Str2Int(row["pc_userRestriction"].ToString());
                p.UsageLimit = Utils.Str2Int(row["pc_usageLimit"].ToString());
                p.UsageCurrent = Utils.Str2Int(row["pc_usageCurent"].ToString());
                p.DiscountValuea = Utils.Str2Decimal(row["pca_discountValue"].ToString());
                p.DiscountTypea = Utils.Str2Int(row["pca_discountType"].ToString());
            }
            return p;
        }
        public PromocodeResponse PromocodeGet(string pc_name)
        {
            PromocodeResponse pr = new PromocodeResponse();
            PromoCode p = PromocodeGetObj(pc_name);
            int errorCode = 0;

            if (string.IsNullOrWhiteSpace(p.Name))
                errorCode = -1; // not available

            if (p.UsageCurrent >= p.UsageLimit && p.UsageLimit >= 0)
                errorCode = -2;//'promotion run out
            
            if(errorCode==0 && p.UserRestriction==1 && ClientCompletedJobs() <1)
            {
                if (p.DateEnd > 0)
                    errorCode = -5;//' end date is exceeded
                else
                    errorCode = -3;//' only for existing customers
            }
            if (errorCode == 0 && DateStart < 0)
                errorCode = -4;//' start date is later
            if (errorCode == 0 && DateEnd > 0)
                errorCode = -5;//' end date is exceeded

            pr.name = p.Name;
            if (errorCode == 0)
            {
                pr.type = p.DiscountType;
                pr.value = p.DiscountValue;
                pr.appliesTo = p.AppliesTo;
            }
            else
            {
                pr.type = errorCode;
            }

            return pr;
        }

        private int ClientCompletedJobs()
        {
            DataTable dt = new DataTable();
            string jobEmail = Job.Session().JobPersonalInfo.JobEmail;
            int r = new Jobs().Job_GetByEmail(jobEmail, ref dt);

            int retV = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int Id_JobStatus = Utils.Str2Int(dt.Rows[i]["Id_JobStatus"].ToString());
                if(Id_JobStatus>=7 && Id_JobStatus <10)
                {
                    retV = retV + 1;
                }
            }
            return retV;
        }

        public static decimal PromocodeAdd(PromocodeResponse pr)
        {
            if (pr.type >= 0)
            {
                Job j = Job.Session();
                j.Promocode = pr;
                Job.Update(j);
            }
            return Job.AmountTotal();
        }

        public static decimal PromoDiscount(decimal amountTotal, decimal servicesPrice)
        {
            Job j = Job.Session();
            return PromoDiscount(j, amountTotal, servicesPrice);
        }
        public static decimal PromoDiscount(Job j, decimal amountTotal, decimal servicesPrice)
        {
            decimal promoAmount = 0, sourceAmount = amountTotal;
            if (j.Promocode != null)
                if (j.Promocode.type >= 0 && j.Promocode.value>0)
                {
                    if (j.Promocode.appliesTo == 2)
                        sourceAmount = servicesPrice;

                    if (j.Promocode.type == 1)// fixed amount
                        promoAmount = j.Promocode.value;
                    if (j.Promocode.type == 2)// percent
                        promoAmount = Utils.MathRound((sourceAmount * j.Promocode.value) / 100);
                }
            return promoAmount;
        }
        /// <summary>
        /// addiliate commission
        /// </summary>
        /// <param name="amountTotal"></param>
        /// <param name="servicesPrice">not used</param>
        /// <returns></returns>
        public static decimal PromoDiscountCommission(decimal amountTotal, decimal servicesPrice)
        {
            Job j = Job.Session();
            return PromoDiscountCommission(j, amountTotal, servicesPrice);
        }
        public static decimal PromoDiscountCommission(Job j, decimal amountTotal, decimal servicesPrice)
        {
            decimal promoAmount = 0, sourceAmount = amountTotal;
            if (j.Promocode != null)
                if (j.Promocode.typea >= 0 && j.Promocode.valuea>0)
                {
                    //if (j.Promocode.appliesTo == 2)
                    //    sourceAmount = servicesPrice;

                    if (j.Promocode.typea == 1)// fixed amount
                        promoAmount = j.Promocode.valuea;
                    if (j.Promocode.typea == 2)// percent
                        promoAmount = Utils.MathRound((sourceAmount * j.Promocode.valuea) / 100);
                }
            return promoAmount;
        }
    }
    public class PromocodeResponse
    {
        public string name { get; set; }
        public int type;
        public decimal value;
        public int appliesTo;
        public int typea { get; set; }
        public decimal valuea { get; set; }
    }
}