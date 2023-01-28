using Collections;
using System.Xml.Linq;
using System;

namespace CollectionsTests
{
    public class Tests
    {
        private static Collection<int> collection;

        [SetUp]
        public void Setup()
        {
            collection = new Collection<int>();
        }

        [Test]
        public void Test_Collection_EmptyConstructor()
        {
            Assert.That(collection, Is.Empty);

        }
        [Test]
        public void Test_Collection_ConstructorSingleItem()
        {
            collection.Add(77);
            Assert.That(collection.Count, Is.EqualTo(1));
        }
        [Test]
        public void Test_Collection_ConstructorMultipleItems()
        {
            collection.AddRange(new int[] { 1, 3, 7 });
            Assert.That(collection.Count, Is.EqualTo(3));
        }
        [Test]
        public void Test_Collection_Add()
        {
            collection.Add(55);
            Assert.That(collection[0], Is.EqualTo(55));
        }
        [Test]
        public void Test_Collection_AddWithGrow()
        {
            collection.AddRange(new int[] { 1, 3, 7, 87, 534, 334, 10, -1, 4, 19, 89, 74, 168, 42, 158, 98 });
            int countBeforeAdd = collection.Count;
            collection.Add(56);
            Assert.That(collection.Count, Is.GreaterThan(countBeforeAdd));
        }
        [Test]
        public void Test_Collection_AddRange()
        {
            var countBeforeAddRange = collection.Count;
            collection.AddRange(new int[] { 1, 3, 7, });
            Assert.That(collection.Count, Is.GreaterThan(countBeforeAddRange));
        }
        [Test]
        public void Test_Collection_GetByIndex()
        {
            int elemenet = 98;
            collection.Add(elemenet);
            Assert.That(collection[0], Is.EqualTo(elemenet));
        }
        [Test]
        public void Test_Collection_GetByInvalidIndex()
        {
            collection.AddRange(new int[] { 1, 7 });
            Assert.That(() => { int invalidIndex = collection[2]; }, Throws.InstanceOf<ArgumentOutOfRangeException>());

        }
        [Test]
        public void Test_Collection_SetByIndex()
        {
            collection.AddRange(new int[] { 1, 7, 9 });
            int expectedNumber = 5;
            collection[0] = expectedNumber;
            Assert.That(collection[0], Is.EqualTo(expectedNumber));
        }
        [Test]
        public void Test_Collection_SetByInvalidIndex()
        {
            Assert.That(() => { collection[1] = 5; }, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void Test_Collection_AddRangeWithGrow()
        {
            int initialCapacity = collection.Capacity;
            int[] newCollection = new int[20];
            collection.AddRange(newCollection);
            Assert.That(collection.Capacity, Is.GreaterThan(initialCapacity));
        }
        [Test]
        public void Test_Collection_InsertAtStart()
        {
            collection.AddRange(new int[] { 1, 5, 6 });
            int element = 90;
            collection.InsertAt(0, element);
            Assert.That(collection[0], Is.EqualTo(element));
        }
        [Test]
        public void Test_Collection_InsertAtEnd()
        {
            collection.AddRange(new int[] { 1, 5, 6 });
            int element = 23;
            collection.InsertAt(collection.Count, element);
            Assert.That(collection[collection.Count - 1], Is.EqualTo(element));
        }
        [Test]
        public void Test_Collection_InsertAtMiddle()
        {
            collection.AddRange(new int[] { 96, 256 });
            int elemenet = 234;
            collection.InsertAt(collection.Count / 2, elemenet);
            Assert.That(collection[collection.Count / 2], Is.EqualTo(elemenet));
        }
        [Test]
        public void Test_Collection_InsertAtWithGrow()
        {
            int[] randomNumbers = Enumerable.Range(7, 16).ToArray();
            collection.AddRange(randomNumbers);
            int initialCapacity = collection.Capacity;
            collection.InsertAt(collection.Count - 1, 5);
            Assert.That(collection.Capacity, Is.GreaterThan(initialCapacity));
        }
        [Test]
        public void Test_Collection_InsertAtInvalidIndex()
        {
            Assert.That(() => collection.InsertAt(-1, 55), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void Test_Collection_ExchangeMiddle()
        {
            collection.AddRange(new int[] { 1, 3, 67, 8 });
            int leftElemenet = collection[(collection.Count / 2) - 1];
            int rightElemenet = collection[collection.Count / 2];
            collection.Exchange((collection.Count / 2) - 1, collection.Count / 2);
            Assert.That(collection[(collection.Count / 2) - 1], Is.EqualTo(rightElemenet));
            Assert.That(collection[collection.Count / 2], Is.EqualTo(leftElemenet));
        }
        [Test]
        public void Test_Collection_ExchangeFirstLast()
        {
            collection.AddRange(new int[] { 6, 2, 8 });
            int firstElemenet = collection[0];
            int lastElemenet = collection[collection.Count - 1];
            collection.Exchange(0, collection.Count - 1);
            Assert.That(collection[0], Is.EqualTo(lastElemenet));
            Assert.That(collection[collection.Count - 1], Is.EqualTo(firstElemenet));
        }
        [Test]
        public void Test_Collection_ExchangeInvalidIndexes()
        {
            collection.AddRange(new int[] { 8, 2, 4 });
            Assert.That(() => collection.Exchange(-1, 4), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void Test_Collection_RemoveAtStart()
        {
            collection.AddRange(new int[] { 1, 2, 9 });
            int initialNumber = collection[0];
            collection.RemoveAt(0);
            Assert.That(initialNumber, Is.Not.EqualTo(collection[0]));

        }
        [Test]
        public void Test_Collection_RemoveAtEnd()
        {
            collection.AddRange(new int[] { 7, 26, 77, 47 });
            int initialNumber = collection[collection.Count - 1];
            collection.RemoveAt(collection.Count - 1);
            Assert.That(initialNumber, Is.Not.EqualTo(collection[collection.Count - 1]));
        }
        [Test]
        public void Test_Collection_RemoveAtMiddle()
        {
            collection.AddRange(new int[] { 12, 95, 38 });
            collection.RemoveAt(Convert.ToInt32(collection.Count / 2));
            Assert.That(collection.ToString(), Is.EqualTo("[12, 38]"));

        }
    [Test]
        public void Test_Collection_RemoveAtInvalidIndex()
        {
            collection.AddRange(new int[] { 4 });
            Assert.That(() => collection.RemoveAt(1), Throws.InstanceOf<ArgumentOutOfRangeException>());

        }
        [Test]
        public void Test_Collection_RemoveAll()
        {
            collection.AddRange(new int[] { 91 });
            collection.RemoveAt(0);
            Assert.That(collection.ToString(),Is.EqualTo("[]"));
        }
        [Test]
        public void Test_Collection_Clear()
        {
            collection.AddRange(new int[] { 91, 75, 84 });
            collection.Clear();
            Assert.Zero(collection.Count);
        }
        [Test]
        public void Test_Collection_CountAndCapacity()
        {
            int initialCount = collection.Count;
            int initialCapacity = collection.Capacity;
            int[] randomNumbers = Enumerable.Range(1, 17).ToArray();
            collection.AddRange(randomNumbers);
            Assert.That(collection.Count, Is.GreaterThan(initialCount));
            Assert.That(collection.Capacity, Is.GreaterThan(initialCapacity));
        }
        [Test]
        public void Test_Collection_ToStringEmpty()
        {
            Assert.That(collection.ToString(), Is.EqualTo("[]"));
        }
        [Test]
        public void Test_Collection_ToStringSingle()
        {
            collection.Add(55);
            Assert.That(collection.ToString(), Is.EqualTo("[55]"));
        }
        [Test]
        public void Test_Collection_ToStringMultiple()
        {
            collection.AddRange(new int[] { 8, 24, 13 });
            Assert.That(collection.ToString(), Is.EqualTo("[8, 24, 13]"));
        }
        [Test]
        public void Test_Collection_ToStringNastedCollections()
        {
            Collection<char> charCollection = new Collection<char>('o', 'n', 'e');
            Collection<double> doubleCollection = new Collection<double>(7.8, 6.3, 9.1);
            Collection<int> integerCollection = new Collection<int>(55, 77, 33);
            Collection<object> collection = new Collection<object>(charCollection, doubleCollection, integerCollection);
            Assert.That(collection.ToString(), Is.EqualTo("[[o, n, e], [7.8, 6.3, 9.1], [55, 77, 33]]"));
        }
        [Test]
        public void Test_Collection_1MillionItems()
        {
            collection.AddRange(Enumerable.Range(1, 1000000).ToArray());
            Assert.That(collection.Count, Is.EqualTo(1000000));
            Assert.GreaterOrEqual(collection.Capacity, collection.Count);
            collection.Clear();
            Assert.Zero(collection.Count);
            Assert.Greater(collection.Capacity, collection.Count);

        }
    }
}