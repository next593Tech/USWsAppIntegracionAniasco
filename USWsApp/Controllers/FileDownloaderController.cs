
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using USWsLibrary.Models;

namespace USWsApp.Controllers
{

    //[Authorize(Roles = "QueryDocs")]
    public class FileDownloaderController : ApiController
    {

        public FileDownloaderController( )
        {
         
        }


        [HttpPost]
        [ResponseType(typeof(HttpResponseMessage))]
        public HttpResponseMessage DownloadFile(ConsultaDocumentoDto consulta)
        {

            string host = consulta.TenancyFtpHost;
            string username = consulta.TenancyFtpUserName;
            string password = consulta.TenancyFtpPassword;

            string remoteDirectory = "";
            string remoteFileName = consulta.RemoteFileName.Trim();
            string contentType = consulta.ContentType;
            string headerContentType = "";
            string extension = "";
            string dir = remoteFileName.Substring(0,8);
            remoteDirectory = "/" + ReverseDateString(dir) + "/";
            if (contentType.Equals("PDF"))
            {
                headerContentType = "application/pdf";
                extension = ".PDF";
            }
            else
            {
                headerContentType = "text/xml";
                extension = ".xml";
            }


            var file1 = new MemoryStream();
            HttpResponseMessage response;
            using (var sftp = new SftpClient(host, 22, username, password))
            {
                try {
                    string fileRemoteURL = remoteDirectory + remoteFileName + extension;
                    sftp.Connect();
                    if(sftp.Exists(fileRemoteURL))
                        sftp.DownloadFile(fileRemoteURL, file1);
                    else
                    {
                        file1.Close();
                        throw new Exception("File not Found");
                    }
                } catch(Exception e) {
                    file1.Close();
                    throw e;
                }
            }
            response = Request.CreateResponse(System.Net.HttpStatusCode.OK,"value");
            response.Content = new ByteArrayContent(file1.ToArray());
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(headerContentType);
            file1.Close();
            return response;

        }

        

        private static string ReverseDateString(string s)
        {
            string day = s.Substring(0, 2);
            string month = s.Substring(2, 2);
            string year = s.Substring(4, 4);
            s = year + month + day;
            return s;
        }
    }
}
