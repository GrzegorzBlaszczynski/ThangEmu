using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThangEmu.Packets.Tools
{
    public class PacketTools
    {
        static byte[] password = new byte[] { 0x77, 0x00, 0x00, 0x00 };

        public static unsafe byte[] GetBytesAtIndex<T>(byte[] data, int index)
        {
            int count = sizeof(T);

            if (index < 0 || index + count > data.Length)
            {
                throw new IndexOutOfRangeException("Index Out Of Array");
            }

            byte[] result = new byte[count];

            Array.Copy(data, index, result, 0, count);

            return result;
        }
        public static unsafe byte[] GetBytesAtIndex(byte[] data, int index, int count)
        {


            if (index < 0 || index + count > data.Length)
            {
                throw new IndexOutOfRangeException("Index Out Of Array");
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
}
