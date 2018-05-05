using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XConnect.Demo.Contacts.Web.Models
{
    public class CreateContactViewModel
    {
        public string PageTitle { get; set; }
        public string PageIntro { get; set; }
        public string FormFirstName { get; set; }
        public string FormLastName { get; set; }
        public string FormJobTitle { get; set; }
        public string FormEmailAddress { get; set; }
        public string OperationStatus { get; set; }
        
    }
}