using System;

namespace ProductionSchedulingGantt.Models
{
    /// <summary>
    /// Représente une tâche/ordre de fabrication affecté à une machine
    /// </summary>
    public class Task
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public string Description { get; set; }
        public int Priority { get; set; } = 1; // 1 = Low, 2 = Medium, 3 = High
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Durée de la tâche en heures
        /// </summary>
        public double DurationHours
        {
            get { return (EndDate - StartDate).TotalHours; }
        }

        /// <summary>
        /// Vérifie si la tâche chevauche une autre période
        /// </summary>
        public bool OverlapsWith(DateTime start, DateTime end)
        {
            return StartDate < end && EndDate > start;
        }

        public override string ToString()
        {
            return $"{Name} ({StartDate:dd/MM/yyyy HH:mm} - {EndDate:dd/MM/yyyy HH:mm})";
        }
    }

    /// <summary>
    /// Énumération des statuts possibles d'une tâche
    /// </summary>
    public enum TaskStatus
    {
        Pending,    // En attente
        Running,    // En cours
        Completed,  // Terminée
        Cancelled   // Annulée
    }
}