 using Laboratorio2.Models;
 using System;
 using System.Collections.Generic;
using System.IO;
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
                    Cifrado(espiral);
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
        public void Cifrado(EspiralViewModel espiral)
        {
            var bufferLength = 750;
            var path = Path.Combine(Server.MapPath("~/Archivo"), espiral.NombreArchivo);
            
            var byteBuffer = new byte[320000000];
            List<string> Text_archivo = new List<string>();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                
                using (var reader = new BinaryReader(stream))
                {
                    byteBuffer = reader.ReadBytes(bufferLength);

                    foreach (var item in byteBuffer)
                    {
                        Text_archivo.Add(Convert.ToString(item));
                    }
                }
            }
            string[,] matriz = new string[espiral.TamañoM, espiral.TamañoN];
            int textoescrito = 0;
            for (int i = 0; i < espiral.TamañoN; i++)
            {
                for (int j = 0; j < espiral.TamañoM; j++)
                {
                    matriz[j, i] = textoescrito < Text_archivo.Count ? Text_archivo[textoescrito] : "*";
                    textoescrito++;
                }
            }
            string direccion = espiral.DireccionRecorrido;
            
        }
    }
}
