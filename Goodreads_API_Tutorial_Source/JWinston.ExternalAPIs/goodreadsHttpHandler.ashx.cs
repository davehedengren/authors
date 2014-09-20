using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Reflection;
using System.Xml.Linq;
using System.Web.Configuration;
using JWinston.ExternalAPIs.JWinston.ExternalAPIs;

namespace JWinston.ExternalAPIs
{
    /// <summary>
    /// Summary description for goodreadsHttpHandler
    /// </summary>
    public class goodreadsHttpHandler : IHttpHandler
    {
        CommonWebConfiguration keyManager = new CommonWebConfiguration();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "application/json";

            string bookTitle = context.Request.QueryString[Literals.BookTitle];
            string bookAuthor = context.Request.QueryString[Literals.BookAuthor];

            string uri = GetGoodreadsURI(bookAuthor, bookTitle);
            int serverStatusCode = 200;

            int secondsToCacheData = Int32.Parse(keyManager.GetConfigurationByKey(Literals.SecondsToCacheData));
            CacheManager cm = new CacheManager(secondsToCacheData);
            string returnData = cm.GetCacheValue(uri);

            if (null == returnData)
            {
                returnData = GetGoodreadsJSONResponse();
                string responseData = GetResponseFromAPI(uri, out serverStatusCode);
                if (serverStatusCode == 200)
                {
                    returnData = ConvertGoodreadsXMLtoJSON(responseData, returnData);
                }
                cm.InsertToCache(uri, returnData);
            }
            
            context.Response.Write(returnData);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public string GetGoodreadsURI(string bookAuthor, string bookTitle)
        {
            string myKey = keyManager.GetConfigurationByKey(Literals.GoodreadsDeveloperKey);
            string uri = Literals.GoodreadsURI;
            return String.Format(uri, "xml", bookAuthor, myKey, bookTitle);
        }

        public string GetResponseFromAPI(string uri, out int serverStatus)
        {
            string responseData = String.Empty;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                serverStatus = (int)res.StatusCode;

                using (Stream s = res.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        responseData = sr.ReadToEnd();
                    }
                }
                return responseData;
            }
            catch (WebException e)
            {
                HttpWebResponse res = (HttpWebResponse)e.Response;
                serverStatus = (int)res.StatusCode;
                return responseData;
            }
        }

        public string GetGoodreadsJSONResponse()
        {
            string returnData;
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(Literals.GoodreadsJSONTxtFileURL))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    returnData = sr.ReadToEnd();
                }
            }
            return returnData;
        }

        public string ConvertGoodreadsXMLtoJSON(string responseData, string bookFieldsAsJSON)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                XDocument xmlResponseData = XDocument.Parse(responseData);
                XElement goodreadsRoot = xmlResponseData.Element(Literals.XMLGoodreadsResponseElement);
                XElement bookRootElement = goodreadsRoot.Element(Literals.XMLBookElement);
                XElement authorsRootElement = bookRootElement.Element(Literals.XMLAuthorsElement);
                XElement authorPrimaryRootElement = authorsRootElement.Element(Literals.XMLAuthorElement);
                XElement workRootElement = bookRootElement.Element(Literals.XMLWorkElement);


                XElement authorNameElement = authorPrimaryRootElement.Element(Literals.XMLAuthor);
                string bookAuthorValue = authorNameElement.Value;

                XElement titleElement = bookRootElement.Element(Literals.XMLTitle);
                string bookTitleValue = titleElement.Value;

                XElement descriptionElement = bookRootElement.Element(Literals.XMLDescription);
                string bookDescriptionValue = js.Serialize(descriptionElement.Value).Trim();
                bookDescriptionValue = bookDescriptionValue.Substring(1, bookDescriptionValue.Length - 2);

                XElement averageRatingElement = bookRootElement.Element(Literals.XMLAvgRating);
                string averageRatingValue = averageRatingElement.Value;

                XElement coverImageElement = bookRootElement.Element(Literals.XMLCoverImg);
                string coverImageValue = coverImageElement.Value;

                XElement yearPublishedElement = workRootElement.Element(Literals.XMLPublicationYear);
                string yearPublishedValue = yearPublishedElement.Value;

                XElement reviewsElement = bookRootElement.Element(Literals.XMLReviews);
                string reviewsValue = js.Serialize(reviewsElement.Value).Trim();
                reviewsValue = reviewsValue.Substring(1, reviewsValue.Length - 2);

                XElement publisherElement = bookRootElement.Element(Literals.XMLPublisher);
                string publisherValue = publisherElement.Value;

                XElement isbnElement = bookRootElement.Element(Literals.XMLISBN);
                string isbnValue = isbnElement.Value;

                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONAuthor, bookAuthorValue);
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONTitle, bookTitleValue);
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONDescription, bookDescriptionValue);
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONAvgRating, averageRatingValue);
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONCoverImg, coverImageValue);
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONPublicationYear, yearPublishedValue);
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONPublisher, publisherValue);
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONISBN, isbnValue);
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONReviews, reviewsValue);
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONStatus, Literals.StatusOK);

                return bookFieldsAsJSON;
            }
            catch
            {
                bookFieldsAsJSON = bookFieldsAsJSON.Replace(Literals.JSONStatus, Literals.BadXML);
                return bookFieldsAsJSON;
            }
        }

        private class Literals
        {
            public const string GoodreadsJSONTxtFileURL = "JWinston.ExternalAPIs.JSON_Responses.goodreads.BookInformationFields.txt";
            public const string BookTitle = "bookTitle";
            public const string BookAuthor = "bookAuthor";
            public const string StatusOK = "Status_OK";
            public const string BadXML = "Bad_XML";
            public const string GoodreadsURI = "http://www.goodreads.com/book/title?format={0}&author={1}&key={2}&title={3}";
            public const string GoodreadsDeveloperKey = "goodreadsDeveloperKey";
            public const string SecondsToCacheData = "secondsToCacheGoodreadsData";

            public const string JSONAuthor = "{{!Author!}}";
            public const string JSONTitle = "{{!Title!}}";
            public const string JSONDescription = "{{!Description!}}";
            public const string JSONAvgRating = "{{!Average_Rating!}}";
            public const string JSONCoverImg = "{{!Cover_Image!}}";
            public const string JSONPublicationYear = "{{!Publication_Year!}}";
            public const string JSONPublisher = "{{!Publisher!}}";
            public const string JSONISBN = "{{!ISBN!}}";
            public const string JSONReviews = "{{!Reviews!}}";
            public const string JSONStatus = "{{!Status!}}";

            public const string XMLGoodreadsResponseElement = "GoodreadsResponse";
            public const string XMLBookElement = "book";
            public const string XMLAuthorsElement = "authors";
            public const string XMLAuthorElement = "author";
            public const string XMLWorkElement = "work";
            public const string XMLAuthor = "name";
            public const string XMLTitle = "title";
            public const string XMLDescription = "description";
            public const string XMLAvgRating = "average_rating";
            public const string XMLCoverImg = "image_url";
            public const string XMLPublicationYear = "original_publication_year";
            public const string XMLReviews = "reviews_widget";
            public const string XMLPublisher = "publisher";
            public const string XMLISBN = "isbn13";
        }
    }
}