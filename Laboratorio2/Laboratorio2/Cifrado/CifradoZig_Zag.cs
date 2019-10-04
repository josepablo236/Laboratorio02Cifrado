using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Laboratorio2.Cifrado
{
    public class CifradoZig_Zag
    {
        const int bufferLength = 1000;
        public void Cifrar(string fileName, string path, string FilePath, int clave)
        {
            var pathCif = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + ".Zcif");
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var totalC = reader.BaseStream.Length;
                    double m = (2 * (Convert.ToDouble(clave)) + Convert.ToDouble(totalC) - 3) / (2 * (Convert.ToDouble(clave)) - 2);
                    var arriba = Math.Ceiling(m);
                    //var abajo = arriba - 1;
                    //var cantIntermedios = clave - 2;
                    //var intermedios = 2 * (abajo);
                    var filas = new string[clave];
                    var byteBuffer = new char[bufferLength];
                    var x = 0;
                    var textfromfile = new List<string>();
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadChars(bufferLength);
                        foreach (var item in byteBuffer)
                        {
                            textfromfile.Add(item.ToString());
                        }
                    }
                    while (textfromfile.Count > 0)
                    {
                        if (x == 0)
                        {
                            for (int i = 0; i < clave; i++)
                            {
                                if (textfromfile.Count == 0)
                                {
                                    filas[i] += "$";
                                }
                                else
                                {
                                    filas[i] += textfromfile.First();
                                    textfromfile.Remove(textfromfile.First());
                                }
                            }
                            x = clave;
                        }
                        if (x == clave)
                        {
                            for (int i = clave - 2; i > 0; i--)
                            {
                                if (textfromfile.Count == 0)
                                {
                                    filas[i] += "$";
                                }
                                else
                                {
                                    filas[i] += textfromfile.First();
                                    textfromfile.Remove(textfromfile.First());
                                }
                            }
                            x = 0;
                        }
                    }
                    if (filas[0].Length < arriba)
                    {
                        filas[0] += "$";
                    }
                    //var texto = filas;
                    using (var writeStream = new FileStream(pathCif, FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            for (int i = 0; i < filas.Length; i++)
                            {
                                foreach (var item in filas[i])
                                {
                                    writer.Write(item);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}