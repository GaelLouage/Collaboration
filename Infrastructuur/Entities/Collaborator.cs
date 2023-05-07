using System;

namespace Infrastructuur.Entities
{
    public class Collaborator
    {
        public int Id { get; set; }
        public int ItineraryId { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
