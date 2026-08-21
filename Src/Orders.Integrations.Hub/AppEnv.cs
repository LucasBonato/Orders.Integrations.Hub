using Anv;

namespace Orders.Integrations.Hub;

public static class AppEnv
{
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
        }
        public static class FOOD99
        {
            public static class CLIENT
            {
                public static readonly AnvEnv ID = new("INTEGRATIONS__FOOD99__CLIENT__ID");
                public static readonly AnvEnv SECRET = new("INTEGRATIONS__FOOD99__CLIENT__SECRET");
            }
            public static class ENDPOINT
            {
                public static readonly AnvEnv BASE_URL = new("INTEGRATIONS__FOOD99__ENDPOINT__BASE_URL");
            }
        }
    }
}