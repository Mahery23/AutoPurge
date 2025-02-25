using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace AutoPurge
{
    public partial class ConfigurationForm : Form
    {
        // L'objet de configuration chargé depuis le fichier JSON
        private ConfigModel _config;
        // Service qui gère la lecture et l'écriture de la configuration
        private readonly ConfigService _configService;
        // Chemin complet vers le fichier JSON de configuration
        private string _configPath = Path.Combine(Application.StartupPath, "config.json");

        /// <summary>
        /// Constructeur du formulaire. Il initialise les composants et charge la configuration.
        /// </summary>
        public ConfigurationForm()
        {
            InitializeComponent();
            _configService = new ConfigService(_configPath);
            LoadConfig();
        }

        /// <summary>
        /// Charge la configuration depuis le fichier JSON et remplit les champs de l'interface.
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                // Crée une configuration par défaut si nécessaire, puis la charge
                _configService.CreateDefaultConfigIfNotExists();
                _config = _configService.LoadConfig();

                // Remplit les champs du formulaire avec les valeurs de configuration email
                textBoxFrom.Text = _config.Email.From;
                textBoxPassword.Text = _config.Email.Password;
                textBoxTo.Text = _config.Email.To;
                textBoxCc.Text = _config.Email.Cc;
                textBoxBcc.Text = _config.Email.Bcc;

                // Remplit le DataGridView avec la liste des chemins de purge
                dataGridViewFichiers.Rows.Clear();
                foreach (var path in _config.Paths)
                {
                    dataGridViewFichiers.Rows.Add(
                        path.Chemin,
                        path.JoursEnArriere,
                        path.FormatDate,
                        string.Join(", ", path.Extensions),
                        string.Join(", ", path.Nom),
                        string.Join(", ", path.Exceptions)
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement de la configuration : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Permet à l'utilisateur de sélectionner un dossier via une boîte de dialogue.
        /// Le chemin sélectionné est mis à jour dans la ligne sélectionnée du DataGridView.
        /// </summary>
        private void btnParcourir_Click(object sender, EventArgs e)
        {
            // Vérifie qu'une ligne est sélectionnée dans le DataGridView
            if (dataGridViewFichiers.SelectedRows.Count > 0)
            {
                using (var folderBrowser = new FolderBrowserDialog())
                {
                    if (folderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        int rowIndex = dataGridViewFichiers.SelectedRows[0].Index;
                        dataGridViewFichiers.Rows[rowIndex].Cells["Chemin"].Value = folderBrowser.SelectedPath;
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne dans le tableau.", "Aucune ligne sélectionnée", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Lorsque l'utilisateur clique sur "Sauvegarder", met à jour l'objet de configuration et l'enregistre dans le fichier JSON.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateConfig();
                _configService.SaveConfig(_config);
                MessageBox.Show("Configuration sauvegardée avec succès!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Met à jour l'objet de configuration (_config) avec les valeurs entrées dans l'interface utilisateur.
        /// </summary>
        private void UpdateConfig()
        {
            // Met à jour la configuration email
            _config.Email.From = textBoxFrom.Text;
            _config.Email.Password = textBoxPassword.Text;
            _config.Email.To = textBoxTo.Text;
            _config.Email.Cc = textBoxCc.Text;
            _config.Email.Bcc = textBoxBcc.Text;

            // Vide la liste des chemins et la remplit à partir du DataGridView
            _config.Paths.Clear();
            foreach (DataGridViewRow row in dataGridViewFichiers.Rows)
            {
                if (!row.IsNewRow)
                {
                    var pathConfig = new PathConfig
                    {
                        Chemin = row.Cells["Chemin"].Value?.ToString(),
                        JoursEnArriere = Convert.ToInt32(row.Cells["JoursEnArrière"].Value),
                        FormatDate = row.Cells["FormatDate"].Value?.ToString(),
                        Extensions = (row.Cells["Extensions"].Value?.ToString() ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList(),
                        Nom = (row.Cells["Nom"].Value?.ToString() ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList(),
                        Exceptions = (row.Cells["Exceptions"].Value?.ToString() ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()
                    };
                    _config.Paths.Add(pathConfig);
                }
            }
        }
    }
}
