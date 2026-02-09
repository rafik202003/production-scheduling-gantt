using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionSchedulingGantt.Models
{
    /// <summary>
    /// Représente le plan de production d'une machine avec ses tâches
    /// </summary>
    public class MachinePlan
    {
        public Machine Machine { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();

        public MachinePlan() { }

        public MachinePlan(Machine machine)
        {
            Machine = machine;
        }

        /// <summary>
        /// Ajoute une tâche si elle ne chevauche pas les autres
        /// </summary>
        public bool TryAddTask(Task task)
        {
            if (Tasks.Any(t => t.OverlapsWith(task.StartDate, task.EndDate)))
            {
                return false;
            }
            Tasks.Add(task);
            return true;
        }

        /// <summary>
        /// Récupère les tâches ordonnées par date de début
        /// </summary>
        public IEnumerable<Task> GetSortedTasks()
        {
            return Tasks.OrderBy(t => t.StartDate);
        }
    }
}