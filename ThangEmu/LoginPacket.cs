using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThangEmu
{
    public class PacketTools
    {
        static byte[] password = new byte[] { 0x77, 0x00, 0x00, 0x00, 0x77, 0x00, 0x00, 0x00, 0x1F, 0x00, 0x00, 0x00, 0x3B, 0x00, 0x00, 0x00, 0x5A, 0x00, 0x00, 0x00, 0x78, 0x00, 0x00, 0x00, 0x97, 0x00, 0x00, 0x00, 0xB5, 0x00, 0x00, 0x00, 0xD4, 0x00, 0x00, 0x00, 0xF3, 0x00, 0x00, 0x00 };

        public static unsafe byte[] GetBytesAtIndex<T>(byte[] data, int index)
        {
            int count = sizeof(T);

            if (index < 0 || index + count > data.Length)
            {
                throw new IndexOutOfRangeException("Indeks lub liczba bajtów poza zakresem tablicy.");
            }

            byte[] result = new byte[count];

            Array.Copy(data, index, result, 0, count);

            return result;
        }
        public static unsafe byte[] GetBytesAtIndex(byte[] data, int index, int count)
        {


            if (index < 0 || index + count > data.Length)
            {
                throw new IndexOutOfRangeException("Indeks lub liczba bajtów poza zakresem tablicy.");
            }

            byte[] result = new byte[count];

            Array.Copy(data, index, result, 0, count);

            return result;
        }

        public static byte[] XORArrays(byte[] data)
        {
            byte[] result = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (byte)(data[i] ^ password[i % password.Length]);
            }

            return result;
        }
    }



    public class PacketEntryRead
    {
        public short opCode;

        public PacketEntryRead(byte[] packet)
        {
            Read(packet);
        }
        public void Read(byte[] packet)
        {
            int i = 0;
            opCode = BitConverter.ToInt16(PacketTools.XORArrays(PacketTools.GetBytesAtIndex<short>(packet, 0)));

        }
    }

    public class LoginPacket
    {
        public short opCode;
        public short unknow1;
        public short unknow2;
        public short LoginLen;
        public string Login;
        public short PasswordLen;
        public string Pasword;
        public int unknow3;



        public LoginPacket(byte[] packet)
        {
            Read(packet);
        }



        public void Read(byte[] packet)
        {
            int i = 0;
            opCode = BitConverter.ToInt16(PacketTools.XORArrays(PacketTools.GetBytesAtIndex<short>(packet, 0)));
            i += 2;
            unknow1 = BitConverter.ToInt16(PacketTools.XORArrays(PacketTools.GetBytesAtIndex<short>(packet, i)));
            i += 2;
            unknow2 = BitConverter.ToInt16(PacketTools.XORArrays(PacketTools.GetBytesAtIndex<short>(packet, i)));
            i += 2;
            LoginLen = BitConverter.ToInt16(PacketTools.XORArrays(PacketTools.GetBytesAtIndex<short>(packet, i)));
            i += 2;
            Login = ASCIIEncoding.ASCII.GetString(PacketTools.XORArrays(PacketTools.GetBytesAtIndex(packet, i, LoginLen)));
            i += LoginLen;
            PasswordLen = BitConverter.ToInt16(PacketTools.XORArrays(PacketTools.GetBytesAtIndex<short>(packet, i)));
            i += 2;
            Pasword = ASCIIEncoding.ASCII.GetString(PacketTools.XORArrays(PacketTools.GetBytesAtIndex(packet, i, PasswordLen)));
        }
    }
}
