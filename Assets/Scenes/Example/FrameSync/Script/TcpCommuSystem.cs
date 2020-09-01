using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.BatCore;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.Network.TcpSocket.Server;
using UnityEngine;

public struct MsgBox
{

	public int MsgType;

	public string IPAddress;

	public byte[] data;
}

public class TcpCommuSystem : BatSingletion<TcpCommuSystem>
{
    TcpSocketServerMgr tcpMgr;
    public void StartServer(string IPAddress) {
		//MsgParseServer parse = new MsgParseServer();
		//parse.SetMessageFeedback((data, IPAddress) =>
		//{
		//	MsgBody d = new MsgBody();
		//	d.Data = data;
		//	d.IPAddress = IPAddress;
		//	BEventManager.instance.DispatchEvent("TcpServerMsgOnReceive", d);
		//});
		tcpMgr = new TcpSocketServerMgr(new ReceviceDataSKeeper(new MsgParseServer()), new MessagerDataSSender(new HeartbeatSolverServer().SetHeartbeatMsg(" ").SendPeriod(10).SetCheckTick(6)));
        tcpMgr.StartSever(IPAddress, 8080);

    }

	public void SendMsg(byte[] msg) {
		if (tcpMgr != null) tcpMgr.Send(msg);
	}

	public void CloseServer() {
		if(tcpMgr !=null) tcpMgr.Close();
	}

}
