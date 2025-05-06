using Anv;

namespace BizPik.Orders.Hub;

public static partial class AppEnv
{
    public static partial class BIZPIK
    {
        public static partial class MONOLITH
        {
            public static partial class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("BIZPIK__MONOLITH__ENDPOINT__BASE_URL");
            }

            public static partial class API_KEYS
            {
                public static readonly AnvEnv COMPANIES_INTEGRATIONS = new("BIZPIK__MONOLITH__API_KEYS__COMPANIES_INTEGRATIONS");
            }
        }

        public static partial class ORDERS
        {
            public static partial class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("BIZPIK__ORDERS__ENDPOINT__BASE_URL");
            }
        }
    }

    public static partial class INTEGRATIONS
    {
        public static partial class IFOOD
        {
            public static partial class CLIENT
            {
                public static readonly AnvEnv ID = new("INTEGRATIONS__IFOOD__CLIENT__ID");
                public static readonly AnvEnv SECRET = new("INTEGRATIONS__IFOOD__CLIENT__SECRET");
            }

            public static partial class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("INTEGRATIONS__IFOOD__ENDPOINT__BASE_URL");
                public static readonly AnvEnv AUTH = new("INTEGRATIONS__IFOOD__ENDPOINT__AUTH");
            }
        }

        public static partial class RAPPI
        {
            public static partial class CLIENT
            {
                public static readonly AnvEnv ID = new("INTEGRATIONS__RAPPI__CLIENT__ID");
                public static readonly AnvEnv SECRET = new("INTEGRATIONS__RAPPI__CLIENT__SECRET");
                public static readonly AnvEnv AUDIENCE = new("INTEGRATIONS__RAPPI__CLIENT__AUDIENCE");
            }

            public static partial class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("INTEGRATIONS__RAPPI__ENDPOINT__BASE_URL");
                public static readonly AnvEnv AUTH = new("INTEGRATIONS__RAPPI__ENDPOINT__AUTH");
            }
        }
    }

    public static partial class PUB_SUB
    {
        public static partial class TOPICS
        {
            public static readonly AnvEnv ACCEPT_ORDER = new("PUB_SUB__TOPICS__ACCEPT_ORDER");
            public static readonly AnvEnv IS_LOCAL = new("PUB_SUB__TOPICS__IS_LOCAL");
        }
    }

    public static readonly AnvEnv AWS_API_GATEWAY_PROXY = new("AWS_API_GATEWAY_PROXY");
    public static readonly AnvEnv AWS_PROFILE = new("AWS_PROFILE");
    public static readonly AnvEnv OTEL_SERVICE_NAME = new("OTEL_SERVICE_NAME");
}