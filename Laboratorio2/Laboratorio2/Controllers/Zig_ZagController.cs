using Laboratorio2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratorio2.Controllers
{
    public class Zig_ZagController : Controller
    {
        // GET: Zig_Zag
        public ActionResult Index()
        {
            return View();
        }

        // GET: Zig_Zag/Details/5
        public ActionResult ArchivoCifrado()
        {
            return View();
        }

        // GET: Zig_Zag/Clave
        public ActionResult Clave(string fileName, string niveles)
        {
            Zig_ZagViewModel zigzag = new Zig_ZagViewModel();
            zigzag.NombreDelArchivo = fileName;
            zigzag.NivelesDeSeparacion = Convert.ToInt32(niveles);
            return View(zigzag);
        }

        // POST: Zig_Zag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Clave(Zig_ZagViewModel zigzag)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var archivo = zigzag.NombreDelArchivo;
                    var niveles = zigzag.NivelesDeSeparacion;
                    //Mandar a llamar al metodo para cifrar
                    return RedirectToAction(nameof(ArchivoCifrado));
                }
                else
                {
                    return View(zigzag);
                }
            }
            catch
            {
                return RedirectToAction(nameof(ArchivoCifrado));
            }
        }
    }
}
