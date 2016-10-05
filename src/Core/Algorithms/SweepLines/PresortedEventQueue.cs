// <copyright file="PresortedEventQueue.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Algorithms.SweepLines
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a presorted event queue used by Sweep lines.
    /// </summary>
    public sealed class PresortedEventQueue
    {
        #region Private fields

        /// <summary>
        /// The coordinate comparer.
        /// </summary>
        private readonly IComparer<Coordinate> comparer;

        /// <summary>
        /// The list of endpoint events.
        /// </summary>
        private readonly List<EndPointEvent> eventList;

        /// <summary>
        /// The number of polygon edges.
        /// </summary>
        private Int32 edgeCount;

        /// <summary>
        /// The current event index.
        /// </summary>
        private Int32 eventIndex;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PresortedEventQueue" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a single line string.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public PresortedEventQueue(IEnumerable<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), Messages.SourceIsNull);

            this.comparer = new CoordinateComparer();
            this.edgeCount = 0;
            this.eventIndex = 0;
            this.eventList = new List<EndPointEvent>();

            this.AppendCoordinates(source);

            this.eventList.Sort(new EventComparer());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresortedEventQueue" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing multiple line strings.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public PresortedEventQueue(IEnumerable<IEnumerable<Coordinate>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), Messages.SourceIsNull);

            this.comparer = new CoordinateComparer();
            this.edgeCount = 0;
            this.eventIndex = 0;
            this.eventList = new List<EndPointEvent>();

            foreach (IEnumerable<Coordinate> collection in source)
            {
                this.AppendCoordinates(collection);
                this.edgeCount++;
            }

            this.eventList.Sort(new EventComparer());
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Retrieves the next event from the queue.
        /// </summary>
        /// <returns>The next event in the queue.</returns>
        public EndPointEvent Next()
        {
            return (this.eventIndex < this.eventList.Count) ? this.eventList[this.eventIndex++] : null;
        }

        #endregion

        #region Private methods

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
                previous = current;
                current = enumerator.Current;

                EndPointEvent firstEvent = new EndPointEvent { Edge = this.edgeCount, Vertex = previous };
                EndPointEvent secondEvent = new EndPointEvent { Edge = this.edgeCount, Vertex = current };

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

                this.eventList.Add(firstEvent);
                this.eventList.Add(secondEvent);

                this.edgeCount++;
            }
        }

        #endregion
    }
}
