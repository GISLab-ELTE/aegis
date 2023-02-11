// <copyright file="MultiValueDictionaryTest.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
//
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

namespace AEGIS.Tests.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Collections;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="MultiValueDictionary{TKey, TValue}" /> class.
    /// </summary>
    /// <author>Daniel Ballagi</author>
    [TestFixture]
    public class MultiValueDictionaryTest
    {
        /// <summary>
        /// The array of values that are inserted into the dictionary.
        /// </summary>
        private KeyValuePair<Int32, List<String>>[] values;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.values = new KeyValuePair<Int32, List<String>>[2]
            {
                new KeyValuePair<Int32, List<String>>(1, new List<String>() { "1", "11" }),
                new KeyValuePair<Int32, List<String>>(2, new List<String>() { "2", "22" }),
            };
        }

        /// <summary>
        /// Tests the constructor of the <see cref="MultiValueDictionary{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void MultiValueDictionaryConstructorTest()
        {
            // no parameters

            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();
            dictionary.Count.ShouldBe(0);
            dictionary.IsReadOnly.ShouldBeFalse();

            // comparer parameter

            dictionary = new MultiValueDictionary<Int32, String>(EqualityComparer<Int32>.Default);
            dictionary.Count.ShouldBe(0);
            dictionary.IsReadOnly.ShouldBeFalse();

            // copy constructor

            dictionary.Add(1, "one");
            MultiValueDictionary<Int32, String> secondDictionary = new MultiValueDictionary<Int32, String>(dictionary);
            secondDictionary.Count.ShouldBe(1);
            secondDictionary[1].ShouldBe(new List<String> { "one" });
            dictionary.IsReadOnly.ShouldBeFalse();

            // copy constructor with comparer parameter

            secondDictionary = new MultiValueDictionary<Int32, String>(dictionary, EqualityComparer<Int32>.Default);
            secondDictionary.Count.ShouldBe(1);
            secondDictionary[1].ShouldBe(new List<String> { "one" });
            dictionary.IsReadOnly.ShouldBeFalse();
        }

        /// <summary>
        /// Tests the <see cref="MultiValueDictionary{TKey, TValue}.Add(TKey, ICollection{TValue})" /> method.
        /// </summary>
        [Test]
        public void MultiValueDictionaryAddTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();
            dictionary.Add(1, "1");
            dictionary.Count.ShouldBe(1);
            dictionary[1].ShouldBe(new List<String> { "1" });

            dictionary.Add(1, "11");
            dictionary.Count.ShouldBe(1);
            dictionary[1].ShouldBe(new List<String> { "1", "11" });

            dictionary.Add(2, new List<String> { "2", "22" });
            dictionary.Count.ShouldBe(2);
            dictionary[2].ShouldBe(new List<String> { "2", "22" });

            dictionary.Add(2, new List<String> { "222", "2222" });
            dictionary.Count.ShouldBe(2);
            dictionary[2].ShouldBe(new List<String> { "2", "22", "222", "2222" });

            (dictionary as ICollection<KeyValuePair<Int32, ICollection<String>>>).Add(new KeyValuePair<Int32, ICollection<String>>(3, new List<String> { "3" }));
            dictionary.Count.ShouldBe(3);
            dictionary[3].ShouldBe(new List<String> { "3" });

            dictionary[4] = new List<String> { "4" };
            dictionary.Count.ShouldBe(4);
            dictionary[4].ShouldBe(new List<String> { "4" });
        }

        /// <summary>
        /// Tests the <see cref="MultiValueDictionary{TKey, TValue}.Remove(TKey, TValue)" /> method.
        /// </summary>
        [Test]
        public void MultiValueDictionaryRemoveTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> value in this.values)
                dictionary.Add(value.Key, value.Value);

            dictionary.Remove(0, "0").ShouldBeFalse();
            dictionary.Remove(1, "1").ShouldBeTrue();
            dictionary.Remove(1, "1").ShouldBeFalse();
            dictionary.Remove(1, "11").ShouldBeTrue();
            dictionary.Remove(1, "11").ShouldBeFalse();

            dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> value in this.values)
                dictionary.Add(value.Key, value.Value);

            dictionary.Remove(0).ShouldBeFalse();
            dictionary.Remove(1).ShouldBeTrue();
            dictionary.Remove(1).ShouldBeFalse();

            (dictionary as ICollection<KeyValuePair<Int32, ICollection<String>>>).Add(new KeyValuePair<Int32, ICollection<String>>(3, new List<String> { "3" }));
            (dictionary as ICollection<KeyValuePair<Int32, ICollection<String>>>).Remove(new KeyValuePair<Int32, ICollection<String>>(3, new List<String> { "3" })).ShouldBeTrue();
            (dictionary as ICollection<KeyValuePair<Int32, ICollection<String>>>).Remove(new KeyValuePair<Int32, ICollection<String>>(3, new List<String> { "3" })).ShouldBeFalse();
        }

        /// <summary>
        /// Tests the <see cref="MultiValueDictionary{TKey, TValue}.ContainsKey(TKey)" /> method.
        /// </summary>
        [Test]
        public void MultiValueDictionaryContainsTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> value in this.values)
                dictionary.Add(value.Key, value.Value);

            dictionary.ContainsKey(1).ShouldBeTrue();
            dictionary.ContainsKey(0).ShouldBeFalse();
        }

        /// <summary>
        /// Tests the <see cref="MultiValueDictionary{TKey, TValue}.Keys" /> property.
        /// </summary>
        [Test]
        public void MultiValueDictionaryKeysTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> value in this.values)
                dictionary.Add(value.Key, value.Value);
            dictionary.Keys.ShouldBe(new List<Int32> { 1, 2 });
        }

        /// <summary>
        /// Tests the <see cref="MultiValueDictionary{TKey, TValue}.Values" /> property.
        /// </summary>
        [Test]
        public void MultiValueDictionaryValuesTest()
        {
            MultiValueDictionary<Int32, String> dictionary = new MultiValueDictionary<Int32, String>();

            foreach (KeyValuePair<Int32, List<String>> value in this.values)
                dictionary.Add(value.Key, value.Value);

            List<ICollection<String>> expected = new List<ICollection<String>>()
            {
                new List<String>() { "1", "11" },
                new List<String>() { "2", "22" },
            };

            dictionary.Values.ShouldBe(expected);
        }
    }
}
