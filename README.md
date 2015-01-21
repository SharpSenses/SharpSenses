SharpSenses
==============

An easier way to use **Intel 3d Cameras**. It works with both **Perceptual** (2013) and **RealSense** (2014) SDKs! Custom poses, gestures and much more.

*Warning*: This is not ready for production, I'm changing the SDK (breaking changes sometimes) while I add new features, so stay tuned for version 1.0.

## SharpSenses.Perceptual
>  Nuget: Install-Package SharpSenses.Perceptual


## SharpSenses.RealSense
> Nuget: Install-Package SharpSenses.RealSense

## Sample:
```
    ICamera cam = Camera.Create(); //autodiscovers your sdk (perceptual or realsense)
    cam.LeftHand.Visible += (s,a) => Console.WriteLine("Hi left hand!");
    cam.RightHand.Closed += (s,a) => Console.WriteLine("Hand Closed");
    cam.RightHand.Moved += (s,a) => {
        Console.WriteLine("-> x:{0} y:{1}", a.Position.Image.X, a.Position.Image.Y);
    }
    cam.Start();
````
##Gestures

```
    cam.Gestures.SlideLeft += (s, a) => Console.WriteLine("Swipe Left");
    cam.Gestures.SlideRight += (s, a) => Console.WriteLine("Swipe Right");
    cam.Gestures.SlideUp += (s, a) => Console.WriteLine("Swipe Up");
    cam.Gestures.SlideDown += (s, a) => Console.WriteLine("Swipe Down");
    cam.Gestures.MoveForward += (s, a) => Console.WriteLine("Move Forward");
```

##Poses
```
    cam.Poses.PeaceBegin += hand => Console.WriteLine("Make love, not war");
    cam.Poses.PeaceEnd += hand => Console.WriteLine("Bye!");
```

##Custom Poses
```
    var pose = PoseBuilder.Create().ShouldBeNear(_cam.LeftHand, _cam.RightHand, 100).Build();
        pose.Begin += (s, a) => {
        Console.WriteLine("Super pose!");
    };
    pose.Begin += (s, a) => DoSomething();
```

##Facial Expressions

- Anger
- Contempt
- Disgust
- Fear
- Joy 
- Sadness
- Surprise

```
    cam.Face.FacialExpresssionChanged += (s, e) => {
        Console.WriteLine("FacialExpression: " + e.NewFacialExpression);
    }
```

##Face Recognition

Anytime you want to recognite a new face, call:
```
	cam.Face.RecognizeFace();
```

You can always get a notification when a new or pre-recognized face is recognized: 
```
    _cam.Face.PersonRecognized += (s, a) => {
        Console.WriteLine("Hello " + a.UserId); 
    };
```

Don't forget that you have to have the Intel RealSense SDK (and the 3d camera, of course) for this library to work!
