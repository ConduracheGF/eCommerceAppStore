using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

public class ClientStoreControl : UserControl
{
    private readonly ApiService _apiService = new();
    private readonly string _clientEmail;

    public DataGridView DgvProducts { get; private set; } = null!;
    private NumericUpDown _numQuantity = null!;
    private Label _lblTotal = null!;
    private Label _lblSelectedProduct = null!;
    private ProductDto? _selectedProduct;

    public ClientStoreControl(string clientEmail)
    {
        _clientEmail = clientEmail;

        Dock = DockStyle.Fill;
        BackColor = Color.FromArgb(30, 30, 46);
        Padding = new Padding(20);

        BuildUi();
        Load += async (s, e) => await LoadAvailableProductsAsync();
    }

    private void BuildUi()
    {
        var lblTitle = new Label
        {
            Text = $"MAGAZIN ONLINE - Autentificat ca: {_clientEmail}",
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 40
        };

        var pnlOrder = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 110,
            BackColor = Color.FromArgb(40, 40, 60),
            Padding = new Padding(15)
        };

        _lblSelectedProduct = new Label
        {
            Text = "Selectează un produs din listă...",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.LightSkyBlue,
            Location = new Point(15, 12),
            AutoSize = true
        };

        var lblQty = new Label { Text = "Cantitate:", Location = new Point(15, 42), AutoSize = true, ForeColor = Color.LightGray };
        _numQuantity = new NumericUpDown
        {
            Location = new Point(85, 39),
            Width = 80,
            Minimum = 1,
            Maximum = 100,
            Value = 1,
            BackColor = Color.FromArgb(30, 30, 46),
            ForeColor = Color.White,
            Enabled = false
        };
        _numQuantity.ValueChanged += (s, e) => UpdateTotal();

        _lblTotal = new Label
        {
            Text = "Total: 0.00 RON",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(46, 204, 113),
            Location = new Point(180, 40),
            AutoSize = true
        };

        var btnBuy = new Button
        {
            Text = "🛒 Plasează Comanda",
            Location = new Point(450, 25),
            Width = 180,
            Height = 45,
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnBuy.FlatAppearance.BorderSize = 0;
        btnBuy.Click += async (s, e) => await PlaceOrderAsync();

        pnlOrder.Controls.Add(_lblSelectedProduct);
        pnlOrder.Controls.Add(lblQty);
        pnlOrder.Controls.Add(_numQuantity);
        pnlOrder.Controls.Add(_lblTotal);
        pnlOrder.Controls.Add(btnBuy);

        DgvProducts = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.FromArgb(30, 30, 46),
            BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            GridColor = Color.FromArgb(50, 50, 75),
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            ReadOnly = true,
            AllowUserToAddRows = false,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            EnableHeadersVisualStyles = false
        };

        DgvProducts.ColumnHeadersHeight = 40;
        DgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 20, 32);
        DgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(180, 180, 200);
        DgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

        DgvProducts.RowTemplate.Height = 35;
        DgvProducts.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 46);
        DgvProducts.DefaultCellStyle.ForeColor = Color.White;
        DgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 80);
        DgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(36, 36, 54);

        DgvProducts.Columns.Add("Id", "ID");
        DgvProducts.Columns.Add("Name", "Nume Produs");
        DgvProducts.Columns.Add("Price", "Preț Unitari (RON)");
        DgvProducts.Columns.Add("Stock", "Stoc Disponibil");

        DgvProducts.Columns["Price"].DefaultCellStyle.Format = "N2";
        DgvProducts.SelectionChanged += (s, e) => OnProductSelected();

        Controls.Add(DgvProducts);
        Controls.Add(pnlOrder);
        Controls.Add(lblTitle);
    }

    private async Task LoadAvailableProductsAsync()
    {
        var allProducts = await _apiService.GetProductsAsync();
        var availableProducts = allProducts.Where(p => p.Stock > 0).ToList();

        DgvProducts.Rows.Clear();
        foreach (var p in availableProducts)
        {
            DgvProducts.Rows.Add(p.Id, p.Name, p.Price, p.Stock);
        }
    }

    private void OnProductSelected()
    {
        if (DgvProducts.SelectedRows.Count == 0) return;

        var row = DgvProducts.SelectedRows[0];
        _selectedProduct = new ProductDto
        {
            Id = Convert.ToInt32(row.Cells["Id"].Value),
            Name = row.Cells["Name"].Value?.ToString() ?? "",
            Price = Convert.ToDecimal(row.Cells["Price"].Value),
            Stock = Convert.ToInt32(row.Cells["Stock"].Value)
        };

        _lblSelectedProduct.Text = $"Selectat: {_selectedProduct.Name}";
        _numQuantity.Enabled = true;
        _numQuantity.Maximum = _selectedProduct.Stock;
        _numQuantity.Value = 1;

        UpdateTotal();
    }

    private void UpdateTotal()
    {
        if (_selectedProduct == null) return;
        decimal total = _selectedProduct.Price * _numQuantity.Value;
        _lblTotal.Text = $"Total: {total:N2} RON";
    }

    private async Task PlaceOrderAsync()
    {
        if (_selectedProduct == null)
        {
            MessageBox.Show("Alege un produs mai întâi!", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var order = new CreateOrderDto
        {
            ProductId = _selectedProduct.Id,
            Quantity = (int)_numQuantity.Value,
            CustomerEmail = _clientEmail
        };

        var (success, error) = await _apiService.CreateOrderAsync(order);
        if (success)
        {
            MessageBox.Show("Comanda a fost plasată cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAvailableProductsAsync();
        }
        else
        {
            MessageBox.Show($"Eroare la plasarea comenzii:\n{error}", "Eroare API", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}