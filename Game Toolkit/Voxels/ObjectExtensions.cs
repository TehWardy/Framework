using System;

namespace Voxels
{
    public static class ObjectExtensions
    {
        public static void IfNotNull<T>(this T obj, Action<T> action)
        {
            if (obj != null)
                action.Invoke(obj);
        }
    }
}
