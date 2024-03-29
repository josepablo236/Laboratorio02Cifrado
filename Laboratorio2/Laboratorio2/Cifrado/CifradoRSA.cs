﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Numerics;

namespace Laboratorio2.Cifrado
{
    public class CifradoRSA
    {
        const int bufferLength = 1000;
        public static int e;
        List<string> Text_archivo = new List<string>();

        public void GenerarLlaves(int numero1, int numero2, string FilePath)
        {
            var p = numero1;
            var q = numero2;
            //n
            var n = p * q;
            //Calcular Q(n)
            var QN = (p - 1) * (q - 1);
            //calcular e
            int count; int count1;
            for (var i = 2; i < QN; i++)
            {
                count = MCD(i, n);
                count1 = MCD(i, QN);
                if (count == 1 && count1 == 1)
                {
                    e = i;
                    break;
                }
            }

            var tempo = 0;
            //Valor D
            //Calcular d
            int d = 2;
            do
            {
                d++;
                tempo = (d * e) % QN;
            } while (tempo != 1);

                //Lo deje como .K por que si lo pongo como .Key mi compu lo agarra como su fuera una presentacion de KeyNote
                using (var writeStream1 = new FileStream(FilePath + "/" + "Private.Key", FileMode.OpenOrCreate))
            {
                using (var writer = new StreamWriter(writeStream1))
                {
                    writer.Write(e.ToString() + "," + n.ToString());
                 //   writer.Write( "463," + n.ToString());
                }
            }
            using (var writeStream2 = new FileStream(FilePath + "/" + "Public.Key", FileMode.OpenOrCreate))
            {
                using (var writer2 = new StreamWriter(writeStream2))
                {
                    writer2.Write(d.ToString() + "," + n.ToString());
                    //writer2.Write("7," + n.ToString());
                }
            }
        }
        //Calcular d
        public int CalcularD(int QN1, int QN2, int e, int valor, int QNOriginal)
        {
            var div = QN1 / e;
            var multiplicador1 = e * div;
            var multiplicador2 = valor * div;
            var resultado1 = QN1 - multiplicador1;
            var resultado2 = QN2 - multiplicador2;

            if (resultado2 < 0)
            {
                resultado2 = QNOriginal % resultado2;
            }
            if (resultado1 == 1)
            {
                return resultado2;
            }
            else
            {
                QN1 = e;
                e = resultado1;
                QN2 = valor;
                valor = resultado2;
                return CalcularD(QN1, QN2, e, valor, QNOriginal);
            }
        }

        public int MCD(int a, int b)
        {
            int res;
            do
            {
                res = b;
                b = a % b;
                a = res;
            }
            while (b != 0);

            return res;
        }

        public void LeerTxt(string path1, string path2, string FilePath, string fileName)
        {
            System.IO.StreamReader lector = new System.IO.StreamReader(path2);
            var llave = 0;
            var N = 0;
            while (!lector.EndOfStream)
            {
                var linea = lector.ReadLine();
                var valores = linea.Split(Convert.ToChar(","));
                llave = Convert.ToInt32(valores[0]);
                N = Convert.ToInt32(valores[1]);
            }
            byte[] array = BitConverter.GetBytes(N);
            int setSize = Convert.ToInt32(Math.Ceiling(Math.Log(N, 256)));
            var pathCif = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + ".rsacif");
            List<Byte> text_archivocifrado = new List<Byte>();
            using (var stream = new FileStream(path1, FileMode.Open))
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
                                    BigInteger Cifrado = BigInteger.ModPow(item, Convert.ToInt32(llave), N);
                                    string cifrado_binario = Convert.ToString((int)(Cifrado), 2);
                                    string bloquecifrado = cifrado_binario.PadLeft(setSize*8, '0');
                                    while (bloquecifrado.Length != 0)
                                    {
                                        writer.Write(Convert.ToByte(bloquecifrado.Substring(0, 8), 2));
                                        bloquecifrado = bloquecifrado.Remove(0, 8);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void LeerCifrado(string path1, string path2, string FilePath, string fileName)
        {
            System.IO.StreamReader lector = new System.IO.StreamReader(path2);
            var llave = 0;
            var N = 0;
            while (!lector.EndOfStream)
            {
                var linea = lector.ReadLine();
                var valores = linea.Split(Convert.ToChar(","));
                llave = Convert.ToInt32(valores[0]);
                N = Convert.ToInt32(valores[1]);
            }
            int setSize = Convert.ToInt32(Math.Ceiling(Math.Log(N, 256)));
            var pathCif = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + ".descif");
            List<Byte> text_archivocifrado = new List<Byte>();
            using (var stream = new FileStream(path1, FileMode.Open))
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
                                byteBuffer = reader.ReadBytes(bufferLength*setSize);
                                int contador = 1;
                                string bloque = "";
                                foreach (var item in byteBuffer)
                                {
                                    bloque += Convert.ToString((int)(item), 2).PadLeft(8,'0');
                                    if (contador % setSize == 0)
                                    {
                                        int numero = Convert.ToInt32(bloque, 2);
                                        int resultado = Cifrar(numero, llave,N);
                                        writer.Write(Convert.ToByte(resultado)); 
                                        bloque = "";
                                    }
                                    contador++;
                                }
                            }
                        }
                    }
                }
            }
        }
        public int Cifrar(int letra, int llave, int N)
        {
            var numero_base = letra % N;
            var multiplicacion = 1;
            for (var i = 0; i < llave; i++)
            {
                multiplicacion = (multiplicacion * numero_base) % N;
            }
            var cifrado = Convert.ToInt32(multiplicacion);
            return cifrado;
        }
    }
}
        