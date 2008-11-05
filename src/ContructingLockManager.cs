using LinFu.DynamicProxy;


namespace NDependencyInjection
{
    public class ContructingLockManager
    {
        private static int lockCount = 0;

        public static void Lock()
        {
            lockCount++;
        }

        public static void Release()
        {
            lockCount--;
        }

        public static void AddIntercept(IInterceptor interceptor, InvocationInfo info)
        {
            if (lockCount == 0)
            {
                interceptor.Intercept(info);
            }
        }
    }
}
