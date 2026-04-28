using DevelopTest.Data;
using DevelopTest.Models;
using DevelopTest.Responses;
using Microsoft.EntityFrameworkCore;

namespace DevelopTest.Services;

public class ReservationService
{
    private readonly MySqlDbContext _context;

    public ReservationService(MySqlDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<IEnumerable<Reservation>> GetAllReservation()
    {
        var reservation = _context.Reservations
            .Include(r => r.User)
            .Include(r => r.SportArea)  
            .ToList();

        return new ServiceResponse<IEnumerable<Reservation>>()
        {
            Success = true,
            Data = reservation
        };
    }

    public ServiceResponse<Reservation> GetReservationById(int id)
    {
        var reservation = _context.Reservations
            .Include(l => l.User)
            .Include(l => l.SportArea)
            .FirstOrDefault(l => l.Id == id);

        if (reservation != null)
        {
            return new ServiceResponse<Reservation>()
            {
                Success = true,
                Data = reservation,
                Message = "Reservation found"
            };
        }

        return new ServiceResponse<Reservation>()
        {
            Success = false,
            Data = reservation,
            Message = "Reservation not found"
        };
    }

    public ServiceResponse<Reservation> SaveReservation(Reservation reservation)
    {
        try
        {
            if (reservation == null)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "Reservation cannot be null"
                };
            }

            if (reservation.Date == null || reservation.StartTime == null || reservation.EndTime == null)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "Date, StartTime and EndTime are required"
                };
            }

            if (reservation.StartTime >= reservation.EndTime)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "End time must be greater than start time"
                };
            }

            if (reservation.Date < DateOnly.FromDateTime(DateTime.Now))
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "Cannot create reservations in the past"
                };
            }

            var userExists = _context.Users.Any(u => u.Id == reservation.UserId);
            var sportAreaExists = _context.SportAreas.Any(s => s.Id == reservation.SportAreaId);

            if (!userExists || !sportAreaExists)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "User or Sport Area doesn't exist"
                };
            }
            
            var sportAreaConflict = _context.Reservations.Any(r =>
                r.SportAreaId == reservation.SportAreaId &&
                r.Date == reservation.Date &&
                reservation.StartTime < r.EndTime &&
                reservation.EndTime > r.StartTime
            );

            if (sportAreaConflict)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "Sport area already reserved in this time range"
                };
            }
            
            var userConflict = _context.Reservations.Any(r =>
                r.UserId == reservation.UserId &&
                r.Date == reservation.Date &&
                reservation.StartTime < r.EndTime &&
                reservation.EndTime > r.StartTime
            );

            if (userConflict)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "User already has a reservation in this time range"
                };
            }

            reservation.Status = ReservationStatus.Pending;

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return new ServiceResponse<Reservation>()
            {
                Success = true,
                Data = reservation,
                Message = "Reservation created successfully"
            };
        }
        catch (Exception)
        {
            return new ServiceResponse<Reservation>()
            {
                Success = false,
                Message = "Error creating reservation"
            };
        }
    }

    public ServiceResponse<Reservation> UpdateReservation(Reservation reservation)
    {
        try
        {
            if (reservation == null)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "Reservation cannot be null"
                };
            }

            var reservationDb = _context.Reservations.Find(reservation.Id);

            if (reservationDb == null)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "Reservation not found"
                };
            }

            if (reservation.StartTime >= reservation.EndTime)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "End time must be greater than start time"
                };
            }

            if (reservation.Date < DateOnly.FromDateTime(DateTime.Now))
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "Cannot update reservation to a past date"
                };
            }
            
            var sportAreaConflict = _context.Reservations.Any(r =>
                r.Id != reservation.Id &&
                r.SportAreaId == reservation.SportAreaId &&
                r.Date == reservation.Date &&
                reservation.StartTime < r.EndTime &&
                reservation.EndTime > r.StartTime
            );

            if (sportAreaConflict)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "Sport area already reserved in this time range"
                };
            }
            
            var userConflict = _context.Reservations.Any(r =>
                r.Id != reservation.Id &&
                r.UserId == reservation.UserId &&
                r.Date == reservation.Date &&
                reservation.StartTime < r.EndTime &&
                reservation.EndTime > r.StartTime
            );

            if (userConflict)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Data = reservation,
                    Message = "User already has a reservation in this time range"
                };
            }

            reservationDb.Date = reservation.Date;
            reservationDb.StartTime = reservation.StartTime;
            reservationDb.EndTime = reservation.EndTime;
            reservationDb.Status = reservation.Status;
            reservationDb.UserId = reservation.UserId;
            reservationDb.SportAreaId = reservation.SportAreaId;

            _context.SaveChanges();

            return new ServiceResponse<Reservation>()
            {
                Success = true,
                Data = reservation,
                Message = "Reservation updated successfully"
            };
        }
        catch (Exception)
        {
            return new ServiceResponse<Reservation>()
            {
                Success = false,
                Message = "Error updating reservation"
            };
        }
    }

    public ServiceResponse<Reservation> DeleteReservation(Reservation reservation)
    {
        try
        {
            var reservationDb = _context.Reservations.Find(reservation.Id);

            if (reservationDb == null)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "Reservation not found"
                };
            }

            if (reservationDb.Status == ReservationStatus.Finished)
            {
                return new ServiceResponse<Reservation>()
                {
                    Success = false,
                    Message = "Cannot cancel a finished reservation"
                };
            }

            reservationDb.Status = ReservationStatus.Canceled;
            _context.SaveChanges();

            return new ServiceResponse<Reservation>()
            {
                Success = true,
                Data = reservationDb,
                Message = "Reservation canceled successfully"
            };
        }
        catch (Exception)
        {
            return new ServiceResponse<Reservation>()
            {
                Success = false,
                Message = "Error canceling reservation"
            };
        }
    }
}