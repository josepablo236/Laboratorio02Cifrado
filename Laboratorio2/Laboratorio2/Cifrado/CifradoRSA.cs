using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Laboratorio2.Cifrado
{
    public class CifradoRSA
    {
        public void GenerarLlaves(int numero1, int numero2, string FilePath)
        {
            var p = numero1;
            var q = numero2;
            //n
            var n = p * q;
            //Calcular Q(n)
            var QN = (p - 1) * (q - 1);
            //calcular e
            int e = 2;int count;
            while (e < QN)
            {
                count = MCD(e, n);
                if (count == 1)
                    break;
                else
                    e++;
            }
            
            //Calcular d
            var d = 1;
            var tempo = 0;
            do
            {
                d++;
                tempo = (d * e) % n;
            } while (tempo != 1);
            d += n;
            using (var writeStream1 = new FileStream(FilePath + "/"  + "Private.KEY", FileMode.OpenOrCreate))
                {
                    using (var writer = new StreamWriter(writeStream1))
                    {
                         writer.Write(d.ToString() + "," + n.ToString());
                    }
                }
            using (var writeStream2 = new FileStream(FilePath + "/" + "Public.KEY", FileMode.OpenOrCreate))
            {
                using (var writer2 = new StreamWriter(writeStream2))
                {
                    writer2.Write(e.ToString() + "," + n.ToString());
                }
            }
        }
        public int MCD(int a, int h)
        {
            int temp=0;
            while (temp != 1)
            {
                temp = a % h;
                if (temp == 0)
                    return h;
                a = h;
                h = temp;
            }
            return 0;
        }
    }
    
}