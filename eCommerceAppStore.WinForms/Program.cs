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

        while (true)
        {
            using var loginForm = new LoginForm();
            if (loginForm.ShowDialog() != DialogResult.OK)
            {
                break;
            }

            if (loginForm.Mode == AccessMode.Admin)
            {
                Application.Run(new MainForm());
            }
            else
            {
                // Modul Client Autentificat sau Client Fără Cont (Vizitator)
                var clientForm = new Form
                {
                    Text = loginForm.Mode == AccessMode.ClientAuthenticated
                        ? $"Store Client - Autentificat ({loginForm.UserEmail})"
                        : "Store Client - Vizitator fără cont",
                    Size = new Size(950, 650),
                    StartPosition = FormStartPosition.CenterScreen,
                    BackColor = Color.FromArgb(30, 30, 46)
                };

                clientForm.Controls.Add(new ClientStoreControl(loginForm.UserEmail));
                Application.Run(clientForm);
            }
        }
    }
}