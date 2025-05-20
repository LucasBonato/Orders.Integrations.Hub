using Anv;

using Orders.Integrations.Hub.Modules.Core.Orders.Application.Exceptions;

namespace Orders.Integrations.Hub.Modules.Core.Orders.Application.Extensions;

public static class ExceptionsExtensions {
    public static string NotNullEnv(this AnvEnv env) {
        return env.Value?? throw new EnvironmentVariableNotFoundException(env.Name);
    }
}