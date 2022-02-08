using UnityEngine;

namespace Framework
{
    public abstract class InputDeviceBase : IInputDevice
    {
        public abstract bool CanUse();
 
        public float GetAxis(string name)
        {
            float result = 0;
            switch (name)
            {
                case InputAxisType.MouseX:
                    result = MouseX();
                    break;
                case InputAxisType.MouseY:
                    result = MouseY();
                   break;
                case InputAxisType.MouseScrollWheel:
                    result = MouseScrollWheel();
                    break;
                case InputAxisType.Horizontal:
                    result = Horizontal();
                    break;
                case InputAxisType.Vertical:
                    result = Vertical();
                    break;
            }
            return result;
        }

        public virtual void Update()
        {
            
        }

        protected abstract float MouseX();

        protected abstract float MouseY();

        protected abstract float MouseScrollWheel();

        protected abstract float Horizontal();

        protected abstract float Vertical();
    }
    

    public static class InputAxisType
    {
        /// <summary>
        /// 鼠标X轴移动
        /// </summary>
        public const string MouseX = "MouseX";
        /// <summary>
        /// 鼠标Y轴移动
        /// </summary>
        public const string MouseY = "MouseY";
        /// <summary>
        /// 鼠标滚轮滚动
        /// </summary>
        public const string MouseScrollWheel = "MouseScrollWheel";
        /// <summary>
        /// 电脑上跟Horizontal一致，手机上是点击屏幕左右滑动
        /// </summary>
        public const string Horizontal = "Horizontal";
        /// <summary>
        /// 电脑上跟Vertical一致，手机上是点击屏幕上下滑动
        /// </summary>
        public const string Vertical = "Vertical";
    }
}