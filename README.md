SharpSenses
==============

An easier way to use Intel RealSense SDK. Custom poses, gestures and much more.

*Warning*: This is not ready for production, I'm changing the SDK (breaking changes sometimes) while I add new features, so stay tuned for version 1.0.

---
### Nuget: Instal-Package SharpSenses
---

## Sample:
```
	var cam = new Camera();
	cam.RightHand.Closed += () => Console.WriteLine("Hand Closed");
	cam.RightHand.Moved += p => Console.WriteLine("-> x:{0} y:{1}", m.Image.X, m.Image.Y);
	cam.Start();
````

Don't forget that you have to have the Intel RealSense SDK (and the 3d camera, of course) for this library to work!
