using System.Collections.Generic;
using Common;
using Services.Reservations.Models;

namespace Services.Reservations
{
    public interface IReservationsService
    {
        // TODO: filter by date and by upcoming/old ones
        IList<ReservationModel> GetUserReservations(int userId);
        IList<ReservationModel> GetRestaurantReservations(int restaurantId);
        ReservationDetailedModel GetReservation(int reservationId);
        // TODO: parameter type should contain only: restaurantId, userId, participantsCount, list of tableIds, timeRange
        // but it might be he same as returned type
        ReservationDetailedModel AddReservation(ReservationDetailedModel reservation);
        ReservationDetailedModel UpdateReservation(ReservationDetailedModel reservation);
        void RemoveReservation(int reservationId);
    }
}