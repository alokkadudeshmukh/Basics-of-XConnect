using Glass.Mapper.Sc;
//using Sitecore.Analytics;
//using Sitecore.Analytics.Model.Entities;
//using Sitecore.Analytics.Tracking;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XConnect.Demo.Contacts.Web.Models;

namespace XConnect.Demo.Contacts.Web.Controllers
{
    public class PageController : Controller
    {
        #region Properties
        private ISitecoreContext _context { get; set; }
        public static string contactIdentifierPrefix = "";
        #endregion
        // GET: Page
        public ActionResult Index()
        {
            return View();
        }

        public PageController()
        {
           
        }

        public PageController(ISitecoreContext Context)
        {
            _context = Context;
        }

        [HttpGet]
        public ActionResult CreateContacts()
        {
            CreateContactViewModel contactVM = new CreateContactViewModel
            {
                PageTitle = "Contacts Creation",
                PageIntro = "Use this page to create contacts in Experience Profile"
            };
            return View("~/Views/Page/Contacts/CreateContact.cshtml", contactVM);
        }

        [HttpPost]
        public JsonResult CreateContacts(CreateContactViewModel contactVM)
        {
            // TriggerProfileKey("ProfileNAME", "ProfileKEY", 10);
           contactVM= RegisterContact(contactVM);
            // return View("~/Views/Page/Contacts/CreateContact.cshtml",contactVM);
            return Json(new { Message = contactVM.OperationStatus }, JsonRequestBehavior.AllowGet);
            
        }

        public static CreateContactViewModel RegisterContact(CreateContactViewModel contactVM)
        {
            CreateContactViewModel conctVMLocal = contactVM;
            // var offlineGoal = Guid.Parse("A9948719-E6E4-46D2-909B-3680E724ECE9");//offline goal - KioskSubmission goal          
            // var channelId = Guid.Parse("3FC61BB8-0D9F-48C7-9BBD-D739DCBBE032"); // /sitecore/system/Marketing Control Panel/Taxonomies/Channel/Offline/Store/Enter store - offline enter storl channel
            var goal = Guid.Parse("D59F316C-8C87-44A9-8347-E66C5A996CF5"); //online goal- directsitesubmission goal
            var channelId = Guid.Parse("B418E4F2-1013-4B42-A053-B6D4DCA988BF"); // /sitecore/system/Marketing Control Panel/Taxonomies/Channel/Online/Direct/Direct - online direct channel
            CertificateWebRequestHandlerModifierOptions options =
               CertificateWebRequestHandlerModifierOptions.Parse("StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue=587d948806e57cf511b37a447a2453a02dfd3686");

            // Optional timeout modifier
            var certificateModifier = new CertificateWebRequestHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            // This overload takes three client end points - collection, search, and configuration
            var collectionClient = new CollectionWebApiClient(new Uri("https://sc9.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri("https://sc9.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri("https://sc9.xconnect/configuration"), clientModifiers, new[] { certificateModifier });



            var config = new XConnectClientConfiguration(
                new XdbRuntimeModel(CollectionModel.Model), collectionClient, searchClient, configurationClient);

            config.Initialize();


            using (Sitecore.XConnect.Client.XConnectClient client = new XConnectClient(config))
            {
                bool isExist = false;
                Contact extContact = client.Get<Contact>(new IdentifiedContactReference("HIxConnect", contactIdentifierPrefix + contactVM.FormEmailAddress), new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey, EmailAddressList.DefaultFacetKey }));
                if (extContact != null)
                {
                    isExist = true;
                }

                try
                {
                    // Identifier for a 'known' contact
                    var identifier = new ContactIdentifier[]
                    {
                                     new ContactIdentifier("HIxConnect",contactIdentifierPrefix + contactVM.FormEmailAddress, ContactIdentifierType.Known)
                    };
                    Contact knownContact = null;
                    if (isExist)
                    {
                        knownContact = extContact;
                    }
                    else
                    {
                        // Create a new contact with the identifier
                        knownContact = new Contact(identifier);
                        client.AddContact(knownContact);
                    }
                    //Persona information facet
                    PersonalInformation personalInfoFacet = new PersonalInformation();

                    personalInfoFacet.FirstName = contactVM.FormFirstName;
                    personalInfoFacet.LastName = contactVM.FormLastName;
                    personalInfoFacet.JobTitle = contactVM.FormJobTitle;

                    client.SetFacet<PersonalInformation>(knownContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                    Interaction interaction = null;
                    if (!isExist)
                    {

                        // Create a new interaction for that contact
                        interaction = new Interaction(knownContact, InteractionInitiator.Contact, channelId, "");

                        // Add events - all interactions must have at least one event
                        var xConnectEvent = new Goal(goal, DateTime.UtcNow);
                        interaction.Events.Add(xConnectEvent);

                    }
                    #region EmailAddress Facet
                    EmailAddressList emails = new EmailAddressList(new EmailAddress(contactVM.FormEmailAddress, true), EmailAddressList.DefaultFacetKey);
                    //OR the following code
                    //var emails = existingContact.GetFacet<EmailAddressList>(EmailAddressList.DefaultFacetKey);
                    //emails.PreferredEmail = new EmailAddress("myrtle@test.test", true);
                    client.SetFacet<EmailAddressList>(knownContact, EmailAddressList.DefaultFacetKey, emails);
                    #endregion

                    IpInfo ipInfo = new IpInfo("127.0.0.1");

                    ipInfo.BusinessName = "WebSiteSitecore";

                    client.SetFacet<IpInfo>(interaction, IpInfo.DefaultFacetKey, ipInfo);

                    // Submit contact and interaction - a total of two operations
                    if (!isExist)
                    {
                        // Add the contact and interaction
                        client.AddInteraction(interaction);
                    }
                    client.Submit();

                    var operations = client.LastBatch;
                    contactVM.OperationStatus = "created";
                   
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    conctVMLocal.OperationStatus = "false";
                }
                catch (Exception ex)
                {
                    conctVMLocal.OperationStatus = "false";
                }
                return conctVMLocal;

            }
        }        


    }
}