using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.EventSystem;
using THEDARKKNIGHT.Network.TcpSocket.Client;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using THEDARKKNIGHT.ThreadHelper;
using UnityEngine;


namespace THEDARKKNIGHT.Example.FameSync.UI
{


    public class MsgDataParse : MessagerSolverServer
    {
        public class MsgBoxData
        {

            public string IPAddress;

            public MsgBox Body;

            public MsgBoxData(string IPAddress, MsgBox body)
            {
                this.IPAddress = IPAddress;
                this.Body = body;
            }
        }



        public override void MessageSolver(byte[] data, string IPAddress)
        {
            MsgBox box = (MsgBox)BFrameSyncUtility.Decode("MsgBox", data, 0, data.Length);
            if (box.MsgType == 0) return;
            ThreadCrossHelper.Instance().ExcutionFunc(() =>
            {
                BEventManager.Instance().DispatchEvent("MsgOnReceive", new MsgBoxData(IPAddress, box));
            });
        }

    }
}
