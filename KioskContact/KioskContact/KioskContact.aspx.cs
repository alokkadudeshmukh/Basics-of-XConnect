using Newtonsoft.Json;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KioskContact
{
    public partial class KioskContact : System.Web.UI.Page
    {
        public static byte[] userImage { get; set; }
        public static string contactIdentifierPrefix = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                ltrl_status.Text = "";
            }
        }

        [System.Web.Services.WebMethod]
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
                userImage = imageBytes;
                return "success";

            }
            catch (Exception excp)
            {
                return "failure";
            }
        }
        //Blog Code
        public void createUpdateContact()
        {
            //Certificate
            CertificateWebRequestHandlerModifierOptions options =
            CertificateWebRequestHandlerModifierOptions.Parse("StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue=587d948806e57cf511b37a447a2453a02dfd3686");
            var certificateModifier = new CertificateWebRequestHandlerModifier(options);

            //Model - xConnect Client Configuration
            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            // This overload takes three client end points - collection, search, and configuration
            var collectionClient = new CollectionWebApiClient(new Uri("https://sc9.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri("https://sc9.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri("https://sc9.xconnect/configuration"), clientModifiers, new[] { certificateModifier });

            var config = new XConnectClientConfiguration(
                new XdbRuntimeModel(CollectionModel.Model), collectionClient, searchClient, configurationClient);
            //initialize the configuration
            config.Initialize();

            //create the xConnect Client
            using (Sitecore.XConnect.Client.XConnectClient client = new XConnectClient(config))
            {
                // Identifier
                var identifier = new ContactIdentifier[]
                {
                    new ContactIdentifier("HIxConnect", txtEmailAddress.Text, ContactIdentifierType.Known)
                };

                //Contact & Facets                
                // Create a new contact with the identifier
                Contact knownContact = new Contact(identifier);
                client.AddContact(knownContact);

                //Facets
                #region Personal Information Facet
                //Persona information facet
                PersonalInformation personalInfoFacet = new PersonalInformation();
                personalInfoFacet.Title = ddTitle.SelectedValue;
                personalInfoFacet.FirstName = "Alok";
                personalInfoFacet.MiddleName = "S";
                personalInfoFacet.LastName = "KaduDeshmukh";
                personalInfoFacet.PreferredLanguage = "en";
                personalInfoFacet.Gender = "Male";
                personalInfoFacet.JobTitle = "Sitecore Web Developer";
                client.SetFacet<PersonalInformation>(knownContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);
                #endregion
                #region EmailAddress Facet
                EmailAddressList emails = new EmailAddressList(new EmailAddress("alok.kadudeshmukh4@gmail.com", true), EmailAddressList.DefaultFacetKey);
                //OR the following code
                //var emails = existingContact.GetFacet<EmailAddressList>(EmailAddressList.DefaultFacetKey);
                //emails.PreferredEmail = new EmailAddress("alok.kadudeshmukh@gmail.com", true);
                client.SetFacet<EmailAddressList>(knownContact, EmailAddressList.DefaultFacetKey, emails);
                #endregion

                //Interaction
                var offlineGoal = Guid.Parse("A9948719-E6E4-46D2-909B-3680E724ECE9");//offline goal - KioskSubmission goal
                var channelId = Guid.Parse("3FC61BB8-0D9F-48C7-9BBD-D739DCBBE032"); // /sitecore/system/Marketing Control Panel/Taxonomies/Channel/Offline/Store/Enter store - offline enter storl channel
                //Create a new interaction for that contact
                Interaction interaction = new Interaction(knownContact, InteractionInitiator.Contact, channelId, "");
                // Add events - all interactions must have at least one event
                var xConnectEvent = new Goal(offlineGoal, DateTime.UtcNow);
                interaction.Events.Add(xConnectEvent);
                //Add interaction to client
                client.AddInteraction(interaction);

                client.Submit();
            }
        }
        //Demo Code
        public void createUpdateContactDemo()
        {
            var offlineGoal = Guid.Parse("A9948719-E6E4-46D2-909B-3680E724ECE9");//offline goal - KioskSubmission goal
            var channelId = Guid.Parse("3FC61BB8-0D9F-48C7-9BBD-D739DCBBE032"); // /sitecore/system/Marketing Control Panel/Taxonomies/Channel/Offline/Store/Enter store - offline enter storl channel

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
                try
                {
                    bool isExist = false;
                    var existingContact = client.Get<Contact>(new IdentifiedContactReference("HIxConnect", contactIdentifierPrefix + txtEmailAddress.Text), new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey, EmailAddressList.DefaultFacetKey }));
                    if (existingContact != null)
                    {

                        var personalInfoFacet = existingContact.GetFacet<PersonalInformation>(PersonalInformation.DefaultFacetKey);
                        if (personalInfoFacet != null)
                        {
                            personalInfoFacet.FirstName = txtFName.Text;
                            personalInfoFacet.LastName = txtLname.Text;
                            personalInfoFacet.MiddleName = txtMName.Text;
                            personalInfoFacet.PreferredLanguage = ddLanguage.SelectedValue;
                            personalInfoFacet.Title = ddTitle.SelectedValue;
                            personalInfoFacet.Gender = ddGender.SelectedValue;
                            personalInfoFacet.JobTitle = txtJobRole.Text;
                            client.SetFacet<PersonalInformation>(existingContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);
                        }

                    }
                    else
                    {
                        // Identifier for a 'known' contact
                        var identifier = new ContactIdentifier[]
                        {
                                     new ContactIdentifier("HIxConnect", contactIdentifierPrefix + txtEmailAddress.Text, ContactIdentifierType.Known)
                        };
                        // Create a new contact with the identifier
                        Contact knownContact = new Contact(identifier);
                        client.AddContact(knownContact);
                        #region Personal Information Facet
                        //Persona information facet
                        PersonalInformation personalInfoFacet = new PersonalInformation();
                        personalInfoFacet.FirstName = txtFName.Text;
                        personalInfoFacet.LastName = txtLname.Text;
                        personalInfoFacet.MiddleName = txtMName.Text;
                        personalInfoFacet.PreferredLanguage = ddLanguage.SelectedValue;
                        personalInfoFacet.Title = ddTitle.SelectedValue;
                        personalInfoFacet.Gender = ddGender.SelectedValue;
                        personalInfoFacet.JobTitle = txtJobRole.Text;
                        client.SetFacet<PersonalInformation>(knownContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);
                        #endregion

                        //Create a new interaction for that contact
                        Interaction interaction = new Interaction(knownContact, InteractionInitiator.Contact, channelId, "");
                        // Add events - all interactions must have at least one event
                        var xConnectEvent = new Goal(offlineGoal, DateTime.UtcNow);
                        interaction.Events.Add(xConnectEvent);

                        #region EmailAddress Facet
                        EmailAddressList emails = new EmailAddressList(new EmailAddress(txtEmailAddress.Text, true), EmailAddressList.DefaultFacetKey);
                        //OR the following code
                        //var emails = existingContact.GetFacet<EmailAddressList>(EmailAddressList.DefaultFacetKey);
                        //emails.PreferredEmail = new EmailAddress("myrtle@test.test", true);
                        client.SetFacet<EmailAddressList>(knownContact, EmailAddressList.DefaultFacetKey, emails);
                        #endregion


                        #region Set Avatar
                        var userAvatar = new Avatar("image/jpeg", userImage);
                        client.SetAvatar(knownContact, userAvatar);
#endregion


                        #region IPInfo Facet
                        //IpInfo ipInfo = new IpInfo("127.0.0.1");
                        //ipInfo.BusinessName = "Kiosk Desk";
                        //client.SetFacet<IpInfo>(knownContact, IpInfo.DefaultFacetKey, ipInfo);
                        #endregion
                        //Add interaction to client
                        client.AddInteraction(interaction);
                    }
                    client.Submit();

                    #region  Display Purpose - Operations display
                    //var operations = client.LastBatch;
                    //// Loop through operations and check status
                    //foreach (var operation in operations)
                    //{
                    //    ltrl_status.Text += operation.OperationType + operation.Target.GetType().ToString() + " Operation: " + operation.Status + "<br/>";
                    //}
                    // ltrl_status.Text= "<p class=\"success\">Thank you for sharing your contact details!</p>";
                    #endregion
                    ltrl_status.Text = "Thank you for sharing your details";
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    //ltrl_status.Text += "Error Occured while Xdb Execution:" + ex.Message + "<br/>" + ex.StackTrace;
                    ltrl_status.Text = " <p style=\"color:red;\">Something went wrong!</p>";
                }
                catch (Exception ex)
                {
                    //ltrl_status.Text += "Error Occured:" + ex.Message + "<br/>" + ex.StackTrace;
                    ltrl_status.Text = " <p style=\"color:red;\">Something went wrong!</p>";
                }




            }
        }
        //Only Creates new contact
        public void createContact()
        {
            //Goals and Channels to tracke the event - interaction
            var offlineGoal = Guid.Parse("A9948719-E6E4-46D2-909B-3680E724ECE9");//offline goal - KioskSubmission goal
            var channelId = Guid.Parse("3FC61BB8-0D9F-48C7-9BBD-D739DCBBE032"); // /sitecore/system/Marketing Control Panel/Taxonomies/Channel/Offline/Store/Enter store - offline enter storl channel

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
                try
                {
                    // Identifier for a 'known' contact
                    var identifier = new ContactIdentifier[]
                    {
                                 new ContactIdentifier("HIxConnect", contactIdentifierPrefix + txtEmailAddress.Text, ContactIdentifierType.Known)
                    };

                    // Create a new contact with the identifier
                    Contact knownContact = new Contact(identifier);
                    client.AddContact(knownContact);

                    
                    #region Personal Information Facet
                    //Persona information facet
                    PersonalInformation personalInfoFacet = new PersonalInformation();
                    personalInfoFacet.FirstName = txtFName.Text;
                    personalInfoFacet.LastName = txtLname.Text;
                    personalInfoFacet.MiddleName = txtMName.Text;
                    personalInfoFacet.PreferredLanguage = ddLanguage.SelectedValue;
                    personalInfoFacet.Title = ddTitle.SelectedValue;
                    personalInfoFacet.Gender = ddGender.SelectedValue;
                    personalInfoFacet.JobTitle = txtJobRole.Text;
                    client.SetFacet<PersonalInformation>(knownContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);
                    #endregion

                    #region Email Address Facet
                    EmailAddressList emails = new EmailAddressList(new EmailAddress(txtEmailAddress.Text, true), EmailAddressList.DefaultFacetKey);
                    client.SetFacet<EmailAddressList>(knownContact, EmailAddressList.DefaultFacetKey, emails);
                    #endregion

                    #region Set Avatar
                    var userAvatar = new Avatar("image/jpeg", userImage);
                    client.SetAvatar(knownContact, userAvatar);
                    #endregion

                    #region Interaction and Event

                    // Create a new interaction for that contact
                    Interaction interaction = new Interaction(knownContact, InteractionInitiator.Contact, channelId, "");

                    // Add events - all interactions must have at least one event
                    var xConnectEvent = new Goal(offlineGoal, DateTime.UtcNow);
                    interaction.Events.Add(xConnectEvent);
                    #endregion


                    // Submit contact and interaction - a total of two operations

                    // Add the contact and interaction
                    client.AddInteraction(interaction);

                    client.Submit();
                    #region Displaying Operations
                    var operations = client.LastBatch;



                    // Loop through operations and check status
                    foreach (var operation in operations)
                    {
                        ltrl_status.Text += operation.OperationType + operation.Target.GetType().ToString() + " Operation: " + operation.Status + "<br/>";
                    }
                    #endregion
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    ltrl_status.Text += "Error Occured:" + ex.Message + "<br/>" + ex.StackTrace;
                }
                catch (Exception ex)
                {
                    ltrl_status.Text += "Error Occured:" + ex.Message + "<br/>" + ex.StackTrace;
                }

            }
        }

      

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            createUpdateContactDemo();
            //createUpdateContact();
        }

        protected void btn_capture_Click(object sender, EventArgs e)
        {
            byte[] imageBytes = Convert.FromBase64String(hdnbase.Value);         


            System.Drawing.Image imageFile;
            using (MemoryStream mst = new MemoryStream(imageBytes))
            {
                imageFile = System.Drawing.Image.FromStream(mst);
            }
            imageFile.Save(Server.MapPath("~/Captures/A" + DateTime.Now.ToString("dd-MM-yy hh-mm-ss") + ".png"));

        }


    }
}