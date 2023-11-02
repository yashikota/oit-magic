using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPManager : MonoBehaviour
{
    private const int ReceivePort = 5000;
    private const int SendPort = 5001;
    private static UdpClient _udp;
    private Thread thread;
    private string coordinate;

    private void Start()
    {
        _udp = new UdpClient();
        _udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        _udp.Client.Bind(new IPEndPoint(IPAddress.Any, ReceivePort));

        thread = new Thread(ThreadMethod);
        thread.Start();
    }

    private void OnApplicationQuit()
    {
        SendFinish();
        thread.Abort();
        _udp.Close();
    }

    private void ThreadMethod()
    {
        while (true)
        {
            IPEndPoint remoteEp = null;
            var data = _udp.Receive(ref remoteEp);
            coordinate = Encoding.UTF8.GetString(data);
        }
    }

    public string GetCoordinate()
    {
        return coordinate;
    }

    public static void SendReset()
    {
        var data = Encoding.UTF8.GetBytes("reset");
        _udp.Send(data, data.Length, "localhost", SendPort);
    }

    private void SendFinish()
    {
        var data = Encoding.UTF8.GetBytes("finish");
        _udp.Send(data, data.Length, "localhost", SendPort);
    }
}
