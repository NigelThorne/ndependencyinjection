
```
ISubsystemBuilder bananaBuilder = new BananaBuilder();
ISubsystemBuilder appleBuilder  = new AppleBuilder();
ISubsystemBuilder pearBuilder   = new PearBuilder();

ISystemDefinition system = new SystemDefinition();

system.HasCollection(bananaBuilder, appleBuilder, pearBuilder).Provides<IFruit[]>();

IFruit[] fruits = system.Get<IFruit[]>();

Assert.AreEqual(3, fruits.Length());
```