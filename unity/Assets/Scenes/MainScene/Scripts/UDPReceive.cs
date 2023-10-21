using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
    const int LOCAL_PORT = 5000;
    static UdpClient udp;
    Thread thread;
    private string coordinate;

    void Start()
    {
        udp = new UdpClient();
        udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        udp.Client.Bind(new IPEndPoint(IPAddress.Any, LOCAL_PORT));

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
}
