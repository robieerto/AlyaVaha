using Reinforced.Typings.Fluent;

namespace VahaAPI
{
    public static class ReinforcedTypingsConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            builder.Global(config =>
            {
                config.AutoOptionalProperties();
                config.UseModules();
            });
        }
    }
}
