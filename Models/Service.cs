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
        public Service(int id, string name, ...)
        {
            Id = id;
            Name = name;
            Text = text;
            PackageId = packageId;
            ...
        }
        public List<Service> GetByPackageCountry(int packageId)
        {
            DataTable dt = new DataTable();
            int countryId = Models.Job.Session().Zip.CountryId;
            int r = new Packages().P..(countryId, ref dt);

            List<Service> sl = new List<Service>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                int pk_id = Utils.Str2Int(dr["pi"].ToString());
                if (pk_id == packageId)
                {
                    int serviceId = Utils.Str2Int(dr["si"].ToString());
                    Service srv = new Service(
                        serviceId,
                        ...
                        );
                }
            }
            return sl;
        }
    }

}