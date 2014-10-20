SharpSenses
==============

An easier way to use **Intel 3d Cameras**. It works with both **Perceptual** (2013) and **RealSense** (2014) SDKs! Custom poses, gestures and much more.

*Warning*: This is not ready for production, I'm changing the SDK (breaking changes sometimes) while I add new features, so stay tuned for version 1.0.

## SharpSenses.Perceptual
---
#### Nuget: Instal-Package SharpSenses.Perceptual


## SharpSenses.RealSense
---
### Nuget: Instal-Package SharpSenses.RealSense



## Sample:
```
	ICamera cam = new RealSenseCamera();
	cam.RightHand.Closed += () => Console.WriteLine("Hand Closed");
	cam.RightHand.Moved += p => Console.WriteLine("-> x:{0} y:{1}", p.Image.X, p.Image.Y);
	cam.Start();
````
##Gestures

```
	cam.Gestures.SwipeLeft += s => Console.WriteLine("Swipe Left");
    cam.Gestures.SwipeRight += s => Console.WriteLine("Swipe Right");
    cam.Gestures.SwipeUp += s => Console.WriteLine("Swipe Up");
    cam.Gestures.SwipeDown += s => Console.WriteLine("Swipe Down");
```

##Poses
```
	cam.Poses.PeaceBegin += hand => Console.WriteLine("Make love, not war");
	cam.Poses.PeaceEnd += hand => Console.WriteLine("Bye!");
```

##Custom Poses
```
            var bothHandsClosed = PoseBuilder
                .Combine(cam.LeftHand, State.Closed)
                .With(cam.RightHand, State.Closed)
                .Build("bothhandsclosed");
            bothHandsClosed.Begin += s => Console.WriteLine("BOTH Begin");
            bothHandsClosed.End += s => Console.WriteLine("BOTH End");
```


Don't forget that you have to have the Intel RealSense SDK (and the 3d camera, of course) for this library to work!
