using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace THEDARKKNIGHT
{
    public abstract class BHttpCore<T,K> : ILifeCycle where T : DownloadHandler where K :UploadHandler
    {
        private LifeCycleTool tool;

        private string RequestUrl;

        private UnityWebRequest request;

        protected MonoBehaviour mono;

        private float TimeOut;

        private Dictionary<string, string> HeaderDic = new Dictionary<string, string>();

        private T DownloadOperate;

        private K UploadOperate;

        private CertificateHandler CertHelper;

        private float DownloadSpeed = 0;

        private float UploadSpeed = 0;

        private float LastDownloadData = 0;

        private float LastUploadData = 0;

        private RequsetOperater operater;

        public enum RequsetMethod{
            GET,
            POST,
            PUT
        };

        private RequsetMethod RequestMethod;

        public virtual void InitBHttpCore(){
            this.Enable();
        }

        public void SetCertificate(CertificateHandler handler){

            this.CertHelper = handler;
        }

        public void SetDownloadHelper(T downloadHelper){
            this.DownloadOperate = downloadHelper;
        }

        public void SetUploadHelper(K uploadHandler){
            this.UploadOperate = uploadHandler;
        }

        public void SetRequesUrl(string url)
        {
            this.RequestUrl = url;
        }

        public void SetReuqestMethod(RequsetMethod method){
            this.RequestMethod = method;
        }

        public virtual void SetHeader(string Resquest,string data){
            HeaderDic.Add(Resquest,data);
        }

        public void StartSendResquest(){
            if(operater != null) operater = new RequsetOperater();
            operater.RequestOperate = PerpareTransportComponent();
            mono.StartCoroutine(SendRequest(operater));
        }

        private UnityWebRequest PerpareTransportComponent()
        {
            request = new UnityWebRequest();
            switch(RequestMethod){
                case RequsetMethod.GET:
                    request.method = UnityWebRequest.kHttpVerbGET;
                    break;
                case RequsetMethod.POST:
                    request.method = UnityWebRequest.kHttpVerbPOST;
                    break;
                case RequsetMethod.PUT:
                    request.method = UnityWebRequest.kHttpVerbPUT;
                    break;
                default :
                    break;
            }
            request.url = RequestUrl;
            foreach(KeyValuePair<string,string> pair in HeaderDic){
                request.SetRequestHeader(pair.Key, pair.Value);
            }
            request.downloadHandler = DownloadOperate;
            request.uploadHandler = UploadOperate;
            request.certificateHandler = CertHelper;
            return request;
        }

        private IEnumerator SendRequest(RequsetOperater oprater){
            yield return oprater.RequestOperate.SendWebRequest();
            if(oprater.RequestOperate.isNetworkError){
                NetworkErrorHappen(oprater);
            }
            if(oprater.RequestOperate.isHttpError){
                HttpErrorHappen(oprater);
            }
            if(oprater.RequestOperate.isDone){
                HttpRequsetDone((T)oprater.RequestOperate.downloadHandler, (K)oprater.RequestOperate.uploadHandler);

            }
        }

        protected abstract void HttpRequsetDone(T donwnloadHelper, K uploadloadHelper);


        protected abstract void HttpErrorHappen(RequsetOperater oprater);

        protected abstract void NetworkErrorHappen(RequsetOperater oprater);

        protected float ProgressUpdate(){
            float progress = request != null ?request.downloadProgress : 0;
            return progress;
        }

        protected float GetDownloadSpeed(){

            return DownloadSpeed/(1024*1024);
        }

        protected float GetUploadSpeed(){

            return UploadSpeed/(1024 * 1024);
        }

        public virtual void RecycleTrash(){
            this.Disable();
        }

        void ILifeCycle.BAwake(MonoBehaviour main)
        {

        }

        void ILifeCycle.BDisable(MonoBehaviour main)
        {

        }

        void ILifeCycle.BFixedUpdate(MonoBehaviour main)
        {
            DownloadSpeed = request != null ? (request.downloadedBytes - LastDownloadData)/Time.fixedDeltaTime : 0;
            UploadSpeed   = request != null ? (request.uploadedBytes   - LastUploadData) / Time.fixedDeltaTime : 0;
        }

        void ILifeCycle.BLateUpdate(MonoBehaviour main)
        {

        }

        void ILifeCycle.BOnApplicationFocus(MonoBehaviour main)
        {

        }

        void ILifeCycle.BOnApplicationPause(MonoBehaviour main)
        {

        }

        void ILifeCycle.BOnApplicationQuit(MonoBehaviour main)
        {

        }

        void ILifeCycle.BOnDestory(MonoBehaviour main)
        {

        }

        void ILifeCycle.BOnDestroy(MonoBehaviour main)
        {

        }

        void ILifeCycle.BOnEnable(MonoBehaviour main)
        {

        }

        void ILifeCycle.BStart(MonoBehaviour main)
        {
            this.mono = main;
        }

        void ILifeCycle.BUpdate(MonoBehaviour main)
        {

        }

        public class RequsetOperater{
            
            public UnityWebRequest RequestOperate;

            public RequsetMethod RequestMethod;

            public string Url;

            public int ConnectionCount = 0;
        }
    }
}

