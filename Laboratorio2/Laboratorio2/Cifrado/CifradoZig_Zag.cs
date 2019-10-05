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


        public void Descifrar(string fileName, string path, string FilePath, int clave)
        {
            var pathDes = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + "DescifradoZ.txt");
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var totalC = reader.BaseStream.Length;
                    var arriba = 0;
                    var abajo = 0;
                    double m = (2 * (Convert.ToDouble(clave)) + Convert.ToDouble(totalC) - 3) / (2 * (Convert.ToDouble(clave)) - 2);
                    var mRedondeada = Convert.ToInt32(Math.Round(m));
                    if (Convert.ToDouble(mRedondeada) > m)
                    {
                        arriba = Convert.ToInt32(Math.Floor(m));
                        abajo = arriba;
                    }
                    else
                    {
                        arriba = Convert.ToInt32(Math.Floor(m));
                        abajo = arriba - 1;
                    }
                    var cantIntermedios = clave - 2;
                    var intermedios = 2 * (abajo);
                    var filas = new string[clave];
                    var todoIntermedio = new List<string>();
                    var byteBuffer = new char[bufferLength];
                    var textfromfile = new List<string>();
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadChars(bufferLength);
                        foreach (var item in byteBuffer)
                        {
                            textfromfile.Add(item.ToString());
                        }
                    }
                    //Incerta los primeros caracteres a la primer fila
                    for(int i = 0; i < arriba; i++)
                    {
                        filas[0] += textfromfile[i];
                    }
                    //Inserta los ultimos caracteres a la ultima fila
                    for (int i = textfromfile.Count-abajo; i < textfromfile.Count; i++)
                    {
                        filas[clave-1] += textfromfile[i];
                    }
                    //Agarro todos los que no estan ni en la primera ni en la ultima
                    for(int i = arriba; i < (textfromfile.Count - abajo); i++)
                    {
                        todoIntermedio.Add(textfromfile[i]);
                    }

                    var division = Convert.ToDouble(todoIntermedio.Count) / Convert.ToDouble(cantIntermedios);
                    var nivelesConUnoMas = todoIntermedio.Count % cantIntermedios;
                    var caracteresxFila = Math.Floor(division);
                    var cantidadUnomas = todoIntermedio.Count - (nivelesConUnoMas * caracteresxFila);
                    var cantidadNormales = todoIntermedio.Count - cantidadUnomas;
                    if (division > caracteresxFila)
                    {
                        var posicionFilas = 1;
                        var posicionIntermedios = 0;
                        while (posicionFilas <= cantIntermedios-nivelesConUnoMas)
                        {
                            for (int i = 0; i < caracteresxFila; i++)
                            {
                                filas[posicionFilas] += todoIntermedio[posicionIntermedios];
                                posicionIntermedios++;
                            }
                            posicionFilas++;
                        }
                        while (posicionFilas <= cantIntermedios)
                        {
                            for (int i = 0; i < caracteresxFila+1; i++)
                            {
                                filas[posicionFilas] += todoIntermedio[posicionIntermedios];
                                posicionIntermedios++;
                            }
                            posicionFilas++;
                        }
                    }
                    if (division <= caracteresxFila)
                    {
                        var posicionFilas = 1;
                        var posicionIntermedios = 0;
                        while (posicionFilas <= nivelesConUnoMas)
                        {
                            for (int i = 0; i < caracteresxFila + 1; i++)
                            {
                                filas[posicionFilas] += todoIntermedio[posicionIntermedios];
                                posicionIntermedios++;
                            }
                            posicionFilas++;
                        }
                        while(posicionFilas <= cantIntermedios)
                        {
                            for (int i = 0; i < caracteresxFila; i++)
                            {
                                filas[posicionFilas] += todoIntermedio[posicionIntermedios];
                                posicionIntermedios++;
                            }
                            posicionFilas++;
                        }
                    }
                    var x = 0;
                    var textoDescifrado = new List<string>();

                    while (textoDescifrado.Count < textfromfile.Count)
                    {
                        if (x == 0)
                        {
                            for (int i = 0; i < clave; i++)
                            {
                                if (textoDescifrado.Count == textfromfile.Count)
                                {
                                    break;
                                }
                                else
                                {
                                    textoDescifrado.Add(filas[i].First().ToString());
                                    filas[i] = filas[i].Substring(1);
                                }
                            }
                            x = clave;
                        }
                        if (x == clave)
                        {
                            for (int i = clave - 2; i > 0; i--)
                            {
                                if (textoDescifrado.Count == textfromfile.Count)
                                {
                                    break;
                                }
                                else
                                {
                                    textoDescifrado.Add(filas[i].First().ToString());
                                    filas[i] = filas[i].Substring(1);
                                }
                            }
                            x = 0;
                        }
                    }

                    using (var writeStream = new FileStream(pathDes, FileMode.OpenOrCreate))
                    {
                        using (var writer = new StreamWriter(writeStream))
                        {
                            foreach (var item in textoDescifrado)
                            {
                                if(item != "$")
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