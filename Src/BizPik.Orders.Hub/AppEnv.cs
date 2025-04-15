using Anv;

namespace BizPik.Orders.Hub;

public static partial class AppEnv
{
    public static partial class BIZPIK
    {
        public partial class MONOLITH
        {
            public partial class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("BIZPIK__MONOLITH__ENDPOINT__BASE_URL");
            }

            public partial class API_KEYS
            {
                public static readonly AnvEnv COMPANIES_INTEGRATIONS = new("BIZPIK__MONOLITH__API_KEYS__COMPANIES_INTEGRATIONS");
            }
        }
        public partial class ORDERS
        {
            public partial class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("BIZPIK__ORDERS__ENDPOINT__BASE_URL");
            }
        }
    }

    public static partial class IFOOD
    {
        public static partial class CLIENT
        {
            public static readonly AnvEnv ID = new("IFOOD__CLIENT__ID");
            public static readonly AnvEnv SECRET = new("IFOOD__CLIENT__SECRET");
        }

        public static partial class ENDPOINT
        {
            public static readonly AnvEnv AUTH = new("IFOOD__ENDPOINT__AUTH");
        }

        public static readonly AnvEnv BASE_URL = new("IFOOD__BASE_URL");
    }

    public static readonly AnvEnv LOCAL_SNS = new("LOCAL_SNS");
    public static readonly AnvEnv AWS_PROFILE = new("AWS_PROFILE");
    public static readonly AnvEnv OTEL_SERVICE_NAME = new("OTEL_SERVICE_NAME");

}