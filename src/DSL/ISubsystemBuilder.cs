//Copyright (c) 2008 Nigel Thorne

namespace NDependencyInjection.DSL
{
    /// <summary>
    /// Builders know the description of a subsystem.
    /// They are given a child scope when Build is called. 
    /// This means they can define what they want and it's all in a safe bubble.
    /// You inherate everything from the parent scope. 
    /// The parent can only access the interfaces your system exposes.
    /// </summary>
    public interface ISubsystemBuilder
    {
        void Build(ISystemDefinition system);
    }
}