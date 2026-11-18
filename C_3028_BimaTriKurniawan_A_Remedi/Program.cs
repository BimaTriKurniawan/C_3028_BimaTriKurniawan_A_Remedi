using System;
using System.Collections.Generic;

namespace SistemManajemenMBG
{
    // 1. Class, Object, Attribute, dan Method
    public class Sekolah
    {
        public string idSekolah;
        public string namaSekolah;
        public int jumlahPaketDiterima;

        public Sekolah(string idSekolah, string namaSekolah, int jumlahPaketDiterima = 0)
        {
            this.idSekolah = idSekolah;
            this.namaSekolah = namaSekolah;
            this.jumlahPaketDiterima = jumlahPaketDiterima;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"Sekolah {namaSekolah} dengan ID {idSekolah}, mendapatkan jatah MBG: {jumlahPaketDiterima} paket");
        }
    }

    // 2. Encapsulation & 5. Abstract Class
    public abstract class PaketMBG
    {
        private string idPaket;
        private string namaPaket;
        private int stokDapur;


        public string IdPaket
        {
            get { return idPaket; }
            set { idPaket = value; }
            // Halo
        }

        public string NamaPaket
        {
            get { return namaPaket; }
            set { namaPaket = value; }
        }

        // Setter dibuat private/protected agar stok tidak bisa dikurangi sembarangan dari luar class
        public int StokDapur
        {
            get { return stokDapur; }
            protected set
            {
                if (value < 0)
                {
                    Console.WriteLine("Peringatan: Stok dapur tidak boleh negatif. Diatur ke 0.");
                    stokDapur = 0;
                }
                else
                {
                    stokDapur = value;
                }
            }
        }

        public PaketMBG(string idPaket, string namaPaket, int stokAwal)
        {
            this.idPaket = idPaket;
            this.namaPaket = namaPaket;
            StokDapur = stokAwal;
        }

        // Pengurangan stok hanya boleh lewat method ini
        public bool KurangiStok(int jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Gagal: Jumlah pengurangan stok harus lebih dari 0.");
                return false;
            }
            else if (jumlah > stokDapur)
            {
                Console.WriteLine($"Gagal: Stok {namaPaket} tidak mencukupi (Tersedia: {stokDapur}, Diminta: {jumlah}).");
                return false;
            }
            else
            {
                stokDapur -= jumlah;
                return true;
            }
        }

        // 4. Polymorphism & 5. Abstract Method (tanpa body {})
        public abstract int HitungBiaya(int jumlah);
    }

    // 3. Inheritance (Class Turunan 1)
    public class PaketReguler : PaketMBG
    {
        public int harga = 15000;

        public PaketReguler(string idPaket, string namaPaket, int stokAwal)
            : base(idPaket, namaPaket, stokAwal)
        {
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * harga;
        }
    }

    // 3. Inheritance (Class Turunan 2 - sesuai soal bernama PaketKhusus)
    public class PaketKhusus : PaketMBG
    {
        public int harga = 20000;
        public string jenisDiet;

        public PaketKhusus(string idPaket, string namaPaket, int stokAwal, string jenisDiet)
            : base(idPaket, namaPaket, stokAwal)
        {
            this.jenisDiet = jenisDiet;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * harga;
        }
    }

    // 6. Interface (tanpa body {} dan tanpa access modifier)
    public interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    public class DistribusiMBG : DapatDidistribusikan
    {
        public Sekolah sekolahTujuan;
        public PaketMBG paketDikirim;
        public int jumlah;

        public DistribusiMBG(Sekolah sekolahTujuan, PaketMBG paketDikirim, int jumlah)
        {
            this.sekolahTujuan = sekolahTujuan;
            this.paketDikirim = paketDikirim;
            this.jumlah = jumlah;
        }

        // Implementasi interface TIDAK memakai keyword 'override'
        public void KirimPaket()
        {
            // Penerapan if-else untuk validasi aturan operasional distribusi
            if (jumlah <= 0)
            {
                Console.WriteLine($"[GAGAL] Pengiriman ke {sekolahTujuan.namaSekolah} dibatalkan: Jumlah paket harus lebih dari 0.");
            }
            else if (jumlah > paketDikirim.StokDapur)
            {
                Console.WriteLine($"[GAGAL] Pengiriman ke {sekolahTujuan.namaSekolah} dibatalkan: Stok {paketDikirim.NamaPaket} kurang (Tersedia: {paketDikirim.StokDapur}, Diminta: {jumlah}).");
            }
            else
            {
                // Kurangi stok dapur dan tambah jatah sekolah hanya jika valid
                if (paketDikirim.KurangiStok(jumlah))
                {
                    sekolahTujuan.jumlahPaketDiterima += jumlah;
                    Console.WriteLine($"Distribusi MBG berhasil! Sebanyak {jumlah} paket makanan telah dikirim ke sekolah {sekolahTujuan.namaSekolah}");
                }
            }
        }
    }

    // 7. Opsional: Class PengelolaMBG
    public class PengelolaMBG
    {
        private List<Sekolah> daftarSekolah = new List<Sekolah>();
        private List<PaketMBG> daftarPaket = new List<PaketMBG>();

        public void TambahSekolah(Sekolah sekolah)
        {
            daftarSekolah.Add(sekolah);
        }

        public void TambahPaket(PaketMBG paket)
        {
            daftarPaket.Add(paket);
        }

        public void TampilkanDataMBG()
        {
            Console.WriteLine("\n=== DATA SEKOLAH PENERIMA MBG ===");
            foreach (Sekolah s in daftarSekolah)
            {
                s.TampilInfo();
            }

            Console.WriteLine("\n=== DATA STOK PAKET DAPUR MBG ===");
            foreach (PaketMBG p in daftarPaket)
            {
                Console.WriteLine($"[{p.IdPaket}] {p.NamaPaket} - Sisa Stok: {p.StokDapur} paket");
            }
            Console.WriteLine("=================================\n");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            // Membuat minimal 2 objek Sekolah
            Sekolah sekolah1 = new Sekolah("SCH-01", "SDN 1 Jember", 0);
            Sekolah sekolah2 = new Sekolah("SCH-02", "SMPN 2 Jember", 0);

            // Penerapan Polymorphism: Variabel bertipe parent (PaketMBG) menampung objek turunan
            PaketMBG paketReguler = new PaketReguler("PKT-REG", "Paket Reguler (Nasi, Ayam, Sayur, Buah)", 100);
            PaketMBG paketKhusus = new PaketKhusus("PKT-KHS", "Paket Khusus Diet", 50, "Rendah Gula");

            // Demonstrasi pemanggilan HitungBiaya secara dinamis (Polymorphism)
            Console.WriteLine("=== SIMULASI PERHITUNGAN BIAYA ===");
            Console.WriteLine($"Biaya 20 {paketReguler.NamaPaket}: Rp{paketReguler.HitungBiaya(20):N0}");
            Console.WriteLine($"Biaya 15 {paketKhusus.NamaPaket}: Rp{paketKhusus.HitungBiaya(15):N0}\n");

            // Daftarkan ke PengelolaMBG (Opsional)
            PengelolaMBG pengelola = new PengelolaMBG();
            pengelola.TambahSekolah(sekolah1);
            pengelola.TambahSekolah(sekolah2);
            pengelola.TambahPaket(paketReguler);
            pengelola.TambahPaket(paketKhusus);

            // Proses Distribusi MBG
            Console.WriteLine("=== PROSES DISTRIBUSI MBG ===");
            DistribusiMBG dist1 = new DistribusiMBG(sekolah1, paketReguler, 40); // Berhasil
            dist1.KirimPaket();

            DistribusiMBG dist2 = new DistribusiMBG(sekolah2, paketKhusus, 60);  // Gagal (stok cuma 50)
            dist2.KirimPaket();

            DistribusiMBG dist3 = new DistribusiMBG(sekolah2, paketKhusus, 0);   // Gagal (jumlah <= 0)
            dist3.KirimPaket();

            DistribusiMBG dist4 = new DistribusiMBG(sekolah2, paketKhusus, 25);  // Berhasil
            dist4.KirimPaket();

            // Tampilkan data akhir
            pengelola.TampilkanDataMBG();
        }
    }
}