using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using System.IO;
using System.IO.Compression;

namespace USWsApp.Controllers
{
    public class UploadController : ApiController
    {

 //       [Authorize(Roles = "ServiciosSignatured")]
        [Route("api/Files/UploadSignatured")]
        public async Task<string> PostDelivery()
        {

            string tamanio = String.Empty;
            try
            {
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        var fileName = postedFile.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();

                        var filePath = HttpContext.Current.Server.MapPath("~/FilesUploads/Signatured/" + fileName);

                        postedFile.SaveAs(filePath);


                        FileInfo f = new FileInfo(filePath);
                        tamanio = f.Length.ToString();

                        var dirunzip = fileName.Substring(0, fileName.Length - 4);

                        //string startPath = @"c:\example\start";
                        string zipPath = filePath; // @"c:\example\result.zip";
                        string extractPath = HttpContext.Current.Server.MapPath("~/FilesUploads/Signatured/" + dirunzip);// @"~\FilesUploads\Photos"; // @"c:\example\extract";

                        //ZipFile.CreateFromDirectory(startPath, zipPath);

                        //   ZipFile.ExtractToDirectory(zipPath, extractPath);

                        var tmpparacreardir = extractPath + "\\" + fileName;

                        try
                        {
                            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                            {


                                foreach (ZipArchiveEntry entry in archive.Entries)
                                {
 
                                    if (entry.FullName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string directory = Path.GetDirectoryName(tmpparacreardir);

                                        if (!Directory.Exists(directory))
                                            Directory.CreateDirectory(directory);

                                        entry.ExtractToFile(Path.Combine(extractPath, entry.FullName), true);
                                    }
                                }
                            }
 
                            try
                            {

                                if (System.IO.File.Exists(zipPath))
                                    System.IO.File.Delete(zipPath);

                            }
                            catch (Exception eexc)
                            {

                            }
                              
                        }
                        catch (Exception eex)
                        {
                            return "no files";

                        } 
                        return tamanio; //"/FilesUploads/Photos/" + fileName;
                    }
                }
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            return "no files";
        }

    }
}