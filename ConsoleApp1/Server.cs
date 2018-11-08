using Library;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Server
{
    class Server
    {
        #region ServerTCP
        public static NetConnection server = new NetConnection();
        public static int port = 55555;
        #endregion

        #region Events
        public delegate void CommandDelegate(string command);
        public static event CommandDelegate OnCommand;

        public delegate void ClosingDelegate();
        public static event ClosingDelegate OnClosing;

        private static bool ProgramIsRunning = false;
        #endregion

        #region LogVar
        public static DateTime SavedTime;
        public static ConcurrentBag<string> LogsList = new ConcurrentBag<string>();
        public static bool WannaReadLogs = false;
        #endregion

        static void Main(string[] args)
        {
            server.OnConnect += server_OnConnect;
            server.OnDataReceived += server_OnDataReceived;
            server.OnDisconnect += server_OnDisconnect;

            OnClosing += Program_OnClosing;

            ProgramIsRunning = true;

            server.Start(port);

            while (true) ;
        }
        
        #region ServerEvents

        #region ServerEvents_TCP
        private static void server_OnConnect(object sender, NetConnection connection)
        {
            LogData(connection.RemoteEndPoint + " connected");
            //connection.Disconnect();
            //de adaugat verificarea id-ului placii de baza
        }

        private static void server_OnDisconnect(object sender, NetConnection connection)
        {
            LogData(connection.RemoteEndPoint + " disconnected");
            Console.WriteLine("deconectareee");
        }

        private static void server_OnDataReceived(object sender, NetConnection connection, byte[] e)
        {
            
        }
        #endregion

        private static void Program_OnClosing()
        {

        }

        #endregion

        #region Logging
        public static void StartLogging()
        {
            new Thread((ThreadStart)(() =>
            {
                while (ProgramIsRunning)
                {
                    try
                    {
                        if (!Tools.General.IsFileLocked(new FileInfo(Tools.General.GetLogFilePath())))
                        {
                            if (WannaReadLogs)
                            {
                                using (var reader = new StreamReader(Tools.General.GetLogFilePath(), true))
                                {
                                    string line;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        Tools.General.WriteOnColor(line, ConsoleColor.DarkGreen, true);
                                    }
                                }
                                WannaReadLogs = false;
                            }
                            if (!LogsList.IsEmpty)
                            {
                                try
                                {
                                    while (LogsList.TryTake(out string logString))
                                    {

                                        File.AppendAllText(Tools.General.GetLogFilePath(),
                                            DateTime.Now.ToShortDateString() + "/" +
                                            DateTime.Now.ToShortTimeString() + ":" +
                                            logString + Environment.NewLine);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                            }
                            //Thread.Sleep(1);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                }
            })).Start();
        }

        public static void LogData(string logString)
        {
            LogsList.Add(logString);
        }
        #endregion
    }
}
