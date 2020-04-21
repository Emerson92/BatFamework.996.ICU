using System;
using THEDARKKNIGHT.Interface;
namespace THEDARKKNIGHT.Network.TcpSocket.Client
{

    /// <summary>
    /// 消息接受器
    /// </summary>
    public class ReceviceDataCKeeper
    {
        public IMessagerParseSolver Solver;

        public Func<ByteArray, int, string, byte[]> SolverRuleExcution;

        public Action ReceivedFeedbackMsg;

        public ReceviceDataCKeeper(IMessagerParseSolver solver = null) {
            if (solver != null)SetMessagerSolver(solver);
        }

        /// <summary>
        /// 消息处理器
        /// </summary>
        /// <param name="solver"></param>
        public void SetMessagerSolver(IMessagerParseSolver solver)
        {
            this.Solver = solver;
        }

        /// <summary>
        /// 设置消息规则解析器
        /// </summary>
        /// <param name="function"></param>
        public void SetRuleSolver(Func<ByteArray, int, string, byte[]> function) {
            this.SolverRuleExcution = function;
        }

        public void MessageDataRecevice(ByteArray data, int length, string IPAddress)
        {
            while (data.Length > 2) {
                if (ReceivedFeedbackMsg != null) ReceivedFeedbackMsg();
                byte[] CompleteMsg = MsgpPareRule(data, length, IPAddress);
                if (CompleteMsg != null) Solver.MessageSolver(CompleteMsg, IPAddress);
                else
                    break;
            }
        }

        public void Close(TcpSocketClient.STATE currentState) {
            Solver.Close();
            SolverRuleExcution = null;
            Solver = null;
        }

        /// <summary>
        /// 消息解析规则 
        /// 普通解析规则（2字节消息头+消息体）
        /// </summary>
        public byte[] MsgpPareRule(ByteArray data, int length, string IPAddress)
        {
            if (SolverRuleExcution != null)return SolverRuleExcution(data, length, IPAddress);
            if (data.Length <= 2) return null;
            int readIdx = data.ReadIdx;
            byte[] bytes = data.Bytes;
            Int16 bodyLength = data.ReadInt16();
            if (data.Length < bodyLength + 2) return null;
            data.ReadIdx += 2;
            byte[] msgByte = new byte[bodyLength];
            data.Read(msgByte,0, bodyLength);
            if (data.Length == 0) data.Reset();//重置buffer状态
            return msgByte;
        }

    }

}
