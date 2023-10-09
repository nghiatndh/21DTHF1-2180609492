using Bai2_BT2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai2_BT2
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            StudentContextDB context = new StudentContextDB();
            try
            {
                var listFacultys = context.Faculties.ToList()   ;
                FillFalcultyCombobox(listFacultys);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            this.cmbKhoa.DataSource = listFacultys;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";
        }

        private void FillMajorCombobox(List<Major> listMajors)
        {
            this.cmbChuyenNganh.DataSource = listMajors;
            this.cmbChuyenNganh.DisplayMember = "Name";
            this.cmbChuyenNganh.ValueMember = "MajorID";
        }

        private void cmbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            StudentContextDB context = new StudentContextDB();
            Faculty selectedFacullty = cmbKhoa.SelectedItem as Faculty;
            if (selectedFacullty != null) 
            {
                var listMajor = context.Majors.Where(p=>p.FacultyID ==selectedFacullty.FacultyID).ToList();
                FillMajorCombobox(listMajor);
                var listStudents = context.Students.Where(p=>p.MajorID == null && p.FacultyID == selectedFacullty.FacultyID).ToList();
                BindGrid(listStudents);
            }
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in listStudent) 
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells["MSSV"].Value = item.StudentID;
                dgvSinhVien.Rows[index].Cells["HoTen"].Value = item.FullName;
                if(item.Faculty!=null)
                {
                    dgvSinhVien.Rows[index].Cells["Khoa"].Value = item.Faculty.FacultyName;
                }
                dgvSinhVien.Rows[index].Cells["DiemTB"].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvSinhVien.Rows[index].Cells["ChuyenNganh"].Value = item.Major.Name + "";
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            StudentContextDB context = new StudentContextDB();
            List<Student> listStudent = context.Students.ToList();
            try
            {
                for (int i = 0; i < dgvSinhVien.Rows.Count; i++)

                {
                    string checker = (string)dgvSinhVien.Rows[i].Cells[0].Value;

                    if (checker == "1")
                    {
                        string mssv = dgvSinhVien.Rows[i].Cells["MSSV"].Value.ToString();
                        var updateStudent = context.Students.SingleOrDefault(c=>c.StudentID.Equals(mssv));
                        if(updateStudent.MajorID == null)
                        {
                            updateStudent.MajorID = (int)cmbChuyenNganh.SelectedValue;
                        }
                        
                        context.SaveChanges();
                    }

                }
                MessageBox.Show("Thêm chuyên ngành thành công!");
                frmRegister_Load(sender, e);
            }
            catch
            {

            }
            

        }


    }
}
