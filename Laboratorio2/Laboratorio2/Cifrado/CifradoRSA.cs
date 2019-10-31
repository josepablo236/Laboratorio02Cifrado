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
            var d = CalcularD(QN, QN, e, 1, QN);
            //Lo deje como .K por que si lo pongo como .Key mi compu lo agarra como su fuera una presentacion de KeyNote
            using (var writeStream1 = new FileStream(FilePath + "/" + "Private.Key", FileMode.OpenOrCreate))
            {
                using (var writer = new StreamWriter(writeStream1))
                {
                    writer.Write( e.ToString() + "," + n.ToString());
                }
            }
            using (var writeStream2 = new FileStream(FilePath + "/" + "Public.Key", FileMode.OpenOrCreate))
            {
                using (var writer2 = new StreamWriter(writeStream2))
                {
                    writer2.Write(d.ToString() + "," + n.ToString());
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

            var pathCif = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + ".rsacif");

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
                                    //var byteCifrado = Cifrar(item, llave, N);
                                    Text_archivo.Add(Cifrar(item, llave, N).ToString());
                                    // writer.Write(Convert.ToByte(byteCifrado));
                                }
                               var  maxlength = Convert.ToString(Convert.ToInt32(N-1), 2).Length;
                                var binary = StringToBinary(Text_archivo, maxlength);
                                var bytes = StringToBytes(binary);
                                var compresstext = bytes;
                                foreach (var item in compresstext)
                                {
                                    writer.Write(item);
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
                                List<string> text_archivocifrado = new List<string>();
                                byteBuffer = reader.ReadBytes(bufferLength);
                                foreach (var item in byteBuffer)
                                {
                                    //var byteCifrado = Cifrar(item, llave, N);
                                    text_archivocifrado.Add(Convert.ToString(item));
                                    // writer.Write(Convert.ToByte(byteCifrado));
                                }
                                var maxlength = Convert.ToString(Convert.ToInt32(N - 1), 2).Length;
                                var temp = DecimalToBinary(text_archivocifrado);
                                var agrupados = Agrupar(temp, maxlength);
                                Text_archivo.Clear();
                                foreach (var item in agrupados)
                                {
                                    //var byteCifrado = Cifrar(item, llave, N);
                                    Text_archivo.Add(Cifrar(item, llave, N).ToString());
                                    // writer.Write(Convert.ToByte(byteCifrado));
                                }
                                foreach (var item in Text_archivo)
                                {
                                    writer.Write(Convert.ToByte(item));
                                }
                            }
                        }
                    }
                }
            }
        }
        private string StringToBinary(List<string> data, int tamaño)
        {
            string byteList = "";
            foreach (var item in data)
            {
                var temp = Convert.ToString(Convert.ToInt32(item), 2);
                if (temp.Length < tamaño)
                {
                    temp = temp.PadLeft(tamaño, '0');
                    byteList += temp;
                }
                else { byteList += temp; }
            }
            return byteList;
        }

        private List<byte> StringToBytes(string data)
        {
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < data.Length; i += 8)
            {
                if (data.Length - i >= 8)
                {
                    byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
                }
                else
                {
                    string temp = data.Substring(i, data.Length - i);
                    temp = temp.PadLeft(8, '0');
                    byteList.Add(Convert.ToByte(temp, 2));
                }
            }
            return byteList;
        }
        //Convertir a Binario
        static string DecimalToBinary(List<string> n)
        {
            string binario = "";
            foreach (var item in n)
            {
                var temp = Convert.ToString(Convert.ToInt32(item), 2);
                if (temp.Length == 8)
                {
                    binario += temp;
                }
                else
                {
                    binario += temp.PadLeft(8, '0');
                }

            }
            return binario;
        }
        public List<int> Agrupar(string binario, int tamaño)
        {
            List<int> compress = new List<int>();
            for (int i = 0; i < binario.Length; i += tamaño)
            {
                if (i + tamaño <= binario.Length)
                {
                    compress.Add(Convert.ToInt32(binario.Substring(i, tamaño), 2));
                }
                else
                {
                    compress.Add(Convert.ToInt32(binario.Substring(i, binario.Length - i), 2));
                    break;
                }
            }
            return compress;
        }


        public int Cifrar(int letra, int llave, int N)
        {
            //var c = Math.Pow(letra % N, llave);
            var numero_base = letra % N;
            var multiplicacion = 1;
            for (var i = 0; i < llave; i++)
            {
                multiplicacion = (multiplicacion * numero_base) % N;
            }
            //var resultado = c % N;
            var cifrado = Convert.ToInt32(multiplicacion);
            return cifrado;
        }
    }

}
