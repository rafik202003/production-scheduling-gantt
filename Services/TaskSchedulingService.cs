using System;
using System.Collections.Generic;
using System.Linq;
using ProductionSchedulingGantt.Models;
using ProductionSchedulingGantt.Repositories;

namespace ProductionSchedulingGantt.Services
{
    /// <summary>
    /// Service métier pour la gestion des tâches et planification
    /// </summary>
    public class TaskSchedulingService
    {
        private readonly IDataRepository _repository;

        public TaskSchedulingService(IDataRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Crée une nouvelle tâche
        /// </summary>
        public bool CreateTask(int machineId, string name, DateTime startDate, DateTime endDate, 
            string description = "", int priority = 1)
        {
            // Validation
            if (startDate >= endDate)
                throw new ArgumentException("La date de début doit être avant la date de fin");

            // Vérifier les chevauchements
            var machine = _repository.GetMachineById(machineId);
            if (machine == null)
                throw new ArgumentException("Machine introuvable");

            var existingTasks = _repository.GetTasksByMachineId(machineId);
            if (existingTasks.Any(t => t.OverlapsWith(startDate, endDate)))
                return false; // Chevauchement détecté

            var task = new Task
            {
                MachineId = machineId,
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                Description = description,
                Priority = priority,
                Status = TaskStatus.Pending
            };

            _repository.AddTask(task);
            _repository.Save();
            return true;
        }

        /// <summary>
        /// Récupère toutes les tâches groupées par machine
        /// </summary>
        public List<MachinePlan> GetAllMachinePlans()
        {
            var machines = _repository.GetAllMachines().Where(m => m.IsActive).ToList();
            var plans = new List<MachinePlan>();

            foreach (var machine in machines)
            {
                var plan = new MachinePlan(machine);
                var tasks = _repository.GetTasksByMachineId(machine.Id);
                plan.Tasks = tasks;
                plans.Add(plan);
            }

            return plans;
        }

        /// <summary>
        /// Modifie une tâche existante
        /// </summary>
        public bool UpdateTask(int taskId, string name, DateTime startDate, DateTime endDate, 
            string description = "", int priority = 1)
        {
            var task = _repository.GetTaskById(taskId);
            if (task == null)
                return false;

            if (startDate >= endDate)
                throw new ArgumentException("La date de début doit être avant la date de fin");

            // Vérifier les chevauchements (sauf la tâche elle-même)
            var existingTasks = _repository.GetTasksByMachineId(task.MachineId)
                .Where(t => t.Id != taskId)
                .ToList();

            if (existingTasks.Any(t => t.OverlapsWith(startDate, endDate)))
                return false;

            task.Name = name;
            task.StartDate = startDate;
            task.EndDate = endDate;
            task.Description = description;
            task.Priority = priority;

            _repository.UpdateTask(task);
            _repository.Save();
            return true;
        }

        /// <summary>
        /// Supprime une tâche
        /// </summary>
        public void DeleteTask(int taskId)
        {
            _repository.DeleteTask(taskId);
            _repository.Save();
        }

        /// <summary>
        /// Déplace une tâche vers une autre machine
        /// </summary>
        public bool MoveTask(int taskId, int newMachineId)
        {
            var task = _repository.GetTaskById(taskId);
            if (task == null)
                return false;

            // Vérifier les chevauchements sur la nouvelle machine
            var existingTasks = _repository.GetTasksByMachineId(newMachineId);
            if (existingTasks.Any(t => t.OverlapsWith(task.StartDate, task.EndDate)))
                return false;

            task.MachineId = newMachineId;
            _repository.UpdateTask(task);
            _repository.Save();
            return true;
        }

        /// <summary>
        /// Change le statut d'une tâche
        /// </summary>
        public void UpdateTaskStatus(int taskId, TaskStatus status)
        {
            var task = _repository.GetTaskById(taskId);
            if (task != null)
            {
                task.Status = status;
                _repository.UpdateTask(task);
                _repository.Save();
            }
        }
    }
}