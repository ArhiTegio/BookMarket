using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Telnet
{
    /// <summary>
    /// A connected client
    /// </summary>
    public sealed class ClientInfo
    {
        readonly string m_ClientID;
        readonly Socket m_ClientSocket;
        readonly byte[] m_Buffer;

        public string ClientID { get { return m_ClientID; } }
        public Socket ClientSocket { get { return m_ClientSocket; } }
        public byte[] Buffer { get { return m_Buffer; } }

        public ClientInfo(string id, Socket clientSocket, byte[] buffer)
        {
            m_ClientID = id;
            m_ClientSocket = clientSocket;
            m_Buffer = buffer;
        }
    }
}
