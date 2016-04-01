using Amazon.AWS;
using AmazonProductAdvtApi;
using MarketplaceWebServiceProducts;
using MarketplaceWebServiceProducts.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace UKBeautyLibrary.BLCode
{
    public static class UKItemSearch
    {
        static string[] AWSSearchIndicies = new string[] { "Toys" };

        public static void ItemLookup(IEnumerable<string> lstASIN)
        {
            try
            {
                // Create a configuration object
                MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProductsConfig
                {
                    ServiceURL = "https://mws.amazonservices.com"
                };
                MarketplaceWebServiceProducts.MarketplaceWebServiceProducts client = new MarketplaceWebServiceProductsClient("AmazonJavascriptScratchpad", "1.0", "AKIAITUQFOC6SOCWDK3A", "2yaTZZIc8SmjPlihxwxDhnDhdM9JBH4IuGB3I/o4", config);
                GetLowestOfferListingsForASINResponse response = client.GetLowestOfferListingsForASIN(new GetLowestOfferListingsForASINRequest
                {
                    SellerId = "A3AC8PQ96D2D8O",
                    MWSAuthToken = "amzn.mws.54a5ad20-5a53-66ca-c870-38f6da526f93",
                    MarketplaceId = "ATVPDKIKX0DER",
                    ASINList = new ASINListType { ASIN = new List<string>(lstASIN.ToArray()) },
                    ItemCondition = "new",
                    ExcludeMe = true
                });
                if (response != null && response.GetLowestOfferListingsForASINResult.Count > 0)
                {
                    string xml = "<Items>";
                    foreach (GetLowestOfferListingsForASINResult ASINResult in response.GetLowestOfferListingsForASINResult)
                    {
                        try
                        {
                            if (ASINResult.Product.LowestOfferListings.LowestOfferListing.Count > 0)
                            {
                                LowestOfferListingType lowestOfferListing = ASINResult.Product.LowestOfferListings.LowestOfferListing.FirstOrDefault(s => s.Qualifiers.FulfillmentChannel == "Amazon");
                                if (lowestOfferListing == null)
                                {
                                    lowestOfferListing = ASINResult.Product.LowestOfferListings.LowestOfferListing.FirstOrDefault();
                                }
                                xml += "<item>";
                                xml += "<ASIN>" + ASINResult.ASIN + "</ASIN>";
                                xml += "<FormattedPrice>" + lowestOfferListing.Price.ListingPrice.Amount + "</FormattedPrice>";
                                xml += "<IsEligibleForPrime>";
                                xml += lowestOfferListing.Qualifiers.FulfillmentChannel == "Amazon" ? "1" : "0";
                                xml += "</IsEligibleForPrime>";
                                xml += "</item>";
                            }
                        }
                        catch (Exception ex)
                        {
                            LogErrors.WriteErrorToFile(ex);
                        }
                    }
                    xml += "</Items>";
                    SendAWSResponse(xml);
                }
            }
            catch (Exception ex)
            {
                LogErrors.WriteErrorToFile(ex);
            }
        }

        private static void SendAWSResponse(string xml)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UKMain"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("ParseXml", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@xmldata", xml);
                        cmd.Parameters.AddWithValue("@flag", 1);
                        cmd.CommandTimeout = 2200;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrors.WriteErrorToFile(ex);
            }
        }

        public static void UpdateAsin()
        {
            try
            {
                int skipAsinCount = 0;
                using (UKOmnimarkEntities db = new UKOmnimarkEntities())
                {
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 1800;
                    var ASINList = db.tbl_Baby.Where(x => (x.Status == 1) || (x.UPC != "null" && x.Status == 0 && x.Prime == "1" && x.WeightUnits <= 500 && x.UK_Prohibited != 1 && x.HeightUnits < 3000 && x.WidthUnits < 3000 && x.LengthUnits < 3000)).OrderBy(x => x.ExportDate).ThenBy(x => x.ReExportTimeStamp).Select(x => x.ASIN.TrimEnd()).ToList();
                    double r = (ASINList.Count / 20.00);
                    int requestCount = int.Parse(Math.Ceiling(r).ToString());
                    for (int i = 0; i < requestCount; i++)
                    {
                        UKItemSearch.ItemLookup(ASINList.Skip(skipAsinCount).Take(20));
                        skipAsinCount = skipAsinCount + 20;
                    }
                }
            }
            catch(Exception ex)
            {
                LogErrors.WriteErrorToFile(ex);
            }
        }
    }
}
