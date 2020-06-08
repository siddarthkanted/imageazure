using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace ImageWebApplication.Controllers
{
    public class ImageController : ApiController
    {
        // GET: api/Image
        // http://localhost:63087/api/Image
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();
            response.Content = new ByteArrayContent(Image.GetFileContent(""));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return response;
        }

        public async Task<HttpResponseMessage> GetUsingPushStreamAsync(string fileName, string fileType)
        {
            fileName = fileName + "." + fileType;
            var response = new HttpResponseMessage();
            var streamToDownload = await Image.GetFileStream(fileName).ConfigureAwait(false);
            response.Content = Image.GetPushStreamContent(streamToDownload);

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
            {
                FileName = fileName
            };

            response.Headers.Add("Access-Control-Allow-Origin", "*");

            return response;
        }


        //http://localhost:63087/Image/office-1356793_1280/png
        public HttpResponseMessage GetByFileName(string fileName, string fileType)
        {
            fileName = fileName + "." + fileType;
            var response = new HttpResponseMessage();
            response.Content = new ByteArrayContent(Image.GetFileContent(fileName));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        // POST: api/Image
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Image/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Image/5
        public void Delete(int id)
        {
        }
    }
}
