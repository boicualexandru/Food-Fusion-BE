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
                concurrentEvents.FirstOrDefault().Reservations.Count == 1 &&
                concurrentEvents.FirstOrDefault().Reservations.FirstOrDefault().Id == 20);
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
            var events = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(5)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = DateTime.Now.AddHours(3),
                    EndTime = DateTime.Now.AddHours(8)
                },
                new Reservation
                {
                    Id = 2,
                    StartTime = DateTime.Now.AddHours(6),
                    EndTime = DateTime.Now.AddHours(11)
                }
            };

            // Act
            var concurrentEvents = _concurrentEventsService.GetConcurrentEvents(events);

            // Assert
            Assert.True(concurrentEvents.Count == 5);

            Assert.True(concurrentEvents[0].Reservations.Count == 1);
            Assert.True(concurrentEvents[1].Reservations.Count == 2);
        }
    }
}
