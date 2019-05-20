using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Text;

namespace Portal.Areas.Tools.Controllers
{
    public class ToolsController : Controller
    {
        //
        // GET: /Tools/
        [OutputCache(CacheProfile = "Static")]
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult Forum()
        {
            return View();
        }

        public ActionResult FileHost()
        {
            return View();
        }

        public ActionResult UsersInformation()
        {
            return View();
        }

        public ActionResult ServerInformation()
        {
            System.Text.StringBuilder StringBuilder1 = new System.Text.StringBuilder(string.Empty);
            //.....Method 2:
            int loop1, loop2;
            System.Collections.Specialized.NameValueCollection coll;
            // Load ServerVariable collection into NameValueCollection object.
            coll = Request.ServerVariables;
            // Get names of all keys into a string array. 
            String[] arr1 = coll.AllKeys;
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                StringBuilder1.AppendLine("Key: " + arr1[loop1] + "<br>");
                String[] arr2 = coll.GetValues(arr1[loop1]);
                for (loop2 = 0; loop2 < arr2.Length; loop2++)
                {
                    StringBuilder1.AppendLine("Value " + loop2 + ": " + Server.HtmlEncode(arr2[loop2]) + "<br>");
                }
            }
            ViewBag.ServerVariables = StringBuilder1.ToString();
            return View();
        }

        public ActionResult SendMail()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMail(string SendTo, string Subject, string Message, IEnumerable<HttpPostedFileBase> Files, string YourEmail, string Password, string SMTPserver, string Port, string SSL)
        {
            string stringResult = null;
            try
            {
                System.Net.Mail.MailMessage MailMessage1 = new System.Net.Mail.MailMessage();
                MailMessage1.To.Add(SendTo);
                MailMessage1.From = new System.Net.Mail.MailAddress(YourEmail);
                MailMessage1.Subject = Subject;
                MailMessage1.Body = Message;
                MailMessage1.IsBodyHtml = false;

                System.Net.Mail.SmtpClient SmtpClient1 = new System.Net.Mail.SmtpClient();
                SmtpClient1.Host = SMTPserver;
                SmtpClient1.Port = int.Parse(Port);
                SmtpClient1.Credentials = new System.Net.NetworkCredential(YourEmail, Password);
                if (SSL == "On")
                    SmtpClient1.EnableSsl = true;
                else
                    SmtpClient1.EnableSsl = false;
                foreach (HttpPostedFileBase file1 in Files)
                {
                    if ((file1 != null) && (file1.ContentLength > 0) && (file1.ContentLength < 20000000))
                    {
                        System.Net.Mail.Attachment Attachment1 = new System.Net.Mail.Attachment(file1.InputStream, file1.ContentType);
                        MailMessage1.Attachments.Add(Attachment1);
                    }
                }
                try
                {
                    SmtpClient1.Send(MailMessage1);
                    stringResult = "Mail send successfully.";
                }
                catch (Exception ex)
                {
                    stringResult = string.Format("Send email failed: {0}", ex.Message);
                }
            }
            catch (Exception ex)
            {
                stringResult = string.Format("Send email failed: {0}", ex.Message);
            }
            @ViewBag.Result = stringResult;
            return View();
        }


        public ActionResult RSS()
        {
            //...XmlTextWriter RSSFeed = new XmlTextWriter(MapPath("./" + "RSS.rss"), System.Text.Encoding.UTF8);
            System.Xml.XmlTextWriter RSSFeed = new System.Xml.XmlTextWriter(Response.OutputStream, System.Text.Encoding.UTF8);
            // Write the rss tags like title, version,
            // Channel, title, link description copyright etc. 
            RSSFeed.WriteStartDocument();
            RSSFeed.WriteStartElement("rss");
            RSSFeed.WriteAttributeString("version", "2.0");
            RSSFeed.WriteStartElement("channel");
            RSSFeed.WriteElementString("title", "Mehdi Naseri RSS");
            RSSFeed.WriteElementString("description", "This Website has been made by: Mehdi Naseri");
            RSSFeed.WriteElementString("link", "http://Naseri.Net");
            RSSFeed.WriteElementString("pubDate", DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss +0000"));
            RSSFeed.WriteElementString("copyright", "Copyright Mehdi Naseri 2013");
            //Items of RSS
            for (int i = 0; i < 3; i++)
            {
                RSSFeed.WriteStartElement("item");
                RSSFeed.WriteElementString("title", string.Format("Title " + (i + 1).ToString()));
                RSSFeed.WriteElementString("description", string.Format("Description " + (i + 1).ToString()));
                RSSFeed.WriteElementString("link", "http://Naseri.Net/RSS");
                //RSSFeed.WriteElementString("pubDate", "Mon, 06 Sep 2009 16:45:00 +0000");
                RSSFeed.WriteElementString("pubDate", DateTime.Now.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss +0000"));
                RSSFeed.WriteEndElement();
            }
            RSSFeed.WriteEndElement();
            RSSFeed.WriteEndElement();
            RSSFeed.WriteEndDocument();
            RSSFeed.Flush();
            RSSFeed.Close();
            Response.End();

            return Content(RSSFeed.ToString(), "text/xml", System.Text.Encoding.UTF8);
            //return View();
        }
        public ActionResult DNS()
        {
            return View();
        }
        [HttpPost]
        public JsonResult DNS(string Text2DNS)
        {
            string stringResult = null;
            if (Text2DNS != null)
            {
                try
                {
                    System.Text.StringBuilder StringBuilder1 = new System.Text.StringBuilder(string.Empty);
                    //System.Net.IPHostEntry hostInfo = System.Net.Dns.Resolve(Text2DNS);
                    //This line replaces by next line (recomended by compiler).
                    System.Net.IPHostEntry hostInfo = System.Net.Dns.GetHostEntry(Text2DNS);
                    // Get the IP address list that resolves to the host names contained in the 
                    // Alias property.
                    System.Net.IPAddress[] address = hostInfo.AddressList;
                    // Get the alias names of the addresses in the IP address list.
                    String[] alias = hostInfo.Aliases;

                    StringBuilder1.Append("Host Name: " + hostInfo.HostName + "<br/>");
                    StringBuilder1.Append("<br/>Aliases:<br/>");
                    for (int index = 0; index < alias.Length; index++)
                    {
                        StringBuilder1.AppendLine(alias[index] + "<br/>");
                    }
                    StringBuilder1.Append("<br/>IP Address list :<br/>");
                    for (int index = 0; index < address.Length; index++)
                    {
                        StringBuilder1.Append(address[index] + "<br/>");
                    }
                    stringResult = StringBuilder1.ToString();
                }
                catch (Exception ex)
                {
                    stringResult = string.Format("DNS Failed: {0}", ex.Message);
                }
            }
            return Json(stringResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ping()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Ping(string Text2Ping)
        {
            string stringPingResult = null;
            if (Text2Ping != null)
            {
                try
                {
                    System.Text.StringBuilder StringBuilder1 = new System.Text.StringBuilder();
                    System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
                    System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();

                    // Use the default Ttl value which is 128,
                    // but change the fragmentation behavior.
                    options.DontFragment = true;

                    // Create a buffer of 32 bytes of data to be transmitted.
                    string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                    byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
                    int timeout = 999;
                    //TextBox 2 hold Address to be pinged 
                    System.Net.NetworkInformation.PingReply reply = pingSender.Send(Text2Ping, timeout, buffer, options);

                    if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        StringBuilder1.AppendFormat("Address: {0}<br/>", reply.Address.ToString());
                        StringBuilder1.AppendFormat("RoundTrip time: {0}<br/>", reply.RoundtripTime);
                        StringBuilder1.AppendFormat("Time to live: {0}<br/>", reply.Options.Ttl);
                        StringBuilder1.AppendFormat("Don't fragment: {0}<br/>", reply.Options.DontFragment);
                        StringBuilder1.AppendFormat("Buffer size: {0}<br/>", reply.Buffer.Length);
                    }
                    else
                    {
                        StringBuilder1.AppendLine(reply.Status.ToString());
                    }
                    stringPingResult = StringBuilder1.ToString();
                }
                catch (Exception ex)
                {
                    stringPingResult = string.Format("<i>Ping Failed:</i> {0}", ex.Message);
                }
            }
            return Json(stringPingResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string Text2Search)
        {
            string stringSearchResult = null;
            if (Text2Search != null)
            {
                try
                {
                    System.Text.StringBuilder StringBuilder1 = new System.Text.StringBuilder();
                    // used on each read operation
                    byte[] buf = new byte[8192];
                    string GS = "http://google.com/search?q=" + Text2Search;
                    // prepare the web page we will be asking for
                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(GS);

                    // execute the request
                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();

                    // we will read data via the response stream
                    System.IO.Stream resStream = response.GetResponseStream();
                    string tempString = null;
                    int count = 0;
                    do
                    {
                        // fill the buffer with data
                        count = resStream.Read(buf, 0, buf.Length);
                        // make sure we read some data
                        if (count != 0)
                        {
                            // translate from bytes to ASCII text
                            tempString = System.Text.Encoding.ASCII.GetString(buf, 0, count);
                            // continue building the string
                            StringBuilder1.Append(tempString);
                        }
                    }
                    while (count > 0);
                    stringSearchResult = StringBuilder1.ToString();
                }
                catch (Exception ex)
                {
                    stringSearchResult = string.Format("Search Failed: {0}", ex.Message);
                }
            }
            return Json(stringSearchResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WebTools()
        {
            return View();
        }
        //public ActionResult WebTools(string Text2DNS, string Text2Ping, string Text2Search)
        //{
        //    if (Text2DNS != null)
        //    {
        //        try
        //        {
        //            System.Text.StringBuilder StringBuilder1 = new System.Text.StringBuilder(string.Empty);
        //            System.Net.IPHostEntry hostInfo = System.Net.Dns.Resolve(Text2DNS);
        //            // Get the IP address list that resolves to the host names contained in the 
        //            // Alias property.
        //            System.Net.IPAddress[] address = hostInfo.AddressList;
        //            // Get the alias names of the addresses in the IP address list.
        //            String[] alias = hostInfo.Aliases;

        //            StringBuilder1.Append("Host Name: " + hostInfo.HostName + "<br/>");
        //            StringBuilder1.Append("<br/>Aliases:<br/>");
        //            for (int index = 0; index < alias.Length; index++)
        //            {
        //                StringBuilder1.AppendLine(alias[index] + "<br/>");
        //            }
        //            StringBuilder1.Append("<br/>IP Address list :<br/>");
        //            for (int index = 0; index < address.Length; index++)
        //            {
        //                StringBuilder1.Append(address[index] + "<br/>");
        //            }
        //            ViewBag.DNSResult = StringBuilder1.ToString();
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.DNSResult = string.Format("DNS Failed: {0}", ex.Message);
        //        }
        //    }
        //    if (Text2Ping != null)
        //    {
        //        try
        //        {
        //            System.Text.StringBuilder StringBuilder1 = new System.Text.StringBuilder();

        //            System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
        //            System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();

        //            // Use the default Ttl value which is 128,
        //            // but change the fragmentation behavior.
        //            options.DontFragment = true;

        //            // Create a buffer of 32 bytes of data to be transmitted.
        //            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        //            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
        //            int timeout = 300;
        //            //TextBox 2 hold Address to be pinged 
        //            System.Net.NetworkInformation.PingReply reply = pingSender.Send(Text2Ping, timeout, buffer, options);
        //            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
        //            {
        //                StringBuilder1.AppendFormat("Address: {0}", reply.Address.ToString());
        //                StringBuilder1.AppendFormat("RoundTrip time: {0}", reply.RoundtripTime);
        //                StringBuilder1.AppendFormat("Time to live: {0}", reply.Options.Ttl);
        //                StringBuilder1.AppendFormat("Don't fragment: {0}", reply.Options.DontFragment);
        //                StringBuilder1.AppendFormat("Buffer size: {0}", reply.Buffer.Length);
        //            }
        //            else
        //            {
        //                StringBuilder1.AppendLine(reply.Status.ToString());
        //            }
        //            ViewBag.PingResult = StringBuilder1.ToString();
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.PingResult = string.Format("Ping Failed: {0}", ex.Message);
        //        }
        //    }
        //    if (Text2Search != null)
        //    {
        //        try
        //        {
        //            System.Text.StringBuilder StringBuilder1 = new System.Text.StringBuilder();
        //            // used on each read operation
        //            byte[] buf = new byte[8192];
        //            string GS = "http://google.com/search?q=" + Text2Search;
        //            // prepare the web page we will be asking for
        //            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(GS);
        //            // execute the request
        //            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        //            // we will read data via the response stream
        //            System.IO.Stream resStream = response.GetResponseStream();
        //            string tempString = null;
        //            int count = 0;
        //            do
        //            {
        //                // fill the buffer with data
        //                count = resStream.Read(buf, 0, buf.Length);
        //                // make sure we read some data
        //                if (count != 0)
        //                {
        //                    // translate from bytes to ASCII text
        //                    tempString = System.Text.Encoding.ASCII.GetString(buf, 0, count);
        //                    // continue building the string
        //                    StringBuilder1.Append(tempString);
        //                }
        //            }
        //            while (count > 0);
        //            ViewBag.SearchResult = StringBuilder1.ToString();
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.SearchResult = string.Format("Search Failed: {0}", ex.Message);
        //        }
        //    }
        //    return View();
        //}

        public ActionResult GoogleMap()
        {
            return View();
        }
        public ActionResult FacebookLike()
        {
            return View();
        }
        public ActionResult GooglePlus()
        {
            return View();
        }
        public ActionResult VisitorsLocation()
        {
            return View();
        }
        public ActionResult ChangeCSS()
        {
            return View();
        }


        public ActionResult MouseOverResizeImage()
        {
            return View();
        }
        public ActionResult VisitorsIP()
        {
            return View();
        }
        public ActionResult jQueryUI()
        {
            return View();
        }
        public ActionResult FinishedTools()
        {
            return View();
        }
        public ActionResult AjaxjQuery()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AjaxjQuery(string Text)
        {
            string JsonResult = string.Format("Your text is: {0}", Text);
            if (!Request.IsAjaxRequest())
                return View();
            else
                return Json(JsonResult);
        }
        public ActionResult AjaxjQueryPartialView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AjaxjQueryPartialView(string Text)
        {
            string JsonResult = string.Format("Your text is: {0}", Text);
            if (!Request.IsAjaxRequest())
                return View();
            else
                return PartialView("_AjaxjQueryPartialView", Text);
        }
        public ActionResult AjaxBeginForm()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AjaxBeginform(string Text)
        {
            string JsonResult = string.Format("Your text is: {0}", Text);
            if (!Request.IsAjaxRequest())
                return View();
            else
                return Json(JsonResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PersianDatePicker()
        {
            ViewBag.Message = "Persian Date Picker";
            return View();
        }
        public ActionResult PersianDateTimePicker()
        {
            ViewBag.Message = "Persian DateTime Picker";
            return View();
        }
        public ActionResult PersianDateTimePicker2()
        {
            ViewBag.Message = "Persian DateTime Picker 2";
            return View();
        }
        public ActionResult BootstrapDateTimePicker()
        {
            ViewBag.Message = "Bootstrap DateTime Picker";
            return View();
        }
        public ActionResult BootstrapDateTimePicker1()
        {
            ViewBag.Message = "Bootstrap DateTime Picker";
            return View();
        }
        public ActionResult RedirectPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RedirectPage(string URL, string ReturnType)
        {
            string stringWebpage, stringJsonResult;
            try
            {
                WebRequest http = (HttpWebRequest)WebRequest.Create(URL);
                WebResponse response = http.GetResponse();
                System.IO.Stream stream = response.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(stream);
                stringWebpage = sr.ReadToEnd();

                stringJsonResult = ConvertURL(stringWebpage, URL);
            }
            catch (Exception ex)
            {
                stringJsonResult = ex.Message;
            }
            UTF8Encoding a1 = new UTF8Encoding();
            if (!Request.IsAjaxRequest())
                return File((new UTF8Encoding()).GetBytes(stringJsonResult), "text/html", "WebPage.html");
            else
                return Json(stringJsonResult);

            //WebClient همان کار httpWebRequest  را انجام میدهد اما با قابلیتهای کمتر
            //var client = new WebClient();
            /*var content = client.DownloadString("http://example.com");*/
            //ViewBag.result = content;
        }

        public ActionResult iFrame()
        {
            //ViewBag.URL = "";
            return View();
        }
        [HttpPost]
        public ActionResult iFrame(string URL)
        {
            ViewBag.URL = URL;
            return View();
        }

        private string ConvertURL(string stringInput, string URL)
        {
            string[] stringArraySeprators = { "//", "/" };
            string[] stringArray = URL.Split(stringArraySeprators, StringSplitOptions.RemoveEmptyEntries);
            string stringTopURL = stringArray[0] + "//" + stringArray[1];

            int i = 0;
            while ((i + 15) < stringInput.Length)
            {
                if (((stringInput.Substring(i, 5) == "src='") || (stringInput.Substring(i, 5) == "src=\"")) && (stringInput.Substring(i + 5, 4) != "http"))
                {
                    if (stringInput[i + 5] == '/')
                    {
                        stringInput = stringInput.Insert(i + 5, stringTopURL);
                    }
                    else
                    {
                        stringInput = stringInput.Insert(i + 5, stringTopURL + '/');
                    }
                }
                else if (((stringInput.Substring(i, 6) == "herf='") || (stringInput.Substring(i, 6) == "href=\"")) && (stringInput.Substring(i + 6, 4) != "http"))
                {
                    if (stringInput[i + 6] == '/')
                    {
                        stringInput = stringInput.Insert(i + 6, stringTopURL);
                    }
                    else
                    {
                        stringInput = stringInput.Insert(i + 6, stringTopURL + '/');
                    }
                }
                i++;
            }
            return stringInput;
        }
        public ActionResult Clock()
        {
            return View();
        }
        public ActionResult SlideShow()
        {
            return View();
        }
        public ActionResult SlideShowBootstrap()
        {
            return View();
        }
        public ActionResult Video()
        {
            return View();
        }
        public ActionResult Weather()
        {
            return View();
        }

        public ActionResult BootstrapModal()
        {
            return View();
        }

        public ActionResult BootstrapModalAjax()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BootstrapModalAjax(string Text)
        {
            string JsonResult = string.Format("Your text is: {0}", Text);
            if (!Request.IsAjaxRequest())
                return View();
            else
                return Json(JsonResult);
        }

        public ActionResult jQueryCycle()
        {
            return View();
        }


        //---------------------
        //Test Actions
        //---------------------



        public ActionResult Test1()
        {
            ViewBag.test1 = "Test1";
            return View();
        }
        public ActionResult Test2()
        {
            return View();
        }
        public ActionResult Test3()
        {
            return View();
        }
        public ActionResult Test4()
        {
            return View();
        }
        public ActionResult Test5()
        {
            return View();
        }
    }
}