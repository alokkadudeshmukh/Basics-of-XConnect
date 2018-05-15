using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsConsoleApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("");
            Console.WriteLine("END OF PROGRAM.");
            Console.ReadKey();
        }

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine(" ");

            CertificateWebRequestHandlerModifierOptions options =
           CertificateWebRequestHandlerModifierOptions.Parse("StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue=587d948806e57cf511b37a447a2453a02dfd3686");

            var certificateModifier = new CertificateWebRequestHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            var collectionClient = new CollectionWebApiClient(new Uri("https://sc9.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri("https://sc9.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri("https://sc9.xconnect/configuration"), clientModifiers, new[] { certificateModifier });

            var cfg = new XConnectClientConfiguration(
                new XdbRuntimeModel(CollectionModel.Model), collectionClient, searchClient, configurationClient);

            try
            {
                cfg.Initialize(); // cfg.InitializeAsync();
                Console.WindowWidth = 160;

                var arr1 = new[]
               {
                            @"                                                                                ",
                            @" __    __  _____                                                                ",
                            @"\  \  \ \ \     \                                                               ",
                            @"|##|  |## |######                                                               ",
                            @"|##|  |##  \ ##                                                                 ",
                            @"|##| _|##   |##                                                                 ",
                            @"|########|  |##                                                                 ",
                            @"|##|   ##   |##|                                                                ",
                            @"|##|   ##   |## \                                                               ",
                            @"|##|   ## |######|                                                              "
                        };
                foreach (string line in arr1)
                    Console.WriteLine(line);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                // Print xConnect if configuration is valid
                var arr = new[]
                {
                            @"            ______                                                       __     ",
                            @"           /      \                                                     |  \    ",
                            @" __    __ |  $$$$$$\  ______   _______   _______    ______    _______  _| $$_   ",
                            @"|  \  /  \| $$   \$$ /      \ |       \ |       \  /      \  /       \|   $$ \  ",
                            @"\$$\/  $$| $$      |  $$$$$$\| $$$$$$$\| $$$$$$$\|  $$$$$$\|  $$$$$$$ \$$$$$$   ",
                            @" >$$  $$ | $$   __ | $$  | $$| $$  | $$| $$  | $$| $$    $$| $$        | $$ __  ",
                            @" /  $$$$\ | $$__/  \| $$__/ $$| $$  | $$| $$  | $$| $$$$$$$$| $$_____   | $$|  \",
                            @"|  $$ \$$\ \$$    $$ \$$    $$| $$  | $$| $$  | $$ \$$     \ \$$     \   \$$  $$",
                            @" \$$   \$$  \$$$$$$   \$$$$$$  \$$   \$$ \$$   \$$  \$$$$$$$  \$$$$$$$    \$$$$ "
                        };
               
                foreach (string line in arr)
                    Console.WriteLine(line);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

            }
            catch (XdbModelConflictException ce)
            {
                Console.WriteLine("ERROR:" + ce.Message);
                return;
            }

            Console.WriteLine("Enter the Contact Id:");
            var contactid = Console.ReadLine();

            // Initialize a client using the validated configuration
            using (var client = new XConnectClient(cfg))
            {
                try
                {
                    // Get a known contact
                    IdentifiedContactReference reference = new IdentifiedContactReference("HIxConnect", contactid);

                    Contact existingContact = await client.GetAsync<Contact>(reference, new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey,EmailAddressList.DefaultFacetKey }));
                  

                    PersonalInformation existingPsnlContactFacet = existingContact.GetFacet<PersonalInformation>(PersonalInformation.DefaultFacetKey);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("-=-=-=-=-=-=-=-=-Contact Details-=-=-=-=-=-=");
                    Console.WriteLine();
                    Console.WriteLine("Contact ID: " + existingContact.Id.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Contact Name: " + existingPsnlContactFacet.Title+" "+ existingPsnlContactFacet.FirstName +" "+ existingPsnlContactFacet.LastName);
                    Console.WriteLine();
                    Console.WriteLine("Preferred Langauge: " + existingPsnlContactFacet.PreferredLanguage);
                    Console.WriteLine();
                    EmailAddressList emailAddressListFacet = existingContact.GetFacet<EmailAddressList>(EmailAddressList.DefaultFacetKey);
                    if(emailAddressListFacet!=null)
                    {
                        Console.WriteLine("Email Address:" + emailAddressListFacet.PreferredEmail.SmtpAddress);
                    }
                    Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
                    #region IP Information
                    IpInfo existingContactIPFacet = existingContact.GetFacet<IpInfo>(IpInfo.DefaultFacetKey);
                    if (existingContactIPFacet != null)
                    {
                        Console.WriteLine("---------===--------===--------IP Information-------====-------===-------");
                        Console.WriteLine("IP used:" + existingContactIPFacet.IpAddress);
                        Console.WriteLine("IP Business Name:" + existingContactIPFacet.BusinessName);
                    }
                    #endregion
                    Console.ReadLine();
                }
                catch (XdbExecutionException ex)
                {
                    // Deal with exception
                }
            }
        }
    }
}
