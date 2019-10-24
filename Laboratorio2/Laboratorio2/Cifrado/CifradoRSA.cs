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
			for (var i=2; i<QN; i++)
            {
                count = MCD(i, n);
				count1 = MCD(i, QN);
				if (count == 1 && count1 == 1)
				{
					e = i;
					break;
				}
            }
            
            //Calcular d
            var d = 1;
            var tempo = 0;
            do
            {
                d++;
                tempo = (d * e) % n;
            } while (tempo != 1);
            d += QN;
            //Lo deje como .K por que si lo pongo como .Key mi compu lo agarra como su fuera una presentacion de KeyNote
            using (var writeStream1 = new FileStream(FilePath + "/"  + "Private.Key", FileMode.OpenOrCreate))
                {
                    using (var writer = new StreamWriter(writeStream1))
                    {
                         writer.Write(d.ToString() + "," + n.ToString());
                    }
                }
            using (var writeStream2 = new FileStream(FilePath + "/" + "Public.Key", FileMode.OpenOrCreate))
            {
                using (var writer2 = new StreamWriter(writeStream2))
                {
                    writer2.Write(e.ToString() + "," + n.ToString());
                }
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

        public void LeerArchivo(string path1, string path2, string FilePath, string fileName)
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
                                    var enbyte = Convert.ToInt32(byteCifrado, 2);
                                    writer.Write(Convert.ToByte(enbyte));
                                }
                            }
                        }
                    }
                }
            }
        }

        public string Cifrar(int letra, int llave, int N)
        {
            var c = Math.Pow(letra, llave);
            var resultado = c % N;
            var cifrado = resultado.ToString();
            return cifrado;
        }
    }
    
}