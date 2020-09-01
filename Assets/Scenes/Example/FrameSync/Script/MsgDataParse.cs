using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.Network.TcpSocket.Client;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using THEDARKKNIGHT.ThreadHelper;
using UnityEngine;




public class MsgDataParse : MessagerSolverServer
{
    public override void MessageSolver(byte[] data, string IPAddress)
    {
        MsgTalk t = (MsgTalk)BFrameSyncUtility.Decode("MsgTalk", data, 0, data.Length);
        Debug.Log("Msgtype :" + t.MsgType + " Msgbody :" + t.Msgbody);

        MsgBox box = new MsgBox();
        box.data = data;
        box.IPAddress = IPAddress;
        box.MsgType = t.MsgType;
        //MsgTalk t = (MsgTalk)Decode("MsgTalk", data, 0, data.Length);
        //Debug.Log("Msgtype :" + t.MsgType + " Msgbody :" + t.Msgbody);
        switch (t.MsgType)
        {
            case 0:
                if (MsgFeedback != null)
                {
                    MsgFeedback(data, IPAddress);
                }
                break;
            default:
                ThreadCrossHelper.Instance().ExcutionFunc(() =>
                {
                    BEventManager.Instance().DispatchEvent("MsgOnReceive", box);
                    Debug.Log(IPAddress + "收到消息：" + t.Msgbody);
                });
                break;

        }

    }

}
