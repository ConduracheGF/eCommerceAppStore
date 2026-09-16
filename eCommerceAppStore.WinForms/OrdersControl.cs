using System;
using System.Drawing;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

public class OrdersControl : UserControl
{
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
        DgvOrders.Columns.Add("Status", "Status");

        DgvOrders.Rows.Add(1042, "client@email.com", 2400.00, "Procesată");
        DgvOrders.Rows.Add(1043, "popescu@test.ro", 450.00, "În Așteptare");

        Controls.Add(DgvOrders);
        Controls.Add(lblTitle);
    }
}