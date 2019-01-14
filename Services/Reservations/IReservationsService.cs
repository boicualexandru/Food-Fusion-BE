using System.Collections.Generic;
using Services.Reservations.Models;

namespace Services.Reservations
{
    public interface IReservationsService
    {
        // TODO: filter by date and by upcoming/old ones
        IList<ReservationDetailedModel> GetUserReservations(int userId);
        IList<ReservationDetailedModel> GetRestaurantReservations(int restaurantId);
        ReservationDetailedModel GetReservation(int reservationId);
        // TODO: parameter type should contain only: restaurantId, userId, participantsCount, list of tableIds, timeRange
        // but it might be he same as returned type
        ReservationDetailedModel AddReservation(ReservationRequestModel reservation);
        ReservationDetailedModel UpdateReservation(ReservationRequestModel reservation);
        void RemoveReservation(int reservationId);
    }
}