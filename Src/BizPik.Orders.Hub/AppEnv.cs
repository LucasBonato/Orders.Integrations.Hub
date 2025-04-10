using Anv;

namespace BizPik.Orders.Hub;

public static partial class AppEnv
{
    public static partial class IFOOD
    {
        public static partial class CLIENT
        {
            public static readonly AnvEnv ID = new("IFOOD__CLIENT__ID");
            public static readonly AnvEnv SECRET = new("IFOOD__CLIENT__SECRET");
        }

        public static readonly AnvEnv BASE_URL = new("IFOOD__BASE_URL");
    }

    public static readonly AnvEnv OTEL_SERVICE_NAME = new("OTEL_SERVICE_NAME");
}