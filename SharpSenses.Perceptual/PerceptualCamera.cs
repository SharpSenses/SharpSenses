using System;
using System.Diagnostics;

namespace SharpSenses.Perceptual {
    public class PerceptualCamera : Camera {
        private Pipeline _pipeline;

        public PerceptualCamera() {
            _pipeline = new Pipeline(this);
        }

        public override int ResolutionWidth {
            get { return 320; }
        }

        public override int ResolutionHeight {
            get { return 240; }
        }

        public override void Start() {
            _pipeline.Start();
        }

        public void OnNewFrame() {
            TrackHandAndFingers(LeftHand, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_LEFT);
            TrackHandAndFingers(RightHand, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_RIGHT);
            TrackFace();

            //Get face location
            //faceLocation = (PXCMFaceAnalysis.Detection)faceAnalysis.DynamicCast(PXCMFaceAnalysis.Detection.CUID);
            //locationStatus = faceLocation.QueryData(faceId, out faceLocationData);
            //detectionConfidence = faceLocationData.confidence.ToString();
            //parent.label1.Text = "Confidence: " + detectionConfidence;

            ////Get face landmarks (eye, mouth, nose position, etc)
            //faceLandmark = (PXCMFaceAnalysis.Landmark)faceAnalysis.DynamicCast(PXCMFaceAnalysis.Landmark.CUID);
            //faceLandmark.QueryProfile(1, out landmarkProfile);
            //faceLandmark.SetProfile(ref landmarkProfile);
            //faceLandmarkData = new PXCMFaceAnalysis.Landmark.LandmarkData[7];
            //landmarkStatus = faceLandmark.QueryLandmarkData(faceId, PXCMFaceAnalysis.Landmark.Label.LABEL_7POINTS, faceLandmarkData);

            ////Get face attributes (smile, age group, gender, eye blink, etc)
            //faceAttributes = (PXCMFaceAnalysis.Attribute)faceAnalysis.DynamicCast(PXCMFaceAnalysis.Attribute.CUID);
            //faceAttributes.QueryProfile(PXCMFaceAnalysis.Attribute.Label.LABEL_EMOTION, 0, out attributeProfile);
            //faceAttributes.SetProfile(PXCMFaceAnalysis.Attribute.Label.LABEL_EMOTION, ref attributeProfile);
            //attributeStatus = faceAttributes.QueryData(PXCMFaceAnalysis.Attribute.Label.LABEL_EMOTION, faceId, out smile);

            //faceAttributes.QueryProfile(PXCMFaceAnalysis.Attribute.Label.LABEL_EYE_CLOSED, 0, out attributeProfile);
            //attributeProfile.threshold = 50; //Must be here!
            //faceAttributes.SetProfile(PXCMFaceAnalysis.Attribute.Label.LABEL_EYE_CLOSED, ref attributeProfile);
            //attributeStatus = faceAttributes.QueryData(PXCMFaceAnalysis.Attribute.Label.LABEL_EYE_CLOSED, faceId, out blink);

            //faceAttributes.QueryProfile(PXCMFaceAnalysis.Attribute.Label.LABEL_GENDER, 0, out attributeProfile);
            //faceAttributes.SetProfile(PXCMFaceAnalysis.Attribute.Label.LABEL_GENDER, ref attributeProfile);
            //attributeStatus = faceAttributes.QueryData(PXCMFaceAnalysis.Attribute.Label.LABEL_GENDER, faceId, out gender);

            //faceAttributes.QueryProfile(PXCMFaceAnalysis.Attribute.Label.LABEL_AGE_GROUP, 0, out attributeProfile);
            //faceAttributes.SetProfile(PXCMFaceAnalysis.Attribute.Label.LABEL_AGE_GROUP, ref attributeProfile);
            //attributeStatus = faceAttributes.QueryData(PXCMFaceAnalysis.Attribute.Label.LABEL_AGE_GROUP, faceId, out age_group);
        }

        private void TrackHandAndFingers(Hand hand, PXCMGesture.GeoNode.Label bodyLabel) {
            var geoNode = QueryGeoNode(bodyLabel);
            TrackPosition(hand, geoNode);
            TrackOpeness(hand, geoNode);

            TrackFingers(hand.Thumb, bodyLabel | PXCMGesture.GeoNode.Label.LABEL_FINGER_THUMB);
            TrackFingers(hand.Index, bodyLabel | PXCMGesture.GeoNode.Label.LABEL_FINGER_INDEX);
            TrackFingers(hand.Middle, bodyLabel | PXCMGesture.GeoNode.Label.LABEL_FINGER_MIDDLE);
            TrackFingers(hand.Ring, bodyLabel | PXCMGesture.GeoNode.Label.LABEL_FINGER_RING);
            TrackFingers(hand.Pinky, bodyLabel | PXCMGesture.GeoNode.Label.LABEL_FINGER_PINKY);
        }

        private void TrackFace() {
            PXCMFaceAnalysis.Detection.Data data;
            var face = _pipeline.QueryFace();
            ulong timestamp;
            int faceId;
            face.QueryFace(0, out faceId, out timestamp);
            var location = (PXCMFaceAnalysis.Detection) face.DynamicCast(PXCMFaceAnalysis.Detection.CUID);
            location.QueryData(faceId, out data);
            Face.IsVisible = data.rectangle.x > 0;
            //Debug.WriteLine("{0}|{1}|{2}|{3}",
            //    data.rectangle.x,
            //    data.rectangle.w,
            //    data.rectangle.y,
            //    data.rectangle.h
            //    );
            var rect = data.rectangle;
            var point = new Point3D(rect.x - (rect.w), rect.y);
            Face.Position = CreatePosition(point, new Point3D());
        }

        private void TrackPosition(Item item, PXCMGesture.GeoNode geoNode) {
            item.IsVisible = geoNode.body != PXCMGesture.GeoNode.Label.LABEL_ANY;
            if (!item.IsVisible) {
                return;
            }
            var imagePosition = ToPoint3D(geoNode.positionImage);
            var worldPosition = ToPoint3D(geoNode.positionWorld);
            item.Position = CreatePosition(imagePosition, worldPosition);
        }

        private Point3D ToPoint3D(PXCMPoint3DF32 p) {
            return new Point3D(p.x, p.y, p.z);
        }

        private void TrackOpeness(FlexiblePart part, PXCMGesture.GeoNode geoNode) {
            if (!part.IsVisible) {
                return;
            }
            if (geoNode.openness > 75) {
                part.IsOpen = true;
            }
            else if (geoNode.openness < 10) {
                part.IsOpen = false;
            }
        }

        private void TrackFingers(Item finger, PXCMGesture.GeoNode.Label fingerLabel) {
            var geoNode = QueryGeoNode(fingerLabel);
            TrackPosition(finger, geoNode);
        }

        private PXCMGesture.GeoNode QueryGeoNode(PXCMGesture.GeoNode.Label bodyLabel) {
            PXCMGesture.GeoNode values;
            _pipeline.QueryGesture().QueryNodeData(0, bodyLabel, out values);
            return values;
        }

        public void OnGesture(PXCMGesture.Gesture gesture) {
            Hand hand;
            if (gesture.body == PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_LEFT) {
                hand = LeftHand;
            }
            else {
                hand = RightHand;
            }
            Debug.WriteLine(gesture.body + " Gesture: {0} Visible: {1} Confidence: {2}", gesture.label, gesture.active,
                gesture.confidence);
            switch (gesture.label) {
                case PXCMGesture.Gesture.Label.LABEL_POSE_BIG5:
                    //IfThisElseThat(gesture.active, Poses.OnBigFiveBegin, Poses.OnBigFiveEnd);
                    break;
                case PXCMGesture.Gesture.Label.LABEL_POSE_PEACE:
                    IfThisElseThat(gesture.active, () => _poses.OnPosePeaceBegin(hand), () => _poses.OnPosePeaceEnd(hand));
                    break;
                case PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_DOWN:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_HAND_CIRCLE:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_HAND_WAVE:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_DOWN:
                    _gestures.OnGestureSwipeDown(hand);
                    break;
                case PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_UP:
                    _gestures.OnGestureSwipeUp(hand);
                    break;
                case PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_RIGHT:
                    _gestures.OnGestureSwipeRight(hand);
                    break;
                case PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_LEFT:
                    _gestures.OnGestureSwipeLeft(hand);
                    break;
                case PXCMGesture.Gesture.Label.LABEL_SET_CUSTOMIZED:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_SET_POSE:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_SET_NAVIGATION:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_SET_HAND:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_MASK_DETAILS:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_MASK_SET:
                    break;
                case PXCMGesture.Gesture.Label.LABEL_ANY:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Disable(FlexiblePart flexiblePart, PXCMGesture.Alert alert) {
            if (alert.label == PXCMGesture.Alert.Label.LABEL_GEONODE_INACTIVE) {
                flexiblePart.IsVisible = false;
            }
        }

        private void IfThisElseThat(bool value, Action ifTrue, Action ifFalse) {
            if (value) {
                ifTrue();
            }
            else {
                ifFalse();
            }
        }

        public override void Dispose() {
            _pipeline.Dispose();
        }
    }
}