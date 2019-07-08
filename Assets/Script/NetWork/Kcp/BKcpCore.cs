using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
namespace THEDARKKNIGHT.Network.Kcp
{
    /// <summary>
    /// KCP的下层协议输出函数，KCP需要发送数据时会调用它,buf/len 表示缓存和长度,user指针为 kcp对象创建时传入的值，用于区别多个 KCP对象
    /// </summary>
    /// <param name="buf">buf对象</param>
    /// <param name="len">缓存和长度</param>
    /// <param name="kcp">kcp对象</param>
    /// <param name="user">user对象</param>
    /// <returns></returns>
    public delegate int kcp_output(IntPtr buf, int len, IntPtr kcp, IntPtr user);

    public class BKcpCore
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        const string KcpDLL = "__Internal";
#else
        const string KcpDLL = "kcp";
#endif

        /// <summary>
        /// 以一定频率调用 ikcp_update来更新 kcp状态，并且传入当前时钟（毫秒单位）
        /// 如 10ms调用一次，或用 ikcp_check确定下次调用 update的时间不必每次调用
        /// </summary>
        /// <param name="kcp"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ikcp_check(IntPtr kcp, uint current);

        /// <summary>
        /// 初始化 kcp对象，conv为一个表示会话编号的整数，和tcp的 conv一样，通信双
        /// 方需保证 conv相同，相互的数据包才能够被认可，user是一个给回调函数的指针
        /// </summary>
        /// <param name="conv">会话编号的整数</param>
        /// <param name="user">回调函数的指针</param>
        /// <returns></returns>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ikcp_create(uint conv, IntPtr user);

        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ikcp_flush(IntPtr kcp);

        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern uint ikcp_getconv(IntPtr ptr);

        /// <summary>
        /// 收到一个下层数据包（比如UDP包）时需要调用,识别具体包的内型
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="data">数据buffer</param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ikcp_input(IntPtr kcp, byte[] data, int offset, int size);

        /// <summary>
        /// 工作模式：
        /// 普通模式： ikcp_nodelay(kcp, 0, 40, 0, 0);
        /// 极速模式： ikcp_nodelay(kcp, 1, 10, 2, 1);
        /// </summary>
        /// <param name="kcp">Kcp对象</param>
        /// <param name="nodelay">是否启用 nodelay模式，0不启用；1启用</param>
        /// <param name="interval">协议内部工作的 interval，单位毫秒，比如 10ms或者 20ms</param>
        /// <param name="resend">快速重传模式，默认0关闭，可以设置2（2次ACK跨越将会直接重传）</param>
        /// <param name="nc">是否关闭流控，默认是0代表不关闭，1代表关闭</param>
        /// <returns></returns>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ikcp_nodelay(IntPtr kcp, int nodelay, int interval, int resend, int nc);

        /// <summary>
        /// 下一个udp包的总大小（全是小包组合）
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <returns></returns>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ikcp_peeksize(IntPtr kcp);

        /// <summary>
        /// kcp获取新的数据
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ikcp_recv(IntPtr kcp, byte[] buffer, int len);

        /// <summary>
        /// 释放kcp对象资源
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ikcp_release(IntPtr kcp);

        /// <summary>
        /// kcp对象发送消息
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="buffer">buff对象</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ikcp_send(IntPtr kcp, byte[] buffer, int len);

        /// <summary>
        /// 设置最小RTO
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="minrto"></param>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ikcp_setminrto(IntPtr ptr, int minrto);

        /// <summary>
        /// 设置分包大小
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="mtu">大小字节</param>
        /// <returns></returns>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ikcp_setmtu(IntPtr kcp, int mtu);

        /// <summary>
        /// 设置回调函数
        /// </summary>
        /// <param name="kcp"></param>
        /// <param name="output"></param>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ikcp_setoutput(IntPtr kcp, kcp_output output);

        /// <summary>
        /// 以一定频率调用 ikcp_update来更新 kcp状态，并且传入当前时钟（毫秒单位）
        /// 如 10ms调用一次，或用 ikcp_check确定下次调用 update的时间不必每次调用
        /// </summary>
        /// <param name="kcp"></param>
        /// <param name="current"></param>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ikcp_update(IntPtr kcp, uint current);

        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ikcp_waitsnd(IntPtr kcp);

        /// <summary>
        /// 该调用将会设置协议的最大发送窗口和最大接收窗口大小，默认为32. 
        /// 这个可以理解为 TCP的 SND_BUF 和 RCV_BUF，只不过单位不一样 SND/RCV_BUF 单位是字节，这个单位是包。
        /// </summary>
        /// <param name="kcp"></param>
        /// <param name="sndwnd">发送窗口</param>
        /// <param name="rcvwnd">接受窗口</param>
        /// <returns></returns>
        [DllImport(KcpDLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int ikcp_wndsize(IntPtr kcp, int sndwnd, int rcvwnd);

        /// <summary>
        /// ikcp_check确定下次调用 update的时间不必每次调用
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="current">目前时间毫秒</param>
        /// <returns></returns>
        public static uint KcpCheck(IntPtr kcp, uint current)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_check(kcp, current);
        }

        /// <summary>
        /// 初始化 kcp对象，conv为一个表示会话编号的整数，和tcp的 conv一样，通信双
        /// 方需保证 conv相同，相互的数据包才能够被认可，user是一个给回调函数的指针
        /// </summary>
        /// <param name="conv">会话编号的整数</param>
        /// <param name="user">回调函数的指针</param>
        /// <returns></returns>
        public static IntPtr Create(uint conv, IntPtr user)
        {
            return ikcp_create(conv, user);
        }

       
        public static void Flush(IntPtr kcp)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            ikcp_flush(kcp);
        }

        /// <summary>
        /// 获取会话ID
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static uint Getconv(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_getconv(ptr);
        }

        /// <summary>
        /// 收到一个下层数据包（比如UDP包）时需要调用,识别具体包的内型
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="data">数据buffer</param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int Input(IntPtr kcp, byte[] data, int offset, int size)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_input(kcp, data, offset, size);
        }

        /// <summary>
        /// 工作模式：
        /// 普通模式： ikcp_nodelay(kcp, 0, 40, 0, 0);
        /// 极速模式： ikcp_nodelay(kcp, 1, 10, 2, 1);
        /// </summary>
        /// <param name="kcp">Kcp对象</param>
        /// <param name="nodelay">是否启用 nodelay模式，0不启用；1启用</param>
        /// <param name="interval">协议内部工作的 interval，单位毫秒，比如 10ms或者 20ms</param>
        /// <param name="resend">快速重传模式，默认0关闭，可以设置2（2次ACK跨越将会直接重传）</param>
        /// <param name="nc">是否关闭流控，默认是0代表不关闭，1代表关闭</param>
        /// <returns></returns>
        public static int Nodelay(IntPtr kcp, int nodelay, int interval, int resend, int nc)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_nodelay(kcp, nodelay, interval, resend, nc);
        }

        /// <summary>
        /// 获取udp的总大小(该包或许是由seg碎片小包构成)
        /// </summary>
        /// <param name="kcp">kcp的包大小</param>
        /// <returns>包体大小</returns>
        public static int Peeksize(IntPtr kcp)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_peeksize(kcp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kcp"></param>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int Recv(IntPtr kcp, byte[] buffer, int len)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_recv(kcp, buffer, len);
        }

        public static void Release(IntPtr kcp)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            ikcp_release(kcp);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="buffer">发送区</param>
        /// <param name="len">大小长度</param>
        /// <returns></returns>
        public static int Send(IntPtr kcp, byte[] buffer, int len)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_send(kcp, buffer, len);
        }

        /// <summary>
        /// 不管是 TCP还是 KCP计算 RTO时都有最小 RTO的限制，即便计算出来RTO为40ms，
        /// 由于默认的 RTO是100ms，协议只有在100ms后才能检测到丢包，快速模式下为30ms，可以手动更改该值：
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="minrto">延迟毫秒</param>
        public static void Setminrto(IntPtr kcp, int minrto)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            ikcp_setminrto(kcp, minrto);
        }

        /// <summary>
        /// 设置最小分包大小
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="mtu">分包大小，字节</param>
        /// <returns></returns>
        public static int Setmtu(IntPtr kcp, int mtu)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_setmtu(kcp, mtu);
        }

        public static void Setoutput(IntPtr kcp, kcp_output output)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            ikcp_setoutput(kcp, output);
        }

        public static void Update(IntPtr kcp, uint current)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            ikcp_update(kcp, current);
        }

        /// <summary>
        /// 获取滑动窗口区发送大小
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <returns>字节数</returns>
        public static int Waitsnd(IntPtr kcp)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_waitsnd(kcp);
        }

        /// <summary>
        /// 设置协议的最大发送窗口和最大接收窗口大小，默认为32.
        /// </summary>
        /// <param name="kcp">kcp对象</param>
        /// <param name="sndwnd">发送窗口大小（单位为包）</param>
        /// <param name="rcvwnd">接受窗口大小（单位为包）</param>
        /// <returns></returns>
        public static int Wndsize(IntPtr kcp, int sndwnd, int rcvwnd)
        {
            if (kcp == IntPtr.Zero)
            {
                throw new Exception($"kcp error, kcp point is zero");
            }
            return ikcp_wndsize(kcp, sndwnd, rcvwnd);
        }
    }

}

