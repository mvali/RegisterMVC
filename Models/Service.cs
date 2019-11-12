using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int PackageId { get; set; }
        public int UsersId { get; set; }
        public bool IncludedInPackage { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyValue { get; set; }
        public string CurrencySymbol { get; set; }

        public Service() { }
        public Service(int id, string name, string text, int packageId, int usersId, bool includedInPackage, decimal price, string currency, int currencyId, decimal currencyVaue, string currencySymbol)
        {
            Id = id;
            Name = name;
            Text = text;
            PackageId = packageId;
            UsersId = usersId;
            IncludedInPackage = includedInPackage;
            Price = price;
            Currency = currency;
            CurrencyId = currencyId;
            CurrencyValue = currencyVaue;
            CurrencySymbol = currencySymbol;
        }
        public List<Service> GetByPackageCountry(int packageId)
        {
            DataTable dt = new DataTable();
            int countryId = Models.Job.Session().Zip.CountryId;
            int r = new Packages().Packages_get_byCountryId(countryId, ref dt);

            List<Service> sl = new List<Service>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                int pk_id = Utils.Str2Int(dr["pk_id"].ToString());
                if (pk_id == packageId)
                {
                    int serviceId = Utils.Str2Int(dr["services_id"].ToString());
                    Service srv = new Service(
                        serviceId,
                        dr["services_name"].ToString(),
                        dr["services_text"].ToString(),
                        Utils.Str2Int(dr["pk_id"].ToString()),
                        Utils.Str2Int(dr["fk_users"].ToString()),
                        Utils.Str2Bool(dr["p_status"].ToString()),
                        Utils.Str2Decimal(dr["p_value"].ToString()),
                        dr["c_name"].ToString(),
                        Utils.Str2Int(dr["c_id"].ToString()),
                        Utils.Str2Decimal(dr["c_value"].ToString()),
                        dr["c_symbol"].ToString()
                        );

                    if (!Utils.Str2Bool(dr["p_status"].ToString()) && serviceId != 5)
                        sl.Add(srv);
                }
            }
            return sl;
        }
    }

}