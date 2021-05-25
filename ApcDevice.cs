using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Scada.Comm.Devices.ApcUps
{
    public class ApcDevice
    {
        public string ApcUpsDaemonHost = "127.0.0.1";
        public int NisPort = 3551;
        private readonly TcpClient client;
        private readonly int MaxString = 256;

        public ApcDevice()
        {
            client = new TcpClient(ApcUpsDaemonHost, NisPort);
        }

        public List<string> GetStatus()
        {
            var stream = client.GetStream();
            Byte[] command = Encoding.ASCII.GetBytes("status");

            NisNetSend(stream, command);

            var responsesData = new List<string>();
            var receiveBuffer = new Byte[MaxString + 1];

            do {
                var bytes = NisNetRecv(stream, receiveBuffer, receiveBuffer.Length);
                if(bytes <= 0)
                    break;
                responsesData.Add(Encoding.ASCII.GetString(receiveBuffer, 0, bytes));
            } while(stream.DataAvailable);

            stream.Close();
            client.Close();

            return responsesData;
        }
        public string GetParamValue(string paramName)
        {
            var stream = client.GetStream();
            Byte[] command = Encoding.ASCII.GetBytes("status");

            NisNetSend(stream, command);

            var responsesData = String.Empty; 
            var receiveBuffer = new Byte[MaxString + 1];

            do {
                var bytes = NisNetRecv(stream, receiveBuffer, receiveBuffer.Length);
                if(bytes <= 0)
                    break;
                responsesData = Encoding.ASCII.GetString(receiveBuffer, 0, bytes);
                if(responsesData.Contains(paramName))
                    break;
            } while(stream.DataAvailable);

            stream.Close();
            client.Close();

            return responsesData;

        }

        /// Send data over NIS protocol
        private void NisNetSend(NetworkStream stream, Byte[] data)
        {
            // write length of data
            stream.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)data.Length)), 0, 2);
            // write data
            stream.Write(data, 0, data.Length);
        }

        /// Receive data over NIS protocol
        private int NisNetRecv(NetworkStream stream, Byte[] buffer, int maxlen)
        {
            Byte[] localBuffer = new Byte[2];

            var bytes = stream.Read(localBuffer, 0, 2);
            if(bytes != 2)
                throw new Exception("Can't get packet size");

            short pktsize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(localBuffer, 0));

            if(pktsize > maxlen)
                throw new Exception("Packet size > maxlen");
            
            if(pktsize == 0)
                return 0; /* soft EOF */

            /* now read the actual data */
            bytes = stream.Read(buffer, 0, pktsize);

            return bytes; /* return actual length of message */
        }
    }
}