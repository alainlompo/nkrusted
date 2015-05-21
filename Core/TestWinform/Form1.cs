using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excelta.NKrusted.Core.Metadatas;
using Excelta.NKrusted.Core;

namespace TestWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void sqlServerMetadatasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlServerMetadatas sqlMetas = new SqlServerMetadatas();

            DataTable metaDatas = sqlMetas.GetSqlServerSchemas(@"server=LOMPO-PC\SQL2000PE; initial catalog = NKRUSTEDDEMO; user id = sa; password = loverofmysoul");
            Form f2 = new Form();
            DataGridView dgV = new DataGridView();
            dgV.DataSource = metaDatas;
            f2.Controls.Add(dgV);
            dgV.Dock = DockStyle.Fill;
            f2.Show();

        }

        private void oleDbMetadatasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OleDbMetadatas oMetas = new OleDbMetadatas();
            DataTable metaDatas = oMetas.GetTablesFrom(@"Provider=SQLOLEDB; Data Source=LOMPO-PC\SQL2000PE; initial catalog=NKRUSTEDDEMO; user id =sa; password=loverofmysoul");
            DataTable meta2 = oMetas.GetSqlServerTablesFrom(@"LOMPO-PC\SQL2000PE", "NKRUSTEDDEMO", "sa", "loverofmysoul");
            Form f2 = new Form();
            DataGridView dgV = new DataGridView();
            dgV.DataSource = meta2;
            f2.Controls.Add(dgV);
            dgV.Dock = DockStyle.Fill;
            f2.Show();


        }

        private void oleDbTableColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OleDbMetadatas oMetas = new OleDbMetadatas();
            DataTable metaDatas = oMetas.GetColumnsFrom(@"Provider=SQLOLEDB; Data Source=LOMPO-PC\SQL2000PE; initial catalog=NKRUSTEDDEMO; user id =sa; password=loverofmysoul", "Employees");

            Form f2 = new Form();
            DataGridView dgV = new DataGridView();
            dgV.DataSource = metaDatas;
            f2.Controls.Add(dgV);
            dgV.Dock = DockStyle.Fill;
            f2.Show();


        }

        private void sqlServerMetadataCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlServerMetadatas sMetas = new SqlServerMetadatas();
            DataTable metaDatasCollection = sMetas.GetSqlServerMetaDataCollections(@"server=LOMPO-PC\SQL2000PE; initial catalog = NKRUSTEDDEMO; user id = sa; password = loverofmysoul");
            Form f2 = new Form();
            DataGridView dgV = new DataGridView();
            dgV.DataSource = metaDatasCollection;
            f2.Controls.Add(dgV);
            dgV.Dock = DockStyle.Fill;
            f2.Show();

        }

        private void sqlMetaDatasSelectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MetaDataSelector selector = new MetaDataSelector();
            selector.StartPosition = FormStartPosition.CenterParent;
            selector.ShowDialog();
               
        }

        private void sqlServerTableColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SqlServerMetadatas sMetas = new SqlServerMetadatas();
            DataTable metaDatasCollection = sMetas.GetSqlServerColumnsFrom(@"server=LOMPO-PC\SQL2000PE; initial catalog = NKRUSTEDDEMO; user id = sa; password = loverofmysoul","NKRUSTEDDEMO","Employees");
            Form f2 = new Form();
            DataGridView dgV = new DataGridView();
            dgV.DataSource = metaDatasCollection;
            f2.Controls.Add(dgV);
            dgV.Dock = DockStyle.Fill;
            f2.Show();

        }

        private void sqlServerIndexColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SqlServerMetadatas sMetas = new SqlServerMetadatas();
            DataTable indexCols = sMetas.GetSqlServerIndexColumnsFrom(@"server=LOMPO-PC\SQL2000PE; initial catalog = NKRUSTEDDEMO; user id = sa; password = loverofmysoul", "NKRUSTEDDEMO", "Employees");

            Form f2 = new Form();
            DataGridView dgV = new DataGridView();
            dgV.DataSource = indexCols;
            f2.Controls.Add(dgV);
            dgV.Dock = DockStyle.Fill;
            f2.Show();

        }
    }
}
