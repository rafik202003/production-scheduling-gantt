using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductionSchedulingGantt.Models;

namespace ProductionSchedulingGantt.Repositories
{
    /// <summary>
    /// Implémentation du repository avec persistance JSON
    /// </summary>
    public class JsonDataRepository : IDataRepository
    {
        private readonly string _machinesFilePath = "data/machines.json";
        private readonly string _tasksFilePath = "data/tasks.json";

        private List<Machine> _machines;
        private List<Task> _tasks;
        private int _nextMachineId = 1;
        private int _nextTaskId = 1;

        public JsonDataRepository()
        {
            _machines = new List<Machine>();
            _tasks = new List<Task>();
            EnsureDataDirectory();
            Load();
        }

        private void EnsureDataDirectory()
        {
            var dir = Path.GetDirectoryName(_machinesFilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public List<Machine> GetAllMachines() => _machines;

        public Machine GetMachineById(int id) => _machines.FirstOrDefault(m => m.Id == id);

        public void AddMachine(Machine machine)
        {
            machine.Id = _nextMachineId++;
            _machines.Add(machine);
        }

        public void UpdateMachine(Machine machine)
        {
            var existing = GetMachineById(machine.Id);
            if (existing != null)
            {
                var index = _machines.IndexOf(existing);
                _machines[index] = machine;
            }
        }

        public void DeleteMachine(int id)
        {
            var machine = GetMachineById(id);
            if (machine != null)
                _machines.Remove(machine);
        }

        public List<Task> GetAllTasks() => _tasks;

        public List<Task> GetTasksByMachineId(int machineId) => 
            _tasks.Where(t => t.MachineId == machineId).ToList();

        public Task GetTaskById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

        public void AddTask(Task task)
        {
            task.Id = _nextTaskId++;
            _tasks.Add(task);
        }

        public void UpdateTask(Task task)
        {
            var existing = GetTaskById(task.Id);
            if (existing != null)
            {
                var index = _tasks.IndexOf(existing);
                _tasks[index] = task;
            }
        }

        public void DeleteTask(int id)
        {
            var task = GetTaskById(id);
            if (task != null)
                _tasks.Remove(task);
        }

        public void Save()
        {
            try
            {
                var machinesJson = JsonConvert.SerializeObject(_machines, Formatting.Indented);
                File.WriteAllText(_machinesFilePath, machinesJson);

                var tasksJson = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
                File.WriteAllText(_tasksFilePath, tasksJson);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erreur lors de la sauvegarde des données", ex);
            }
        }

        public void Load()
        {
            try
            {
                if (File.Exists(_machinesFilePath))
                {
                    var json = File.ReadAllText(_machinesFilePath);
                    _machines = JsonConvert.DeserializeObject<List<Machine>>(json) ?? new List<Machine>();
                    if (_machines.Any())
                        _nextMachineId = _machines.Max(m => m.Id) + 1;
                }

                if (File.Exists(_tasksFilePath))
                {
                    var json = File.ReadAllText(_tasksFilePath);
                    _tasks = JsonConvert.DeserializeObject<List<Task>>(json) ?? new List<Task>();
                    if (_tasks.Any())
                        _nextTaskId = _tasks.Max(t => t.Id) + 1;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erreur lors du chargement des données", ex);
            }
        }
    }
}