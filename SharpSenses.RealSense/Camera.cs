using System;
using SharpSenses.Gestures;
using SharpSenses.Poses;
using SharpSenses.RealSense;

namespace SharpSenses {
    public abstract class Camera : ICamera {
        private Face _face;
        
        protected GestureSensor _gestures;
        protected PoseSensor _poses;
        
        public abstract int ResolutionWidth { get; }
        public abstract int ResolutionHeight { get; }
        public Hand LeftHand { get; private set; }
        public Hand RightHand { get; private set; }
        
        public Face Face {
            get {
                return _face ?? (_face = new Face(GetFaceRecognizer()));
            }
        }

        public IGestureSensor Gestures {
            get { return _gestures; }
        }

        public IPoseSensor Poses {
            get { return _poses; }
        }

        public abstract ISpeech Speech { get; }
        public abstract void Start();
        public abstract void Dispose();

        protected Camera() {
            LeftHand = new Hand(Side.Left);
            RightHand = new Hand(Side.Right);
            _gestures = new GestureSensor();
            _poses = new PoseSensor();
        }


        public static ICamera Create() {
            RealSenseAssembliesLoader.Load();
            return new RealSenseCamera();
        }

        protected abstract IFaceRecognizer GetFaceRecognizer();

        protected Position CreatePosition(Point3D imagePosition, Point3D worldPosition) {
            return new Position {
                Image = new Point3D(imagePosition.X, imagePosition.Y),
                World = new Point3D(ToRoundedCentimeters(worldPosition.X),
                                    ToRoundedCentimeters(worldPosition.Y),
                                    ToRoundedCentimeters(worldPosition.Z))
            };
        }
        protected double ToRoundedCentimeters(double value) {
            return Math.Round(value * 100, 2);
        }
    }
}