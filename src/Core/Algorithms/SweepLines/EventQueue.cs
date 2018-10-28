// <copyright file="EventQueue.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Algorithms.SweepLines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Collections;
    using Resources;

    /// <summary>
    /// Represents an event queue used by Sweep lines.
    /// </summary>
    public class EventQueue
    {
        /// <summary>
        /// Represents a heap data structure containing <see cref="Event" /> instances.
        /// </summary>
        private sealed class EventHeap : Heap<Event, Event>
        {
            /// <summary>
            /// An inner <see cref="CoordinateComparer" /> instance.
            /// </summary>
            private readonly CoordinateComparer coordinateComparer;

            /// <summary>
            /// Initializes a new instance of the <see cref="EventHeap" /> class.
            /// </summary>
            public EventHeap()
                : base(new EventComparer())
            {
                this.coordinateComparer = new CoordinateComparer();
            }

            /// <summary>
            /// Inserts the specified event element into the heap.
            /// </summary>
            /// <param name="eventElement">The event element of the element to insert.</param>
            /// <exception cref="System.ArgumentNullException">The event is null.</exception>
            public void Insert(Event eventElement)
            {
                if (eventElement == null)
                    throw new ArgumentNullException(nameof(eventElement));

                this.Insert(eventElement, eventElement);
            }

            /// <summary>
            /// Determines whether the <see cref="EventHeap" /> contains any event element with the given coordinate.
            /// </summary>
            /// <param name="position">The coordinate position to locate in the <see cref="EventHeap" />.</param>
            /// <returns><c>true</c> if the <see cref="EventHeap" /> contains an event element with the specified position; otherwise <c>false</c>.</returns>
            public Boolean Contains(Coordinate position)
            {
                return this.Any(item => this.coordinateComparer.Compare(item.Key.Vertex, position) == 0);
            }
        }

        /// <summary>
        /// The coordinate comparer.
        /// </summary>
        private readonly IComparer<Coordinate> comparer;

        /// <summary>
        /// The event heap.
        /// </summary>
        private EventHeap eventHeap;

        /// <summary>
        /// The number of coordinates.
        /// </summary>
        private Int32 coordinateCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventQueue" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public EventQueue(IEnumerable<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            this.comparer = new CoordinateComparer();
            this.coordinateCount = 0;
            this.eventHeap = new EventHeap();

            this.AppendCoordinates(source);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventQueue" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public EventQueue(IEnumerable<IEnumerable<Coordinate>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            this.comparer = new CoordinateComparer();
            this.coordinateCount = 0;
            this.eventHeap = new EventHeap();

            foreach (IEnumerable<Coordinate> collection in source)
            {
                this.AppendCoordinates(collection);
            }
        }

        /// <summary>
        /// Retrieves the next event from the queue.
        /// </summary>
        /// <returns>The next event in the queue.</returns>
        public Event Next()
        {
            return (this.eventHeap.Count > 0) ? this.eventHeap.RemovePeek() : null;
        }

        /// <summary>
        /// Adds an intersection event to the queue.
        /// </summary>
        /// <param name="intersectionEvent">The intersection event.</param>
        /// <exception cref="System.ArgumentNullException">The intersection event is null.</exception>
        public void Add(IntersectionEvent intersectionEvent)
        {
            if (intersectionEvent == null)
                throw new ArgumentNullException(nameof(intersectionEvent));

            this.eventHeap.Insert(intersectionEvent);
        }

        /// <summary>
        /// Determines whether the queue contains an event at the given coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the queue contains an event at <paramref name="coordinate" />; otherwise <c>false</c>.</returns>
        public Boolean Contains(Coordinate coordinate)
        {
            return this.eventHeap.Contains(coordinate);
        }

        /// <summary>
        /// Determines whether the queue contains an intersection event.
        /// </summary>
        /// <param name="intersectionEvent">The intersection event.</param>
        /// <returns><c>true</c> if the queue contains the <paramref name="intersectionEvent" />; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The intersection event is null.</exception>
        public Boolean Contains(IntersectionEvent intersectionEvent)
        {
            if (intersectionEvent == null)
                throw new ArgumentNullException(nameof(intersectionEvent));

            return this.eventHeap.Contains(intersectionEvent);
        }

        /// <summary>
        /// Appends the coordinates to the queue.
        /// </summary>
        /// <param name="coordinates">The collection of coordinates.</param>
        private void AppendCoordinates(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null)
                return;

            IEnumerator<Coordinate> enumerator = coordinates.GetEnumerator();
            if (!enumerator.MoveNext())
                return;

            Coordinate previous, current = enumerator.Current;

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == null)
                    continue;

                previous = current;
                current = enumerator.Current;

                EndPointEvent firstEvent = new EndPointEvent { Edge = this.coordinateCount, Vertex = previous };
                EndPointEvent secondEvent = new EndPointEvent { Edge = this.coordinateCount, Vertex = current };

                Int32 compare = this.comparer.Compare(previous, current);
                if (compare == 0)
                    continue;

                if (compare < 0)
                {
                    firstEvent.Type = EventType.Left;
                    secondEvent.Type = EventType.Right;
                }
                else
                {
                    firstEvent.Type = EventType.Right;
                    secondEvent.Type = EventType.Left;
                }

                this.eventHeap.Insert(firstEvent);
                this.eventHeap.Insert(secondEvent);

                this.coordinateCount++;
            }

            this.coordinateCount++;
        }
    }
}
