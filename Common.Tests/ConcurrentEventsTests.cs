using Common.ConcurrentEvents;
using System.Collections.Generic;
using Xunit;
using System;
using System.Linq;

namespace Common.Tests
{
    public class ConcurrentEventsTests
    {
        private readonly IConcurrentEventsService<TestEvent> _concurrentEventsService;

        public ConcurrentEventsTests()
        {
            _concurrentEventsService = new ConcurrentEventsService<TestEvent>();
        }

        private class TestEvent : IEvent
        {
            public int Id;
            public TimeRange Range { get; set; }
        }

        [Fact]
        public void Should_ReturnEmpty_When_Null()
        {
            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(null);

            // Assert
            Assert.Empty(concurrentEvents);
        }

        [Fact]
        public void Should_ReturnEmpty_When_Empty()
        {
            // Arange
            var events = new List<TestEvent>();

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.Empty(concurrentEvents);
        }

        [Fact]
        public void Should_ReturnOne_When_One()
        {
            // Arange
            var events = new List<TestEvent>
            {
                new TestEvent
                {
                    Id = 20,
                    Range = new TimeRange
                    {
                        Start = DateTime.Now,
                        End = DateTime.Now.AddMinutes(40)
                    }
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 1 &&
                concurrentEvents.FirstOrDefault().Events.Count == 1 &&
                ((TestEvent)concurrentEvents.FirstOrDefault().Events.FirstOrDefault()).Id == 20);
        }

        /// <summary>
        ///  -----
        ///     -----
        ///        -----
        /// </summary>
        [Fact]
        public void Should_Have5Intervals()
        {
            // Arange
            var Start = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<TestEvent>
            {
                new TestEvent
                {
                    Id = 1,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(5)
                    }
                },
                new TestEvent
                {
                    Id = 2,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(3),
                        End = Start.AddHours(8)
                    }
                },
                new TestEvent
                {
                    Id = 3,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(6),
                        End = Start.AddHours(11)
                    }
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 5);

            Assert.True(concurrentEvents[0].Events.Count == 1);
            Assert.True(concurrentEvents[1].Events.Count == 2);
            Assert.True(concurrentEvents[2].Events.Count == 1);
            Assert.True(concurrentEvents[3].Events.Count == 2);
            Assert.True(concurrentEvents[4].Events.Count == 1);
        }

        /// <summary>
        ///  --
        ///     --
        ///     ---
        /// </summary>
        [Fact]
        public void Should_Have4Intervals_When_Pause()
        {
            // Arange
            var Start = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<TestEvent>
            {
                new TestEvent
                {
                    Id = 1,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(2)
                    }
                },
                new TestEvent
                {
                    Id = 2,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(3),
                        End = Start.AddHours(5)
                    }
                },
                new TestEvent
                {
                    Id = 3,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(3),
                        End = Start.AddHours(6)
                    }
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 4);

            Assert.True(concurrentEvents[0].Events.Count == 1);
            Assert.True(concurrentEvents[1].Events.Count == 0);
            Assert.True(concurrentEvents[2].Events.Count == 2);
            Assert.True(concurrentEvents[3].Events.Count == 1);
        }

        /// <summary>
        ///  ---
        ///   ---
        ///   -
        /// </summary>
        [Fact]
        public void Should_Have4Intervals()
        {
            // Arange
            var Start = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<TestEvent>
            {
                new TestEvent
                {
                    Id = 1,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(3)
                    }
                },
                new TestEvent
                {
                    Id = 2,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(4)
                    }
                },
                new TestEvent
                {
                    Id = 3,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(2)
                    }
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 4);

            Assert.True(concurrentEvents[0].Events.Count == 1);
            Assert.True(concurrentEvents[1].Events.Count == 3);
            Assert.True(concurrentEvents[2].Events.Count == 2);
            Assert.True(concurrentEvents[3].Events.Count == 1);
        }

        /// <summary>
        ///  ----
        ///   -
        ///   -
        ///   --
        ///   --
        /// </summary>
        [Fact]
        public void Should_Have4Intervals_When_5Events()
        {
            // Arange
            var Start = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<TestEvent>
            {
                new TestEvent
                {
                    Id = 1,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(4)
                    }
                },
                new TestEvent
                {
                    Id = 2,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(2)
                    }
                },
                new TestEvent
                {
                    Id = 3,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(2)
                    }
                },
                new TestEvent
                {
                    Id = 4,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(3)
                    }
                },
                new TestEvent
                {
                    Id = 5,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(3)
                    }
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 4);

            Assert.True(concurrentEvents[0].Events.Count == 1);
            Assert.True(concurrentEvents[1].Events.Count == 5);
            Assert.True(concurrentEvents[2].Events.Count == 3);
            Assert.True(concurrentEvents[3].Events.Count == 1);
        }

        /// <summary>
        ///  -----
        ///    -----
        ///      -----
        /// </summary>
        [Fact]
        public void Should_Have5Intervals_When_Overlaps()
        {
            // Arange
            var Start = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<TestEvent>
            {
                new TestEvent
                {
                    Id = 1,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(3)
                    }
                },
                new TestEvent
                {
                    Id = 2,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                    End = Start.AddHours(4)
                    }
                },
                new TestEvent
                {
                    Id = 3,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(2),
                        End = Start.AddHours(5)
                    }
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 5);

            Assert.True(concurrentEvents[0].Events.Count == 1);
            Assert.True(concurrentEvents[1].Events.Count == 2);
            Assert.True(concurrentEvents[2].Events.Count == 3);
            Assert.True(concurrentEvents[3].Events.Count == 2);
            Assert.True(concurrentEvents[4].Events.Count == 1);
        }

        /// <summary>
        ///  --
        ///   --
        ///    --
        /// </summary>
        [Fact]
        public void Should_Have4Intervals_When_Consecutive()
        {
            // Arange
            var Start = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<TestEvent>
            {
                new TestEvent
                {
                    Id = 1,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(2)
                    }
                },
                new TestEvent
                {
                    Id = 2,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(3)
                    }
                },
                new TestEvent
                {
                    Id = 3,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(2),
                        End = Start.AddHours(4)
                    }
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 4);

            Assert.True(concurrentEvents[0].Events.Count == 1);
            Assert.True(concurrentEvents[1].Events.Count == 2);
            Assert.True(concurrentEvents[2].Events.Count == 2);
            Assert.True(concurrentEvents[3].Events.Count == 1);
        }

        /// <summary>
        ///  --
        ///  -
        ///  ---
        /// </summary>
        [Fact]
        public void Should_Have3Intervals_When_SameStart()
        {
            // Arange
            var Start = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<TestEvent>
            {
                new TestEvent
                {
                    Id = 1,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(2)
                    }
                },
                new TestEvent
                {
                    Id = 2,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(1)
                    }
                },
                new TestEvent
                {
                    Id = 3,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(3)
                    }
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 3);

            Assert.True(concurrentEvents[0].Events.Count == 3);
            Assert.True(concurrentEvents[1].Events.Count == 2);
            Assert.True(concurrentEvents[2].Events.Count == 1);
        }

        /// <summary>
        ///  --
        ///  ---
        ///   ---
        ///   -----
        ///   -----
        ///       ---
        ///         ---
        ///          -
        ///          ---
        ///              -----
        ///               ---
        ///                -
        ///                ----
        /// </summary>
        [Fact]
        public void Should_Have18Intervals_When_13Events()
        {
            // Arange
            var Start = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<TestEvent>
            {
                new TestEvent
                {
                    Id = 1,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(2)
                    }
                },
                new TestEvent
                {
                    Id = 2,
                    Range = new TimeRange
                    {
                        Start = Start,
                        End = Start.AddHours(3)
                    }
                },
                new TestEvent
                {
                    Id = 3,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(4)
                    }
                },
                new TestEvent
                {
                    Id = 4,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(6)
                    }
                },
                new TestEvent
                {
                    Id = 5,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(1),
                        End = Start.AddHours(6)
                    }
                },
                new TestEvent
                {
                    Id = 6,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(5),
                        End = Start.AddHours(8)
                    }
                },
                new TestEvent
                {
                    Id = 7,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(7),
                        End = Start.AddHours(10)
                    }
                },
                new TestEvent
                {
                    Id = 8,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(8),
                        End = Start.AddHours(9)
                    }
                },
                new TestEvent
                {
                    Id = 9,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(8),
                        End = Start.AddHours(11)
                    }
                },
                new TestEvent
                {
                    Id = 10,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(12),
                        End = Start.AddHours(17)
                    }
                },
                new TestEvent
                {
                    Id = 11,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(13),
                        End = Start.AddHours(16)
                    }
                },
                new TestEvent
                {
                    Id = 12,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(14),
                        End = Start.AddHours(15)
                    }
                },
                new TestEvent
                {
                    Id = 13,
                    Range = new TimeRange
                    {
                        Start = Start.AddHours(14),
                        End = Start.AddHours(18)
                    }
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 18);

            Assert.True(concurrentEvents[0].Events.Count == 2);
            Assert.True(concurrentEvents[1].Events.Count == 5);
            Assert.True(concurrentEvents[2].Events.Count == 4);
            Assert.True(concurrentEvents[3].Events.Count == 3);
            Assert.True(concurrentEvents[4].Events.Count == 2);
            Assert.True(concurrentEvents[5].Events.Count == 3);
            Assert.True(concurrentEvents[6].Events.Count == 1);
            Assert.True(concurrentEvents[7].Events.Count == 2);
            Assert.True(concurrentEvents[8].Events.Count == 3);
            Assert.True(concurrentEvents[9].Events.Count == 2);
            Assert.True(concurrentEvents[10].Events.Count == 1);
            Assert.True(concurrentEvents[11].Events.Count == 0);
            Assert.True(concurrentEvents[12].Events.Count == 1);
            Assert.True(concurrentEvents[13].Events.Count == 2);
            Assert.True(concurrentEvents[14].Events.Count == 4);
            Assert.True(concurrentEvents[15].Events.Count == 3);
            Assert.True(concurrentEvents[16].Events.Count == 2);
            Assert.True(concurrentEvents[17].Events.Count == 1);
        }
    }
}
