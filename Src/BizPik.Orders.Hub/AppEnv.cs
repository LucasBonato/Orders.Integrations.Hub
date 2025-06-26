using Anv;

namespace BizPik.Orders.Hub;

public static class AppEnv
{
    public static class BIZPIK
    {
        public static class MONOLITH
        {
            public static class API_KEYS
            {
                public static readonly AnvEnv COMPANIES_INTEGRATIONS = new("BIZPIK__MONOLITH__API_KEYS__COMPANIES_INTEGRATIONS");
            }
            public static class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("BIZPIK__MONOLITH__ENDPOINT__BASE_URL");
            }
        }
        public static class ORDERS
        {
            public static class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("BIZPIK__ORDERS__ENDPOINT__BASE_URL");
            }
        }
    }
    public static class INTEGRATIONS
    {
        public static class IFOOD
        {
            public static class CLIENT
            {
                public static readonly AnvEnv ID = new("INTEGRATIONS__IFOOD__CLIENT__ID");
                public static readonly AnvEnv SECRET = new("INTEGRATIONS__IFOOD__CLIENT__SECRET");
            }
            public static class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("INTEGRATIONS__IFOOD__ENDPOINT__BASE_URL");
            }
            public static class CACHE
            {
                public static readonly AnvEnv KEY = new("INTEGRATIONS__IFOOD__CACHE__KEY");
            }
        }
        public static class RAPPI
        {
            public static class CLIENT
            {
                public static readonly AnvEnv ID = new("INTEGRATIONS__RAPPI__CLIENT__ID");
                public static readonly AnvEnv SECRET = new("INTEGRATIONS__RAPPI__CLIENT__SECRET");
                public static readonly AnvEnv AUDIENCE = new("INTEGRATIONS__RAPPI__CLIENT__AUDIENCE");
            }
            public static class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("INTEGRATIONS__RAPPI__ENDPOINT__BASE_URL");
                public static readonly AnvEnv AUTH = new("INTEGRATIONS__RAPPI__ENDPOINT__AUTH");
            }
            public static class CACHE
            {
                public static readonly AnvEnv KEY = new("INTEGRATIONS__RAPPI__CACHE__KEY");
            }
        }
    }
    public static class MEMCACHED
    {
        public static readonly AnvEnv ADDRESS = new("MEMCACHED__ADDRESS");
        public static readonly AnvEnv PORT = new("MEMCACHED__PORT");
    }
    public static class PUB_SUB
    {
        public static class TOPICS
        {
            public static readonly AnvEnv ACCEPT_ORDER = new("PUB_SUB__TOPICS__ACCEPT_ORDER");
            public static readonly AnvEnv IS_LOCAL = new("PUB_SUB__TOPICS__IS_LOCAL");
        }
    }
    public static class OBJECT_STORAGE
    {
        public static class BUCKET
        {
            public static readonly AnvEnv NAME = new("OBJECT_STORAGE__BUCKET__NAME");
        }
    }
    public static class LOCALSTACK
    {
        public static class AWS
        {
            public static readonly AnvEnv IS_LOCALSTACK = new("LOCALSTACK__AWS__IS_LOCALSTACK");
        }
        public static readonly AnvEnv ENDPOINT_URL = new("LOCALSTACK__ENDPOINT_URL");
    }
    public static readonly AnvEnv AWS_PROFILE = new("AWS_PROFILE");
    public static readonly AnvEnv AWS_REGION = new("AWS_REGION");
    public static readonly AnvEnv OTEL_SERVICE_NAME = new("OTEL_SERVICE_NAME");
    public static readonly AnvEnv OTEL_EXPORTER_OTLP_ENDPOINT = new("OTEL_EXPORTER_OTLP_ENDPOINT");
}