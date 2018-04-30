using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Hosting;
using Newtonsoft.Json;
using System.IO;

namespace KioskContact
{
    /// <summary>
    /// Summary description for ImageService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ImageService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public static string GetCapturedImage(string basestring)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(basestring);
                System.Drawing.Image imageFile;
                using (MemoryStream mst = new MemoryStream(imageBytes))
                {
                    imageFile = System.Drawing.Image.FromStream(mst);
                }
                imageFile.Save(HostingEnvironment.MapPath("~/Captures/" + DateTime.Now.ToString("dd-MM-yy-hh-mm-ss") + ".png"));
                return "success";

            }
            catch (Exception excp)
            {
                return "failure";
            }
        }
    }
}
