using System;
using System.Drawing;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

public enum UserRole { Admin, Client }

public class LoginForm : Form
{
    public UserRole SelectedRole { get; private set; } = UserRole.Client;
    public string UserEmail { get; private set; } = string.Empty;

    private ComboBox _cboRole = null!;
    private TextBox _txtEmail = null!;
    private TextBox _txtPassword = null!;

    public LoginForm()
    {
        Text = "Autentificare Store Manager";
        Size = new Size(380, 360);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.FromArgb(30, 30, 46);
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 9.5F);

        BuildUi();
    }

    private void BuildUi()
    {
        var lblTitle = new Label
        {
            Text = "AUTENTIFICARE",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.White,
            Location = new Point(20, 20),
            AutoSize = true
        };

        var lblRole = new Label { Text = "Tip Cont / Rol:", Location = new Point(20, 65), AutoSize = true, ForeColor = Color.LightGray };
        _cboRole = new ComboBox
        {
            Location = new Point(20, 90),
            Width = 320,
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = Color.FromArgb(40, 40, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        _cboRole.Items.Add("Client (Cumpărături)");
        _cboRole.Items.Add("Administrator (Gestiune)");
        _cboRole.SelectedIndex = 0;
        _cboRole.SelectedIndexChanged += (s, e) =>
        {
            bool isAdmin = _cboRole.SelectedIndex == 1;
            _txtPassword.Enabled = isAdmin;
            _txtPassword.BackColor = isAdmin ? Color.FromArgb(40, 40, 60) : Color.FromArgb(25, 25, 35);
        };

        var lblEmail = new Label { Text = "Email Utilizator:", Location = new Point(20, 130), AutoSize = true, ForeColor = Color.LightGray };
        _txtEmail = new TextBox
        {
            Location = new Point(20, 155),
            Width = 320,
            BackColor = Color.FromArgb(40, 40, 60),
            ForeColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Text = "client@magazin.ro"
        };

        var lblPassword = new Label { Text = "Parolă Admin (Implicit: admin):", Location = new Point(20, 195), AutoSize = true, ForeColor = Color.LightGray };
        _txtPassword = new TextBox
        {
            Location = new Point(20, 220),
            Width = 320,
            PasswordChar = '•',
            Enabled = false,
            BackColor = Color.FromArgb(25, 25, 35),
            ForeColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var btnLogin = new Button
        {
            Text = "Intră în Aplicație",
            Location = new Point(20, 265),
            Width = 320,
            Height = 38,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.Click += (s, e) => AttemptLogin();

        Controls.Add(lblTitle);
        Controls.Add(lblRole);
        Controls.Add(_cboRole);
        Controls.Add(lblEmail);
        Controls.Add(_txtEmail);
        Controls.Add(lblPassword);
        Controls.Add(_txtPassword);
        Controls.Add(btnLogin);
    }

    private void AttemptLogin()
    {
        if (string.IsNullOrWhiteSpace(_txtEmail.Text))
        {
            MessageBox.Show("Introdu adresa de email!", "Validare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cboRole.SelectedIndex == 1)
        {
            if (_txtPassword.Text != "admin")
            {
                MessageBox.Show("Parolă incorectă pentru Administrator!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SelectedRole = UserRole.Admin;
            ApiService.SetJwtToken("ADMIN_MOCK_TOKEN");
        }
        else
        {
            SelectedRole = UserRole.Client;
        }

        UserEmail = _txtEmail.Text.Trim();
        DialogResult = DialogResult.OK;
    }
}