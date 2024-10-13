namespace AirDefenseOptimizer.FuzzyLogic
{
    /// <summary>
    /// Bulanık mantıkta kullanılan üyelik fonksiyonlarının temel sınıfıdır.
    /// Bu sınıf, her fonksiyonun sahip olması gereken zorunlu metodları tanımlar.
    /// </summary>
    public abstract class MembershipFunction
    {
        /// <summary>
        /// Üyelik fonksiyonunun adı.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Üyelik fonksiyonunun adıyla birlikte oluşturulmasını sağlar.
        /// </summary>
        /// <param name="name">Üyelik fonksiyonunun adı</param>
        protected MembershipFunction(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Üyelik derecesini hesaplayan soyut metod.
        /// Girdi değeri (x) ile üyelik derecesi hesaplanır.
        /// </summary>
        /// <param name="x">Değerlendirilecek giriş değeri</param>
        /// <returns>Üyelik derecesi (0 ile 1 arası bir değer)</returns>
        public abstract double CalculateMembership(double x);

        /// <summary>
        /// Her fonksiyonun temsil edici bir değerini döndüren metod.
        /// Üyelik fonksiyonunun karakteristik zirve değerini döndürür.
        /// </summary>
        /// <returns>Temsil edici değer</returns>
        public abstract double GetRepresentativeValue();
    }

    /// <summary>
    /// Üçgen şeklinde üyelik fonksiyonlarını temsil eden sınıf.
    /// </summary>
    public class TriangleMembershipFunction : MembershipFunction
    {
        public double A { get; set; } // Sol sınır
        public double B { get; set; } // Zirve (en yüksek nokta)
        public double C { get; set; } // Sağ sınır

        /// <summary>
        /// Üçgen üyelik fonksiyonunu oluşturur.
        /// </summary>
        /// <param name="name">Fonksiyonun adı</param>
        /// <param name="a">Sol sınır</param>
        /// <param name="b">Zirve noktası</param>
        /// <param name="c">Sağ sınır</param>
        public TriangleMembershipFunction(string name, double a, double b, double c)
            : base(name)
        {
            A = a;
            B = b;
            C = c;
        }

        /// <summary>
        /// Üçgen üyelik fonksiyonuna göre üyelik derecesini hesaplar.
        /// </summary>
        /// <param name="x">Giriş değeri</param>
        /// <returns>Üyelik derecesi (0 ile 1 arası)</returns>
        public override double CalculateMembership(double x)
        {
            if (x <= A || x >= C) return 0;
            else if (x == B) return 1;
            else if (x > A && x < B) return (x - A) / (B - A);
            else return (C - x) / (C - B);
        }

        /// <summary>
        /// Üçgen fonksiyonun zirve noktasını (B) temsil edici değer olarak döndürür.
        /// </summary>
        /// <returns>Zirve değeri (B)</returns>
        public override double GetRepresentativeValue()
        {
            return B;
        }
    }

    /// <summary>
    /// Trapez şeklinde üyelik fonksiyonlarını temsil eden sınıf.
    /// </summary>
    public class TrapezoidalMembershipFunction : MembershipFunction
    {
        public double A { get; set; } // Sol kenar
        public double B { get; set; } // Sol üst kenar
        public double C { get; set; } // Sağ üst kenar
        public double D { get; set; } // Sağ kenar

        /// <summary>
        /// Trapez üyelik fonksiyonunu oluşturur.
        /// </summary>
        /// <param name="name">Fonksiyonun adı</param>
        /// <param name="a">Sol kenar</param>
        /// <param name="b">Sol üst kenar</param>
        /// <param name="c">Sağ üst kenar</param>
        /// <param name="d">Sağ kenar</param>
        public TrapezoidalMembershipFunction(string name, double a, double b, double c, double d)
            : base(name)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        /// <summary>
        /// Trapez üyelik fonksiyonuna göre üyelik derecesini hesaplar.
        /// </summary>
        /// <param name="x">Giriş değeri</param>
        /// <returns>Üyelik derecesi (0 ile 1 arası)</returns>
        public override double CalculateMembership(double x)
        {
            if (x <= A || x >= D) return 0;
            else if (x >= B && x <= C) return 1;
            else if (x > A && x < B) return (x - A) / (B - A);
            else return (D - x) / (D - C);
        }

        /// <summary>
        /// Trapez fonksiyonun ortalamasını temsil edici değer olarak döndürür.
        /// </summary>
        /// <returns>Ortalama değer (B ve C'nin ortalaması)</returns>
        public override double GetRepresentativeValue()
        {
            return (B + C) / 2; // B ve C'nin ortalaması
        }
    }

    /// <summary>
    /// Gauss (Bell Eğrisi) üyelik fonksiyonlarını temsil eden sınıf.
    /// </summary>
    public class GaussianMembershipFunction : MembershipFunction
    {
        public double Mean { get; set; }  // Ortalama (merkez)
        public double Sigma { get; set; } // Dağılım

        /// <summary>
        /// Gauss üyelik fonksiyonunu oluşturur.
        /// </summary>
        /// <param name="name">Fonksiyonun adı</param>
        /// <param name="mean">Ortalama (merkez) değeri</param>
        /// <param name="sigma">Standart sapma (dağılım)</param>
        public GaussianMembershipFunction(string name, double mean, double sigma)
            : base(name)
        {
            Mean = mean;
            Sigma = sigma;
        }

        /// <summary>
        /// Gauss üyelik fonksiyonuna göre üyelik derecesini hesaplar.
        /// </summary>
        /// <param name="x">Giriş değeri</param>
        /// <returns>Üyelik derecesi (0 ile 1 arası)</returns>
        public override double CalculateMembership(double x)
        {
            return Math.Exp(-0.5 * Math.Pow((x - Mean) / Sigma, 2));
        }

        /// <summary>
        /// Gauss fonksiyonun ortalama (mean) değeri temsil edici değer olarak döndürülür.
        /// </summary>
        /// <returns>Ortalama (mean) değeri</returns>
        public override double GetRepresentativeValue()
        {
            return Mean;
        }
    }
}
