using System;
using System.Drawing;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

public class ProductForm : Form
{
    public ProductDto ProductData { get; private set; }

    private TextBox _txtName = null!;
    private NumericUpDown _numPrice = null!;
    private NumericUpDown _numStock = null!;

    public ProductForm(ProductDto? existingProduct = null)
    {
        ProductData = existingProduct ?? new ProductDto();

        Text = existingProduct == null ? "Adăugare Produs" : "Editare Produs";
        Size = new Size(380, 320);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.FromArgb(30, 30, 46);
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 9.5F);

        BuildUi();

        if (existingProduct != null)
        {
            _txtName.Text = existingProduct.Name;
            _numPrice.Value = existingProduct.Price;
            _numStock.Value = existingProduct.Stock;
        }
    }

    private void BuildUi()
    {
        var lblName = new Label { Text = "Nume Produs (max 20 caractere):", Location = new Point(20, 20), AutoSize = true, ForeColor = Color.LightGray };
        _txtName = new TextBox
        {
            Location = new Point(20, 45),
            Width = 320,
            MaxLength = 20,
            BackColor = Color.FromArgb(40, 40, 60),
            ForeColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var lblPrice = new Label { Text = "Preț (RON):", Location = new Point(20, 85), AutoSize = true, ForeColor = Color.LightGray };
        _numPrice = new NumericUpDown
        {
            Location = new Point(20, 110),
            Width = 320,
            DecimalPlaces = 2,
            Maximum = 100000,
            Minimum = 0.01m,
            BackColor = Color.FromArgb(40, 40, 60),
            ForeColor = Color.White
        };

        var lblStock = new Label { Text = "Stoc:", Location = new Point(20, 150), AutoSize = true, ForeColor = Color.LightGray };
        _numStock = new NumericUpDown
        {
            Location = new Point(20, 175),
            Width = 320,
            Maximum = 10000,
            Minimum = 0,
            BackColor = Color.FromArgb(40, 40, 60),
            ForeColor = Color.White
        };

        var btnSave = new Button
        {
            Text = "Salvează",
            Location = new Point(20, 225),
            Width = 150,
            Height = 35,
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.Click += (s, e) => SaveData();

        var btnCancel = new Button
        {
            Text = "Renunță",
            Location = new Point(190, 225),
            Width = 150,
            Height = 35,
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

        Controls.Add(lblName);
        Controls.Add(_txtName);
        Controls.Add(lblPrice);
        Controls.Add(_numPrice);
        Controls.Add(lblStock);
        Controls.Add(_numStock);
        Controls.Add(btnSave);
        Controls.Add(btnCancel);
    }

    private void SaveData()
    {
        if (string.IsNullOrWhiteSpace(_txtName.Text))
        {
            MessageBox.Show("Introdu numele produsului!", "Validare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ProductData.Name = _txtName.Text.Trim();
        ProductData.Price = _numPrice.Value;
        ProductData.Stock = (int)_numStock.Value;

        DialogResult = DialogResult.OK;
    }
}