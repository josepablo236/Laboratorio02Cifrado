using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Laboratorio2.Controllers
{
    public class FileUploadController : Controller
    {
        public static string Message;
        // GET: FileUpload
        public ActionResult Index()
        {
            //La vista que me va a mostrar todos los archivos que ya se han subido
            var items = FilesUploaded();
            return View(items);
        }

        //----------------------------SUBIR ARCHIVO ------------------------------------------------------------------------
        //El metodo que mando a llamar desde el Index.cshtml al momento de presionar el submit ("Upload File")

        [HttpPost]          //Recibo un archivo
        public ActionResult Index(HttpPostedFileBase file)
        {
            //Valido que no sea nulo y que contenga texto, ya que valido que el archivo pese.
            if (file != null && file.ContentLength > 0)
                try
                {
                    //Valido que unicamente puedan cargar archivos de text
                    if (Path.GetExtension(file.FileName) == ".txt" || Path.GetExtension(file.FileName) == ".cif")
                    {
                        //Me va a devolver la ruta en la que se encuentra la carpeta "Archivos" 
                        string path = Path.Combine(Server.MapPath("~/Archivo"),
                                      //Toma el nombre del archivo
                                      Path.GetFileName(file.FileName));
                        //Entonces path, va a ser igual a la ruta +  el nombre del archivo
                        file.SaveAs(path); //Guarda el archivo en la carpeta "Archivos"
                        ViewBag.Message = "File uploaded";
                    }

                }
                catch
                {
                    ViewBag.Message = "Invalid file, please upload a .txt or .cif";
                }
            else
            {
                ViewBag.Message = "Please upload a file";
            }

            var items = FilesUploaded();
            return View(items);
        }

        private List<string> FilesUploaded()
        {
            if (!System.IO.Directory.Exists(Server.MapPath("~/Archivo")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Archivo"));
            }
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/Archivo"));
            //Unicamente tome los archivos de text, ahorita lo puse como doc para probar pero al final lo podriamos dejar como .txt
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");
            //Creo una lista con los nombres de todos los archivos para luego poder mostrarlos
            List<string> filesupld = new List<string>();
            foreach (var file in fileNames)
            {
                filesupld.Add(file.Name);
            }
            //Devuelvo la lista
            return filesupld;
        }

        // Este lo vamos a usar luego que ya podamos descomprimir jajaja

        public ActionResult Zig_Zag(string TxtName)
        {
            if (Path.GetExtension(TxtName) == ".txt")
            {
                return RedirectToAction("Clave", "Zig_Zag", new { filename = TxtName });
            }
            else
            {
                Message = "No es un archivo comprimible";
                return RedirectToAction("Index", "FileUpload");
            }

        }
        public ActionResult Espiral(string TxtName)
        {

            if (Path.GetExtension(TxtName) == ".txt")
            {
                return RedirectToAction("Clave", "Espiral", new { filename = TxtName });
            }
            else
            {
                Message = "No es un archivo comprimible";
                return RedirectToAction("Index", "FileUpload");
            }

        }
        public ActionResult Cesar(string TxtName)
        {

            if (Path.GetExtension(TxtName) == ".txt")
            {
                return RedirectToAction("Clave", "Cesar", new { filename = TxtName });
            }
            else
            {
                Message = "No es un archivo comprimible";
                return RedirectToAction("Index", "FileUpload");
            }

        }

        public ActionResult Descifrar(string TxtName)
        {
            if (Path.GetExtension(TxtName) == ".cif")
            {
                string filepath = Server.MapPath("~/Archivo");
                //Descompresion descomprimir = new Descompresion();
                //var FileName = descomprimir.LeerArchivo(TxtName, filepath);
                return RedirectToAction("Download", "ReadText"/*, new { TxtName = FileName }*/);
            }
            //else if (Path.GetExtension(TxtName) == ".lzw")
            //{
            //    string filepath = Server.MapPath("~/Archivo");
            //    DescompresionLZW descompresionLZW = new DescompresionLZW();
            //    descompresionLZW.LeerArchivo(TxtName, filepath);
            //    return RedirectToAction("Download", "ReadText" /*new { TxtName = FileName }*/);
            //}
            else
            {
                Message = "No es un archivo .cif, por lo que no puede descifrarse";
                return RedirectToAction("Index", "FileUpload");
            }
        }
    }
}