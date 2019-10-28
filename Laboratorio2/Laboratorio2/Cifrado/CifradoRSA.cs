using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Laboratorio2.Cifrado
{
    public class CifradoRSA
    {
        const int bufferLength = 1000;
        public static int e;


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
                if ( count1 == 1 && count == 1)
                {
                    e = i;
                    break;
                }
            }
            //Valor D
            var d = CalcularD(QN, QN, e, 1, QN);

            //Escribir llave privada
            using (var writeStream1 = new FileStream(FilePath + "/" + "Private.Key", FileMode.OpenOrCreate))
            {
                using (var writer = new StreamWriter(writeStream1))
                {
                    writer.Write( "33," + n.ToString());
                }
            }
            //Escribir llave publica
            using (var writeStream2 = new FileStream(FilePath + "/" + "Public.Key", FileMode.OpenOrCreate))
            {
                using (var writer2 = new StreamWriter(writeStream2))
                {
                    writer2.Write("17," + n.ToString());
                }
            }
        }

        //MCD
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

        //Cifrado
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

            var pathCif = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + ".scif");

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
                                    var byteCifrado = Cifrar(item, llave, N);
                                    writer.Write(Convert.ToByte(byteCifrado));
                                }
                            }
                        }
                    }
                }
            }
        }

        //Descifrado
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

            var pathCif = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + ".descif");

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
                                    var byteCifrado = Cifrar(item, llave, N);
                                    var bc = Convert.ToInt32(byteCifrado);
                                    writer.Write(Convert.ToByte(bc));
                                }
                            }
                        }
                    }
                }
            }
        }

        public int Cifrar(int letra, int llave, int N)
        {
            //var c = Math.Pow(letra % N, llave);
            var numero_base = letra % N;
            var multiplicacion = 1;
            for (var i = 0; i < llave; i++)
            {
                multiplicacion = (multiplicacion * numero_base) %N;
            }
            //var resultado = c % N;
            var cifrado = Convert.ToInt32(multiplicacion);
            return cifrado;
        }
    }
}
