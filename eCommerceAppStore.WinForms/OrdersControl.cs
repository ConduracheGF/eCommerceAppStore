using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

public class OrdersControl : UserControl
{
    private readonly ApiService _apiService = new();
    public DataGridView DgvOrders { get; private set; } = null!;

    public OrdersControl()
    {
        Dock = DockStyle.Fill;
        BackColor = Color.FromArgb(30, 30, 46);
        Padding = new Padding(20);

        var lblTitle = new Label
        {
            Text = "GESTIUNE COMENZI",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 40
        };

        DgvOrders = new DataGridView
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

        DgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        DgvOrders.ColumnHeadersHeight = 40;
        DgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 20, 32);
        DgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(180, 180, 200);
        DgvOrders.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 20, 32);
        DgvOrders.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(180, 180, 200);
        DgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

        DgvOrders.RowTemplate.Height = 38;
        DgvOrders.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 46);
        DgvOrders.DefaultCellStyle.ForeColor = Color.White;
        DgvOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 80);
        DgvOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(36, 36, 54);

        DgvOrders.Columns.Add("Id", "ID Comandă");
        DgvOrders.Columns.Add("Customer", "Email Client");
        DgvOrders.Columns.Add("Total", "Total (RON)");
        DgvOrders.Columns.Add("Date", "Data Creării");

        if (DgvOrders.Columns["Total"] != null)
        {
            DgvOrders.Columns["Total"]!.DefaultCellStyle.Format = "N2";
        }

        Controls.Add(DgvOrders);
        Controls.Add(lblTitle);

        Load += async (s, e) => await LoadOrdersAsync();
    }

    private async Task LoadOrdersAsync()
    {
        var orders = await _apiService.GetOrdersAsync();
        DgvOrders.Rows.Clear();

        foreach (var o in orders)
        {
            DgvOrders.Rows.Add(o.Id, o.CustomerEmail, o.TotalAmount, o.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
        }
    }
}