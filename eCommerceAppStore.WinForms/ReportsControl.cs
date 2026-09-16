using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace eCommerceAppStore.WinForms;

public class ReportsControl : UserControl
{
    private readonly Color _bgDark = Color.FromArgb(30, 30, 46);
    private readonly Color _bgCard = Color.FromArgb(40, 40, 60);
    private readonly Color _gridLine = Color.FromArgb(55, 55, 75);
    private readonly Color _accentBlue = Color.FromArgb(52, 152, 219);
    private readonly Color _accentGreen = Color.FromArgb(46, 204, 113);
    private readonly Color _accentOrange = Color.FromArgb(230, 126, 34);
    private readonly Color _accentRed = Color.FromArgb(231, 76, 60);

    public ReportsControl()
    {
        Dock = DockStyle.Fill;
        BackColor = _bgDark;
        Padding = new Padding(20);

        var lblTitle = new Label
        {
            Text = "RAPOARTE & SIMULĂRI DE CONTINUITATE",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 40
        };

        var pnlCards = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 90,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 15)
        };

        pnlCards.Controls.Add(CreateKpiCard("TOTAL VÂNZĂRI (LUNAR)", "48.500 RON", "+14.2% vs luna trecută", _accentGreen));
        pnlCards.Controls.Add(CreateKpiCard("PROIECȚIE CONTINUITATE", "62.000 RON", "Estimare luna viitoare", _accentBlue));
        pnlCards.Controls.Add(CreateKpiCard("AUTONOMIE MEDIE STOC", "38 Zile", "Risc scăzut de epuizare", _accentOrange));

        var pnlMainContainer = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        pnlMainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        pnlMainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

        var pnlSalesChart = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = _bgCard,
            Margin = new Padding(0, 10, 10, 0),
            Padding = new Padding(15)
        };

        var lblSalesTitle = new Label
        {
            Text = "📈 Evoluție Vânzări și Simulare Proiecție",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 30
        };

        var pnlCanvas = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent
        };
        pnlCanvas.Paint += DrawSalesChart;

        pnlSalesChart.Controls.Add(pnlCanvas);
        pnlSalesChart.Controls.Add(lblSalesTitle);

        var pnlContinuity = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = _bgCard,
            Margin = new Padding(10, 10, 0, 0),
            Padding = new Padding(15)
        };

        var lblContTitle = new Label
        {
            Text = "🛡️ Simulare Risc & Continuitate Stoc",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Top,
            Height = 30
        };

        var pnlContContent = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
        AddContinuityMetric(pnlContContent, "Monitoare Gaming 27\"", 85, _accentGreen, "Stoc Optim (45 zile)");
        AddContinuityMetric(pnlContContent, "Tastaturi Mecanice", 45, _accentOrange, "Stoc Mediu (18 zile)");
        AddContinuityMetric(pnlContContent, "Mouse Wireless", 12, _accentRed, "⚠️ Risc Ruptură (4 zile)");
        AddContinuityMetric(pnlContContent, "Căști Gaming", 65, _accentBlue, "Stoc Stabil (30 zile)");

        pnlContinuity.Controls.Add(pnlContContent);
        pnlContinuity.Controls.Add(lblContTitle);

        pnlMainContainer.Controls.Add(pnlSalesChart, 0, 0);
        pnlMainContainer.Controls.Add(pnlContinuity, 1, 0);

        Controls.Add(pnlMainContainer);
        Controls.Add(pnlCards);
        Controls.Add(lblTitle);
    }

    private Panel CreateKpiCard(string title, string value, string subtext, Color accentColor)
    {
        var card = new Panel
        {
            Width = 240,
            Height = 80,
            BackColor = _bgCard,
            Margin = new Padding(0, 0, 15, 0),
            Padding = new Padding(12)
        };

        var lblTitle = new Label { Text = title, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Dock = DockStyle.Top, Height = 16 };
        var lblValue = new Label { Text = value, ForeColor = accentColor, Font = new Font("Segoe UI", 13F, FontStyle.Bold), Dock = DockStyle.Top, Height = 24 };
        var lblSubtext = new Label { Text = subtext, ForeColor = Color.LightGray, Font = new Font("Segoe UI", 8F, FontStyle.Regular), Dock = DockStyle.Top, Height = 16 };

        card.Controls.Add(lblSubtext);
        card.Controls.Add(lblValue);
        card.Controls.Add(lblTitle);
        return card;
    }

    private void AddContinuityMetric(Panel container, string item, int percentage, Color color, string statusText)
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(0, 5, 0, 5) };
        var lblItem = new Label { Text = $"{item} - {statusText}", ForeColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Regular), Dock = DockStyle.Top, Height = 20 };

        var progressBarBg = new Panel { Dock = DockStyle.Bottom, Height = 10, BackColor = Color.FromArgb(25, 25, 38) };
        var progressBarFill = new Panel { Width = (int)(progressBarBg.Width * (percentage / 100.0)), Dock = DockStyle.Left, BackColor = color };

        progressBarBg.SizeChanged += (s, e) => {
            progressBarFill.Width = (int)(progressBarBg.Width * (percentage / 100.0));
        };

        progressBarBg.Controls.Add(progressBarFill);
        row.Controls.Add(lblItem);
        row.Controls.Add(progressBarBg);

        container.Controls.Add(row);
    }

    private void DrawSalesChart(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var panel = (Panel)sender!;
        int width = panel.Width;
        int height = panel.Height;

        if (width <= 0 || height <= 0) return;

        int padding = 40;
        int chartWidth = width - padding * 2;
        int chartHeight = height - padding * 2;

        string[] months = { "Ian", "Feb", "Mar", "Apr", "Mai", "Iun*", "Iul*" };
        float[] values = { 15, 22, 18, 35, 48, 62, 75 };
        float maxVal = 80;

        using var gridPen = new Pen(_gridLine, 1) { DashStyle = DashStyle.Dash };
        using var textFont = new Font("Segoe UI", 8F);
        using var textBrush = new SolidBrush(Color.Gray);

        for (int i = 0; i <= 4; i++)
        {
            float y = padding + (chartHeight / 4f) * i;
            g.DrawLine(gridPen, padding, y, width - padding, y);
            float labelVal = maxVal - (maxVal / 4f) * i;
            g.DrawString($"{labelVal:0}k", textFont, textBrush, 5, y - 8);
        }

        PointF[] points = new PointF[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            float x = padding + (chartWidth / (float)(values.Length - 1)) * i;
            float y = height - padding - (values[i] / maxVal) * chartHeight;
            points[i] = new PointF(x, y);

            g.DrawString(months[i], textFont, textBrush, x - 10, height - padding + 8);
        }

        using var realPen = new Pen(_accentBlue, 3);
        for (int i = 0; i < 4; i++)
        {
            g.DrawLine(realPen, points[i], points[i + 1]);
            g.FillEllipse(new SolidBrush(_accentBlue), points[i].X - 4, points[i].Y - 4, 8, 8);
        }
        g.FillEllipse(new SolidBrush(_accentBlue), points[4].X - 4, points[4].Y - 4, 8, 8);

        using var simPen = new Pen(_accentOrange, 3) { DashStyle = DashStyle.Dash };
        for (int i = 4; i < values.Length - 1; i++)
        {
            g.DrawLine(simPen, points[i], points[i + 1]);
            g.FillEllipse(new SolidBrush(_accentOrange), points[i + 1].X - 4, points[i + 1].Y - 4, 8, 8);
        }

        g.DrawString("— Istoric Real", textFont, new SolidBrush(_accentBlue), width - 180, 5);
        g.DrawString("--- Simulare Proiecție", textFont, new SolidBrush(_accentOrange), width - 90, 5);
    }
}