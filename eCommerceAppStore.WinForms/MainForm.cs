using System;
using System.Drawing;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

public partial class MainForm : Form
{
    private readonly Color _bgDark = Color.FromArgb(30, 30, 46);
    private readonly Color _bgSidebar = Color.FromArgb(24, 24, 37);
    private readonly Color _bgCard = Color.FromArgb(40, 40, 60);
    private readonly Color _textLight = Color.FromArgb(230, 230, 240);

    private Panel _pnlMain = null!;
    private Button? _activeNavButton;

    public MainForm()
    {
        InitializeComponentCustom();
    }

    private void InitializeComponentCustom()
    {
        Text = "Store Manager Pro";
        Size = new Size(1150, 680);
        MinimumSize = new Size(1000, 600);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = _bgDark;
        ForeColor = _textLight;
        Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

        var pnlHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 45,
            BackColor = _bgSidebar,
            Padding = new Padding(15, 0, 15, 0)
        };

        var lblAppTitle = new Label
        {
            Text = "≡  Store Manager Pro",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(15, 10)
        };

        var lblStatus = new Label
        {
            Text = "[ Status: Conectat API ]   [ Auth: Admin JWT ]",
            Font = new Font("Segoe UI", 9F, FontStyle.Regular),
            ForeColor = Color.DarkGray,
            AutoSize = true,
            Dock = DockStyle.Right,
            TextAlign = ContentAlignment.MiddleRight
        };

        pnlHeader.Controls.Add(lblAppTitle);
        pnlHeader.Controls.Add(lblStatus);

        var pnlSidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 180,
            BackColor = _bgSidebar,
            Padding = new Padding(10)
        };

        var lblNavHeader = new Label
        {
            Text = "NAVIGARE",
            ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 8F, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 30
        };

        var btnNavProducts = CreateNavButton("📦  Produse");
        var btnNavOrders = CreateNavButton("🛒  Comenzi");
        var btnNavReports = CreateNavButton("📊  Rapoarte");
        var btnNavSettings = CreateNavButton("⚙️  Setări");

        var btnLogout = new Button
        {
            Text = "🚪 Deconectare",
            Dock = DockStyle.Bottom,
            Height = 40,
            FlatStyle = FlatStyle.Flat,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0),
            ForeColor = Color.FromArgb(231, 76, 60),
            BackColor = _bgSidebar,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnLogout.FlatAppearance.BorderSize = 0;
        btnLogout.Click += (s, e) => Close();

        pnlSidebar.Controls.Add(btnLogout);
        pnlSidebar.Controls.Add(btnNavSettings);
        pnlSidebar.Controls.Add(btnNavReports);
        pnlSidebar.Controls.Add(btnNavOrders);
        pnlSidebar.Controls.Add(btnNavProducts);
        pnlSidebar.Controls.Add(lblNavHeader);

        _pnlMain = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = _bgDark
        };

        btnNavProducts.Click += (sender, args) =>
        {
            SetActiveNavButton(btnNavProducts);
            SwitchView(new ProductsControl());
        };

        btnNavOrders.Click += (sender, args) =>
        {
            SetActiveNavButton(btnNavOrders);
            SwitchView(new OrdersControl());
        };

        btnNavReports.Click += (sender, args) =>
        {
            SetActiveNavButton(btnNavReports);
            SwitchView(new ReportsControl());
        };

        btnNavSettings.Click += (sender, args) =>
        {
            SetActiveNavButton(btnNavSettings);
            SwitchView(new SettingsControl());
        };

        Controls.Add(_pnlMain);
        Controls.Add(pnlSidebar);
        Controls.Add(pnlHeader);

        SetActiveNavButton(btnNavProducts);
        SwitchView(new ProductsControl());
    }

    private void SetActiveNavButton(Button btn)
    {
        if (_activeNavButton != null)
        {
            _activeNavButton.BackColor = _bgSidebar;
            _activeNavButton.ForeColor = Color.Gray;
            _activeNavButton.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        }

        _activeNavButton = btn;
        _activeNavButton.BackColor = _bgCard;
        _activeNavButton.ForeColor = Color.White;
        _activeNavButton.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
    }

    private void SwitchView(UserControl newView)
    {
        _pnlMain.Controls.Clear();
        newView.Dock = DockStyle.Fill;
        _pnlMain.Controls.Add(newView);
    }

    private Button CreateNavButton(string text)
    {
        var btn = new Button
        {
            Text = text,
            Dock = DockStyle.Top,
            Height = 40,
            FlatStyle = FlatStyle.Flat,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0),
            ForeColor = Color.Gray,
            BackColor = _bgSidebar,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }
}