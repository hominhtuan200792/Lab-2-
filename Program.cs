using System;
using System.Collections;          // Hashtable (Bài 3)
using System.Collections.Generic;  // List<int> (Bài 2)

namespace Lab02
{
    // ===== Bài 1: ngoại lệ tự định nghĩa =====
    class NotNegativeException : Exception
    {
        public NotNegativeException()
            : base("Gia tri duoi dau can < 0")
        { }
    }

    // ===== Bài 3: cấu trúc mặt hàng =====
    struct MatHang
    {
        public int MaMH;
        public string TenMH;
        public int SoLuong;
        public float DonGia;

        public MatHang(int MaMH, string TenMH, int SoLuong, float DonGia)
        {
            this.MaMH = MaMH;
            this.TenMH = TenMH;
            this.SoLuong = SoLuong;
            this.DonGia = DonGia;
        }

        public float ThanhTien()
        {
            return SoLuong * DonGia;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n===== LAB 02 =====");
                Console.WriteLine("1. Bài 1: Tính H = căn((3x+2y)/(2x-y))");
                Console.WriteLine("2. Bài 2: Sinh ngẫu nhiên n số và sắp xếp");
                Console.WriteLine("3. Bài 3: Quản lý mặt hàng (Hashtable)");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn: ");

                switch (Console.ReadLine())
                {
                    case "1": Bai1(); break;
                    case "2": Bai2(); break;
                    case "3": Bai3(); break;
                    case "0": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ!"); break;
                }
            }
        }

        // ================= BÀI 1 =================
        static void Bai1()
        {
            try
            {
                Console.Write("Nhập x: ");
                int x = int.Parse(Console.ReadLine());      // sai format -> FormatException
                Console.Write("Nhập y: ");
                int y = int.Parse(Console.ReadLine());

                double mau = 2.0 * x - y;
                if (mau == 0)
                    throw new DivideByZeroException();       // chia cho 0

                double bieuThuc = (3.0 * x + 2.0 * y) / mau;
                if (bieuThuc < 0)
                    throw new NotNegativeException();        // dưới dấu căn < 0

                double h = Math.Sqrt(bieuThuc);
                Console.WriteLine("H = " + h);
            }
            catch (FormatException)
            {
                Console.WriteLine("Lỗi: x và y phải là số nguyên!");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Lỗi: mẫu số 2x - y bằng 0, không thể chia!");
            }
            catch (NotNegativeException ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            catch (OverflowException)
            {
                Console.WriteLine("Lỗi: số nhập quá lớn!");
            }
        }

        // ================= BÀI 2 =================
        static void Bai2()
        {
            int n;
            Console.Write("Nhập n (n > 0): ");
            if (!int.TryParse(Console.ReadLine(), out n) || n <= 0)
            {
                Console.WriteLine("n phải là số nguyên dương!");
                return;
            }

            List<int> ml = new List<int>();
            Random rnd = new Random();

            for (int i = 0; i < n; i++)
                ml.Add(rnd.Next(100));          // số ngẫu nhiên 0..99

            Console.WriteLine("Dãy ban đầu:");
            foreach (int so in ml) Console.Write(so + " ");

            ml.Sort();                          // sắp xếp tăng dần

            Console.WriteLine("\nDãy sau khi sắp xếp tăng dần:");
            foreach (int so in ml) Console.Write(so + " ");
            Console.WriteLine();
        }

        // ================= BÀI 3 =================
        static void ThemMatHang(Hashtable danhsach, MatHang m)
        {
            danhsach.Add(m.MaMH, m);
        }

        static bool TimMatHang(Hashtable danhsach, int MaMH)
        {
            return danhsach.ContainsKey(MaMH);
        }

        static void XoaMatHang(Hashtable danhsach, int MaMH)
        {
            if (TimMatHang(danhsach, MaMH))
                danhsach.Remove(MaMH);
        }

        static void XuatDanhSach(Hashtable danhsach)
        {
            if (danhsach.Count == 0)
            {
                Console.WriteLine("Danh sách rỗng!");
                return;
            }

            Console.WriteLine("{0,-8}{1,-20}{2,-10}{3,-12}{4,-12}",
                "Mã MH", "Tên MH", "Số lượng", "Đơn giá", "Thành tiền");
            Console.WriteLine(new string('-', 62));

            foreach (DictionaryEntry de in danhsach)
            {
                MatHang m = (MatHang)de.Value;      // lấy lại struct từ Hashtable
                Console.WriteLine("{0,-8}{1,-20}{2,-10}{3,-12}{4,-12}",
                    m.MaMH, m.TenMH, m.SoLuong, m.DonGia, m.ThanhTien());
            }
        }

        static void Bai3()
        {
            Hashtable danhsach = new Hashtable();
            string tiepTuc;

            // a) Nhập danh sách, hỏi tiếp tục sau mỗi mặt hàng
            do
            {
                try
                {
                    Console.Write("Mã mặt hàng (số nguyên): ");
                    int ma = int.Parse(Console.ReadLine());

                    if (TimMatHang(danhsach, ma))
                    {
                        Console.WriteLine("Mã mặt hàng đã tồn tại!");
                    }
                    else
                    {
                        Console.Write("Tên mặt hàng: ");
                        string ten = Console.ReadLine();
                        Console.Write("Số lượng: ");
                        int sl = int.Parse(Console.ReadLine());
                        Console.Write("Đơn giá: ");
                        float gia = float.Parse(Console.ReadLine());

                        ThemMatHang(danhsach, new MatHang(ma, ten, sl, gia));
                        Console.WriteLine("Đã thêm mặt hàng.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Lỗi: nhập sai định dạng số!");
                }

                Console.Write("Tiếp tục nhập? (c/k): ");
                tiepTuc = Console.ReadLine();
            } while (tiepTuc != null && tiepTuc.Trim().ToLower() == "c");

            // b) Xuất danh sách
            Console.WriteLine("\n=== DANH SÁCH MẶT HÀNG ===");
            XuatDanhSach(danhsach);

            // c) Nhập mã, tìm, nếu thấy thì xóa và xuất lại
            try
            {
                Console.Write("\nNhập mã mặt hàng cần tìm để xóa: ");
                int maXoa = int.Parse(Console.ReadLine());

                if (TimMatHang(danhsach, maXoa))
                {
                    Console.WriteLine("Tìm thấy. Tiến hành xóa...");
                    XoaMatHang(danhsach, maXoa);
                    Console.WriteLine("\n=== DANH SÁCH SAU KHI XÓA ===");
                    XuatDanhSach(danhsach);
                }
                else
                {
                    Console.WriteLine("Không tìm thấy mặt hàng có mã " + maXoa);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Lỗi: mã phải là số nguyên!");
            }
        }
    }
}