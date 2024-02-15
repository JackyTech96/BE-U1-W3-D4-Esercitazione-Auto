using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace BE_U3_W3_D4_Esercitazione_Auto
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Popola la DropDownList con le auto
                PopolaDropDownListAuto();

                // Popola la CheckBoxList con gli optional
                PopolaCheckBoxListOptional();
            }
        }

        private void PopolaDropDownListAuto()
        {
            // Ottieni la stringa di connessione dal file di configurazione
            string connectionString = ConfigurationManager.ConnectionStrings["ConcessionariaAutoDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                // Crea un comando SQL per selezionare le auto
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT AutoID, NomeAuto, PrezzoBase, Immagine FROM Auto";

                // Esegui la query e leggi i risultati
                SqlDataReader reader = cmd.ExecuteReader();

                // Popola la DropDownList con i dati ottenuti dal database
                while (reader.Read())
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = reader["NomeAuto"].ToString();
                    listItem.Value = reader["AutoID"].ToString();

                    ddlAuto.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                // Gestisci l'errore in modo appropriato, ad esempio, registrandolo o mostrando un messaggio all'utente
                Response.Write($"Errore: {ex.Message}");
            }
            finally
            {
                // Chiudi la connessione nel blocco finally per garantire che venga chiusa anche in caso di eccezione
                conn.Close();
            }
        }

        private void PopolaCheckBoxListOptional()
        {
            // Ottieni la stringa di connessione dal file di configurazione
            string connectionString = ConfigurationManager.ConnectionStrings["ConcessionariaAutoDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                // Crea un comando SQL per selezionare gli optional
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT OptionalID, NomeOptional, Prezzo FROM Optional";

                // Esegui la query e leggi i risultati
                SqlDataReader reader = cmd.ExecuteReader();

                // Popola la CheckBoxList con i dati ottenuti dal database
                while (reader.Read())
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = reader["NomeOptional"].ToString();
                    listItem.Value = reader["OptionalID"].ToString();

                    checkOptional.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                // Gestisci l'errore in modo appropriato, ad esempio, registrandolo o mostrando un messaggio all'utente
                Response.Write($"Errore: {ex.Message}");
            }
            finally
            {
                // Chiudi la connessione nel blocco finally per garantire che venga chiusa anche in caso di eccezione
                conn.Close();
            }
        }

        protected void ddlAuto_Selezionata(object sender, EventArgs e)
        {
            // Ottieni l'ID dell'auto selezionata nella DropDownList
            string autoSelezionataId = ddlAuto.SelectedValue;

            // Ottieni la stringa di connessione dal file di configurazione
            string connectionString = ConfigurationManager.ConnectionStrings["ConcessionariaAutoDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                // Crea un comando SQL per ottenere informazioni sull'auto selezionata
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT Immagine, PrezzoBase FROM Auto WHERE AutoID = @AutoID";
                cmd.Parameters.AddWithValue("@AutoID", autoSelezionataId);

                // Esegui la query e leggi i risultati
                SqlDataReader reader = cmd.ExecuteReader();

                // Aggiorna l'immagine dell'auto e salva il prezzo base nella ViewState per l'utilizzo futuro
                if (reader.Read())
                {
                    imgAuto.ImageUrl = reader["Immagine"].ToString();
                    imgAuto.Visible = true;

                    ViewState["PrezzoBase"] = Convert.ToDouble(reader["PrezzoBase"]);
                }
            }
            catch (Exception ex)
            {
                // Gestisci l'errore in modo appropriato, ad esempio, registrandolo o mostrando un messaggio all'utente
                Response.Write($"Errore: {ex.Message}");
            }
            finally
            {
                // Chiudi la connessione nel blocco finally per garantire che venga chiusa anche in caso di eccezione
                conn.Close();
            }
        }

        protected void btnCalcola_Click(object sender, EventArgs e)
        {
            // Recupera il prezzo di base salvato in precedenza dalla ViewState
            double prezzoBase = ViewState["PrezzoBase"] != null ? (double)ViewState["PrezzoBase"] : 0.00;

            // Calcola il totale degli optional, della garanzia e il totale complessivo
            double totaleOptional = CalcolaTotaleOptional();
            double totaleGaranzia = Convert.ToDouble(ddlGaranzia.SelectedValue) * 120.00;
            double totaleComplessivo = prezzoBase + totaleOptional + totaleGaranzia;

            // Mostra i risultati nella pagina
            lblPrezzoBase.Text = $"Prezzo di base: {prezzoBase.ToString("C")}";
            lblTotaleOptional.Text = $"Totale Optional: {totaleOptional.ToString("C")}";
            lblTotaleGaranzia.Text = $"Totale Garanzia: {totaleGaranzia.ToString("C")}";
            lblTotaleComplessivo.Text = $"Totale Complessivo: {totaleComplessivo.ToString("C")}";
        }

        private double CalcolaTotaleOptional()
        {
            // Inizializza il totale degli optional
            double totaleOptional = 0.00;

            // Ottieni la stringa di connessione dal file di configurazione
            string connectionString = ConfigurationManager.ConnectionStrings["ConcessionariaAutoDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                // Itera attraverso gli elementi selezionati nella CheckBoxList degli optional
                foreach (ListItem item in checkOptional.Items)
                {
                    if (item.Selected)
                    {
                        // Ottieni il prezzo dell'optional dal database e aggiungilo al totale
                        double prezzoOptional = OttieniPrezzoOptional(item.Value);
                        totaleOptional += prezzoOptional;
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestisci l'errore in modo appropriato, ad esempio, registrandolo o mostrando un messaggio all'utente
                Response.Write($"Errore durante il calcolo del totale degli optional: {ex.Message}");
            }
            finally
            {
                // Chiudi la connessione nel blocco finally per garantire che venga chiusa anche in caso di eccezione
                conn.Close();
            }

            // Restituisci il totale degli optional
            return totaleOptional;
        }

        private double OttieniPrezzoOptional(string optionalId)
        {
            // Inizializza il prezzo dell'optional a 0.00
            double prezzoOptional = 0.00;

            // Ottieni la stringa di connessione dal file di configurazione
            string connectionString = ConfigurationManager.ConnectionStrings["ConcessionariaAutoDB"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                // Crea un comando SQL per ottenere il prezzo dell'optional
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT Prezzo FROM Optional WHERE OptionalID = @OptionalID";
                cmd.Parameters.AddWithValue("@OptionalID", optionalId);

                // Esegui la query e leggi i risultati
                SqlDataReader reader = cmd.ExecuteReader();

                // Se esiste un risultato, assegna il prezzo dell'optional
                if (reader.Read())
                {
                    prezzoOptional = Convert.ToDouble(reader["Prezzo"]);
                }
            }
            catch (Exception ex)
            {
                // Gestisci l'errore in modo appropriato, ad esempio, registrandolo o mostrando un messaggio all'utente
                Response.Write($"Errore durante l'ottenimento del prezzo dell'optional: {ex.Message}");
            }
            finally
            {
                // Chiudi la connessione nel blocco finally per garantire che venga chiusa anche in caso di eccezione
                conn.Close();
            }

            // Restituisci il prezzo dell'optional
            return prezzoOptional;
        }
    }
}




