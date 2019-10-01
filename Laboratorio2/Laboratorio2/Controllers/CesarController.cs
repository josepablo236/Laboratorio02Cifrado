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
    public class CesarController : Controller
    {
        public string FilePath = "";
        // GET: Cesar
        public ActionResult Index()
        {
            return View();
        }

        // GET: Cesar/Details/5
        public ActionResult ArchivoCifrado()
        {
            return View();
        }

        // GET: Cesar/Create
        public ActionResult Clave(string fileName, string clave)
        {
            CesarViewModel cesar = new CesarViewModel();
            cesar.NombreArchivo = fileName;
            cesar.ClaveDiccionario = clave;
            return View(cesar);
        }

        // POST: Cesar/Create
        [HttpPost]
        public ActionResult Clave(CesarViewModel cesar)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var listaClave = new List<byte>();
                    foreach (var item in cesar.ClaveDiccionario)
                    {
                        if (!listaClave.Contains(Convert.ToByte(item)))
                        {
                            listaClave.Add(Convert.ToByte(item));
                        }
                        else
                        {
                            ViewBag.Message = "File uploaded";
                            return View(cesar);
                        }
                    }
                    //Mandar a llamar al metodo para cifrar
                    var path = Path.Combine(Server.MapPath("~/Archivo"), cesar.NombreArchivo);
                    FilePath = Server.MapPath("~/Archivo");
                    CifradoCesar cifradocesar = new CifradoCesar();
                    cifradocesar.Cifrar(cesar.NombreArchivo, path, FilePath, listaClave);
                    return RedirectToAction(nameof(ArchivoCifrado));
                }
                else
                {
                    return View(cesar);
                }
            }
            catch
            {
                return RedirectToAction(nameof(ArchivoCifrado));
            }
        }


        public ActionResult ClaveDes(string fileName, string clave)
        {
            CesarViewModel cesar = new CesarViewModel();
            cesar.NombreArchivo = fileName;
            cesar.ClaveDiccionario = clave;
            return View(cesar);
        }

        // POST: Cesar/Create
        [HttpPost]
        public ActionResult ClaveDes(CesarViewModel cesar)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var listaClave = new List<byte>();
                    foreach (var item in cesar.ClaveDiccionario)
                    {
                        if (!listaClave.Contains(Convert.ToByte(item)))
                        {
                            listaClave.Add(Convert.ToByte(item));
                        }
                        else
                        {
                            ViewBag.Message = "File uploaded";
                            return View(cesar);
                        }
                    }
                    //Mandar a llamar al metodo para cifrar
                    var path = Path.Combine(Server.MapPath("~/Archivo"), cesar.NombreArchivo);
                    FilePath = Server.MapPath("~/Archivo");
                    CifradoCesar cifradocesar = new CifradoCesar();
                    cifradocesar.Descifrar(cesar.NombreArchivo, path, FilePath, listaClave);
                    return RedirectToAction(nameof(ArchivoCifrado));
                }
                else
                {
                    return View(cesar);
                }
            }
            catch
            {
                return RedirectToAction(nameof(ArchivoCifrado));
            }
        }
    }
}
