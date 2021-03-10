using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using TinifyAPI;

public partial class Compress : System.Web.UI.Page
{
    public string SourcePath { get; set; }
    public string DestinationFolderPath = HttpContext.Current.Server.MapPath("~/Compressed");

    public class MyFile
    {
        public string FilePath { get; set; }
        public double Size { get; set; }
    }

    private List<MyFile> GetFiles(string sourcePath)
    {
        var extensions = "jpg,jpeg,png";
        if (!Directory.Exists(sourcePath))
        {
            throw new System.Exception("The source path does not exist: " + sourcePath);
        }

        return Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories)
                   .Where(x => extensions.Contains(x.Split('.').Last()))
                   .Where(x => !x.ToLower().StartsWith(Path.Combine(sourcePath, "_assets").ToLower()))
                   .Select(x =>
                   {
                       return new MyFile
                       {
                           FilePath = x,
                           Size = new FileInfo(x).Length // / 1024
                       };
                   })
                   .OrderByDescending(x => x.Size).ToList();
    }
    public class ImageTransformation
    {
        public string Path { get; set; }
        public double OldSize { get; set; }
        public double? NewSize { get; set; }

        public double? CompressionSize { get { return this.OldSize - this.NewSize; } }
        public double? CompressionPercentage { get { return (this.NewSize / this.OldSize) * 100; } }
    }

    public async Task CompressImages(string path)
    {
        var log = new List<string>();
        var compressions = new List<ImageTransformation>();

        Tinify.Key = tbTinyPngKey.Text;
        var isApiKeyValid = await Tinify.Validate();

        if (!isApiKeyValid)
        {
            log.Add("API key is invald: " + Tinify.Key);
        }
        else
        {
            var files = GetFiles(path).Take(5);
            foreach (var img in files)
            {
                var sourcePath = img.FilePath;
                var filename = PathExtensions.GetRelativePath(path, sourcePath);
                var destinationPath = Path.Combine(DestinationFolderPath, filename);
                // ensure the folder exists for proper working source.ToFile();  
                new DirectoryInfo(Path.GetDirectoryName(destinationPath)).Create();

                var compression = new ImageTransformation()
                {
                    Path = destinationPath,
                    OldSize = img.Size,
                };

                var compressionsThisMonth = Tinify.CompressionCount == null ? 0 : Tinify.CompressionCount;
                if (compressionsThisMonth < 500)
                {
                    try
                    {
                        // Use the Tinify API client.
                        var source = Tinify.FromFile(sourcePath);
                        await source.ToFile(destinationPath);
                        compression.NewSize = source.GetResult().Result.Size;
                        compressions.Add(compression);
                    }
                    catch (AccountException e)
                    {
                        log.Add(string.Format("Error for image '{0}'. Error message: ", filename, e.Message));
                        //System.Console.WriteLine("The error message is: " + e.Message);
                        // Verify your API key and account limit.
                    }
                    catch (ClientException e)
                    {
                        log.Add(string.Format("Error for image '{0}'. Error message: ", filename, e.Message));
                        var mimetype = MimeTypes.getMimeFromFile(sourcePath);
                        log.Add(String.Format("The actual mime type of '{0}' image is '{1}' ", filename, mimetype));

                        // Check your source image and request options.
                    }
                    catch (ServerException e)
                    {
                        log.Add(string.Format("Error for image '{0}'. Error message: ", filename, e.Message));

                        // Temporary issue with the Tinify API.
                    }
                    catch (ConnectionException e)
                    {
                        log.Add(string.Format("Error for image '{0}'. Error message: ", filename, e.Message));

                        // A network connection error occurred.
                    }
                    catch (System.Exception e)
                    {
                        log.Add(string.Format("Error for image '{0}'. Error message: ", filename, e.Message));

                        // Something else went wrong, unrelated to the Tinify API.
                    }

                }
                else
                {
                    log.Add("Free API usage is near the limit: " + compressionsThisMonth);
                    //break;
                }

            }
        }

        using (TextWriter writer = new StreamWriter(Server.MapPath("~/errors.xml")))
        {
            var serializer = new XmlSerializer(typeof(List<string>));
            serializer.Serialize(writer, log);
        }

        using (TextWriter writer = new StreamWriter(Server.MapPath("~/compressions.xml")))
        {

            var serializer = new XmlSerializer(typeof(List<ImageTransformation>));
            serializer.Serialize(writer, compressions);
        }

        RadGrid1.DataSource = log;
        RadGrid1.DataBind();
    }
    protected void RadButton1_Click(object sender, EventArgs e)
    {
        var sourcePath = tbSourcePath.Text;

        if (!Directory.Exists(sourcePath))
        {
            string scriptstring = "alert('The source path does not exist: " + sourcePath.Replace('\\', '/') + "');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myalert", scriptstring, true);
        }
        else
        {
            Task.Run(() => CompressImages(tbSourcePath.Text)).Wait();
        }
    }

    public class MyTestClass
    {
        public string Source { get; set; }
        public string Destination { get; set; }
    }
}

