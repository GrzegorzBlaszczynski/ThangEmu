using System.Net.Sockets;
using System.Net;
using System.Text;
using ThangEmu;


    int port = 20000;
    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
    TcpListener server = new TcpListener(localAddr, port);

    server.Start();
    Console.WriteLine("Emu is running.");

    while (true)
    {
        TcpClient client = await server.AcceptTcpClientAsync();
        Console.WriteLine("Client connected.");

        // Obsługa klienta w osobnym zadaniu
        _ = HandleClientAsync(client);
    }



static byte[] HexStringToByteArray(string hex)
{
    if (hex.Length % 2 != 0)
        throw new ArgumentException("Wrong byte array structure");

    int length = hex.Length / 2;
    byte[] bytes = new byte[length];

    for (int i = 0; i < length; i++)
    {
        string byteValue = hex.Substring(i * 2, 2);
        bytes[i] = Convert.ToByte(byteValue, 16);
    }

    return bytes;
}



async Task HandleClientAsync(TcpClient client)
{
    byte[] buffer = new byte[1024];
    NetworkStream stream = client.GetStream();

    try
    {
        while (client.Connected)
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            if (bytesRead == 0)
            {
                Console.WriteLine("Disconnected");
                break;
            }

            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"recived: {receivedData}");


            PacketEntryRead read = new PacketEntryRead(buffer);


            if (read.opCode == 0x039D)
            {
                LoginPacket login = new LoginPacket(buffer);
                await stream.WriteAsync(new byte[] { 0x65, 0xea, 0x04, 0x00 }, 0, 4);
                byte[] charPacket = HexStringToByteArray("52460D0077ED8ED8F7EC1DE6401E2708007700000063EA070077770064EA0C004B419C8C3E41E640122778027600070174007B000701000088FF7600760077D09D467780EDC57700000077000000FE137F001C456D751272776177007700000077007700000077007B008E2A00007E520000770000005EA00000770000003EEE00002E1501001E3C01007700000077000000EEB1010077000000777B000701000088FF750076007AA4B54642B0FEC57700000077000000FE137200167327661077007700000077007700000077007B008E2A00007E520000770000005EA00000770000003EEE00002E1501001E3C01007700000077000000EEB1010077000000777C000701000088FF7400760005B0CC454CB808C67700000077000000FE137B0016612761166161611661616177007700000077007700000077007B008E2A00007E520000770000005EA00000770000003EEE00002E1501001E3C01007700000077000000EEB101007700000077740077007700E040ED99993FBACC8C40ED9919407700C0401166664077007300770077009242770074427700B24275000F040000770000003B040000770000007C0076001200770000007700004077000000770000007700000076000000770077007700770077007700A0407700803F77000040770000407700A04077004040770073007700E0996B427F004E42ABDDA64276003B00000077000000F3000000770000007C0076001200770000007700004077000000770000007700000077000000770077007700770077007700A0407700803F77000040770000407700A0407700404077007300770077007C42770060427700AC427600770000007700000077000000770000007C0076001200770000007700004077000000770000007700000077000000770077007700770077000000");
                await stream.WriteAsync(charPacket, 0, charPacket.Length);

                Console.WriteLine($"Logged as {login.Login}");
            }

            string response = $"";
            byte[] responseData = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseData, 0, responseData.Length);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    finally
    {
        client.Close();
    }
}