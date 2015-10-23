Application architecture should separate the BuildingBlocks that make up an application from the ApplicationWiring that defines how they are linked together.

NDI is a DependencyInjection framework for .Net that provides a DesignPattern influenced DSL for defining the ApplicationWiring.

Using NDI promotes the use of highly DecoupledClasses and a scoped ServiceBasedArchitecture.

Check out GettingStarted for a quick intro and ExamplesOfUse for specific examples.


Author: [Nigel Thorne](http://www.nigelthorne.com/blog)

Dependencies : NMock2Extensions