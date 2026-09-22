using System;
using System.Drawing;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var loginForm = new LoginForm();
        if (loginForm.ShowDialog() == DialogResult.OK)
        {
            if (loginForm.SelectedRole == UserRole.Admin)
            {
                // Deschide panoul de administrare
                Application.Run(new MainForm());
            }
            else
            {
                // Declarare și inițializare corectă a variabilei clientForm
                var clientForm = new Form
                {
                    Text = "Store Client - Cumpărături Online",
                    Size = new Size(900, 600),
                    StartPosition = FormStartPosition.CenterScreen,
                    BackColor = Color.FromArgb(30, 30, 46)
                };

                clientForm.Controls.Add(new ClientStoreControl(loginForm.UserEmail));

                Application.Run(clientForm);
            }
        }
    }
}