using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpSenses.Gestures;

namespace SharpSenses.Tests {
    public class GestureSlideRightTests {

        private GestureSlideRight _sensor;
        private Hand _hand;
        private const int Middle = 100;
        private const int BeginLimit = 130;
        private const int EndLimit = 70;
        private bool _gestureFired;

        [SetUp]
        public void SetUp() {
            GestureSlide.WrongDirectionTolerance = 0;
            GestureSlide.GestureLength = 30;
            _gestureFired = false;
            _hand = new Hand(Side.Left);
            _sensor = new GestureSlideRight(_hand, Middle);
            _sensor.SlideDetected += (sender, args) => {
                _gestureFired = true;
            };
        }

        [Test]
        [TestCase(Middle)]
        [TestCase(EndLimit)]
        [TestCase(BeginLimit-1)]
        public void Is_out_begin_area(int position) {
            SetHandPositionWidth(position);
            Assert.IsFalse(_sensor.GestureHappening);
        }

        [Test]
        [TestCase(BeginLimit)]
        [TestCase(BeginLimit+1)]
        public void Enters_begin_area(int position) {
            SetHandPositionWidth(Middle);
            SetHandPositionWidth(position);
            Assert.IsTrue(_sensor.GestureHappening);
        }

        [Test]
        public void Enters_begin_area_and_goes_wrong_direction() {
            SetHandPositionWidth(Middle);
            SetHandPositionWidth(BeginLimit);
            SetHandPositionWidth(BeginLimit+10);
            Assert.IsFalse(_sensor.GestureHappening);
        }

        [Test]
        public void Enters_begin_area_and_goes_right_direction() {
            SetHandPositionWidth(Middle);
            SetHandPositionWidth(BeginLimit);
            SetHandPositionWidth(BeginLimit - 10);
            SetHandPositionWidth(BeginLimit - 10);
            Assert.IsTrue(_sensor.GestureHappening);
        }

        [Test]
        public void Respects_wrong_direction_tolerance() {
            GestureSlide.WrongDirectionTolerance = 1;
            SetHandPositionWidth(Middle);
            SetHandPositionWidth(BeginLimit);
            SetHandPositionWidth(BeginLimit+1);
            Assert.IsTrue(_sensor.GestureHappening);
        }

        [Test]
        public void Gesture_fired() {
            SetHandPositionWidth(Middle);
            SetHandPositionWidth(BeginLimit);
            SetHandPositionWidth(EndLimit);
            Assert.IsTrue(_gestureFired);
            Assert.IsFalse(_sensor.GestureHappening);
        }

        [Test]
        public void Hand_has_to_return_before_firing_gesture_again() {
            SetHandPositionWidth(Middle);
            SetHandPositionWidth(BeginLimit);
            SetHandPositionWidth(EndLimit);
            _gestureFired = false;
            SetHandPositionWidth(EndLimit-10);
            Assert.IsFalse(_gestureFired);
        }

        private void SetHandPositionWidth(int value) {
            var p = new Position {Image = new Point3D(value)};
            _hand.Position = p;
        }
    }
}
