# Introduction #

When more than one class wants to be an Observer of another class then your need to handle a list of observers. When you have more than one class with multiple observers, you have symantic duplication. This pattern solves that.


# Details #

If you have more than once Observer implementing the IAObserver interface, then rather than class A having a list of observers, you can have a Broadcaster.

A Broadcaster is an Observer so would implement IAObserver, and is passed to A as A's only observer. The Broadcaster also has a list of IAObservers and passes on any calls to its IAObserver interface to all it's observers, thereby broadcasting the event.

The Broadcaster inherently implements the NullObjectPattern as if there are no observers registered with it, it swallows the event. The sender has no idea the event was swallowed, however as the sender always has one listener (the broadcaster) it doesn't need to test for null.