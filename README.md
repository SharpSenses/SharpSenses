SharpSenses
==============

An easier way to use the **RealSense** (2014) SDK! Custom poses, gestures and much more.

*Warning*: Make sure you have the RealSense SDK version 5.0.3.7777 installed before using SharpSenses. This is not ready for production, I'm changing the SDK (breaking changes sometimes) while I add new features, so stay tuned for version 1.0.

## SharpSenses.RealSense
> Nuget: Install-Package SharpSenses.RealSense

## Sample:
```
    ICamera cam = Camera.Create();
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
    cam.Poses.PeaceBegin += (s, a) => Console.WriteLine("Make love, not war");
    cam.Poses.PeaceEnd += (s, a) => Console.WriteLine("Bye!");
```

##Eyes
```
    cam.Face.LeftEye.Blink += (sender, eventArgs) => {
        Console.WriteLine("Blink");
    };
    cam.Face.LeftEye.DoubleBlink += (sender, eventArgs) => {
        Console.WriteLine("Double Blink");
    };
    cam.Face.WinkedLeft += (sender, eventArgs) => {
        Console.WriteLine("WinkedLeft");
    };
    cam.Face.WinkedRight += (sender, eventArgs) => {
        Console.WriteLine("WinkedRight");
    };
```

##Mouth
```
    cam.Face.Mouth.Opened += (s, a) => {
        Console.WriteLine("-> month opened");
    };

    cam.Face.Mouth.Closed += (s, a) => {
        Console.WriteLine("-> month closed");
    };

    cam.Face.Mouth.Smiled += (s, a) => {
        Console.WriteLine("-> month smiled");
    };
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

##Voice/Speech Synthesis 

Oh yeah, we speak!
```
    cam.Speech.Say("Isn't that cool?");
```

##Voice/Speech Recognition 

I can hear you, man!
```
    cam.Speech.SpeechRecognized += (s, a) => {
        Console.WriteLine("-> " + a.Sentence);
    };
    cam.Speech.EnableRecognition();
```

Don't forget that you have to have the Intel RealSense SDK v5.0.3.7777 (and the 3d camera, of course) for this library to work!
