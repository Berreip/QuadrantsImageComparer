using System;
using System.Diagnostics;
using System.Threading.Tasks;

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
        
        public static T Wrap<T>(Func<T> callback)
        {
            try
            {
                return callback.Invoke();
            }
            catch (Exception e)
            {
                Debug.Fail($"unhandled error: {e}");
                return default;
            }
        }
        
        public static async Task Wrap(Func<Task> callback)
        {
            try
            {
                await callback.Invoke();
            }
            catch (Exception e)
            {
                Debug.Fail($"unhandled error: {e}");
            }
        }
        
        public static async Task<T> Wrap<T>(Func<Task<T>> callback)
        {
            try
            {
                return await callback.Invoke();
            }
            catch (Exception e)
            {
                Debug.Fail($"unhandled error: {e}");
                return default;
            }
        }
    }
}