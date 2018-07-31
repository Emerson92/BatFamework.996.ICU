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

        protected string RequestUrl;

        private UnityWebRequest request = new UnityWebRequest();

        private MonoBehaviour mono;

        private Dictionary<string, string> HeaderDic = new Dictionary<string, string>();

        private T DownloadOperate;

        private K UploadOperate;

        private CertificateHandler CertHelper;

        public enum RequsetMethod{
            GET,
            POST,
            PUT
        };

        private RequsetMethod RequestMethod;

        public void InitBHttpCore(){
            tool = this.Enable(new LifeCycleTool()
            {
                priority = 0,
                Icycle = this
            }.SetLifeCycle(LifeCycleTool.LifeType.Start, true));

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
            RequsetOperater operater= new RequsetOperater();
            operater.RequestOperate = PerpareTransportComponent();
            mono.StartCoroutine(SendRequest(operater));
        }

        private UnityWebRequest PerpareTransportComponent()
        {
            UnityWebRequest request = new UnityWebRequest();
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
                 NetworkErrorHappen();
            }
            if(oprater.RequestOperate.isHttpError){
                HttpErrorHappen();
            }
            if(oprater.RequestOperate.isDone){
                HttpRequsetDone((T)oprater.RequestOperate.downloadHandler, (K)oprater.RequestOperate.uploadHandler);

            }
        }

        protected virtual void HttpRequsetDone(T donwnloadHelper,K uploadloadHelper)
        {
            
        }

        protected virtual void HttpErrorHappen()
        {
            
        }

        protected virtual void NetworkErrorHappen()
        {
           
        }

        public virtual void RecycleTrash(){
            this.Disable(tool);
        }

        void ILifeCycle.BAwake(MonoBehaviour main)
        {

        }

        void ILifeCycle.BDisable(MonoBehaviour main)
        {

        }

        void ILifeCycle.BFixedUpdate(MonoBehaviour main)
        {

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
        }
    }
}

