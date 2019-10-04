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
    public class Zig_ZagController : Controller
    {
        public string FilePath = "";
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
                    if(zigzag.NivelesDeSeparacion > 0)
                    {
                        //Mandar a llamar al metodo para cifrar
                        var path = Path.Combine(Server.MapPath("~/Archivo"), zigzag.NombreDelArchivo);
                        FilePath = Server.MapPath("~/Archivo");
                        CifradoZig_Zag zig = new CifradoZig_Zag();
                        zig.Cifrar(zigzag.NombreDelArchivo, path, FilePath, zigzag.NivelesDeSeparacion);
                        return RedirectToAction(nameof(ArchivoCifrado));
                    }
                    else
                    {
                        return View(zigzag);
                    }
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




        public ActionResult ClaveDes(string fileName, string niveles)
        {
            Zig_ZagViewModel zigzag = new Zig_ZagViewModel();
            zigzag.NombreDelArchivo = fileName;
            zigzag.NivelesDeSeparacion = Convert.ToInt32(niveles);
            return View(zigzag);
        }

        // POST: Zig_Zag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClaveDes(Zig_ZagViewModel zigzag)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    if (zigzag.NivelesDeSeparacion > 0)
                    {
                        //Mandar a llamar al metodo para cifrar
                        var path = Path.Combine(Server.MapPath("~/Archivo"), zigzag.NombreDelArchivo);
                        FilePath = Server.MapPath("~/Archivo");
                        CifradoZig_Zag zig = new CifradoZig_Zag();
                        zig.Descifrar(zigzag.NombreDelArchivo, path, FilePath, zigzag.NivelesDeSeparacion);
                        return RedirectToAction(nameof(ArchivoCifrado));
                    }
                    else
                    {
                        return View(zigzag);
                    }
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
