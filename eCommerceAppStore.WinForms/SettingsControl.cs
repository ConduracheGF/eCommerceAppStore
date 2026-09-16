using System;
using System.Drawing;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

public class SettingsControl : UserControl
{
    public TextBox TxtApiUrl { get; private set; } = null!;

    public SettingsControl()
    {
        Dock = DockStyle.Fill;
        BackColor = Color.FromArgb(30, 30, 46);
        Padding = new Padding(20);

        var lblTitle = new Label
        {
            Text = "SETĂRI APLICAȚIE",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 40
        };

        var pnlForm = new Panel
        {
            Dock = DockStyle.Top,
            Height = 200,
            BackColor = Color.FromArgb(40, 40, 60),
            Padding = new Padding(20)
        };

        var lblUrl = new Label { Text = "URL Server API (Backend):", ForeColor = Color.White, Dock = DockStyle.Top, Height = 25 };
        TxtApiUrl = new TextBox { Text = "http://localhost:5113/", BackColor = Color.FromArgb(30, 30, 46), ForeColor = Color.White, Dock = DockStyle.Top, Height = 30, BorderStyle = BorderStyle.FixedSingle };

        var btnSave = new Button
        {
            Text = "Salvează Configurația",
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Dock = DockStyle.Bottom,
            Height = 35,
            Cursor = Cursors.Hand
        };
        btnSave.FlatAppearance.BorderSize = 0;

        pnlForm.Controls.Add(btnSave);
        pnlForm.Controls.Add(TxtApiUrl);
        pnlForm.Controls.Add(lblUrl);

        Controls.Add(pnlForm);
        Controls.Add(lblTitle);
    }
}