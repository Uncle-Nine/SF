using System;
using System.Collections.Generic;
using Framework.Execution;
using UnityEngine;

namespace Framework.Asynchronous
{
    public interface IMulProgress
    {
        float Current { get; }
        float Total { get; }
    }
    
    public class MulAsyncResult : AsyncResult
    {
        public float Progress { get; private set;}
        private Callbackable _callbackable;
        private List<IAsyncResult> _allProgress = new List<IAsyncResult>();

        public override bool IsDone => _allProgress.Count <= 0 || base.IsDone;

        public MulAsyncResult(params IAsyncResult[] allProgress) : this(false, allProgress)
        {
        }

        public MulAsyncResult(bool cancelable, params IAsyncResult[] allProgress) : base(cancelable)
        {
            AddAsyncResult(allProgress);
        }

        public void AddAsyncResult(IAsyncResult progressResult)
        {
            if (progressResult == null) return;
            _allProgress.Add(progressResult);
            SetSubProgressCb(progressResult);
        }

        public void AddAsyncResult(IEnumerable<IAsyncResult> progressResults)
        {
            foreach (var progressResult in progressResults)
            {
                AddAsyncResult(progressResult);
            }
        }

        private void SetSubProgressCb(IAsyncResult progressResult)
        {
            if (progressResult.IsDone) return;
            progressResult.Callbackable().OnCallback(f => RaiseOnProgressCallback(0));
        }

        protected virtual void RaiseOnProgressCallback(float progress)
        {
            UpdateProgress();
            //延迟一帧 否则会比子任务提前完成
            if (Progress >= 1)
            {
                GameLoop.Ins.Delay(() => SetResult());
            }
        }

        protected override void RaiseOnCallback()
        {
            base.RaiseOnCallback();
            _callbackable?.RaiseOnCallback();
        }

        private void UpdateProgress()
        {
            float totalProgress = 0;
            foreach (var progressResult in _allProgress)
            {
                if (progressResult.IsDone)
                {
                    totalProgress += 1;
                }
            }
            Progress = totalProgress / _allProgress.Count;
        }
        
        public override ICallbackable Callbackable()
        {
            lock (Lock)
            {
                return this._callbackable ?? (this._callbackable = new Callbackable(this));
            }
        }
    }
    
    public class MulProgressResult<TProgress> : ProgressResult<float> where TProgress : IMulProgress
    {
        private List<IProgressResult<TProgress>> _allProgress = new List<IProgressResult<TProgress>>();
        
        public override bool IsDone => _allProgress.Count <= 0 || base.IsDone;

        public MulProgressResult(params IProgressResult<TProgress>[] allProgress) : this(false, allProgress)
        {
        }

        public MulProgressResult(bool cancelable, params IProgressResult<TProgress>[] allProgress) : base(cancelable)
        {
            AddAsyncResult(allProgress);
        }

        public void AddAsyncResult(IProgressResult<TProgress> progressResult)
        {
            if (progressResult == null) return;
            _allProgress.Add(progressResult);
            SetSubProgressCb(progressResult);
        }

        public void AddAsyncResult(IEnumerable<IProgressResult<TProgress>> progressResults)
        {
            foreach (var progressResult in progressResults)
            {
                AddAsyncResult(progressResult);
            }
        }

        private void SetSubProgressCb(IProgressResult<TProgress> progressResult)
        {
            if (progressResult.IsDone) return;
            progressResult.Callbackable().OnProgressCallback((progress => RaiseOnProgressCallback(0)));
            progressResult.Callbackable().OnCallback(progress => CheckAllFinish());
        }

        protected override void RaiseOnProgressCallback(float progress)
        {
            UpdateProgress();
            base.RaiseOnProgressCallback(Progress);
        }

        private void CheckAllFinish()
        {
            RaiseOnProgressCallback(0);
            Exception exception = null;
            foreach (var progressResult in _allProgress)
            {
                if (progressResult.Exception != null)
                {
                    exception = progressResult.Exception;
                    break;
                }
                if(!progressResult.IsDone) return;
            }
            
            //延迟一帧 否则会比子任务提前完成
            GameLoop.Ins.Delay(() =>
            {
                if (exception != null)
                    SetException(exception);
                else
                    SetResult();
            });
        }

        private void UpdateProgress()
        {
            float totalProgress = 0;
            float current = 0;
            foreach (var progressResult in _allProgress)
            {
                totalProgress += progressResult.Progress.Total;
                if (progressResult.IsDone)
                {
                    current += progressResult.Progress.Total;
                }
                else
                {
                    current += progressResult.Progress.Current;
                }
            }
            Progress = current / totalProgress;
        }
    }
    
    public class MulProgressResult : ProgressResult<float>
    {
        private List<IProgressResult<float>> _allProgress = new List<IProgressResult<float>>();

        public override bool IsDone => _allProgress.Count <= 0 || base.IsDone;

        public MulProgressResult(params IProgressResult<float>[] allProgress) : this(false, allProgress)
        {
        }

        public MulProgressResult(bool cancelable, params IProgressResult<float>[] allProgress) : base(cancelable)
        {
            AddAsyncResult(allProgress);
        }

        public void AddAsyncResult(IProgressResult<float> progressResult)
        {
            if (progressResult == null) return;
            _allProgress.Add(progressResult);
            SetSubProgressCb(progressResult);
        }

        public void AddAsyncResult(IEnumerable<IProgressResult<float>> progressResults)
        {
            foreach (var progressResult in progressResults)
            {
                AddAsyncResult(progressResult);
            }
        }

        private void SetSubProgressCb(IProgressResult<float> progressResult)
        {
            if (progressResult.IsDone) return;
            progressResult.Callbackable().OnProgressCallback((progress => RaiseOnProgressCallback(0)));
            progressResult.Callbackable().OnCallback(progress => CheckAllFinish());
        }

        protected override void RaiseOnProgressCallback(float progress)
        {
            UpdateProgress();
            base.RaiseOnProgressCallback(Progress);
        }

        private void CheckAllFinish()
        {
            RaiseOnProgressCallback(0);
            foreach (var progressResult in _allProgress)
            {
                if(!progressResult.IsDone) return;
            }
            //延迟一帧 否则会比子任务提前完成
            GameLoop.Ins.Delay(() => SetResult());
        }

        private void UpdateProgress()
        {
            float totalProgress = 0;
            foreach (var progressResult in _allProgress)
            {
                if (progressResult.IsDone)
                {
                    totalProgress += 1;
                }
                else
                {
                    totalProgress += progressResult.Progress;
                }
            }
            Progress = totalProgress / _allProgress.Count;
        }
    }
}