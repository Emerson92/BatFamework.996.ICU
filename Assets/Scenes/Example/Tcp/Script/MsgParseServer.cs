using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using THEDARKKNIGHT.Network.TcpSocket.Client;
using System.IO;
using System;
using THEDARKKNIGHT.ThreadHelper;
using System.Text;
using THEDARKKNIGHT.EventSystem;

public class MsgParseServer : MessagerSolverServer
{

    public override void  MessageSolver(byte[] data, string IPAddress) {
        
        MsgTalk t = (MsgTalk)Decode("MsgTalk",data,0,data.Length);
        Debug.Log("Msgtype :"+ t.MsgType +" Msgbody :"+t.Msgbody);
        switch (t.MsgType) {
            case 0:
                if (MsgFeedback != null) {
                    MsgFeedback(data, IPAddress);
                }
                break;
            case 1:
                ThreadCrossHelper.Instance().ExcutionFunc(() => {
                    BEventManager.Instance().DispatchEvent("Msg", data);
                    Debug.Log(IPAddress + "收到消息：" + t.Msgbody);
                });
                break;
        
        }

    }



    public ProtoBuf.IExtensible Decode(string protoName,byte[] bytes, int offset, int count) {
        using (var memory = new MemoryStream(bytes, offset, count)){
            Type t = Type.GetType(protoName);
            return (ProtoBuf.IExtensible)ProtoBuf.Serializer.NonGeneric.Deserialize(t,memory);
        }
    }

}
