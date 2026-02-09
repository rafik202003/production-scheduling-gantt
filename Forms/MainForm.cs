using System;
using System.Windows.Forms;
using ProductionSchedulingGantt.Services;

namespace ProductionSchedulingGantt.Forms
{
    /// <summary>
    /// Formulaire principal de l'application
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly MachineService _machineService;
        private readonly TaskSchedulingService _taskService;

        public MainForm(MachineService machineService, TaskSchedulingService taskService)
        {
            InitializeComponent();
            _machineService = machineService;
            _taskService = taskService;
            
            this.Text = "Production Scheduling - Gantt Chart";
            this.Size = new System.Drawing.Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadUI();
        }

        private void LoadUI()
        {
            // TODO: Charger les données et remplir l'interface
        }
    }
}