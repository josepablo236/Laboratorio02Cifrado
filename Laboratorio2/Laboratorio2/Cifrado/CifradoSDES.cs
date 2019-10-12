using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Laboratorio2.Cifrado
{
    public class CifradoSDES
    {
        const int bufferLength = 1000;
        public void Cifrar(string fileName, string path, string FilePath, int numero, string pathP10, string pathP8, string pathP4, string pathIP, string pathEP)
        {
            var permutaciones = LeerPermutaciones(pathP10, pathP8, pathP4, pathIP, pathEP);
            var P10 = permutaciones[0];
            var P8 = permutaciones[1];
            var P4 = permutaciones[2];
            var IP = permutaciones[3];
            var EP = permutaciones[4];

            var llaves = GenerarLlaves(numero, P10, P8);
            var llave1 = llaves[0];
            var llave2 = llaves[1];

            var pathCif = Path.Combine(FilePath, Path.GetFileNameWithoutExtension(fileName) + ".scif");

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
                                    var byteCifrado = Cifrado(item, llave1, llave2, P8, P4, IP, EP);
                                    var enbyte = Convert.ToInt32(byteCifrado, 2);
                                    writer.Write(Convert.ToByte(enbyte));
                                }
                            }
                        }
                    }
                }
            }
        }

        public string[] LeerPermutaciones(string pathP10, string pathP8, string pathP4, string pathIP, string pathEP)
        {
            var permutaciones = new string[5];
            //Leer archivos de permutaciones
            System.IO.StreamReader lector1 = new System.IO.StreamReader(pathP10);
            while (!lector1.EndOfStream)
            {
                permutaciones[0] = lector1.ReadLine();
            }
            lector1.Close();
            System.IO.StreamReader lector2 = new System.IO.StreamReader(pathP8);
            while (!lector2.EndOfStream)
            {
                permutaciones[1] = lector2.ReadLine();
            }
            lector2.Close();
            System.IO.StreamReader lector3 = new System.IO.StreamReader(pathP4);
            while (!lector3.EndOfStream)
            {
                permutaciones[2] = lector3.ReadLine();
            }
            lector3.Close();
            System.IO.StreamReader lector4 = new System.IO.StreamReader(pathIP);
            while (!lector4.EndOfStream)
            {
                permutaciones[3] = lector4.ReadLine();
            }
            lector4.Close();
            System.IO.StreamReader lector5 = new System.IO.StreamReader(pathEP);
            while (!lector5.EndOfStream)
            {
                permutaciones[4] = lector5.ReadLine();
            }
            lector5.Close();
            return permutaciones;
        }

        public string[] GenerarLlaves(int numero, string P10, string P8)
        {
            var llaves = new string[2];
            
            //Convertir a binario el numero ingresado
            var numeroBinario = Convert.ToString(numero, 2);
            //Hacer P10
            var permutacion10 = Permutacion10(P10, numeroBinario);
            var division1 = "";
            var division2 = "";
            //Agarrar los primeros 5
            for (int i = 0; i < 5; i++)
            {
                division1 += permutacion10[i];
            }
            //Agarrar los ultimos 5
            for (int i = 5; i < permutacion10.Length; i++)
            {
                division2 += permutacion10[i];
            }
            //Hacer Lshift con cada bloque dividido
            var lsdivision1 = LShift(division1);
            var lsdivision2 = LShift(division2);
            //Union
            var unionLS = lsdivision1 + lsdivision2;
            //Hacer P8
            var llave1 = Permutacion8(P8, unionLS);
            //Hacer Lshift dos veces con cada bloque de division
            var ls1div1 = LShift(lsdivision1);
            var ls2div1 = LShift(ls1div1);
            var ls1div2 = LShift(lsdivision2);
            var ls2div2 = LShift(ls1div2);
            //Union
            var unionLS2 = ls2div1 + ls2div2;
            //Hacer P8
            var llave2 = Permutacion8(P8, unionLS2);
            llaves[0] = llave1;
            llaves[1] = llave2;
            return llaves;
        }

        //Metodo para cifrar
        public string Cifrado(int item, string llave1, string llave2, string P8, string P4, string IP, string EP)
        {
            string[,] S0 = new string[4, 4];
            S0[0, 0] = "01"; S0[0, 1] = "00"; S0[0, 2] = "11"; S0[0, 3] = "10";
            S0[1, 0] = "11"; S0[1, 1] = "10"; S0[1, 2] = "01"; S0[1, 3] = "00";
            S0[2, 0] = "00"; S0[2, 1] = "10"; S0[2, 2] = "01"; S0[2, 3] = "11";
            S0[3, 0] = "11"; S0[3, 1] = "01"; S0[3, 2] = "11"; S0[3, 3] = "10";

            string[,] S1 = new string[4, 4];
            S1[0, 0] = "00"; S1[0, 1] = "01"; S1[0, 2] = "10"; S1[0, 3] = "11";
            S1[1, 0] = "10"; S1[1, 1] = "00"; S1[1, 2] = "01"; S1[1, 3] = "11";
            S1[2, 0] = "11"; S1[2, 1] = "00"; S1[2, 2] = "01"; S1[2, 3] = "00";
            S1[3, 0] = "10"; S1[3, 1] = "01"; S1[3, 2] = "00"; S1[3, 3] = "11";
            var byteCifrado = "";
            //Convertir a binario
            var plainText = Convert.ToString(item, 2);
            plainText = plainText.PadLeft(8, '0');
            //Hacer permutacion IP
            var permutacionIP = PermutacionIP(IP, plainText);
            var div1 = "";
            var div2 = "";
            //Agarrar los primeros 4
            for (int i = 0; i < 4; i++)
            {
                div1 += permutacionIP[i];
            }
            //Agarrar los ultimos 4
            for (int i = 4; i < permutacionIP.Length; i++)
            {
                div2 += permutacionIP[i];
            }
            //Hacer expandir y permutar con div2
            var permutacionEP = PermutacionEP(EP, div2);
            //Hacer XOR con llave1
            var xor1 = XorBins(permutacionEP, llave1).ToCharArray();
            //Hacer Sboxes
            var SBox0 = S0[Convert.ToInt32(xor1[0].ToString() + xor1[3].ToString(), 2), Convert.ToInt32(xor1[1].ToString() + xor1[2].ToString(), 2)];
            var SBox1 = S1[Convert.ToInt32(xor1[4].ToString() + xor1[7].ToString(), 2), Convert.ToInt32(xor1[5].ToString() + xor1[6].ToString(), 2)];
            var unionSboxes = SBox0 + SBox1;
            //Hacer P4
            var permutacionP4 = Permutacion4(P4, unionSboxes);
            //Hacer XOR con div1 de IP
            var xor2 = XorBins(permutacionP4, div1);
            //Unir con div2
            var union1 = xor2 + div2;
            //Hacer SWAP
            union1 = div2 + xor2;
            //Hacer EP con xor2
            var permutacionEP2 = PermutacionEP(EP, xor2);
            //Hacer XOR con llave2
            var xor3 = XorBins(permutacionEP2, llave2).ToCharArray();
            //Dividir en dos y hacer Sboxes
            var SBox20 = S0[Convert.ToInt32(xor3[0].ToString() + xor3[3].ToString(), 2), Convert.ToInt32(xor3[1].ToString() + xor3[2].ToString(), 2)];
            var SBox21 = S1[Convert.ToInt32(xor3[4].ToString() + xor3[7].ToString(), 2), Convert.ToInt32(xor3[5].ToString() + xor3[6].ToString(), 2)];
            //Unir Sboxes
            var union2Sboxes = SBox20 + SBox21;
            //Hacer P4
            var permutacion2P4 = Permutacion4(P4, union2Sboxes);
            //Agarrar el primero del SWAP osea div2 y hacer XOR con P4
            var xor4 = XorBins(div2, permutacion2P4).ToCharArray();
            //Unir xor4 con el segundo del SWAP osea xor2
            var xor4String = new string(xor4);
            var union3 = xor4String + xor2;
            //Hacer IP inversa 
            byteCifrado = PermutacionInversa(IP, union3);


            return byteCifrado;
        }


        //Metodo para hacer permutacion P10
        public string Permutacion10(string P10, string cadena)
        {
            var permutacion10 = "";
            for (int i = 0; i < P10.Length; i++)
            {
                var valor = P10[i].ToString();
                var pos = Convert.ToInt16(valor);
                permutacion10 += cadena[pos].ToString();
            }
            return permutacion10;
        }

        //Metodo para hacer Lshift
        public string LShift(string cadena)
        {
            var ls = cadena.Remove(0, 1) + cadena[0];
            return ls;
        }

        //Metodo para hacer permutacion P8
        public string Permutacion8(string P8, string cadena)
        {
            var permutacion8 = "";
            for (int i = 0; i < P8.Length; i++)
            {
                var valor = P8[i].ToString();
                var pos = Convert.ToInt16(valor);
                permutacion8 += cadena[pos].ToString();
            }
            return permutacion8;
        }

        //Metodo para hacer permutacion P4
        public string Permutacion4(string P4, string cadena)
        {
            var permutacion4 = "";
            for (int i = 0; i < P4.Length; i++)
            {
                var valor = P4[i].ToString();
                var pos = Convert.ToInt16(valor);
                permutacion4 += cadena[pos].ToString();
            }
            return permutacion4;
        }

        //Metodo para hacer permutacion IP
        public string PermutacionIP(string IP, string cadena)
        {
            var permutacionIP = "";
            for (int i = 0; i < IP.Length; i++)
            {
                var valor = IP[i].ToString();
                var pos = Convert.ToInt16(valor);
                permutacionIP += cadena[pos].ToString();
            }
            return permutacionIP;
        }

        //Metodo para hacer permutacion EP
        public string PermutacionEP(string EP, string cadena)
        {
            var permutacionEP = "";
            for (int i = 0; i < EP.Length; i++)
            {
                var valor = EP[i].ToString();
                var pos = Convert.ToInt16(valor);
                permutacionEP += cadena[pos].ToString();
            }
            return permutacionEP;
        }
        //Metodo para hacer XOR
        public string XorBins(string bin1, string bin2)
        {
            string res = string.Empty;
            for (int i = 0; i < bin1.Length; i++)
                res += bin1[i] == bin2[i] ? '0' : '1';
            return res;
        }

        //Metodo para hacer permutacion inversa de IP
        public string PermutacionInversa(string IP, string cadena)
        {
            var resultado = "";
            var permutacionInversa = ""; 
            string ordenado = "01234567";
            for (int i = 0; i < IP.Length; i++)
            {
                permutacionInversa += IP.IndexOf(i.ToString());
            }
            for (int i = 0; i < permutacionInversa.Length; i++)
            {
                var valor = permutacionInversa[i].ToString();
                var pos = Convert.ToInt16(valor);
                resultado += cadena[pos].ToString();
            }
            return resultado;
        }
    }
}