using DataAccess.Models;
using Services.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Services.Tests
{
    public class ConcurrentEventsTests
    {
        private readonly IConcurrentEventsService _concurrentEventsService;

        public ConcurrentEventsTests()
        {
            _concurrentEventsService = new ConcurrentEventsService();
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
            var events = new List<Reservation>();

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.Empty(concurrentEvents);
        }

        [Fact]
        public void Should_ReturnOne_When_One()
        {
            // Arange
            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 20,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddMinutes(40)
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 1 &&
                concurrentEvents.FirstOrDefault().Events.Count == 1 &&
                concurrentEvents.FirstOrDefault().Events.FirstOrDefault().Id == 20);
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
            var startTime = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(5)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = startTime.AddHours(3),
                    EndTime = startTime.AddHours(8)
                },
                new Reservation
                {
                    Id = 3,
                    StartTime = startTime.AddHours(6),
                    EndTime = startTime.AddHours(11)
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
            var startTime = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(2)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = startTime.AddHours(3),
                    EndTime = startTime.AddHours(5)
                },
                new Reservation
                {
                    Id = 3,
                    StartTime = startTime.AddHours(3),
                    EndTime = startTime.AddHours(6)
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
            var startTime = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(3)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(4)
                },
                new Reservation
                {
                    Id = 3,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(2)
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
            var startTime = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(4)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(2)
                },
                new Reservation
                {
                    Id = 3,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(2)
                },
                new Reservation
                {
                    Id = 4,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(3)
                },
                new Reservation
                {
                    Id = 5,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(3)
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
            var startTime = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(3)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(4)
                },
                new Reservation
                {
                    Id = 3,
                    StartTime = startTime.AddHours(2),
                    EndTime = startTime.AddHours(5)
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
            var startTime = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(2)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(3)
                },
                new Reservation
                {
                    Id = 3,
                    StartTime = startTime.AddHours(2),
                    EndTime = startTime.AddHours(4)
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
            var startTime = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(2)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(1)
                },
                new Reservation
                {
                    Id = 3,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(3)
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
            var startTime = new DateTime(2018, 11, 20, 0, 0, 0);

            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(2)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(3)
                },
                new Reservation
                {
                    Id = 3,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(4)
                },
                new Reservation
                {
                    Id = 4,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(6)
                },
                new Reservation
                {
                    Id = 5,
                    StartTime = startTime.AddHours(1),
                    EndTime = startTime.AddHours(6)
                },
                new Reservation
                {
                    Id = 6,
                    StartTime = startTime.AddHours(5),
                    EndTime = startTime.AddHours(8)
                },
                new Reservation
                {
                    Id = 7,
                    StartTime = startTime.AddHours(7),
                    EndTime = startTime.AddHours(10)
                },
                new Reservation
                {
                    Id = 8,
                    StartTime = startTime.AddHours(8),
                    EndTime = startTime.AddHours(9)
                },
                new Reservation
                {
                    Id = 9,
                    StartTime = startTime.AddHours(8),
                    EndTime = startTime.AddHours(11)
                },
                new Reservation
                {
                    Id = 10,
                    StartTime = startTime.AddHours(12),
                    EndTime = startTime.AddHours(17)
                },
                new Reservation
                {
                    Id = 11,
                    StartTime = startTime.AddHours(13),
                    EndTime = startTime.AddHours(16)
                },
                new Reservation
                {
                    Id = 12,
                    StartTime = startTime.AddHours(14),
                    EndTime = startTime.AddHours(15)
                },
                new Reservation
                {
                    Id = 13,
                    StartTime = startTime.AddHours(14),
                    EndTime = startTime.AddHours(18)
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
