using System;
using THEDARKKNIGHT.Interface;
namespace THEDARKKNIGHT.Network.TcpSocket
{

    /// <summary>
    /// 消息接受器
    /// </summary>
    public class ReceviceDataKeeper
    {
        public IMessagerParseSolver Solver;

        public Func<byte[], int, string, object> SolverRuleExcution;

        public ReceviceDataKeeper(IMessagerParseSolver solver = null) {
            if (solver != null)
                SetMessagerSolver(solver);
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
        public void SetRuleSolver(Func<byte[], int, string, object> function) {
            this.SolverRuleExcution = function;
        }

        public void MessageDataRecevice(byte[] data, int length, string IPAddress)
        {
            object CompleteMsg = MsgpPareRule(data, length, IPAddress);
            Solver.MessageSolver(CompleteMsg);
        }

        public void Close() {

        }

        /// <summary>
        /// 消息解析规则
        /// </summary>
        public object MsgpPareRule(byte[] data, int length, string IPAddress)
        {
            if (SolverRuleExcution != null)
                return SolverRuleExcution(data, length, IPAddress);
            else
                return null;
        }

    }

}
