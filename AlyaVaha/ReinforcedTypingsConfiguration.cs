using Reinforced.Typings.Fluent;

namespace AlyaVaha
{
    public static class ReinforcedTypingsConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            builder.Global(config =>
            {
                config.AutoOptionalProperties();
                //config.UseModules();
            });
        }
    }
}
