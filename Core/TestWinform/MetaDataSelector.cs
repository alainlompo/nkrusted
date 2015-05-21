using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excelta.NKrusted.Core.Examples;
using Excelta.NKrusted.Core.Metadatas;

namespace TestWinform
{
    public partial class MetaDataSelector : Form
    {
        private string _connectionString = @"server=LOMPO-PC\SQL2000PE; initial catalog = NKRUSTEDDEMO; user id = sa; password = loverofmysoul";
        private bool firstTime = true;
        public MetaDataSelector()
        {
            InitializeComponent();
        }

        private void MetaDataSelector_Load(object sender, EventArgs e)
        {
            SqlServerMetadatas sMetas = new SqlServerMetadatas();
            DataTable orgTable = sMetas.GetSqlServerMetaDataCollections(_connectionString);
            cbbList.DataSource = orgTable;
            cbbList.DisplayMember = "CollectionName";
            cbbList.ValueMember = "CollectionName";

            dgvMetaDatas.DataSource = orgTable;


        }

        private void cbbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbbList.SelectedIndex;
            DataTable source = (DataTable)cbbList.DataSource;
            string chosenMetaData = source.Rows[index]["CollectionName"].ToString();

           
                SqlServerMetadatas sMetas = new SqlServerMetadatas();
                DataTable orgTable = sMetas.GetSqlMetaDataFor(_connectionString, chosenMetaData);


                dgvMetaDatas.DataSource = orgTable;
           


        }
    }
}
