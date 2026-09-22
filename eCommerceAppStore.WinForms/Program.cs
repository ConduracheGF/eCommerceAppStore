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

            if (loginForm.SelectedRole == UserRole.Admin)
            {
                Application.Run(new MainForm());
            }
            else
            {
                var clientForm = new Form
                {
                    Text = "Store Client - Cumpărături Online",
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