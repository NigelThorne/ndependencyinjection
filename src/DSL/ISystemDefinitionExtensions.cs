#region usings

using NDependencyInjection.interfaces;

#endregion

namespace NDependencyInjection.DSL
{
    public delegate void CreateSubsystem ( ISystemDefinition scope );

    public static class SystemDefinitionExtensions
    {
        public static IServiceDefinition HasCollection ( this ISystemDefinition def,
            params CreateSubsystem[] subsystems )
        {
            var builders = new ISubsystemBuilder[subsystems.Length];
            for ( var i = 0; i < subsystems.Length; i++ ) builders[i] = new DelegateExecutingBuilder ( subsystems[i] );
            return def.HasCollection ( builders );
        }

        public static IServiceDefinition HasSubsystem ( this ISystemDefinition def, CreateSubsystem subsystem )
        {
            return def.HasSubsystem ( new DelegateExecutingBuilder ( subsystem ) );
        }

        public static ISystemDefinition CreateSubsystem ( this ISystemDefinition def, CreateSubsystem method )
        {
            return def.CreateSubsystem ( new DelegateExecutingBuilder ( method ) );
        }
    }
}