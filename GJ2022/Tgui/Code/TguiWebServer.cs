using GJ2022.Subsystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GJ2022.Tgui.Code
{
    public class TguiWebServer
    {

        private TcpListener webServerListener;

        //Queue of stuff to send
        private Dictionary<string, string> htmlCache = new Dictionary<string, string>();

        public TguiWebServer(int port)
        {
            //Set this stuff
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Create the listener
            webServerListener = new TcpListener(IPAddress.Any, port);
            //Start the listener
            webServerListener.Start();
            //Start the listening
            new Thread(StartListenerThread).Start();
        }

        public void Browse(string windowId, string html)
        {
            //Put the HTML into a cache
            htmlCache.Add(windowId, html);
            //Open up a browser form
            Thread thread = new Thread(() => OpenBrowser($"http://localhost:5050/{windowId}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void OpenBrowser(string link)
        {
            Application.Run(new TguiBrowserForm(link));
        }

        private void StartListenerThread()
        {
            while (Subsystem.Firing)
            {
                try
                {
                    //Accept a socket
                    Socket connectingSocket = webServerListener.AcceptSocket();
                    //Ignore if not connected
                    if (!connectingSocket.Connected)
                        continue;
                    //TODO: Verify that the connection came from this computer
                    //Start the handler
                    Task.Run(() => HandleTopic(connectingSocket));
                }
                catch (Exception e)
                {
                    Log.WriteLine(e, LogType.ERROR);
                }
            }
        }

        private void HandleTopic(Socket socket)
        {
            //Get the incomming data
            byte[] data = new byte[1024];
            int amount = socket.Receive(data, data.Length, 0);
            //Convert the incomming bytes to string
            string buffer = Encoding.ASCII.GetString(data);
            //Check for GET
            if (buffer.Substring(0, 3) != "GET")
            {
                socket.Close();
                return;
            }
            //Find the HTTP request
            int startPosition = buffer.IndexOf("HTTP", 1);
            string sHttpVersion = buffer.Substring(startPosition, 8);
            //Sanitize backslashes
            buffer = buffer.Substring(0, startPosition - 1);
            buffer = buffer.Replace("\\", "/");
            //Process the request
            string request = buffer.Substring(buffer.LastIndexOf("/") + 1);
            //Process
            if (htmlCache.ContainsKey(request))
            {
                byte[] bytesToSend = Encoding.ASCII.GetBytes(htmlCache[request]);
                SendHeader(sHttpVersion, "", htmlCache[request].Length, " 200 OK", ref socket);
                SendToBrowser(bytesToSend, ref socket);
            }
            else
            {
                string toSend = "error 404, page not found";
                byte[] bytesToSend = Encoding.ASCII.GetBytes(toSend);
                SendHeader(sHttpVersion, "", toSend.Length, " 200 OK", ref socket);
                SendToBrowser(bytesToSend, ref socket);
            }
            Thread.Sleep(10000);
            if (true)
            {
                string toSend = "haha just kidding!";
                byte[] bytesToSend = Encoding.ASCII.GetBytes(toSend);
                SendHeader(sHttpVersion, "", toSend.Length, " 200 OK", ref socket);
                SendToBrowser(bytesToSend, ref socket);
            }
            //Close the socket
            socket.Close();
        }


        //===============================
        //https://www.c-sharpcorner.com/article/creating-your-own-web-server-using-C-Sharp/
        //===============================

        public void SendHeader(string sHttpVersion, string sMIMEHeader, int iTotBytes, string sStatusCode, ref Socket mySocket)
        {
            String sBuffer = "";
            // if Mime type is not provided set default to text/html  
            if (sMIMEHeader.Length == 0)
            {
                sMIMEHeader = "text/html";// Default Mime Type is text/html  
            }
            sBuffer = sBuffer + sHttpVersion + sStatusCode + "\r\n";
            sBuffer = sBuffer + "Server: cx1193719-b\r\n";
            sBuffer = sBuffer + "Content-Type: " + sMIMEHeader + "\r\n";
            sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
            sBuffer = sBuffer + "Content-Length: " + iTotBytes + "\r\n\r\n";
            Byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);
            SendToBrowser(bSendData, ref mySocket);
        }


        public void SendToBrowser(String sData, ref Socket mySocket)
        {
            SendToBrowser(Encoding.ASCII.GetBytes(sData), ref mySocket);
        }

        public void SendToBrowser(Byte[] bSendData, ref Socket mySocket)
        {
            try
            {
                if (mySocket.Connected)
                {
                    int numBytes;
                    if ((numBytes = mySocket.Send(bSendData, bSendData.Length, 0)) == -1)
                        Console.WriteLine("Socket Error cannot Send Packet");
                    else
                    {
                        Console.WriteLine("No. of bytes send {0}", numBytes);
                    }
                }
                else Console.WriteLine("Connection Dropped....");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occurred : {0} ", e);
            }
        }


    }
}
