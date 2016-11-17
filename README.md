<p align="center">
  <img src="https://raw.githubusercontent.com/SharpSenses/logos/master/sharp_senses.png" width="350px" alt="SharpSenses" />
</p>
<p>
An easier way to use the **RealSense** SDK! Custom poses, gestures and much more.

<sub>***Warning:*** *Make sure you have the RealSense SDK version 10 installed before using SharpSenses. This is not ready for production, I'm changing the SDK (breaking changes sometimes) while I add new features, so stay tuned for version 1.0.*</sub>

## SharpSenses.RealSense
> Nuget: Install-Package SharpSenses.RealSense

## Sample:
```
    var cam = Camera.Create(Capability.HandTracking);
    cam.LeftHand.Visible += (s,a) => Console.WriteLine("Hi left hand!");
    cam.RightHand.Closed += (s,a) => Console.WriteLine("Hand Closed");
    cam.RightHand.Moved += (s,a) => {
        Console.WriteLine("-> x:{0} y:{1}", a.Position.Image.X, a.Position.Image.Y);
    }
    cam.Start();
````

## Enabling Capabilities

For performance reasons, you have to tell the camera which modules will be loaded for use.
The available modules are:

- HandTracking,
- FingersTracking,
- GestureTracking,
- FaceTracking,
- FaceRecognition,
- FacialExpressionTracking,
- ImageStreamTracking,
- SegmentationStreamTracking 

You can enable the modules when creating the Camera object or calling the method "AddCapability", always before calling "Start".

```
    var cam = Camera.Create(Capability.HandTracking, Capability.FingersTracking);
    or
    cam.AddCapability(Capability.FaceTracking);
```

# Examples:

##Gestures

```
    cam.Gestures.SlideLeft += (s, a) => Console.WriteLine("Swipe Left");
    cam.Gestures.SlideRight += (s, a) => Console.WriteLine("Swipe Right");
    cam.Gestures.SlideUp += (s, a) => Console.WriteLine("Swipe Up");
    cam.Gestures.SlideDown += (s, a) => Console.WriteLine("Swipe Down");
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
        Console.WriteLine("-> Mouth opened");
    };

    cam.Face.Mouth.Closed += (s, a) => {
        Console.WriteLine("-> Mouth closed");
    };

    cam.Face.Mouth.Smiled += (s, a) => {
        Console.WriteLine("-> Mouth smiled");
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

OBS: Unfortunately this feature was deprecated by Intel

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

Don't forget that you have to have the Intel RealSense SDK v10 (and the 3d camera, of course) for this library to work!
