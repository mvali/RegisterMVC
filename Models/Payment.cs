using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Net;
using ut = Utils;

namespace Register.Models
{
    public class Payment
    {
        public string JobName { get; set; }
        public string JobCity { get; set; }
        public string JobState { get; set; }
        public string JobZip { get; set; }
        public string token { get; set; }
        static string secret_key = "sk_test_f8UIKCa4xIifDtqCPJswembJ";
        static string Stripe_client_id = "ca_CChAiSc1IIXgkdK7JV68A3NlWlvaS43D";
        static string publishable_key = "pk_test_W2DdRaRLSxfXwLqfnUjsCPOM";

        public Payment() { }

        public static bool PayProcessStripe(int Id_Job, System.Collections.Specialized.NameValueCollection TranArray, ref string errorCode, ref JsonStripeResponse payResponse)
        {
            //Id_Job,TranArray,Amount,parity,OrderID,ByRef Valid, ByRef ErrMsg,byref PayPalAmount,byref PaypalVariables
            //setup some variables
            bool bRet = false;
            errorCode = "";

            bool demomode = true;
            bool offlinepayment = false; //ut.Str2Bool(VariableGetByName("payment_offline"));
            bool offlinepaymentresponse = true;// ut.Str2Bool(VariableGetByName("payment_offlineresponse"));
            string clientEmail = TranArray["email"];

            bool EmailtestMode = false; // Utils.Str2Bool(BasePage.VariableGetByName("EmailTestMode"));
            if (EmailtestMode)
            {
                clientEmail = "";// BasePage.VariableGetByName("EmailTestAddress");
            }
            //clientEmail = "vali.my@gmail.com";

            String result = "";
            //String PaymentString = "x_Test_Request=false&x_Version=3.0&x_Login=lookbetter04&x_Password=klatuu444&x_ADC_Delim_Data=True&x_ADC_Url=False&x_Card_Num="& GetArrayValue(TranArray,0) &"&x_Exp_Date="& GetArrayValue(TranArray,1)  &"&x_Email_Customer=true&x_Email="& GetArrayValue(TranArray,2) &"&x_First_Name="& GetArrayValue(TranArray,3) &"&x_Last_Name="& GetArrayValue(TranArray,4) &"&x_Country="& GetArrayValue(TranArray,5) &"&x_Address="& GetArrayValue(TranArray,6) &"&x_City="& GetArrayValue(TranArray,7) &"&x_Zip="& GetArrayValue(TranArray,8) &"&x_invoice_num="& OrderID &"&x_Type=&x_Amount=" & USD_amount' & "&x_currency_code=" &country_code

            //String PaymentString = "x_Test_Request=" + (demomode ? "true" : "false") + "&x_Version=3.0&x_Login=lookbetter04&x_Password=klatuu444&x_ADC_Delim_Data=True&x_ADC_Url=False&x_Card_Num=" + TranArray["ccname"] + "&x_Exp_Date=" + TranArray["ccexpire"] + "&x_Email_Customer=true&x_Email=" + clientEmail + "&x_First_Name=" + TranArray["firstname"] + "&x_Last_Name=" + TranArray["lastname"] + "&x_Country=" + TranArray["country"] + "&x_Address=" + TranArray["address"] + "&x_City=" + TranArray["city"] + "&x_Zip=" + TranArray["zip"] + "&x_invoice_num=" + TranArray["invoice"] + "&x_Type=&x_Amount=" + TranArray["amountusd"];

            string PaymentString = "amount=" + ( ut.MathRound(ut.Str2Decimal(TranArray["amountusd"].ToString())) * 100).ToString("G29");
            PaymentString += "&currency=usd";
            PaymentString += "&description=" + HttpUtility.UrlEncode("Lookbetteronline Services"); //''An arbitrary string which you can attach to a Charge object. It is displayed when in the web interface alongside the charge. Note that if you use Stripe to send automatic email receipts to your customers, your receipt emails will include the description of the charge(s) that they are describing.
            PaymentString += "&capture=true";
            PaymentString += "&receipt_email=" + clientEmail; // ''email to sent confirmation to
            PaymentString += "&metadata[job_id]=" + Id_Job.ToString();// ''internal view only
            PaymentString += "&metadata[FirstName]=" + HttpUtility.UrlEncode(TranArray["firstname"]);// ''internal view only
            PaymentString += "&metadata[LastName]=" + HttpUtility.UrlEncode(TranArray["lastname"]);// ''internal view only
            PaymentString += "&metadata[email]=" + clientEmail;// ''internal view only
            PaymentString += "&statement_descriptor=" + HttpUtility.UrlEncode("Lookbetteronline Order");
            PaymentString += "&source=" + TranArray["token"];


            String url = "https://api.stripe.com/v1/charges";
            System.IO.StreamWriter myWriter = null;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.DefaultConnectionLimit = 9999;

            System.Net.HttpWebRequest objRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = PaymentString.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";
            objRequest.Headers.Add("Authorization", "Bearer " + secret_key);

            // LIVE mode
            try
            {
                myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
                //uncomment for LIVE
                myWriter.Write(PaymentString);
            }
            catch (Exception e)
            {
//Log.AddLogEntryPayment(e.Message);
                errorCode = "streamwrite";
            }
            finally
            {
                //uncomment for LIVE
                myWriter.Close();
            }


            // remove false for LIVE
            if (String.IsNullOrWhiteSpace(errorCode))
            {
                try
                {
                    System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();

                        // Close and clean up the StreamReader
                        sr.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log.LogException(ex);
                    errorCode = "declined";
                }
            }

            //comment for LIVE
            //result = Log.ReadLogEntry("PayResponse.txt");
//Log.AddLogEntryPayment("PaymentStringLog=" + PaymentString + Environment.NewLine + "_Id_Job=" + Id_Job + "_str_response=" + result);

            /*TranArray (0) = var_json
            TranArray (1) = oJSON.data("id")
            TranArray (2) = oJSON.data("amount")
            TranArray (3) = oJSON.data("livemode")
            TranArray (4) = oJSON.data("paid")
            TranArray (5) = oJSON.data("status")*/

            bool bValidPayment = false;
            if (String.IsNullOrWhiteSpace(errorCode))
            {
                payResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonStripeResponse>(result);
                if (ut.Str2Bool(payResponse.paid) && payResponse.status == "succeeded")
                {
                    // ok transaction paid
                    if (ut.Str2Bool(payResponse.livemode))
                    {
                        //livemode
                        bValidPayment = true;
                    }
                    else
                    {
                        if (demomode) bValidPayment = true;
                    }
                }
                else
                {
                    errorCode = "declined";
                }


                if (String.IsNullOrWhiteSpace(errorCode))
                {
                    if (bValidPayment)
                    {
                        int r = new Jobs().Job_AddPayment(Id_Job, TranArray["amount"], TranArray["amountusd"], PaymentString + "___" + result);
                        bRet = true;
                    }
                    else
                    {
                        errorCode = "resultinvalid";
                    }
                }
            }// end errorcode=null
            return bRet;
        }


        public static int AfterPaymentUpdates(JsonStripeResponse jsRet)
        {
            int retV = 0;
            var j = Models.Job.Session();
            j.Paid = true;
            Models.Job.Update(j);

            Jobs jb = new Jobs(); PaymentsIn pin = new PaymentsIn();
            decimal paidAmount = Math.Truncate(Utils.Str2Decimal(jsRet.amount)) / 100;
            string tranCode = jsRet.id + " receipt_number:" + jsRet.receipt_number;
            int pi_id = pin.PaymentsIn_Add(j.Id, j.JobPersonalInfo.FirstName, j.JobPersonalInfo.LastName, j.JobPersonalInfo.JobEmail, paidAmount, jsRet.currency, tranCode, jsRet.livemode);
            if (pi_id > 0) { retV = pin.PaymentsInDetails_Add(j.Id, pi_id, 1, j.PackageId); }

            List<int> sl = j.ServicesId;
            foreach (var s in sl)
            {
                int r2 = new Services().JobServices_addnew(j.Id, j.PackageId, s);
                retV += r2 < 0 ? r2 : 0;
                if (pi_id > 0) { int r3 = pin.PaymentsInDetails_Add(j.Id, pi_id, 2, s); retV += r3 < 0 ? r3 : 0; }
            }


            int r = jb.Job_AssignPhotographer2(j.Id, j.PhotographerId, paidAmount); retV += r < 0 ? r : 0;
            int r4 = jb.Job_UpdatePasswords(j.Id); retV += r4 < 0 ? r4 : 0;
            int r5 = jb.Job_UpdateTotal(j.Id, paidAmount); retV += r5 < 0 ? r5 : 0;
            // send emails

            int r6 = SendEmails();

            return retV;
        }

        public static int SendEmails()
        {
            int retV = 0;
            Job j = Job.Session();
            DataTable dtj = new DataTable();
            DataTable dt = new DataTable();
            int r = new Jobs().Job_GetByID(j.Id, ref dtj);
            if (dtj.Rows.Count <= 0)
            {
                retV = -1;
            }
            else
            {
                var newservicesadded = "";
                foreach (var x in j.ServicesId) { newservicesadded += (newservicesadded.Length > 0 ? "," : "") + x.ToString(); }

                int r2 = new Services().Services_get_bylist_IdJob(newservicesadded, j.Id, j.PackageId, "services_category_name", "asc", ref dt);
                DataRow d = dtj.Rows[0];

                var sl = new List<Models.Service>();
                var p = new Models.Package();
                decimal promoCodeAmount = 0;

                Decimal totalAmount = Models.Job.AmountTotal(true, ref sl, ref p, ref promoCodeAmount);
                string promoName = j.Promocode == null ? "" : j.Promocode.name;

                // dr(r,"")
                List<EmailParameter> ep = new List<EmailParameter>{
                    new EmailParameter("PHOTOGRAPHERFIRSTNAME", dr(d,"PhotographerFirstName")),
                    new EmailParameter("PHOTOGRAPHERLASTNAME", dr(d,"PhotographerLastName")),
                    new EmailParameter("PHOTOGRAPHEREMAIL", dr(d,"PhotographerEmail")),
                    new EmailParameter("PHOTOGRAPHERPASSWORD", dr(d,"PhotographerPassword")),
                    new EmailParameter("PHOTOGRAPHERADDRESS", dr(d,"PhotographerAddress")),
                    new EmailParameter("PHOTOGRAPHERCITY", dr(d,"PhotographerCity")),
                    new EmailParameter("PHOTOGRAPHERSTATE", dr(d,"PhotographerState")),
                    new EmailParameter("PHOTOGRAPHERZIP", dr(d,"PhotographerZIP")),
                    new EmailParameter("PHOTOGRAPHERPHONE", dr(d,"PhotographerPhone")),
                    new EmailParameter("JOBFIRSTNAME", dr(d,"JobFirstName")),
                    new EmailParameter("JOBLASTNAME", dr(d,"JobLastName")),
                    new EmailParameter("JOBSESSIONDATE", Utils.Str2Date2Str(dr(d,"dJobSessionDate"),"dddd MMMM d, yyyy hh:mm tt")),
                    new EmailParameter("JOBLOCATIONADDRESSHOME", dr(d,"JobAddressHome")),
                    new EmailParameter("JOBLOCATIONCITYHOME", dr(d,"JobCityHome")),
                    new EmailParameter("JOBLOCATIONSTATEHOME", dr(d,"JobStateHome")),
                    new EmailParameter("JOBLOCATIONZIPHOME", dr(d,"JobZipHome")),
                    new EmailParameter("ID_PHOTOGRAPHER", dr(d,"Id_Photographer")),
                    new EmailParameter("ID_JOB", j.Id.ToString()),
                    new EmailParameter("JOBEMAIL", dr(d,"JobEmail")),
                    new EmailParameter("JOBPASSWORD", dr(d,"JobPassword")),
                    new EmailParameter("JOBTOTAL", dr(d,"JobTotal")),
                    new EmailParameter("USERSUSERNAME", dr(d,"UsersUsername")),
                    new EmailParameter("JOB_IDENTITY_GUID", dr(d,"Job_Identity_guid")),
                    new EmailParameter("JTNAME", dr(d,"JtName")),
                    new EmailParameter("PACKAGE", dr(d,"pk_name")),
                    new EmailParameter("JOBPHONE", dr(d,"JobPhone")),
                    new EmailParameter("JOBPHONEALT", ""),
                    new EmailParameter("CONTACT_NAME", ""),
                    new EmailParameter("CONTACT_NAME_ALT", ""),
                    new EmailParameter("USERMESSAGE", ""),
                    new EmailParameter("JOBPAYPALAMOUNT_USD", dr(d,"JobPaypalAmount_USD")),
                    new EmailParameter("CURRENCY", dr(d,"c_name")),
                    new EmailParameter("CURRENCY_SYMBOLSERV", dr(d,"c_symbol")),
                    new EmailParameter("PARTNERSITEURL", "http://lookbetteronline.com"),
                    new EmailParameter("PARTNERNAME", dr(d,"UsersName")),
                    new EmailParameter("USERSPROMOCODENAME", promoName),
                    new EmailParameter("USERSPROMOCODEVALUE", promoCodeAmount.ToString()),
                    new EmailParameter("USERSPROMOCODEDESC", "")
                };

                if (!String.IsNullOrWhiteSpace(dr(d, "JobAddressHome")))
                {
                    ep.Add(new EmailParameter("JOBLOCATIONADDRESS", dr(d, "JobAddressHome")));
                    ep.Add(new EmailParameter("JOBLOCATIONCITY", dr(d, "JobCityHome")));
                    ep.Add(new EmailParameter("JOBLOCATIONSTATE", dr(d, "JobStateHome")));
                    ep.Add(new EmailParameter("JOBLOCATIONZIP", dr(d, "JobZipHome")));
                }
                else
                {
                    ep.Add(new EmailParameter("JOBLOCATIONADDRESS", dr(d, "JobAddress")));
                    ep.Add(new EmailParameter("JOBLOCATIONCITY", dr(d, "JobCity")));
                    ep.Add(new EmailParameter("JOBLOCATIONSTATE", dr(d, "StateLN")));
                    ep.Add(new EmailParameter("JOBLOCATIONZIP", dr(d, "JobZip")));
                }

                List<EmailParameterBlock> epb = new List<EmailParameterBlock>();
                epb.Add(new EmailParameterBlock("PHOTOGRAPHYSESSION1", true));
                epb.Add(new EmailParameterBlock("PHOTOGRAPHYSESSION2", true));
                epb.Add(new EmailParameterBlock("OPTIONALSERVICESJOB", false));

                if (dr(d, "Job_Identity_guid").Length > 0)
                    epb.Add(new EmailParameterBlock("UPDATEJOBSTATUS", true));

                if (!String.IsNullOrWhiteSpace(dr(d, "JobZipHome")))
                {
                    epb.Add(new EmailParameterBlock("ADDRESSHOME", true));
                }
                else
                {
                    epb.Add(new EmailParameterBlock("ADDRESSHOME", false));
                }
                epb.Add(new EmailParameterBlock("JOBDETAILS", true));
                epb.Add(new EmailParameterBlock("INSTUDIO", false));
                epb.Add(new EmailParameterBlock("ONLOCATION", true));
                epb.Add(new EmailParameterBlock("PHADDRESS1", false));
                epb.Add(new EmailParameterBlock("PHADDRESS2", false));
                epb.Add(new EmailParameterBlock("DATING", true));
                epb.Add(new EmailParameterBlock("CORPORATE", false));
                epb.Add(new EmailParameterBlock("CONTACTTIME", false));
                epb.Add(new EmailParameterBlock("CONTACTTIMEALT", false));
                epb.Add(new EmailParameterBlock("PHONEALT", false));

                if (String.IsNullOrWhiteSpace(promoName))
                {
                    epb.Add(new EmailParameterBlock("PROMOCODE", false));
                    epb.Add(new EmailParameterBlock("TITLEPROMOCODE", false));
                }
                else
                {
                    epb.Add(new EmailParameterBlock("PROMOCODE", true));
                    epb.Add(new EmailParameterBlock("TITLEPROMOCODE", true));
                }
                if (Utils.Str2Bool(dr(d, "retouching")))
                {
                    epb.Add(new EmailParameterBlock("RETOUCHINGYES", true));
                    epb.Add(new EmailParameterBlock("RETOUCHINGNO", false));
                }
                else
                {
                    epb.Add(new EmailParameterBlock("RETOUCHINGYES", false));
                    epb.Add(new EmailParameterBlock("RETOUCHINGNO", true));
                }


                epb.Add(new EmailParameterBlock("vidasnapyes", false));
                epb.Add(new EmailParameterBlock("vidasnapno", true));

                if (dr(d, "UsersUsername") == "vida")
                {
                    epb.Add(new EmailParameterBlock("vidayes", true));
                    epb.Add(new EmailParameterBlock("vidano", false));
                }
                else
                {
                    epb.Add(new EmailParameterBlock("vidayes", false));
                    epb.Add(new EmailParameterBlock("vidano", true));
                }

                if (dt.Rows.Count == 0)
                {
                    epb.Add(new EmailParameterBlock("TITLEOPTIONALSERVICES", false));
                    epb.Add(new EmailParameterBlock("OPTIONALSERVICE", false));
                    epb.Add(new EmailParameterBlock("PHOTOGRAPHSERVICE2", false));
                }
                else
                {
                    epb.Add(new EmailParameterBlock("TITLEOPTIONALSERVICES", true));

                    bool services_cat2ph = false;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int services_id = ut.Str2Int(dt.Rows[i]["services_id"].ToString());
                        if ((services_id == 5 || services_id == 6) && !services_cat2ph)
                            services_cat2ph = true;
                        decimal servicePrice = ut.MathRound(ut.Str2Decimal(dt.Rows[i]["p_value"].ToString()) * ut.Str2Decimal(dt.Rows[i]["c_value"].ToString()));

                        epb.Add(new EmailParameterBlock("OPTIONALSERVICE", true, new List<EmailParameter>
                        {
                        new EmailParameter("SERVICES_CATEGORY_NAME",dt.Rows[i]["services_category_name"].ToString()),
                        new EmailParameter("SERVICES_NAME",dt.Rows[i]["services_name"].ToString()),
                        new EmailParameter("CURRENCY",dt.Rows[i]["c_name"].ToString()),
                        new EmailParameter("SERVICES_PRICE",servicePrice.ToString()),
                        new EmailParameter("SERVICES_TEXT",dt.Rows[i]["services_text"].ToString())
                        }
                        ));
                        SendOptionalserviceEmail(dt.Rows[i]["services_email"].ToString(), i, d, dt);
                    }
                    if (services_cat2ph)
                        epb.Add(new EmailParameterBlock("PHOTOGRAPHSERVICE2", true));
                    else
                        epb.Add(new EmailParameterBlock("PHOTOGRAPHSERVICE2", false));
                }
                string jobEmailName = "SMJobSelectedMailHTML";
                string jobEmailPackage = new Packages().PackagesEmails_GetOrder(j.PackageId);
                if (!String.IsNullOrWhiteSpace(jobEmailPackage))
                    jobEmailName = jobEmailPackage;

                //' send mail to client
                int rej = new EmailServer().EmailSend(jobEmailName, new MailType(dr(d, "JobEmail")), ep, epb);

                //'send mail to photographer
                int re = new EmailServer().EmailSend("SMPhotographerSelectedMailHTML", new MailType(dr(d, "PhotographerEmail")), ep, epb);
                if (ut.Str2Bool(dr(d,"Ph_FirstJob")))
                {
                    List<EmailParameter> ep1 = new List<EmailParameter> { new EmailParameter("PHOTOGRAPHERFIRSTNAME", dr(d,"PhotographerFirstName")) };
                    int r1 = new EmailServer().EmailSend("SMPhotographerFirstJob", new MailType(dr(d,"PhotographerEmail")), ep1, null);
                }

                // send mail to admin about new job assignement
                int re1 = new EmailServer().EmailSend("adminassignjob", new MailType(EmailServer.EmailAdministrator()), ep, epb);
            }


            return retV;
        }
        public static string dr(DataRow row, string columnName)
        {
            return row[columnName].ToString();
        }

        static void SendOptionalserviceEmail(string SentTo, int serviceRow, DataRow d, DataTable dtServices)
        {
            if (dtServices.Rows.Count <= serviceRow)
            {// no service found.. error
                return;
            }
            decimal p_value = ut.Str2Decimal(dtServices.Rows[serviceRow]["p_value"].ToString());
            decimal c_value = ut.Str2Decimal(dtServices.Rows[serviceRow]["c_value"].ToString());
            decimal servicePrice = ut.MathRound(p_value * c_value);

            List<EmailParameter> ep = new List<EmailParameter>{
                    new EmailParameter("JOBFIRSTNAME", dr(d,"JobFirstName")),
                    new EmailParameter("JOBLASTNAME", dr(d,"JobLastName")),
                    new EmailParameter("JOBSESSIONDATE", Utils.Str2Date2Str(dr(d,"dJobSessionDate"),"dddd MMMM d, yyyy hh:mm tt")),//????????????????????????
                    new EmailParameter("ID_JOB", dr(d,"Id_Job")),
                    new EmailParameter("JOBEMAIL", dr(d,"JobEmail")),
                    new EmailParameter("JOBPASSWORD", dr(d,"JobPassword")),
                    new EmailParameter("USERSUSERNAME", dr(d,"UsersUsername")),
                    new EmailParameter("SERVICES_CATEGORY_NAME", dtServices.Rows[serviceRow]["services_category_name"].ToString()),
                    new EmailParameter("SERVICES_NAME", dtServices.Rows[serviceRow]["services_name"].ToString()),
                    new EmailParameter("SERVICES_EMAIL", dtServices.Rows[serviceRow]["services_email"].ToString()),
                    new EmailParameter("CURRENCY", ""),
                    new EmailParameter("CURRENCY_SYMBOL", ""),
                    new EmailParameter("SERVICES_PRICE", servicePrice.ToString()),
                    new EmailParameter("PARTNERSITEURL", "http://lookbetteronline.com"),
                    new EmailParameter("PARTNERNAME", dr(d,"UsersName")),
                };
            if (!String.IsNullOrWhiteSpace(dr(d, "JobAddressHome")))
            {
                ep.Add(new EmailParameter("JOBLOCATIONADDRESS", dr(d, "JobAddressHome")));//???????????????????
                ep.Add(new EmailParameter("JOBLOCATIONCITY", dr(d, "JobCityHome")));
                ep.Add(new EmailParameter("JOBLOCATIONSTATE", dr(d, "JobStateHome")));
                ep.Add(new EmailParameter("JOBLOCATIONZIP", dr(d, "JobZipHome")));
            }
            else
            {
                ep.Add(new EmailParameter("JOBLOCATIONADDRESS", dr(d, "JobAddress")));
                ep.Add(new EmailParameter("JOBLOCATIONCITY", dr(d, "JobCity")));
                ep.Add(new EmailParameter("JOBLOCATIONSTATE", dr(d, "StateLN")));
                ep.Add(new EmailParameter("JOBLOCATIONZIP", dr(d, "JobZip")));
            }

            int re = new EmailServer().EmailSend("OptionalService", new MailType(SentTo), ep, null);
        }

    }
    public class JsonStripeResponse
    {
        public string var_json;
        public string id;
        public string amount;
        public string livemode;
        public string paid;
        public string status;
        public string currency;
        public string receipt_number;
    }


}