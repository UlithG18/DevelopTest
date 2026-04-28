using DevelopTest.Data;
using DevelopTest.Models;
using DevelopTest.Responses;

namespace DevelopTest.Services;

public class SportAreaService
{
    private readonly MySqlDbContext _context;
    
    public SportAreaService(MySqlDbContext context)
    { 
        _context = context;
    }

    public ServiceResponse<IEnumerable<SportArea>> GetAllSportAreas()
    {
        var sportAreas = _context.SportAreas.ToList();
        return new ServiceResponse<IEnumerable<SportArea>>()
        {
            Success = true,
            Data = sportAreas
        };
    }

    public ServiceResponse<SportArea> SaveSportArea(SportArea sportArea)
    {
        try
        {
            if (sportArea == null)
            {
                return new ServiceResponse<SportArea>()
                {
                    Success = false,
                    Message = "Sport Area cannot be null"
                };
            }

            if (string.IsNullOrWhiteSpace(sportArea.Name) ||
                sportArea.Type == null ||
                sportArea.OpeningTime == null || 
                sportArea.ClosingTime == null ||
                sportArea.Capacity == null)
            {
                return new ServiceResponse<SportArea>()
                {
                    Success = false,
                    Data = sportArea,
                    Message = "Name, Type, Capacity and Opening/Closing time are required"
                };
            }
            
            var existsName = _context.SportAreas
                .FirstOrDefault(a => a.Name == sportArea.Name && a.Name != sportArea.Name);

            if (existsName != null)
            {
                return new ServiceResponse<SportArea>()
                {
                    Success = false,
                    Data = sportArea,
                    Message = "Another Sport Area already has this Name"
                };
            }
            
            var sportAreaExists = _context.SportAreas
                .FirstOrDefault(a => a.Id == sportArea.Id);

            if (sportAreaExists != null)
            {
                return new ServiceResponse<SportArea>()
                {
                    Success = false,
                    Data = sportArea,
                    Message = "Sport Area already exists"
                };
            }
        
            _context.SportAreas.Add(sportArea);
            _context.SaveChanges();
            return new ServiceResponse<SportArea>()
            {
                Success = true,
                Data = sportArea,
                Message = "Sport Area created successfully"
            };
        }
        catch (Exception)
        {
            return new ServiceResponse<SportArea>()
            {
                Success = false,
                Message = "Error creating Sport Area"
            };
        }
    }

    public ServiceResponse<SportArea> GetSportAreaById(int id)
    {
        var sportArea = _context.SportAreas.FirstOrDefault(a => a.Id == id);
        if (sportArea != null)
        {
            return new ServiceResponse<SportArea>()
            {
                Success = true,
                Data = sportArea,
                Message = "Sport Area found"
            };
        }

        return new ServiceResponse<SportArea>()
        {
            Success = false,
            Data = sportArea,
            Message = "Sport Area not found"
        };
    }

    public ServiceResponse<SportArea> UpdateSportArea(SportArea sportArea)
    {
        try
        {
            if (sportArea == null)
            {
                return new ServiceResponse<SportArea>()
                {
                    Success = false,
                    Message = "Sport Area cannot be null"
                };
            }
            
            var sportAreaDb = _context.SportAreas.Find(sportArea.Id);
            
            if (sportAreaDb == null)
            {
                return new ServiceResponse<SportArea>()
                {
                    Success = false,
                    Message = "Sport Area not found"
                };
            }
            
            if (string.IsNullOrWhiteSpace(sportArea.Name) ||
                sportArea.Type == null ||
                sportArea.OpeningTime == null || 
                sportArea.ClosingTime == null ||
                sportArea.Capacity == null)
            {
                return new ServiceResponse<SportArea>()
                {
                    Success = false,
                    Data = sportArea,
                    Message = "Name, Type, Capacity and Opening/Closing time are required"
                };
            }
            
            
            var existsName = _context.SportAreas
                .FirstOrDefault(a => a.Name == sportArea.Name && a.Name != sportArea.Name);

            if (existsName != null)
            {
                return new ServiceResponse<SportArea>()
                {
                    Success = false,
                    Data = sportArea,
                    Message = "Another Sport Area already has this Name"
                };
            }

            sportAreaDb.Name = sportArea.Name;
            sportAreaDb.Type = sportArea.Type;
            sportAreaDb.Capacity = sportArea.Capacity;
            sportAreaDb.OpeningTime = sportArea.OpeningTime;
            sportAreaDb.ClosingTime = sportArea.ClosingTime;
            _context.SaveChanges();

            return new ServiceResponse<SportArea>()
            {
                Success = true,
                Data = sportArea,
                Message = "Sport Area updated successfully"
            };
        }
        catch (Exception)
        {
            return new ServiceResponse<SportArea>()
            {
                Success = false,
                Message = "Error updating Sport Area"
            };
        }
    }
    
    public ServiceResponse<SportArea> DeleteSportArea(SportArea sportArea)
    {
        try
        {
            var sportAreaDb = _context.SportAreas.Find(sportArea.Id);

            if (sportAreaDb == null)
            {
                return new ServiceResponse<SportArea>()
                {
                    Success = false,
                    Message = "Sport Area not found"
                };
            }
            
            _context.SportAreas.Remove(sportAreaDb);
            _context.SaveChanges();

            return new ServiceResponse<SportArea>()
            {
                Success = true,
                Data = sportArea,
                Message = "Sport Area removed successfully"
            };
        }
        catch (Exception e)
        {
            return new ServiceResponse<SportArea>()
            {
                Success = false,
                Message = "Error removing user"
            };
        }
    }
    
}