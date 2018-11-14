using System.Collections.Generic;

namespace Services.Reservations
{
    public interface IReservationsService
    {
        // TODO: filter by date and by upcoming/old ones
        IList<ReservationModel> GetUserReservations(int userId);
        IList<ReservationModel> GetRestaurantReservations(int restaurantId);
        ReservationDetailedModel GetReservation(int reservationId);
        IList<TimeRange> GetAvailability(int restaurantId, int participantsCount, TimeRange timeRange);
        ReservationDetailedModel AddReservation(ReservationDetailedModel reservation);
        ReservationDetailedModel UpdateReservation(ReservationDetailedModel reservation);
        void RemoveReservation(int reservationId);
    }

    public class TimeRange
    {
    }

    public class ReservationDetailedModel
    {
    }

    public class ReservationModel
    {
    }
}