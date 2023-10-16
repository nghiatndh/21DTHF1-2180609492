using Bai2_BT2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai2_BT2
{
    public partial class loaddata : Form
    {
        public loaddata()
        {
            InitializeComponent();
        }

        private void loaddata_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvSinhVien);
                StudentContextDB context = new StudentContextDB();
                List<Faculty> listFaculties = context.Faculties.ToList();
                List<Student> listStudents = context.Students.ToList();
             
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BindGrid(List<Student> listStudents)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells["MaSV"].Value = item.StudentID;
                dgvSinhVien.Rows[index].Cells["HoTen"].Value = item.FullName;
                if (item.Faculty != null)
                    dgvSinhVien.Rows[index].Cells["Khoa"].Value = item.Faculty.FacultyName;

                dgvSinhVien.Rows[index].Cells["DiemTB"].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvSinhVien.Rows[index].Cells["ChuyenNganh"].Value = item.Major.Name + "";
                

            }
        }
        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle =
           DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

    }
}
