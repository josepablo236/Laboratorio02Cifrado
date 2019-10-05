 using Laboratorio2.Models;
 using System;
 using System.Collections.Generic;
 using System.IO;
 using System.Linq;
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
        public ActionResult ArchivoDescifrado()
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

        public ActionResult ClaveDes(string fileName, string tamañoM, string tamañoN, string direccion)
        {
            EspiralViewModel espiral = new EspiralViewModel();
            espiral.NombreArchivo = fileName;
            espiral.TamañoM = Convert.ToInt32(tamañoM);
            espiral.TamañoN = Convert.ToInt32(tamañoN);
            espiral.DireccionRecorrido = direccion;
            return View(espiral);
        }

        // POST: Cesar/Create
        [HttpPost]
        public ActionResult ClaveDes(EspiralViewModel espiral)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    Deencrypt(espiral);
                    return RedirectToAction(nameof(ArchivoDescifrado));
                }
                else
                {
                    return View(espiral);
                }
            }
            catch
            {
                return RedirectToAction(nameof(ArchivoDescifrado));
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
            if ((espiral.TamañoM * espiral.TamañoN) < Text_archivo.Count)
            {
                decimal division = Text_archivo.Count / espiral.TamañoM;
                espiral.TamañoN = (int)Math.Ceiling(division);
            }

            string[,] matriz = new string[espiral.TamañoM, espiral.TamañoN];
            int textoescrito = 0;
            for (int i = 0; i < espiral.TamañoN; i++)
            {
                for (int j = 0; j < espiral.TamañoM; j++)
                {
                    matriz[j, i] = textoescrito < Text_archivo.Count ? Text_archivo[textoescrito] : "42";
                    textoescrito++;
                }
            }

            int[] limites = { espiral.TamañoM - 1, espiral.TamañoN - 1 };
            int x = 0; int y = 0;
            List<string> Text_encryption = new List<string>();

            switch (espiral.DireccionRecorrido)
            {
                case "horizontal":

                    while ((espiral.TamañoM * espiral.TamañoN) > Text_encryption.Count)
                    {
                        for (int i = x; i <= limites[0]; i++)
                        {
                            Text_encryption.Add(matriz[i, y]);
                        }
                        y++;
                        if ((espiral.TamañoM * espiral.TamañoN) == Text_encryption.Count) { break; }
                        for (int i = y; i <= limites[1]; i++)
                        {
                            Text_encryption.Add(matriz[limites[0], i]);
                        }
                        if ((espiral.TamañoM * espiral.TamañoN) == Text_encryption.Count) { break; }
                        for (int i = limites[0]-1; i >= x; i--)
                        {
                            Text_encryption.Add(matriz[i, limites[1]]);
                        }
                        if ((espiral.TamañoM * espiral.TamañoN) == Text_encryption.Count) { break; }
                        for (int i = limites[1]-1; i >= y; i--)
                        {
                            Text_encryption.Add(matriz[x, i]);
                        }
                        x++; 
                        limites[0]--; limites[1]--;
                    }
                    break;

                case "vertical":

                    while ((espiral.TamañoM * espiral.TamañoN)> Text_encryption.Count)
                    {
                        for (int i = y; i <= limites[1]; i++)
                        {
                            Text_encryption.Add(matriz[x, i]);
                        }
                        x++;
                        if ((espiral.TamañoM * espiral.TamañoN) == Text_encryption.Count) { break; }
                        for (int i = x; i <= limites[0]; i++)
                        {
                            Text_encryption.Add(matriz[x, limites[1]]);
                        }
                        limites[1]--;
                        if ((espiral.TamañoM * espiral.TamañoN) == Text_encryption.Count) { break; }
                        for (int i = limites[1]; i >= y; i--)
                        {
                            Text_encryption.Add(matriz[limites[0], i]);
                        }
                        limites[0]--;
                        if ((espiral.TamañoM * espiral.TamañoN) == Text_encryption.Count) { break; }
                        for (int i = limites[0]; i >= x; i--)
                        {
                            Text_encryption.Add(matriz[i, y]);
                        }
                        y++;
                    }
                    break;
            }
            using (var writeStream1 = new FileStream(Server.MapPath("~/Archivo") + "/" + System.IO.Path.GetFileNameWithoutExtension(espiral.NombreArchivo) + ".Ecif", FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream1))
                {
                    foreach (var item in Text_encryption)
                    {
                        writer.Write(Convert.ToByte(item));
                    }
                }
            }
        }

        public void Deencrypt(EspiralViewModel espiral)
        {
            var bufferLength = 750;
            var path = Path.Combine(Server.MapPath("~/Archivo"), espiral.NombreArchivo);

            var byteBuffer = new byte[320000000];
            List<string> text_archivocifrado = new List<string>();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    byteBuffer = reader.ReadBytes(bufferLength);

                    foreach (var item in byteBuffer)
                    {
                        text_archivocifrado.Add(Convert.ToString(item));
                    }
                }
            }
            if ((espiral.TamañoM * espiral.TamañoN) < text_archivocifrado.Count)
            {
                decimal division = text_archivocifrado.Count / espiral.TamañoM;
                espiral.TamañoN = (int)Math.Ceiling(division);
            }

            string[,] matriz = new string[espiral.TamañoN, espiral.TamañoM];
            int x = 0; int y = 0;
            int[] limites = { espiral.TamañoN - 1, espiral.TamañoM - 1 };

            switch (espiral.DireccionRecorrido)
            {
                case "horizontal":
                    while (text_archivocifrado.Count > 0)
                        {
                            for (int i = y; i <= limites[1]; i++)
                            {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[x, i] = text_archivocifrado.First();
                                text_archivocifrado.Remove(text_archivocifrado.First());
                            }
                            x++;

                            
                            for (int i = x; i <= limites[0]; i++)
                            {
                                if (text_archivocifrado.Count == 0) { break; }
                                matriz[x, limites[1]] = text_archivocifrado.First(); 
                                text_archivocifrado.Remove(text_archivocifrado.First());
                            }
                            limites[1]--;

                            
                            for (int i = limites[1]; i >= y; i--)
                            {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[limites[0], i] = text_archivocifrado.First();
                                text_archivocifrado.Remove(text_archivocifrado.First());
                            }
                            limites[0]--;

                            
                            for (int i = limites[0]; i >= x; i--)
                            {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[i, y]=  text_archivocifrado.First();
                                text_archivocifrado.Remove(text_archivocifrado.First());
                            }
                            y++;
                        }
                    break;

                case "vertical":

                    while (text_archivocifrado.Count > 0)
                    {
                        for (int i = x; i <= limites[0]; i++)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[i, y] =  text_archivocifrado.First() ;
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        y++;
                        

                        for (int i = y; i <= limites[1]; i++)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[limites[0], i]= text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        for (int i = limites[0] - 1; i >= x; i--)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[i, limites[1]] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }

                        for (int i = limites[1] - 1; i >= y; i--)
                        {
                            if (text_archivocifrado.Count == 0) { break; }
                            matriz[x, i] = text_archivocifrado.First();
                            text_archivocifrado.Remove(text_archivocifrado.First());
                        }
                        x++;
                        limites[0]--; limites[1]--;
                    }
                    break;
            }
            List<string> texto = new List<string>();
            for (int i = 0; i < espiral.TamañoN; i++)
            {
                for (int j = 0; j < espiral.TamañoM; j++)
                {
                    if (matriz[i, j] == null || matriz[i, j] =="" || matriz[i,j] =="42")
                    {
                    }
                    else
                    {
                        texto.Add(matriz[i, j]);
                    }
                    
                }
            }


            using (var writeStream1 = new FileStream(Server.MapPath("~/Archivo") + "/" + System.IO.Path.GetFileNameWithoutExtension(espiral.NombreArchivo) + ".descif", FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream1))
                {
                    foreach (var item in texto)
                    {
                        writer.Write(Convert.ToByte(item));
                    }
                }
            }
        
        }
    }
}
