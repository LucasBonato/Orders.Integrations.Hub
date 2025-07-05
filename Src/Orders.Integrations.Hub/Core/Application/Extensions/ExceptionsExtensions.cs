using Anv;

using Orders.Integrations.Hub.Core.Application.Exceptions;

namespace Orders.Integrations.Hub.Core.Application.Extensions;

public static class ExceptionsExtensions {
    public static string NotNullEnv(this AnvEnv env) {
        return env.Value?? throw new EnvironmentVariableNotFoundException(env.Name);
    }
}