using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class ZipDetails
    {
        [Required(ErrorMessage ="Zip code is required") ]
        [StringLength(7, MinimumLength =5, ErrorMessage ="Zip must be between 5 and 7 characters")]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int CountryId { get; set; }
        public string AddressText { get; set; }

        [RequiredIf("AddressBillingSame == false", ErrorMessage ="Zip code required")]
        [AssertThat("Length(ZipCodeBill)>=5 && Length(ZipCodeBill)<=7", ErrorMessage = "Zip must be between 5 and 7 characters")]
        public string ZipCodeBill { get; set; }
        [RequiredIf("AddressBillingSame == false", ErrorMessage = "City required")]
        public string CityBill { get; set; }
        public string StateBill { get; set; }
        public string StateCodeBill { get; set; }
        public int CountryIdBill { get; set; }
        public string AddressTextBill { get; set; }
        public decimal CurrencyValue { get; set; }

        public bool AddressBillingSame { get; set; }
        public bool AddressBillingDifferent { get; set; }// ?? where it is used as reported by compiler

        public ZipDetails() {
            AddressBillingSame = true;
            CountryId = 0;
            CurrencyValue = 1;
        }
        public ZipDetails(string zipCode, string city, string state, string stateCode, decimal latitude, decimal longitude, int coutryId)
        {
            ZipCode = zipCode;
            City = city;
            State = state;
            StateCode = stateCode;
            Latitude = latitude;
            Longitude = longitude;
            CountryId = coutryId;
            AddressBillingSame = true;
        }
        public List<ZipDetails> GetAll(string zipcode)
        {
            List<ZipDetails> ret = new List<ZipDetails>();
            DataTable dt = new DataTable();
            int r = new Zipcodes().Zipcode_GetCitiesCountry(zipcode, ref dt);

            foreach(DataRow dr in dt.Rows)
            {
                ret.Add(new ZipDetails(dr["ZIPCode"].ToString(),
                    dr["City"].ToString(),
                    dr["State"].ToString(),
                    dr["StateCode"].ToString(),
                    Utils.Str2Decimal(dr["Latitude"].ToString()),
                    Utils.Str2Decimal(dr["Longitude"].ToString()),
                    Utils.Str2Int(dr["fk_CountriesId"].ToString())
                    ));
                break;
            }
            
            return ret;
        }
    }
}