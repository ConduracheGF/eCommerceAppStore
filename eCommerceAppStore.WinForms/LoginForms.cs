using System;
using System.Drawing;
using System.Windows.Forms;
using eCommerceAppStore.Api.DataTransferObject;

namespace eCommerceAppStore.WinForms;

public enum AccessMode { Admin, ClientAuthenticated, Guest }

public class LoginForm : Form
{
    private readonly ApiService _apiService = new();

    public AccessMode Mode { get; private set; } = AccessMode.Guest;
    public string UserEmail { get; private set; } = string.Empty;

    private TabControl _tabControl = null!;

    // Controale Login
    private TextBox _txtLoginEmail = null!;
    private TextBox _txtLoginPassword = null!;

    // Controale Register
    private TextBox _txtRegName = null!;
    private TextBox _txtRegEmail = null!;
    private TextBox _txtRegPassword = null!;

    public LoginForm()
    {
        Text = "Autentificare / Înregistrare Magazin";
        Size = new Size(420, 450);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = Color.FromArgb(30, 30, 46);
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 9.5F);

        BuildUi();
    }

    private void BuildUi()
    {
        _tabControl = new TabControl { Dock = DockStyle.Top, Height = 320 };

        // --- TAB 1: LOGIN ---
        var tabLogin = new TabPage("Autentificare");
        tabLogin.BackColor = Color.FromArgb(35, 35, 52);

        var lblLName = new Label { Text = "Email:", Location = new Point(20, 20), AutoSize = true, ForeColor = Color.LightGray };
        _txtLoginEmail = new TextBox { Location = new Point(20, 45), Width = 340, BackColor = Color.FromArgb(50, 50, 70), ForeColor = Color.White };

        var lblLPass = new Label { Text = "Parolă:", Location = new Point(20, 85), AutoSize = true, ForeColor = Color.LightGray };
        _txtLoginPassword = new TextBox { Location = new Point(20, 110), Width = 340, PasswordChar = '•', BackColor = Color.FromArgb(50, 50, 70), ForeColor = Color.White };

        var btnLogin = new Button
        {
            Text = "Intră în cont",
            Location = new Point(20, 160),
            Width = 340,
            Height = 40,
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold)
        };
        btnLogin.Click += async (s, e) => await ExecuteLoginAsync();

        tabLogin.Controls.Add(lblLName);
        tabLogin.Controls.Add(_txtLoginEmail);
        tabLogin.Controls.Add(lblLPass);
        tabLogin.Controls.Add(_txtLoginPassword);
        tabLogin.Controls.Add(btnLogin);

        // --- TAB 2: REGISTER ---
        var tabRegister = new TabPage("Creare Cont");
        tabRegister.BackColor = Color.FromArgb(35, 35, 52);

        var lblRName = new Label { Text = "Nume Complet:", Location = new Point(20, 15), AutoSize = true, ForeColor = Color.LightGray };
        _txtRegName = new TextBox { Location = new Point(20, 35), Width = 340, BackColor = Color.FromArgb(50, 50, 70), ForeColor = Color.White };

        var lblREmail = new Label { Text = "Email:", Location = new Point(20, 70), AutoSize = true, ForeColor = Color.LightGray };
        _txtRegEmail = new TextBox { Location = new Point(20, 90), Width = 340, BackColor = Color.FromArgb(50, 50, 70), ForeColor = Color.White };

        var lblRPass = new Label { Text = "Parolă:", Location = new Point(20, 125), AutoSize = true, ForeColor = Color.LightGray };
        _txtRegPassword = new TextBox { Location = new Point(20, 145), Width = 340, PasswordChar = '•', BackColor = Color.FromArgb(50, 50, 70), ForeColor = Color.White };

        var btnRegister = new Button
        {
            Text = "Înregistrează-te",
            Location = new Point(20, 190),
            Width = 340,
            Height = 40,
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold)
        };
        btnRegister.Click += async (s, e) => await ExecuteRegisterAsync();

        tabRegister.Controls.Add(lblRName);
        tabRegister.Controls.Add(_txtRegName);
        tabRegister.Controls.Add(lblREmail);
        tabRegister.Controls.Add(_txtRegEmail);
        tabRegister.Controls.Add(lblRPass);
        tabRegister.Controls.Add(_txtRegPassword);
        tabRegister.Controls.Add(btnRegister);

        _tabControl.TabPages.Add(tabLogin);
        _tabControl.TabPages.Add(tabRegister);

        // --- BUTON CONTINUĂ FĂRĂ CONT ---
        var btnGuest = new Button
        {
            Text = "🛒 Continuă fără cont (Vizitator)",
            Dock = DockStyle.Bottom,
            Height = 45,
            BackColor = Color.FromArgb(230, 126, 34),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnGuest.Click += (s, e) =>
        {
            Mode = AccessMode.Guest;
            UserEmail = "anonim@client.ro";
            DialogResult = DialogResult.OK;
        };

        Controls.Add(_tabControl);
        Controls.Add(btnGuest);
    }

    private async Task ExecuteLoginAsync()
    {
        var res = await _apiService.LoginAsync(new LoginDto { Email = _txtLoginEmail.Text, Password = _txtLoginPassword.Text });
        if (res.Success)
        {
            ApiService.SetJwtToken(res.Token);
            UserEmail = res.Email;
            Mode = res.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? AccessMode.Admin : AccessMode.ClientAuthenticated;
            DialogResult = DialogResult.OK;
        }
        else
        {
            MessageBox.Show(res.ErrorMessage, "Eroare Autentificare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task ExecuteRegisterAsync()
    {
        var res = await _apiService.RegisterAsync(new RegisterDto
        {
            FullName = _txtRegName.Text,
            Email = _txtRegEmail.Text,
            Password = _txtRegPassword.Text
        });

        if (res.Success)
        {
            ApiService.SetJwtToken(res.Token);
            UserEmail = res.Email;
            Mode = AccessMode.ClientAuthenticated;
            MessageBox.Show("Contul a fost creat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        else
        {
            MessageBox.Show(res.ErrorMessage, "Eroare Înregistrare", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}