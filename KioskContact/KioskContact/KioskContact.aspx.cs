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


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    lbl_status.Text = string.Empty;

            //    #region Camera
            //    if (Request.InputStream.Length > 0)
            //    {
            //        using (StreamReader reader = new StreamReader(Request.InputStream))
            //        {
            //            string hexString = Server.UrlEncode(reader.ReadToEnd());
            //            string imageName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");                        
            //            string imagePath = string.Format("~/Captures/{0}.png", imageName);
            //            userImage = imagePath;
            //            File.WriteAllBytes(Server.MapPath(imagePath), ConvertHexToBytes(hexString));
            //            Session["CapturedImage"] = ResolveUrl(imagePath);
            //        }
            //    }
            //    #endregion
            //}
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
        //private static byte[] ConvertHexToBytes(string hex)
        //{
        //    byte[] bytes = new byte[hex.Length / 2];
        //    for (int i = 0; i < hex.Length; i += 2)
        //    {
        //        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        //    }
        //    return bytes;
        //}

        //[WebMethod(EnableSession = true)]
        //public static string GetCapturedImage()
        //{
        //    string url = HttpContext.Current.Session["CapturedImage"].ToString();
        //    HttpContext.Current.Session["CapturedImage"] = null;
        //    return url;
        //}

        public void Example1()
        {
            var config = new XConnectClientConfiguration(new XdbRuntimeModel(CollectionModel.Model), new Uri("https://sc9.xconnect"), new Uri("https://sc9.xconnect"));
            config.Initialize();
            using (Sitecore.XConnect.Client.XConnectClient client = new XConnectClient(config))
            {
                {
                    try
                    {
                        var contact = new Sitecore.XConnect.Contact(
                            new Sitecore.XConnect.ContactIdentifier("twitter", "myrtlesitecore", Sitecore.XConnect.ContactIdentifierType.Known)
                            );

                        client.AddContactIdentifier(contact, new Sitecore.XConnect.ContactIdentifier("ad-network", "ABC123456", Sitecore.XConnect.ContactIdentifierType.Anonymous));

                        IReadOnlyCollection<Sitecore.XConnect.ContactIdentifier> identifiers = contact.Identifiers;

                        client.Submit();
                    }
                    catch (XdbExecutionException ex)
                    {
                        // Manage exception
                    }
                }
            }
        }

        public void Example4()
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
                    var existingContact = client.Get<Contact>(new IdentifiedContactReference("twitter", "demo" + txtEmailAddress.Text), new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey, EmailAddressList.DefaultFacetKey }));
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
                                    //new ContactIdentifier("twitter", "myrtlesitecore" + Guid.NewGuid().ToString("N"), ContactIdentifierType.Known)
                                  //   new ContactIdentifier("twitter", "demo" + txtEmailAddress.Text, ContactIdentifierType.Known)
                                     new ContactIdentifier("twitter", "demo" + txtEmailAddress.Text, ContactIdentifierType.Known)
                        };
                        // Create a new contact with the identifier
                        Contact knownContact = new Contact(identifier);
                        client.AddContact(knownContact);
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


                        var imagebasestring = "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";
                        var tempUserImage = Convert.FromBase64String(imagebasestring);

                        var userAvatar = new Avatar("image/jpeg", userImage);
                        client.SetAvatar(knownContact, userAvatar);

                        

                        #region IPInfo Facet
                        //IpInfo ipInfo = new IpInfo("127.0.0.1");
                        //ipInfo.BusinessName = "Kiosk Desk";
                        //client.SetFacet<IpInfo>(knownContact, IpInfo.DefaultFacetKey, ipInfo);
                        #endregion
                        //Add interaction to client
                        client.AddInteraction(interaction);
                    }
                    client.Submit();

                    //Display Purpose - Operations display
                    var operations = client.LastBatch;
                    // Loop through operations and check status
                    foreach (var operation in operations)
                    {
                        lbl_status.Text += operation.OperationType + operation.Target.GetType().ToString() + " Operation: " + operation.Status + "<br/>";
                    }
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    lbl_status.Text += "Error Occured while Xdb Execution:" + ex.Message + "<br/>" + ex.StackTrace;
                }
                catch (Exception ex)
                {
                    lbl_status.Text += "Error Occured:" + ex.Message + "<br/>" + ex.StackTrace;
                }


                //try
                //{
                //    // Identifier for a 'known' contact
                //    var identifier = new ContactIdentifier[]
                //    {
                //                //new ContactIdentifier("twitter", "myrtlesitecore" + Guid.NewGuid().ToString("N"), ContactIdentifierType.Known)
                //                 new ContactIdentifier("twitter", "demo" + txtEmailAddress.Text, ContactIdentifierType.Known)
                //    };
                //    Contact knownContact = null;
                //    if (isExist)
                //    {
                //        knownContact = existingContact;
                //    }
                //    else
                //    {
                //        // Create a new contact with the identifier
                //        knownContact = new Contact(identifier);
                //        client.AddContact(knownContact);
                //    }
                //    //Persona information facet
                //    PersonalInformation personalInfoFacet = new PersonalInformation();

                //    personalInfoFacet.FirstName = txtFName.Text;
                //    personalInfoFacet.LastName = txtLname.Text;
                //    personalInfoFacet.JobTitle = "Programmer Writer";

                //    client.SetFacet<PersonalInformation>(knownContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                //    Interaction interaction = null;
                //    if (!isExist)
                //    {

                //        // Create a new interaction for that contact
                //        interaction = new Interaction(knownContact, InteractionInitiator.Contact, channelId, "");

                //        // Add events - all interactions must have at least one event
                //        var xConnectEvent = new Goal(offlineGoal, DateTime.UtcNow);
                //        interaction.Events.Add(xConnectEvent);

                //    }
                //    EmailAddressList emails = new EmailAddressList(new EmailAddress(txtEmailAddress.Text, true), EmailAddressList.DefaultFacetKey);

                //    client.SetFacet<EmailAddressList>(knownContact, EmailAddressList.DefaultFacetKey, emails);


                //    //IpInfo ipInfo = new IpInfo("127.0.0.1");

                //    //ipInfo.BusinessName = "Kiosk Desk";

                //    //client.SetFacet<IpInfo>(knownContact, IpInfo.DefaultFacetKey, ipInfo);

                //    // Submit contact and interaction - a total of two operations
                //    if (!isExist)
                //    {
                //        // Add the contact and interaction
                //        client.AddInteraction(interaction);
                //    }
                //    client.Submit();

                //    var operations = client.LastBatch;



                //    // Loop through operations and check status
                //    foreach (var operation in operations)
                //    {
                //        lbl_status.Text += operation.OperationType + operation.Target.GetType().ToString() + " Operation: " + operation.Status + "<br/>";
                //    }
                //}
                //catch (XdbExecutionException ex)
                //{
                //    // Manage exception
                //    lbl_status.Text += "Error Occured:" + ex.Message + "<br/>" + ex.StackTrace;
                //}
                //catch (Exception ex)
                //{
                //    lbl_status.Text += "Error Occured:" + ex.Message + "<br/>" + ex.StackTrace;
                //}

            }
            }
        //Only Creates new contact
        public void Example3()
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
                                //new ContactIdentifier("twitter", "myrtlesitecore" + Guid.NewGuid().ToString("N"), ContactIdentifierType.Known)
                                 new ContactIdentifier("twitter", "demo" + txtEmailAddress.Text, ContactIdentifierType.Known)
                        };
                    
                        // Create a new contact with the identifier
                        Contact knownContact = new Contact(identifier);
                        client.AddContact(knownContact);
                    
                    //Persona information facet
                    PersonalInformation personalInfoFacet = new PersonalInformation();

                        personalInfoFacet.FirstName = txtFName.Text;
                        personalInfoFacet.LastName = txtLname.Text;
                        personalInfoFacet.JobTitle = txtJobRole.Text;

                        client.SetFacet<PersonalInformation>(knownContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);



                    // Create a new interaction for that contact
                    Interaction interaction = new Interaction(knownContact, InteractionInitiator.Contact, channelId, "");

                        // Add events - all interactions must have at least one event
                        var xConnectEvent = new Goal(offlineGoal, DateTime.UtcNow);
                        interaction.Events.Add(xConnectEvent);
                       
                    
                    EmailAddressList emails = new EmailAddressList(new EmailAddress(txtEmailAddress.Text, true),EmailAddressList.DefaultFacetKey);

                    client.SetFacet<EmailAddressList>(knownContact,EmailAddressList.DefaultFacetKey,emails);


                    
                    // Submit contact and interaction - a total of two operations
                    
                        // Add the contact and interaction
                        client.AddInteraction(interaction);
                    
                        client.Submit();

                        var operations = client.LastBatch;

                        

                        // Loop through operations and check status
                        foreach (var operation in operations)
                        {
                            lbl_status.Text += operation.OperationType + operation.Target.GetType().ToString() + " Operation: " + operation.Status + "<br/>";
                        }
                    }
                    catch (XdbExecutionException ex)
                    {
                        // Manage exception
                        lbl_status.Text += "Error Occured:" + ex.Message + "<br/>" + ex.StackTrace;
                    }
                    catch (Exception ex)
                    {
                        lbl_status.Text += "Error Occured:" + ex.Message + "<br/>" + ex.StackTrace;
                    }
                
            }
        }

        public void Example()
        {

            //XdbModelBuilder modelBuilder = new XdbModelBuilder("Sitecore.XConnect.Collection.Model", new XdbModelVersion(9,0));
            //modelBuilder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);

            //var config=new XConnectClientConfiguration(modelBuilder.BuildModel(), new Uri("https://sc9.xconnect"), new Uri("https://sc9.xconnect"));

            var config = new XConnectClientConfiguration(new XdbRuntimeModel(CollectionModel.Model), new Uri("https://sc9.xconnect"), new Uri("https://sc9.xconnect"));
            config.Initialize();
            using (Sitecore.XConnect.Client.XConnectClient client = new XConnectClient(config))
            {
                try
                {
                    //Contact extcontact = client.Get<Contact>(new IdentifiedContactReference("twitter1", "myrtlesitecore1"), new ContactExpandOptions(PersonalInformation.DefaultFacetKey));

                    //if (extcontact.GetFacet<PersonalInformation>(PersonalInformation.DefaultFacetKey) == null)
                    //{
                    //    // Only create new facet if one does not already exist
                    //    PersonalInformation personalInfoFacet = new PersonalInformation()
                    //    {
                    //        FirstName = "Myrtle",
                    //        LastName = "McSitecore"
                    //    };

                    //    client.SetFacet(extcontact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                    //    client.Submit();
                    //}
                    //else
                    {

                        Contact contact = new Contact(new ContactIdentifier("twitter2", "myrtlesitecore2", ContactIdentifierType.Known));



                        // Facet with a reference object, key is specified
                        PersonalInformation personalInfoFacet = new PersonalInformation()
                        {
                            FirstName = "Myrtle",
                            LastName = "McSitecore"
                        };

                        FacetReference reference = new FacetReference(contact, PersonalInformation.DefaultFacetKey);

                        //client.SetFacet<PersonalInformation>(reference, personalInfoFacet);
                        //OR
                        client.SetPersonal(contact, personalInfoFacet);

                        //// Facet without a reference, using default key
                        //EmailAddressList emails = new EmailAddressList(new EmailAddress(txtEmailAddress.Text, true), "Home");

                        //client.SetFacet(contact, emails);

                        //// Facet without a reference, key is specified

                        //AddressList addresses = new AddressList(new Address() { AddressLine1 = "Cool Street 12", City = "Sitecore City", PostalCode = "ABC 123" }, "Home");

                        //client.SetFacet(contact, AddressList.DefaultFacetKey, addresses);
                        client.AddContact(contact);
                        // Submit operations as batch
                        client.Submit();
                    }
                }
                catch (XdbExecutionException ex)
                {

                    // Manage exception
                }
            }
        }


        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Example4();
            //Example1();
            //var config = new XConnectClientConfiguration(new XdbRuntimeModel(CollectionModel.Model), new Uri("https://sc9.xconnect"), new Uri("https://sc9.xconnect"));

            //try
            //{
            //    config.Initialize();
            //}
            //catch (XdbModelConflictException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return;
            //}

            //using (var client = new XConnectClient(config))
            //{
            //    //var identifier = new ContactIdentifier("twitter", "longhorntaco", ContactIdentifierType.Known);

            //    var identifiers = new ContactIdentifier[]
            //    {
            //    new ContactIdentifier("twitter", "longhorntaco", ContactIdentifierType.Known),
            //    new ContactIdentifier("domain", "longhorn.taco", ContactIdentifierType.Known)
            //    };
            //    var contact = new Contact(identifiers);
            //    var personalInformationFacet = new PersonalInformation
            //    {
            //        FirstName = txtFName.Text,
            //        LastName = txtLname.Text
            //        //Gender = "Male"
            //    };
            //    client.SetFacet<PersonalInformation>(contact, PersonalInformation.DefaultFacetKey, personalInformationFacet);

            //    var emailFacet = new EmailAddressList(new EmailAddress(txtEmailAddress.Text, true), "twitter");
            //    client.SetFacet<EmailAddressList>(contact, EmailAddressList.DefaultFacetKey, emailFacet);

            //    client.AddContact(contact);

            //    client.Submit();
            //}


            //using (Sitecore.XConnect.Client.XConnectClient client = new XConnectClient(config)
            //{
            //    try
            //    {
            //        var webContactIdentifier = Tracker.Current.Contact.Identifiers.FirstOrDefault(t => t.Source == "corporateweb")?.Identifier;
            //        var existingContact = client.Get<Sitecore.XConnect.Contact>(new IdentifiedContactReference("corporateweb", webContactIdentifier), new Sitecore.XConnect.ContactExpandOptions(PersonalInformation.DefaultFacetKey));

            //        if (existingContact != null)
            //        {
            //            var personalFacet = existingContact.GetFacet<PersonalInformation>() ?? new PersonalInformation();

            //            personalFacet.FirstName = "Martin";
            //            personalFacet.LastName = "English";

            //            client.SetFacet(existingContact, PersonalInformation.DefaultFacetKey, personalFacet);

            //            client.Submit();
            //        }
            //    }
            //    catch (XdbExecutionException ex)
            //    {
            //        //Oops, something went wrong  
            //    }
            //}

        }

        protected void btn_capture_Click(object sender, EventArgs e)
        {
            byte[] imageBytes = Convert.FromBase64String(hdnbase.Value);
            //lbl_status.Text = hdnbase.Value;
            //Response.Write(hdnbase.Value);

            //string imageName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
            //string imagePath = string.Format("~/Captures/{0}.png", imageName);
            //File.WriteAllBytes(Server.MapPath(imagePath), imageBytes);

            //int length = imageBytes.Length;
            ////MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            //////ms.Write(imageBytes, 0,length);
            //MemoryStream ms = new MemoryStream(imageBytes);
            //ms.Seek(0, SeekOrigin.Begin);
            //System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            //image.Save(Server.MapPath("~/Captures/A"+ DateTime.Now.ToString("dd-MM-yy hh-mm-ss"+".png")));


            System.Drawing.Image imageFile;
            using (MemoryStream mst = new MemoryStream(imageBytes))
            {
                imageFile = System.Drawing.Image.FromStream(mst);
            }
            imageFile.Save(Server.MapPath("~/Captures/A" + DateTime.Now.ToString("dd-MM-yy hh-mm-ss")+ ".png"));

        }


    }
}