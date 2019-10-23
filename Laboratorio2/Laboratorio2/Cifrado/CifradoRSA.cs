using System;
using System.Collections.Generic;
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