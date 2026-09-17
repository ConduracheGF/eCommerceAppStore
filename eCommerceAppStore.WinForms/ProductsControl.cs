using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

public class ProductsControl : UserControl
{
    private readonly Color _bgDark = Color.FromArgb(30, 30, 46);
    private readonly Color _bgHeader = Color.FromArgb(20, 20, 32);
    private readonly Color _bgRowAlt = Color.FromArgb(36, 36, 54);
    private readonly Color _bgCard = Color.FromArgb(40, 40, 60);
    private readonly Color _gridLine = Color.FromArgb(50, 50, 75);
    private readonly Color _accentBlue = Color.FromArgb(52, 152, 219);
    private readonly Color _accentGreen = Color.FromArgb(46, 204, 113);
    private readonly Color _accentRed = Color.FromArgb(231, 76, 60);

    private readonly ApiService _apiService = new();

    public TextBox TxtSearch { get; private set; } = null!;
    public Button BtnNewProduct { get; private set; } = null!;
    public DataGridView DgvProducts { get; private set; } = null!;

    public ProductsControl()
    {
        Dock = DockStyle.Fill;
        BackColor = _bgDark;
        Padding = new Padding(20);

        var lblTitle = new Label
        {
            Text = "GESTIUNE PRODUSE",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 40
        };

        var pnlFilters = new Panel
        {
            Dock = DockStyle.Top,
            Height = 45
        };

        TxtSearch = new TextBox
        {
            Width = 220,
            Location = new Point(0, 5),
            BackColor = _bgCard,
            ForeColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            PlaceholderText = "🔍 Caută produs..."
        };
        TxtSearch.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await LoadDataAsync(); };

        var btnFilter = new Button
        {
            Text = "Filtrează",
            Location = new Point(230, 4),
            BackColor = _accentBlue,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Width = 90,
            Height = 28,
            Cursor = Cursors.Hand
        };
        btnFilter.FlatAppearance.BorderSize = 0;
        btnFilter.Click += async (s, e) => await LoadDataAsync();

        BtnNewProduct = new Button
        {
            Text = "+ Produs Nou",
            Dock = DockStyle.Right,
            Width = 130,
            Height = 30,
            BackColor = _accentGreen,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        BtnNewProduct.FlatAppearance.BorderSize = 0;
        BtnNewProduct.Click += async (s, e) => await AddProductAsync();

        pnlFilters.Controls.Add(TxtSearch);
        pnlFilters.Controls.Add(btnFilter);
        pnlFilters.Controls.Add(BtnNewProduct);

        DgvProducts = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = _bgDark,
            BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            GridColor = _gridLine,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AllowUserToAddRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            EnableHeadersVisualStyles = false
        };

        DgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvProducts.ColumnHeadersHeight = 40;
        DgvProducts.ColumnHeadersDefaultCellStyle.BackColor = _bgHeader;
        DgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(180, 180, 200);
        DgvProducts.ColumnHeadersDefaultCellStyle.SelectionBackColor = _bgHeader;
        DgvProducts.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(180, 180, 200);
        DgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

        DgvProducts.RowTemplate.Height = 38;
        DgvProducts.DefaultCellStyle.BackColor = _bgDark;
        DgvProducts.DefaultCellStyle.ForeColor = Color.White;
        DgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 80);
        DgvProducts.DefaultCellStyle.SelectionForeColor = Color.White;
        DgvProducts.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

        DgvProducts.AlternatingRowsDefaultCellStyle.BackColor = _bgRowAlt;

        DgvProducts.Columns.Add("Id", "ID");
        DgvProducts.Columns.Add("Name", "Nume Produs");
        DgvProducts.Columns.Add("Price", "Preț (RON)");
        DgvProducts.Columns.Add("Stock", "Stoc");

        DgvProducts.Columns["Id"].Width = 60;
        DgvProducts.Columns["Price"].DefaultCellStyle.Format = "N2";

        var btnEditCol = new DataGridViewButtonColumn
        {
            Name = "Edit",
            HeaderText = "Acțiuni",
            Text = "Editează",
            UseColumnTextForButtonValue = true,
            FlatStyle = FlatStyle.Flat
        };

        var btnDeleteCol = new DataGridViewButtonColumn
        {
            Name = "Delete",
            HeaderText = "",
            Text = "Șterge",
            UseColumnTextForButtonValue = true,
            FlatStyle = FlatStyle.Flat
        };

        DgvProducts.Columns.Add(btnEditCol);
        DgvProducts.Columns.Add(btnDeleteCol);

        DgvProducts.CellPainting += (sender, e) =>
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == DgvProducts.Columns["Edit"].Index || e.ColumnIndex == DgvProducts.Columns["Delete"].Index))
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                bool isEdit = e.ColumnIndex == DgvProducts.Columns["Edit"].Index;
                Color btnColor = isEdit ? _accentBlue : _accentRed;
                string btnText = isEdit ? "Editează" : "Șterge";

                var buttonRect = new Rectangle(e.CellBounds.X + 4, e.CellBounds.Y + 4, e.CellBounds.Width - 8, e.CellBounds.Height - 8);
                using (var brush = new SolidBrush(btnColor))
                {
                    e.Graphics.FillRectangle(brush, buttonRect);
                }

                TextRenderer.DrawText(e.Graphics, btnText, DgvProducts.Font, buttonRect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        };

        DgvProducts.CellContentClick += async (sender, e) => await HandleGridActionAsync(e);

        Controls.Add(DgvProducts);
        Controls.Add(pnlFilters);
        Controls.Add(lblTitle);

        Load += async (s, e) => await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var products = await _apiService.GetProductsAsync(TxtSearch.Text.Trim());
        DgvProducts.Rows.Clear();

        foreach (var p in products)
        {
            DgvProducts.Rows.Add(p.Id, p.Name, p.Price, p.Stock);
        }
    }

    private async Task AddProductAsync()
    {
        using var form = new ProductForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            var (success, error) = await _apiService.CreateProductAsync(form.ProductData);
            if (success)
            {
                await LoadDataAsync();
            }
            else
            {
                MessageBox.Show($"Eroare la salvarea produsului:\n{error}", "Eroare Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private async Task HandleGridActionAsync(DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        int id = Convert.ToInt32(DgvProducts.Rows[e.RowIndex].Cells["Id"].Value);
        string name = DgvProducts.Rows[e.RowIndex].Cells["Name"].Value?.ToString() ?? "";
        decimal price = Convert.ToDecimal(DgvProducts.Rows[e.RowIndex].Cells["Price"].Value);
        int stock = Convert.ToInt32(DgvProducts.Rows[e.RowIndex].Cells["Stock"].Value);

        if (e.ColumnIndex == DgvProducts.Columns["Edit"].Index)
        {
            var product = new ProductDto { Id = id, Name = name, Price = price, Stock = stock };
            using var form = new ProductForm(product);
            if (form.ShowDialog() == DialogResult.OK)
            {
                var (success, error) = await _apiService.UpdateProductAsync(id, form.ProductData);
                if (success)
                {
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show($"Eroare la actualizarea produsului:\n{error}", "Eroare Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else if (e.ColumnIndex == DgvProducts.Columns["Delete"].Index)
        {
            var confirm = MessageBox.Show($"Sigur dorești să ștergi produsul '{name}'?", "Confirmare", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                var (success, error) = await _apiService.DeleteProductAsync(id);
                if (success)
                {
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show($"Eroare la ștergerea produsului:\n{error}", "Eroare Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}