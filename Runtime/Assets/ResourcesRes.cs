using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Asynchronous;
using UnityEngine;
using UnityEngine.SceneManagement;
using IAsyncResult = Framework.Asynchronous.IAsyncResult;
using Object = UnityEngine.Object;

namespace Framework.Assets
{
    public class ResourcesRes : Res
    {

        private List<Object> _handles = new List<Object>();
        private List<ResourceRequest> requests = new List<ResourceRequest>();

        public override IAsyncResult Init()
        {
            return AsyncResult.Void();   
        }

        public override string DownloadURL { get; set; }
        
        protected override void LoadScene(IProgressPromise<float, Scene> promise, string path, LoadSceneMode loadSceneMode)
        {
            throw new NotImplementedException();
        }

        public override IProgressResult<float,string> CheckDownloadSize()
        {
            return ProgressResult<float,string>.Void();
        }
        
        public override IProgressResult<DownloadProgress> DownloadAssets()
        {
            return ProgressResult<DownloadProgress>.Void();
        }

        protected override async void loadAssetAsync<T>(string key, IProgressPromise<float, T> promise)
        {
            var operation = Resources.LoadAsync<T>(key);
            requests.Add(operation);
            while (!operation.isDone)
            {
                promise.UpdateProgress(operation.progress);
                await Task.Yield();
            }
            promise.UpdateProgress(1);
            promise.SetResult(operation.asset as T);
            _handles.Add(operation.asset);
        }

        public override T LoadAsset<T>(string key)
        {
            var obj = Resources.Load<T>(key);
            _handles.Add(obj);
            return obj;
        }

        public override void Release()
        {
            foreach (var handle in _handles)
            {
                Resources.UnloadAsset(handle);
            }
            _handles.Clear();
        }
    }
}