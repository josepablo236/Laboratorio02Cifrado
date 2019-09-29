using Laboratorio2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratorio2.Controllers
{
    public class EspiralController : Controller
    {
        // GET: Espiral
        public ActionResult Index()
        {
            return View();
        }

        // GET: Espiral/Details/5
        public ActionResult ArchivoCifrado()
        {
            return View();
        }

        // GET: Espiral/Create
        public ActionResult Clave(string fileName, string tamañoM, string tamañoN, string direccion)
        {
            EspiralViewModel espiral = new EspiralViewModel();
            espiral.NombreArchivo = fileName;
            espiral.TamañoM = Convert.ToInt32(tamañoM);
            espiral.TamañoN = Convert.ToInt32(tamañoN);
            espiral.DireccionRecorrido = direccion;
            return View(espiral);
        }

        // POST: Espiral/Create
        [HttpPost]
        public ActionResult Clave(EspiralViewModel espiral)
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
                    return View(espiral);
                }
            }
            catch
            {
                return RedirectToAction(nameof(ArchivoCifrado));
            }
        }
    }
}
