using Laboratorio2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratorio2.Controllers
{
    public class CesarController : Controller
    {
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
                    //Mandar a llamar al metodo para cifrar
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
