
```
ISystemDefinition definition = new SystemDefinition();

IBanana b1 = new Banana();
definition.HasInstance(b1).Provides<IBanana>();

IBanana b2 = definition.Get<IBanana>();
IBanana b3 = definition.Get<IBanana>();

Assert.AreSame(b1, b2);
Assert.AreSame(b1, b3);
```