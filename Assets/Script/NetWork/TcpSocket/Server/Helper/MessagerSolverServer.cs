using System;
using THEDARKKNIGHT.Interface;
using THEDARKKNIGHT.ThreadHelper;

namespace THEDARKKNIGHT.Network.TcpSocket.Client
{

    public class MessagerSolverServer : IMessagerParseSolver
    {

        public Action<byte[], string> MsgFeedback;

        public MessagerSolverServer() {
            ThreadCrossHelper.Instance().CreatThreadCrossHelp();
        }

        public void Close()
        {
           
        }

        public virtual void MessageSolver(byte[] data, string IPAddress)
        {
            ThreadCrossHelper.Instance().ExcutionFunc(() => {

            });
        }

        public void SetMessageFeedback(Action<byte[], string> msgFeedback)
        {
            this.MsgFeedback = msgFeedback;
        }
    }

}
