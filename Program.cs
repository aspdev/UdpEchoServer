using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                UdpClient udpServer = new UdpClient(24000);
                udpServer.BeginReceive(OnCompleteReceive, udpServer);

                Console.WriteLine("Listening for incoming requests ...");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }

            Console.ReadLine();

        }

        private static void OnCompleteReceive(IAsyncResult asyncResult)
        {
            try
            {
                UdpClient udpServer = (UdpClient)asyncResult.AsyncState;
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedBytes = udpServer.EndReceive(asyncResult, ref remoteEndPoint);

                Console.WriteLine($"Message received from {remoteEndPoint}: " +
                                  $"{Encoding.UTF8.GetString(receivedBytes)}");

                udpServer.BeginSend(receivedBytes, receivedBytes.Length, remoteEndPoint, OnCompleteSend, udpServer);
                udpServer.BeginReceive(OnCompleteReceive, udpServer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }
            

        }

        private static void OnCompleteSend(IAsyncResult asyncResult)
        {
            UdpClient udpServer = (UdpClient) asyncResult.AsyncState;
            int numberOfBytesSent = udpServer.EndSend(asyncResult);
            Console.WriteLine($"Echoed {numberOfBytesSent} bytes to remote endpoint");
            
        }
    }
}
