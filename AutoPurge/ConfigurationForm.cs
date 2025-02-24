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
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.IO;

namespace AutoPurge
{
    public partial class ConfigurationForm : Form
    {
        private ConfigModel _config;
        private readonly ConfigService _configService;
        private string _configPath = Path.Combine(Application.StartupPath, "config.json");

        /// <summary>
        /// Initialise une nouvelle instance de la classe ConfigurationForm.
        /// </summary>
        public ConfigurationForm()
        {
            InitializeComponent();
            _configService = new ConfigService(_configPath);
            LoadConfig();
        }

        /// <summary>
        /// Charge la configuration depuis le fichier JSON et remplit les champs de l'interface utilisateur.
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                _configService.CreateDefaultConfigIfNotExists();
                _config = _configService.LoadConfig();

                // Populate email fields
                textBoxFrom.Text = _config.Email.From;
                textBoxPassword.Text = _config.Email.Password;
                textBoxTo.Text = _config.Email.To;
                textBoxCc.Text = _config.Email.Cc;
                textBoxBcc.Text = _config.Email.Bcc;

                // Populate DataGridView
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
        /// Ouvre une boîte de dialogue pour sélectionner un dossier et met à jour le chemin dans la ligne sélectionnée du DataGridView.
        /// </summary>
        private void btnParcourir_Click(object sender, EventArgs e)
        {
            // Vérifie si une ligne est sélectionnée dans le DataGridView
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
                MessageBox.Show(
                    "Veuillez sélectionner une ligne dans le tableau.",
                    "Aucune ligne sélectionnée",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        /// <summary>
        /// Sauvegarde la configuration mise à jour dans le fichier JSON.
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
        /// Met à jour l'objet de configuration en fonction des valeurs entrées dans l'interface utilisateur.
        /// </summary>
        private void UpdateConfig()
        {
            // Mise à jour de la configuration email
            _config.Email.From = textBoxFrom.Text;
            _config.Email.Password = textBoxPassword.Text;
            _config.Email.To = textBoxTo.Text;
            _config.Email.Cc = textBoxCc.Text;
            _config.Email.Bcc = textBoxBcc.Text;

            // Mise à jour de la configuration des fichiers à supprimer
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
