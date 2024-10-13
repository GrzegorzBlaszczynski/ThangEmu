using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThangEmu.Packets.Tools;

namespace ThangEmu.Packets.Client
{
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
            Login = Encoding.ASCII.GetString(PacketTools.XORArrays(PacketTools.GetBytesAtIndex(packet, i, LoginLen)));
            i += LoginLen;
            PasswordLen = BitConverter.ToInt16(PacketTools.XORArrays(PacketTools.GetBytesAtIndex<short>(packet, i)));
            i += 2;
            Pasword = Encoding.ASCII.GetString(PacketTools.XORArrays(PacketTools.GetBytesAtIndex(packet, i, PasswordLen)));
        }
    }
}
