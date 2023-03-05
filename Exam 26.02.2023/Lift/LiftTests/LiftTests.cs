using Lift;
using System.Numerics;

namespace LiftTests
{
    public class LiftTests
    {
        private LiftSimulator simulator;
        public LiftTests()
        {
            simulator = new LiftSimulator();
        }
        [Test]
        public void Check_FitPeopleOnTheLift_With_ValidPeopleCount()
        {
            var expected = simulator.FitPeopleOnTheLift(16, new int[] { 0, 0, 0, 0 });
            Assert.That(expected, Is.EqualTo(new int[]{ 4, 4, 4, 4}));
        }
        [Test]
        public void Check_FitPeopleOnTheLift_With_InvalidPeopleCount()
        {
            Assert.That(() => simulator.FitPeopleOnTheLift(-5,new int[] {0,0,0,0}), Throws.InstanceOf<ArgumentException>(), "People waiting should be > 0");
        }
        [Test]
        public void Check_FitPeopleOnTheLift_With_InvalidLiftSize()
        {
            Assert.That(() => simulator.FitPeopleOnTheLift(15, new int[5] { -4, 15, -1, 456, 10 }), Throws.InstanceOf<ArgumentException>(), "Invalid lift size. It should have positive number of cabins");
        }
        [Test]
        public void Check_FitPeopleOnTheLift_With_InvalidLiftState()
        { 
            Assert.That(() => simulator.FitPeopleOnTheLift(15, new int[] { }), Throws.InstanceOf<ArgumentException>(), "Invalid lift seat: ");
        }

        [Test]
        public void Check_FitPeopleOnTheLiftAndGetResult_When_ThereAreNotEnoughSpaceOnLift()
        {
            var expected = simulator.FitPeopleOnTheLiftAndGetResult(20, new int[] { 0, 2, 0 });
            Assert.That(expected, Is.EqualTo("There isn't enough space! 10 people in a queue!\r\n4 4 4"));
        }
        [Test]
        public void Check_FitPeopleOnTheLiftAndGetResult_When_LiftHasEmptySpace()
        {
            var expected = simulator.FitPeopleOnTheLiftAndGetResult(15, new int[] { 0, 0, 0, 0 });
            Assert.That(expected, Is.EqualTo("The lift has 1 empty spots!\r\n4 4 4 3"));
        }
        [Test]
        public void Check_FitPeopleOnTheLiftAndGetResult_When_LiftIsFull()
        {
            var expected = simulator.FitPeopleOnTheLiftAndGetResult(6, new int[] { 1, 2, 3, 4 });
            Assert.That(expected, Is.EqualTo("All people placed and the lift if full.\r\n4 4 4 4"));
        }

    }
}
