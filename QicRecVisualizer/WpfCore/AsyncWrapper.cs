using System;
using System.Diagnostics;

namespace QicRecVisualizer.WpfCore
{
    internal static class AsyncWrapper
    {
        public static void Wrap(Action callback)
        {
            try
            {
                callback.Invoke();
            }
            catch (Exception e)
            {
                Debug.Fail($"unhandled error: {e}");
            }
        }
    }
}