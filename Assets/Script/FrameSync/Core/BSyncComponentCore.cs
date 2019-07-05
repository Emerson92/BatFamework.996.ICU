using THEDARKKNIGHT.SyncSystem.FrameSync.BBuffer;
using THEDARKKNIGHT.SyncSystem.FrameSync.ExtendMethod;
using THEDARKKNIGHT.SyncSystem.FrameSync.Interface;
using THEDARKKNIGHT.SyncSystem.FrameSync.Utility;
using THEDARKKNIGHT.SyncSystem.FrameSync.BStruct.NetworkProtocol;

namespace THEDARKKNIGHT.SyncSystem.FrameSync
{

    public enum SYNCTYPE
    {
        NULL,
        TRANSFORM,
        POSITION,
        ROTATION,
        SCALE,
        ANIMATION,
        ANIMATOR,
        OTHER
    }

    public enum COMPONENTLIFECYCLE
    {
        LIVE,
        HIDDEN,
        DEATH
    }

    public abstract class BSyncComponentCore<T> : ISyncComponent,IRoallbackable where T : class
    {

        public COMPONENTLIFECYCLE Statue;

        /// <summary>
        /// Current Component's Type 
        /// </summary>
        private SYNCTYPE componentType = SYNCTYPE.NULL;

        private uint ComponentID;

        protected BNetworkFrameBuffer<T> NetworkFrameBuffer = new BNetworkFrameBuffer<T>(1);

        protected BLocalFrameBuffer<T> LocalFrameBuffer = new BLocalFrameBuffer<T>(1);

        public BSyncComponentCore(uint componentID,SYNCTYPE type) {
            this.EnableSync();
            this.componentType = type;
            this.ComponentID = componentID;
        }

        public virtual SYNCTYPE GetComponentType()
        {
            return componentType;
        }

        public virtual void Update(float interpolationValue){ }

        public virtual BNOperateCommend UpdateLogic(int frameIndex)
        {
            ////TODO do some Logic thing
            return CreateCmdLogic(frameIndex);
        }

        private BNOperateCommend CreateCmdLogic(int frameIndex)
        {
            ////TODO PS: Warning ,there has a trap, you need to pay a attention
            BFrame<T>? frames = LocalFrameBuffer.DeQuene((uint)frameIndex);
            BNOperateCommend cmd = new BNOperateCommend();
            cmd.ComponentID = ComponentID;/////Wait to create new ID;
            cmd.OperateType = componentType;
            cmd.cmd = BFrameSyncUtility.NSeralizableClassTobytes<T>(frames?.Cmd);////if you get cache much commdend ,theose code make you feel trouble;
            return cmd;
        }

        protected T GetNetworkCmd(uint frameIndex) {
            BFrame<T>?  frames = NetworkFrameBuffer.DeQuene(frameIndex);
            return frames?.Cmd;
        }

        public virtual bool UpdateByNet(uint NframeIndex, byte[] data)
        {
            ////TODO do some Update by net
            T SClass= BFrameSyncUtility.NBytesToSeralizableClass<T>(data);
            if (FrameConfirm(NframeIndex, SClass))////////Check the server frame,see whether we need to rollback our status
            {
                BFrame<T> Scmd = new BFrame<T>()
                {
                    FrameNum = NframeIndex,
                    Cmd = SClass
                };
                NetworkFrameBuffer.EnQuene(Scmd);
                return false;
            }
            else
                return true;
        }

        public virtual void SetComponentType(SYNCTYPE type) {componentType = type;}

        public virtual void TakeSnapshot(SnapshotWriter writer){}

        public virtual void RollbackTo(SnapshotReader reader){}

        public virtual void Dispose() { this.DisableSync();Statue = COMPONENTLIFECYCLE.DEATH;}

        /// <summary>
        /// in order to  confirm the local predicting frame is match that frame from the server 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool FrameConfirm(uint NframeIndex,T data);

        /// <summary>
        /// it dispatched snapshot by Timemachine 
        /// </summary>
        /// <param name="reader"></param>
        public abstract void DistributeSnapshot(SnapshotReader reader);
    }
}