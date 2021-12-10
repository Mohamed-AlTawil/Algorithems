using Algorithmes.Classes;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algorithmes
{
    public partial class DataViewForm : DevExpress.XtraEditors.XtraForm
    {
        public List<List<double>> data = new List<List<double>>();
        public List<string> City = new List<string>();
        Operations operations = new Operations();
        public DataViewForm()
        {
            InitializeComponent();
        }

        private void DataViewForm_Load(object sender, EventArgs e)
        {
            DataTable n= operations.ToDataTable(City, data);
            gridControl1.DataSource = n;
        }
    }
}