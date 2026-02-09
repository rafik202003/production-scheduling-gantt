using System;
using System.Collections.Generic;
using ProductionSchedulingGantt.Models;

namespace ProductionSchedulingGantt.Repositories
{
    /// <summary>
    /// Interface pour la gestion des données (persistance)
    /// </summary>
    public interface IDataRepository
    {
        // ===== MACHINES =====
        List<Machine> GetAllMachines();
        Machine GetMachineById(int id);
        void AddMachine(Machine machine);
        void UpdateMachine(Machine machine);
        void DeleteMachine(int id);

        // ===== TASKS =====
        List<Task> GetAllTasks();
        List<Task> GetTasksByMachineId(int machineId);
        Task GetTaskById(int id);
        void AddTask(Task task);
        void UpdateTask(Task task);
        void DeleteTask(int id);

        // ===== GÉNÉRAL =====
        void Save();
        void Load();
    }
}