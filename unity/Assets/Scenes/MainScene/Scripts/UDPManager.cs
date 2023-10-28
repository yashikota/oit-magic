using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPManager : MonoBehaviour
{
    const int receivePort = 5000;
    const int sendPort = 5001;
    static UdpClient udp;
    Thread thread;
    private string coordinate;

    void Start()
    {
        udp = new UdpClient();
        udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        udp.Client.Bind(new IPEndPoint(IPAddress.Any, receivePort));

        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }

    void OnApplicationQuit()
    {
        thread.Abort();
        udp.Close();
    }

    private void ThreadMethod()
    {
        while (true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            coordinate = Encoding.UTF8.GetString(data);
        }
    }

    public string GetCoordinate()
    {
        return coordinate;
    }

    public void SendReset()
    {
        byte[] data = Encoding.UTF8.GetBytes("reset");
        udp.Send(data, data.Length, "localhost", sendPort);
    }
}
