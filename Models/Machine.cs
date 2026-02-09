using System;

namespace ProductionSchedulingGantt.Models
{
    /// <summary>
    /// Représente une machine de production
    /// </summary>
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Type})";
        }
    }
}