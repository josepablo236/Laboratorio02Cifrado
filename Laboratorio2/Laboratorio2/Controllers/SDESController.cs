using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laboratorio2.Models;
using Laboratorio2.Cifrado;
using System.IO;

namespace Laboratorio2.Controllers
{
    public class SDESController : Controller
    {
        string FilePath = "";
        // GET: SDES
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ArchivoDescifrado()
        {
            return View();
        }

        public ActionResult ArchivoCifrado()
        {
            return View();
        }
        public ActionResult ArchivoDescifrado()
        {
            return View();
        }
        // GET: SDES/Create
        public ActionResult Clave(string fileName, string numero)
        {
            SDESViewModel sdes = new SDESViewModel();
            sdes.NombreArchivo = fileName;
            sdes.Numero = Convert.ToInt32(numero);
            return View(sdes);
        }

        // POST: SDES/Create
        [HttpPost]
        public ActionResult Clave(SDESViewModel sdes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(sdes.Numero > 0 && sdes.Numero < 1024)
                    {
                        //Mandar a llamar al metodo para cifrar
                        FilePath = Server.MapPath("~/Archivo");
                        var path = Path.Combine(Server.MapPath("~/Archivo"), sdes.NombreArchivo);
                        var pathPermutaciones = Path.Combine(Server.MapPath("~/Archivo"), "Permutaciones.txt");
                        CifradoSDES cifSDES = new CifradoSDES();
                        cifSDES.Cifrar(sdes.NombreArchivo, path, FilePath, sdes.Numero, pathPermutaciones);
                        return RedirectToAction(nameof(ArchivoCifrado));
                    }
                    else { return View(sdes); }
                }
                else
                {
                    return View(sdes);
                }
            }
            catch
            {
                return RedirectToAction(nameof(ArchivoCifrado));
            }
        }
        // GET: SDES/Create
        public ActionResult ClaveDes(string fileName, string numero)
        {
            SDESViewModel sdes = new SDESViewModel();
            sdes.NombreArchivo = fileName;
            sdes.Numero = Convert.ToInt32(numero);
            return View(sdes);
        }
 
        // POST: SDES/Create
        [HttpPost]
        public ActionResult ClaveDes(SDESViewModel sdes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (sdes.Numero > 0 && sdes.Numero < 1024)
                    {
                        //Mandar a llamar al metodo para cifrar
                        FilePath = Server.MapPath("~/Archivo");
                        var path = Path.Combine(Server.MapPath("~/Archivo"), sdes.NombreArchivo);
                        var pathPermutaciones = Path.Combine(Server.MapPath("~/Archivo"), "Permutaciones.txt");
                        CifradoSDES cifSDES = new CifradoSDES();
                        cifSDES.Descifrar(sdes.NombreArchivo, path, FilePath, sdes.Numero, pathPermutaciones);
                        return RedirectToAction(nameof(ArchivoDescifrado));
                    }
                    else { return View(sdes); }
                }
                else
                {
                    return View(sdes);
                }
            }
            catch
            {
                return RedirectToAction(nameof(ArchivoDescifrado));
            }
        }

        // GET: SDES/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SDES/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SDES/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SDES/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
