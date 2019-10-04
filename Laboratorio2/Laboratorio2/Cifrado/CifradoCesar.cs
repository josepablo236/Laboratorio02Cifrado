using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laboratorio2.Cifrado
{
    public class CifradoCesar
    {

        const int bufferLength = 1000;
        public void Cifrar(string fileName, string path, string FilePath, List<byte> clave)
        {
            var pathCif = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + ".Ccif");
            var abcedarioBytes = new Dictionary<int, byte>();
            var abcedarioOrdenado = new Dictionary<int, byte>();
            var textoCifrado = new List<byte>();
            var abecedario = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz";
            var i = 0;
            var j = 0;
            foreach (var item in abecedario)
            {
                abcedarioBytes.Add(i, Convert.ToByte(item));
                i++;
            }
            foreach (var item in clave)
            {
                abcedarioOrdenado.Add(j, item);
                j++;
            }
            foreach (var item in abcedarioBytes)
            {
                if (!abcedarioOrdenado.ContainsValue(item.Value))
                {
                    abcedarioOrdenado.Add(j, item.Value);
                    j++;
                }
            }
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    using (var writeStream = new FileStream(pathCif, FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            var byteBuffer = new byte[bufferLength];
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                byteBuffer = reader.ReadBytes(bufferLength);
                                foreach (var item in byteBuffer)
                                {
                                    if (!abcedarioBytes.ContainsValue(item))
                                    {
                                        writer.Write(item);
                                        //textoCifrado.Add(item);
                                    }
                                    else
                                    {
                                        var indiceAbc = abcedarioBytes.FirstOrDefault(x => x.Value == item).Key;
                                        var caracterNuevo = abcedarioOrdenado.FirstOrDefault(x => x.Key == indiceAbc).Value;
                                        writer.Write(caracterNuevo);
                                        //textoCifrado.Add(caracterNuevo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Descifrar(string fileName, string path, string FilePath, List<byte> clave)
        {
            var pathDes = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + "DescifradoC.txt");
            var abcedarioBytes = new Dictionary<int, byte>();
            var abcedarioOrdenado = new Dictionary<int, byte>();
            var textoCifrado = new List<byte>();
            var abecedario = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz";
            var i = 0;
            var j = 0;
            foreach (var item in abecedario)
            {
                abcedarioBytes.Add(i, Convert.ToByte(item));
                i++;
            }
            foreach (var item in clave)
            {
                abcedarioOrdenado.Add(j, item);
                j++;
            }
            foreach (var item in abcedarioBytes)
            {
                if (!abcedarioOrdenado.ContainsValue(item.Value))
                {
                    abcedarioOrdenado.Add(j, item.Value);
                    j++;
                }
            }
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    using (var writeStream = new FileStream(pathDes, FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            var byteBuffer = new byte[bufferLength];
                            while (reader.BaseStream.Position != reader.BaseStream.Length)
                            {
                                byteBuffer = reader.ReadBytes(bufferLength);
                                foreach (var item in byteBuffer)
                                {
                                    if (!abcedarioBytes.ContainsValue(item))
                                    {
                                        writer.Write(item);
                                    }
                                    else
                                    {
                                        var indiceAbc = abcedarioOrdenado.FirstOrDefault(x => x.Value == item).Key;
                                        var caracterNuevo = abcedarioBytes.FirstOrDefault(x => x.Key == indiceAbc).Value;
                                        writer.Write(caracterNuevo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}