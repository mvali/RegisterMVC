using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Register.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Photos { get; set; }
        public int Looks { get; set; }
        public int SessionTime { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyValue { get; set; }
        public string CurrencySymbol { get; set; }
        public List<Service> ServiceList { get; set; }
        public List<Service> ServiceListAll { get; set; }

        public Package() { }
        public Package(int id, string name, ... List<Service> servicesList)
        {
            Id = id;
            Name = name;
            ...
            CurrencySymbol = currencySymbol;
            ServiceList = servicesList;
        }
        public IEnumerable<Package> GetByCountry()
        {
            List <Package> pl = new List<Package>();
            DataTable dt = new DataTable();
            int countryId = Models.Job.Session().Zip.CountryId;
            int r = new Packages().P..._...(countryId, ref dt);

            List<Service> sl = new List<Service>();
            List<Service> slA = new List<Service>();

            int cnt = 0; decimal c_value = 1;
            int drLast = dt.Rows.Count;
            for(int i=0; i<dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                cnt++;
                int pk_id = Utils.Str2Int(dr["pk_id"].ToString());
                int pk_idNext = i + 1 < drLast ? Utils.Str2Int(dt.Rows[i + 1]["pk_id"].ToString()) : -1;

                c_value = Utils.Str2Decimal(dr["c_value"].ToString());

                Service srv = new Service(...);

                slA.Add(srv);
                if (Utils.Str2Bool(dr["p_status"].ToString()))
                    sl.Add(srv);

                if ((pk_idNext != pk_id) || cnt == drLast)
                {
                    List<Service> s1 = new List<Service>(sl);
                    List<Service> sa1 = new List<Service>(slA);
                    Package p = new Package(...);
                    pl.Add(p);
                    sl.Clear();
                    slA.Clear();
                }
            }
            return pl;
        }
        public Package Get()
        {
            int pkid = Job.Session().PackageId;
            var pk = new Package();
            var pkList = G...().Where(x => x.Id == pkid);
            if(pkList.Count()>0)
                pk = pkList.First();

            return pk;
        }
        public static bool Chosen(int pkId)
        {
            var retV = false;
            if (Job.Session().PackageId == pkId)
                retV = true;
            return retV;
        }
        public static decimal PackageUpdate(int pkid)
        {
            int pkId = pkid;
            if (pkId > 0)
            {
                Job jb = Job.Session();
                jb.PackageId = pkId;

                Job.Update(jb);

                new Packages().J...Update(jb.Id, jb.CountryId, jb.CurrencyValue, pkid);
            }
            else
                pkId = Job.Session().PackageId;
            return Job.AmountTotal();
        }


    }
}