using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.BatCore;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.Network.TcpSocket;
using THEDARKKNIGHT.Network.TcpSocket.Client;
using THEDARKKNIGHT.Network.TcpSocket.Server;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using UnityEngine;

namespace THEDARKKNIGHT.Example.FameSync.UI
{
	public class TcpCommuSystem : BatMonoSingletion<TcpCommuSystem>
	{
		TcpSocketServerMgr m_tcpMgr;
		TcpSocketClientMgr m_tcpClient;
		byte[] m_HeartbeatMsg;


		private void Start()
		{
			MsgBox heartbeat = new MsgBox();
			heartbeat.MsgType = 0;
			heartbeat.Msgbody = null;
			m_HeartbeatMsg = BFrameSyncUtility.Encode(heartbeat);
		}

		public void StartServer(string IPAddress)
		{
			m_tcpMgr = new TcpSocketServerMgr(new ReceviceDataSKeeper(new MsgDataParse()), new MessagerDataSSender(new HeartbeatSolverServer().SetHeartbeatMsg(m_HeartbeatMsg).SendPeriod(10).SetCheckTick(6)));
			m_tcpMgr.StartSever(IPAddress, 8080);
		}

		public void SendMsg(byte[] msg)
		{
			if (m_tcpMgr != null) m_tcpMgr.Send(msg);
			if (m_tcpClient != null) m_tcpClient.Send(msg);
		}

		public void Close()
		{
			if (m_tcpMgr != null) m_tcpMgr.Close();
			if (m_tcpClient != null) m_tcpClient.Close();
		}

		public void ConnectToServer(string IPAddress)
		{
			m_tcpClient = new TcpSocketClientMgr(new ReceviceDataCKeeper(new MsgDataParse()), new MessagerDataCSender(new HeartbeatSolverClient().SetHeartbeatMsg(m_HeartbeatMsg).SendPeriod(10).SetCheckTick(6)));
			m_tcpClient.ConnectToServer(IPAddress, 8080);
		}
	}
}
