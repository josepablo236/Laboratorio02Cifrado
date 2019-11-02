using Laboratorio2.Cifrado;
using Laboratorio2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratorio2.Controllers
{
    public class RSAController : Controller
    {
        string FilePath = "";
        // GET: RSA
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cifrar()
        {
            //La vista que me va a mostrar todos los archivos que ya se han subido
            return View();
        }

        //----------------------------SUBIR ARCHIVO ------------------------------------------------------------------------
        //El metodo que mando a llamar desde el Index.cshtml al momento de presionar el submit ("Upload File")

        [HttpPost]          //Recibo un archivo
        public ActionResult Cifrar(HttpPostedFileBase file1, HttpPostedFileBase file2)
        {
            var path1 = "";
            var path2 = "";
            var fileName = "";
            //Valido que no sea nulo y que contenga texto, ya que valido que el archivo pese.
            if (file1 != null && file1.ContentLength > 0)
                try
                {
                    //Valido que unicamente puedan cargar archivos de texto
                    if (Path.GetExtension(file1.FileName) == ".txt")
                    {
                        //Me va a devolver la ruta en la que se encuentra la carpeta "Archivos" 
                        path1 = Path.Combine(Server.MapPath("~/Archivo"),
                                      //Toma el nombre del archivo
                                      Path.GetFileName(file1.FileName));
                        //Entonces path, va a ser igual a la ruta +  el nombre del archivo
                       // file1.SaveAs(path1); //Guarda el archivo en la carpeta "Archivos"
                        ViewBag.Message = "File uploaded";
                        fileName = file1.FileName;
                    }

                }
                catch
                {
                    ViewBag.Message = "Invalid file, please upload a .txt";
                }
            else
            {
                ViewBag.Message = "Please upload a file";
            }

            //Valido que no sea nulo y que contenga texto, ya que valido que el archivo pese.
            if (file2 != null && file2.ContentLength > 0)
            {
                try
                {
                    //Valido que unicamente puedan cargar archivos de texto
                    if (Path.GetExtension(file2.FileName) == ".Key")
                    {
                        //Me va a devolver la ruta en la que se encuentra la carpeta "Archivos" 
                        path2 = Path.Combine(Server.MapPath("~/Archivo"),
                                      //Toma el nombre del archivo
                                      Path.GetFileName(file2.FileName));
                        //Entonces path, va a ser igual a la ruta +  el nombre del archivo
                        file2.SaveAs(path2); //Guarda el archivo en la carpeta "Archivos"
                        ViewBag.Message = "File uploaded";

                        FilePath = Server.MapPath("~/Archivo");
                        CifradoRSA rsa = new CifradoRSA();
                        rsa.LeerTxt(path1, path2, FilePath, fileName);
                    }

                }
                catch
                {
                    ViewBag.Message = "Invalid file, please upload a .key";
                }
            }
            else
            {
                ViewBag.Message = "Please upload a file";
            }
            return View();
        }


        // GET: RSA/Create
        public ActionResult Clave(string primo1, string primo2)
        {
            RSAViewModel rsa = new RSAViewModel();
            rsa.ValorP = Convert.ToInt32(primo1);
            rsa.ValorQ = Convert.ToInt32(primo2);
            return View(rsa);
        }

        // POST: RSA/Create
        [HttpPost]
        public ActionResult Clave(RSAViewModel rsa)
        {
            try
            {
                if (ModelState.IsValid && rsa.ValorP > 0 && rsa.ValorQ > 0)
                {
                    if (rsa.ValorP == 1 || rsa.ValorQ == 1)
                    {
                        return View(rsa);
                    }
                    else
                    {
                        var primo1 = numeroPrimo(rsa.ValorP, 2);
                        var primo2 = numeroPrimo(rsa.ValorQ, 2);
                        if(primo1 == true && primo2 == true)
                        {
                            FilePath = Server.MapPath("~/Archivo");
                            CifradoRSA RSA = new CifradoRSA();
                            RSA.GenerarLlaves(rsa.ValorP, rsa.ValorQ, FilePath); 
                            return RedirectToAction(nameof(Cifrar));
                        }
                        else
                        {
                            return View(rsa);
                        }
                    }
                }
                else
                {
                    return View(rsa);
                }
            }
            catch
            {
                return View();
            }
        }

        public bool numeroPrimo(int num, int divisor)
        {
            if (num / 2 < divisor)
            {
                return true;
            }
            else
            {
                if (num % divisor == 0)
                {
                    return false;
                }
                else
                {
                    return numeroPrimo(num, divisor + 1);
                }
            }
        }


        public ActionResult Descifrar()
        {
            return View();
        }

        [HttpPost]     
        public ActionResult Descifrar(HttpPostedFileBase file1, HttpPostedFileBase file2)
        {
            var path1 = "";
            var path2 = "";
            var fileName = "";
            //Valido que no sea nulo y que contenga texto, ya que valido que el archivo pese.
            if (file1 != null && file1.ContentLength > 0)
                try
                {
                    //Valido que unicamente puedan cargar archivos de texto
                    if (Path.GetExtension(file1.FileName) == ".rsacif")
                    {
                        //Me va a devolver la ruta en la que se encuentra la carpeta "Archivos" 
                        path1 = Path.Combine(Server.MapPath("~/Archivo"),
                                      //Toma el nombre del archivo
                                      Path.GetFileName(file1.FileName));
                        //Entonces path, va a ser igual a la ruta +  el nombre del archivo
                       // file1.SaveAs(path1); //Guarda el archivo en la carpeta "Archivos"
                        ViewBag.Message = "File uploaded";
                        fileName = file1.FileName;
                    }

                }
                catch
                {
                    ViewBag.Message = "Invalid file, please upload a .rsacif";
                }
            else
            {
                ViewBag.Message = "Please upload a file";
            }

            //Valido que no sea nulo y que contenga texto, ya que valido que el archivo pese.
            if (file2 != null && file2.ContentLength > 0)
            {
                try
                {
                    //Valido que unicamente puedan cargar archivos de texto
                    if (Path.GetExtension(file2.FileName) == ".Key")
                    {
                        //Me va a devolver la ruta en la que se encuentra la carpeta "Archivos" 
                        path2 = Path.Combine(Server.MapPath("~/Archivo"),
                                      //Toma el nombre del archivo
                                      Path.GetFileName(file2.FileName));
                        //Entonces path, va a ser igual a la ruta +  el nombre del archivo
                       // file2.SaveAs(path2); //Guarda el archivo en la carpeta "Archivos"
                        ViewBag.Message = "File uploaded";

                        FilePath = Server.MapPath("~/Archivo");
                        CifradoRSA rsa = new CifradoRSA();
                        rsa.LeerCifrado(path1, path2, FilePath, fileName);

                    }

                }
                catch
                {
                    ViewBag.Message = "Invalid file, please upload a .key";
                }
            }
            else
            {
                ViewBag.Message = "Please upload a file";
            }
            return View();
        }

    }
}
